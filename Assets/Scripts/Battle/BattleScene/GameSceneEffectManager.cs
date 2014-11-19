using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Battle
{
	public class GameSceneEffectManager : MonoBehaviour, IMessageDriven
	{
		public const int EXPLODE_CROWD_LIMIT = 10;

		private GameSceneManager _sceneManager;

		private GameElementManager _elementManager;

		private List<TombStoneController> _tombStoneList;
		
		void Start ()
		{
			_sceneManager = GetComponent<GameSceneManager> ();
			_elementManager = GetComponent<GameElementManager> ();
			_tombStoneList = new List<TombStoneController>();

			_sceneManager.MessageRoute.RegistryMessageDrivenBehaviour (this);
		}

		void OnDestroy ()
		{
			_sceneManager.MessageRoute.UnregistryMessageDrivenBehaviour (this);
		}
		
		public String[] GetMessageTypes ()
		{
			String[] messageTypes = new String[]
			{
				Messages.SHOW_SKILL_EFFECT, Messages.SHOW_BULLET_EFFECT
			};

			return messageTypes;
		}
		
		public void OnMessageArrived (MessageContext context)
		{
			int metaID = (int)context.GetConetextValue(MessageParamKey.METAID); 
			Faction faction = (Faction)context.GetConetextValue(MessageParamKey.FACTION);
			
			switch (context.messageType)
			{
			case Messages.SHOW_SKILL_EFFECT:
				Vector2 position2 = (Vector2)context.GetConetextValue(MessageParamKey.POSITION);
				Vector3 position = MathUtil.ParseToVector3(position2, _sceneManager.GetTerrainInterpolatedHeight(position2.x_, position2.y_));
				CastSkill(metaID, faction, position);
				break;
			case Messages.SHOW_BULLET_EFFECT:
				string shooterName = (string)context.GetConetextValue(MessageParamKey.CURRENT_SOLDIER);
				string targetName = (string)context.GetConetextValue(MessageParamKey.TARGET);
				List<SoldierMeta> detectedEnemys = (List<SoldierMeta>)context.GetConetextValue(MessageParamKey.DETECTED_ENEMYS);
				CharacterAttackingContext shooterCharacterController = _sceneManager.GetCharacterAttackingContext (shooterName);
				CharacterAttackingContext targetCharacterController = _sceneManager.GetCharacterAttackingContext (targetName);
                
                if (shooterCharacterController != null && targetCharacterController != null)
                {
					StartCoroutine(ShootBullet(metaID, faction, shooterCharacterController, targetCharacterController, detectedEnemys));
                }
                 
				break;
			}
		}

		public void CastSkill (int skillID, Faction faction, Vector3 position)
		{
			SkillMeta skillMeta = MetaManager.Instance.GetSkillMeta(skillID);
			
			ElementController skillEffect = _elementManager.GetElement(ElementType.Effect, skillMeta.effectID);
			skillEffect.transform.position = position;
		}
		
		public IEnumerator ShootBullet (int bulletID, Faction faction, CharacterAttackingContext shooterCharacterController, 
		                                CharacterAttackingContext targetCharacterController, List<SoldierMeta> detectedEnemys)
		{
			yield return new WaitForSeconds (shooterCharacterController.attackDelay / BattleTime.timeScale);

           if (shooterCharacterController.attackTransform != null && targetCharacterController.attackedTransform != null)
			{
                BulletMeta bulletMeta = MetaManager.Instance.GetBulletMeta(bulletID);
				BulletController bullet = BattleGlobal.elementManager.GetElement(ElementType.Effect, bulletMeta.effectID).GetComponent<BulletController>();

                if (bullet != null)
                {
					bullet.Initialize(this, bulletMeta, faction, shooterCharacterController.attackTransform.position, targetCharacterController.attackedTransform.position, detectedEnemys);
                }
			}
		}   

		public void ShowAreaAttackingEffect (BulletMeta bulletMeta, Faction faction, Vector3 position, string hitName, List<SoldierMeta> detectedEnemys)
		{
			GameObject hitTarget = hitName != null ? _sceneManager.GetSoldierByName (hitName) : null;

			ElementController afterEffect = _elementManager.GetElement(ElementType.Effect, bulletMeta.areaEffectID);
			afterEffect.transform.position = position;

			ExplosionController explosionController = afterEffect.GetComponent<ExplosionController> ();

			if (explosionController != null)
			{
				explosionController.Prepare(hitTarget != null && hitTarget.layer == Layers.AIR_FORCE);
			}

			if (hitTarget == null || hitTarget.layer != Layers.AIR_FORCE)
			{
				ExplosionInfo explosionInfo = afterEffect.GetComponent<ExplosionInfo>();
				explosionInfo.radius = bulletMeta.attackRadius;
				
				position.y = _sceneManager.GetTerrainInterpolatedHeight(position.x, position.z);
				
				if (detectedEnemys.Count > 0 && (explosionInfo.alwaysFlyingSoldier || RandomUtil.value < Mathf.Min(0.5f, detectedEnemys.Count * 1f / EXPLODE_CROWD_LIMIT))) 
				{
					int flyingNum = RandomUtil.Range(explosionInfo.minFlyingNum, explosionInfo.maxFlyingNum);
					
					for (int i = 0; i < flyingNum; ++i)
					{
						SoldierMeta soldierMeta = detectedEnemys[RandomUtil.Range(0, detectedEnemys.Count)];
						
						if (soldierMeta.sizeLevel == SizeLevel.Small)
						{
							ICharacterController characterController = _elementManager.GetElement(ElementType.FlySoldier, soldierMeta.id).GetComponent<ICharacterController>();
							characterController.name = soldierMeta.metaName;
							
							Vector3 sphereOffset = RandomUtil.onUnitSphere * explosionInfo.radius * 0.5f;
							sphereOffset.y = Math.Max(Mathf.Abs(sphereOffset.y), explosionInfo.transform.position.y + 0.5f);
							characterController.transform.position = position + sphereOffset;
							
							characterController.ExplodeAway(explosionInfo);
						}

						if (bulletMeta.additionEffectID > 0)
						{
							Vector3 effectPosition = new Vector3(position.x + UnityEngine.Random.Range(-2.0f, 2.0f), position.y, position.z + UnityEngine.Random.Range(-2.0f, 2.0f));

							ISkillAdditionEffectController additionEffectController 
								= _elementManager.GetElement(ElementType.Effect, bulletMeta.additionEffectID).GetComponent<ISkillAdditionEffectController>();
							additionEffectController.transform.position = effectPosition;
							additionEffectController.StartEffect();
						}
					}
				}
			}
		}
		
		public TombStoneController SearchTombStone (string tag, Vector3 centerPosition, float radius)
		{
			radius = Mathf.Pow(radius, 2);
			
			TombStoneController tombStone = null;
			
			foreach (TombStoneController tomb in _tombStoneList)
			{
				if (tomb.troopTag == tag)
				{
					float distance = MathUtil.SqrDistanceXZ(tomb.transform.position, centerPosition);
					
					if (distance < radius && (tombStone == null || tomb.timeStamp > tombStone.timeStamp))
					{
						tombStone = tomb;
					}
				}
			}
			
			return tombStone;
		}

		//		
		//	public TombStoneController GetTombStone (TroopInfo troopInfo, Vector3 position)
		//	{
		//        #if BATTLE_SIMULATOR
		//        return null;
		//        #endif
		//		TroopDefinition troopDefinition = troopInfo.troopDefinition;
		//		SoldierMeta soldierMeta = MetaManager.Instance.GetSoldierMeta(troopDefinition.metaName);
		//
		//		string tombStoneName = "Tombstone_" + soldierMeta.sizeLevel.ToString();
		//
		//		TombStoneController tombStone = _elementManager.GetElement(ElementType.TombStone, tombStoneName).GetComponent<TombStoneController>();
		//		tombStone.transform.position = position;
		//		tombStone.timeStamp = BattleTime.time;
		//		tombStone.troopDefinition = troopDefinition;
		//		tombStone.troopTag = troopInfo.tag;
		//
		//		_tombStoneList.Add(tombStone);
		//		BattleGlobal.sceneDataManager.AddTombStone(tombStone.ID, tombStone.gameObject);
		//
		//		return tombStone;
		//	}
		//
		//	public void RemoveTombStone (TombStoneController tombStone)
		//	{
		//		_tombStoneList.Remove(tombStone);
		//		BattleGlobal.sceneDataManager.RemoveTombStone(tombStone.ID);
		//	}
	}
}
