using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Battle
{
    public class GameSceneManager : MonoBehaviour
    {
        public GameObject playSide;
        public GameObject enemySide;

        [Range(1f, 20f)]
        public float characterRotateSpeed = 10f;

        private Dictionary<BattleAgentStatus, CharacterState> enumMapping = new Dictionary<BattleAgentStatus, CharacterState>();
        private Dictionary<string, GameObject> soldiers = new Dictionary<string, GameObject>();
		private Dictionary<string, ICharacterController> dyingSoldiers = new Dictionary<string, ICharacterController>();
        private Dictionary<string, ICharacterController> characterControllers = new Dictionary<string, ICharacterController>();
        private Vector3 __vector3 = new Vector3();

		private GameMessageRoute messageRoute = new GameMessageRoute();
        private BattleSimulator battleSimulator;
		private GameSceneEffectManager sceneEffectManager;

        private Dictionary<string, GameObject> tombStoneDic = new Dictionary<string, GameObject>();

        void Awake()
        {
           	enumMapping.Add(BattleAgentStatus.IDLE, CharacterState.Idle);
            enumMapping.Add(BattleAgentStatus.MOVE, CharacterState.Move);
            enumMapping.Add(BattleAgentStatus.ATTACK, CharacterState.Attack);
            enumMapping.Add(BattleAgentStatus.ATTACKED, CharacterState.Attacked);
            enumMapping.Add(BattleAgentStatus.DIE, CharacterState.Die);
        }
        
        void Start()
        {
			BattleProxy battleProxy = ApplicationFacade.Instance.RetrieveProxy(BattleProxy.NAME) as BattleProxy;
			this.battleSimulator = new BattleSimulator (this.messageRoute, battleProxy.GetCurrentLevel().Meta.battlefieldSize);
		}

		void OnDestroy ()
		{
			foreach (KeyValuePair<string, GameObject> kvPair in soldiers) 
			{
				ResourceFacade.Instance.Unload(kvPair.Value);
			}

			battleSimulator.Dispose ();
		}

        void Update()
        {

			messageRoute.Tick(BattleTime.deltaTime);

			Dictionary<string, ICharacterController> dyingAgents = new Dictionary<string, ICharacterController> ();

			Dictionary<string, BattleAgent> outputInfo = battleSimulator.agentsOutputInfo;

			if (outputInfo != null)
			{
				Dictionary<string, GameObject>.KeyCollection soldierNames = soldiers.Keys;
				
				foreach(string soldierName in soldierNames)
				{
					dyingAgents.Add(soldierName, characterControllers[soldierName]);
				}

				foreach(KeyValuePair<string, BattleAgent> kvPair in outputInfo)
				{
					BattleAgent soldierAgent = kvPair.Value;
					GameObject soldier = null;
					soldiers.TryGetValue(soldierAgent.Name, out soldier);
					
					if (soldier == null)
					{
						soldier = addSoldier(soldierAgent);
					}
					else
					{
						dyingAgents.Remove(soldierAgent.Name);
					}

					ICharacterController characterController = characterControllers[soldierAgent.Name];
					CharacterState currentCharacterState = characterController.GetAnimationState();

					if (CharacterState.Die != currentCharacterState)
					{
						Transform soldierTransform = soldier.transform;
						Vector3 transformPosition = soldierTransform.position;
						Vector2 position = soldierAgent.Position;
						
						if (transformPosition.x != position.x_ || transformPosition.z != position.y_)
						{
							transformPosition.x = position.x_;
							transformPosition.z = position.y_;
							transformPosition.y = GetTerrainInterpolatedHeight(position.x_, position.y_);
							
							soldierTransform.position = transformPosition;
						}
						
						Vector2 rotation = soldierAgent.Rotation;
						
						if (! Vector2.IsZero(rotation))
						{
							__vector3.x = rotation.x_;
							__vector3.z = rotation.y_;
							__vector3.y = 0;
							
							soldierTransform.rotation = Quaternion.Lerp(soldierTransform.rotation, Quaternion.LookRotation(__vector3), BattleTime.deltaTime * this.characterRotateSpeed);
						}
						
						CharacterState characterState = enumMapping[soldierAgent.CurrentStatus];
						
						if (characterState != currentCharacterState)
						{
							switch (characterState)
							{
							case CharacterState.Move:
								characterController.Move();
								break;
							case CharacterState.Attack:
								GameObject attackTarget;
								soldiers.TryGetValue(soldierAgent.AttackTarget, out attackTarget);
								
								if (attackTarget != null)
								{
									characterController.Attack(attackTarget, soldierAgent.AttackMode, soldierAgent.Metadata.attackFrequency);
								}
								break;
							case CharacterState.Attacked:
								characterController.Attacked();
								break;
							default:
								characterController.Idle(__vector3);
								break;
							}
						}
					}
				}
			}

			List<string> removeSoldierNames = new List<string>();
			
			foreach(KeyValuePair<string, ICharacterController> kvp in dyingSoldiers)
			{
				ICharacterController characterController = kvp.Value;
				CharacterState currentCharacterState = characterController.GetAnimationState();
				
				if (CharacterState.Destroy == currentCharacterState)
				{
					removeSoldierNames.Add(kvp.Key);
				}
			}

			foreach (KeyValuePair<string, ICharacterController> kvPair in dyingAgents) 
			{
				if (! dyingSoldiers.ContainsKey(kvPair.Key))
				{
					ICharacterController characterController = kvPair.Value;

					if (characterController != null)
					{
						dyingSoldiers[kvPair.Key] = characterController;
						characterController.Die();
					}


				}
			}
			
			foreach (string soldierName in removeSoldierNames)
			{
				RemoveSoldier(soldierName);
			}
			
			updateTombStone();
		}
		
		private void updateTombStone()
		{
			Dictionary<string, TroopDefinition> tombStones = battleSimulator.AllTombStones ();

			foreach(KeyValuePair<string, TroopDefinition> kvp in tombStones)
			{
				if (!tombStoneDic.ContainsKey(kvp.Value.name))
				{
					GameObject soldier = ResourceFacade.Instance.LoadPrefab(PrefabType.Tombstone, kvp.Value.tombStonePath);
					soldier.name = kvp.Value.name;
					
					soldier.transform.localRotation = MathUtil.LookRotationXZ(Camera.main.transform.position - transform.position);
                    soldier.transform.localEulerAngles += RandomUtil.onUnitSphere * 5;
                    
                    soldier.transform.position = MathUtil.ParseToVector3(kvp.Value.position);
                    tombStoneDic.Add(kvp.Value.name, soldier);
                }
            }

            List<string> removeTombStoneKey = new List<string>();

            foreach(KeyValuePair<string, GameObject> kvp in tombStoneDic)
            {
				if (!tombStones.ContainsKey(kvp.Key))
                {
                    removeTombStoneKey.Add(kvp.Key);
                }
            }

            foreach(string key in removeTombStoneKey)
            {
                Destroy(tombStoneDic[key]);
                tombStoneDic.Remove(key);
            }
        }

		public void Initialize ()
		{
			battleSimulator.Start ();
		}

        public GameMessageRoute MessageRoute
        {
            get
            {
                return this.messageRoute;
            }
        }

		public void ExecuteSkill (SkillMeta skillMeta, Vector3 position)
		{
			SkillVO skillVO = new SkillVO(skillMeta, Faction.Self);
			skillVO.centerPosition = MathUtil.ParseToVector2(position);

			battleSimulator.ExecuteSkill (skillVO);
		}
        
        public GameObject GetSoldierByName(string name)
        {
            GameObject soldier = null;
            soldiers.TryGetValue(name, out soldier);
            
            return soldier;
        }
        
        public CharacterAttackingContext GetCharacterAttackingContext(string name)
        {
            ICharacterController characterController = null;
            characterControllers.TryGetValue(name, out characterController);
            
            return characterController != null ? characterController.AttackingContext : null;
        }
        
        public float GetTerrainInterpolatedHeight(float x, float y)
        {
            return 0;
        }

		public BattleResult CheckGameState()
        {
			return battleSimulator.BattleAgentManager.CheckGameState();
        }

		public void AddSide(SideDefinition side)
        {
            battleSimulator.AddSide(side);
        }

        private GameObject addSoldier(BattleAgent battleAgent)
        {
            GameObject side = battleAgent.UnitController.Faction == Faction.Self ? playSide : enemySide;
            string tag = FactionUtil.ParseToTag(battleAgent.UnitController.Faction);

            GameObject troopObject = GameObject.Find(battleAgent.TroopName);
            if (troopObject == null)
            {
                troopObject = new GameObject();
                troopObject.layer = Layers.VIRTUAL;
                troopObject.name = battleAgent.TroopName;
//                troopObject.tag = battleAgent.TroopName;
                troopObject.transform.parent = side.transform;
                troopObject.transform.position = MathUtil.ParseToVector3(battleAgent.TroopPostion, GetTerrainInterpolatedHeight(battleAgent.TroopPostion.x(), battleAgent.TroopPostion.y()));
            }

            SoldierMeta metadata = battleAgent.Metadata;
            GameObject soldier = ResourceFacade.Instance.LoadPrefab(PrefabType.Soldier, metadata.prefabPath);
            
            ElementController elementController = soldier.GetComponent<ElementController>();
            if (elementController != null)
                elementController.enabled = false;
            
            soldier.name = battleAgent.Name;
            soldier.layer = metadata.layer;
            soldier.tag = tag;
            soldier.transform.parent = troopObject.transform;

            soldiers[battleAgent.Name] = soldier;
            characterControllers[battleAgent.Name] = soldier.GetComponent<ICharacterController>();

			return soldier;
        }

        public void RemoveSoldier(string soldierName)
        {
			ICharacterController soldier = dyingSoldiers[soldierName];
            Transform soldierTransform = soldier.transform;

            dyingSoldiers.Remove(soldierName);
			soldiers.Remove (soldierName);
            characterControllers.Remove(soldierName);

            Transform troop = soldierTransform.parent;
            soldierTransform.parent = null;

			ResourceFacade.Instance.Unload (soldier.gameObject);

            if (troop.childCount == 0)
            {
                troop.parent = null;
                Destroy(troop.gameObject);
            }
        }

        void OnDrawGizmos()
        {    
            if (battleSimulator == null)
            {
                return;
            }

            // 设置矩阵
            Matrix4x4 defaultMatrix = Gizmos.matrix;
                
            // 设置颜色
            Color defaultColor = Gizmos.color;

			if (battleSimulator.agentsOutputInfo != null)
			{
				foreach (KeyValuePair<string,BattleAgent> kvp in battleSimulator.agentsOutputInfo)
				{
					//                    Gizmos.color = agent.GizmosColorbattleAgentManager;
					//                    Vector3 center = new Vector3(agent.position_.x(), 0, agent.position_.y());
					//                    drawCircle(center, 4.0f);
					BattleAgent agent = kvp.Value;
					DebugDrawUtil.drawCircle(MathUtil.ParseToVector3(agent.Position), agent.Metadata.boundsRadius, agent.m_color);
				}
			}

			// 恢复默认颜色
            Gizmos.color = defaultColor;
                
            // 恢复默认矩阵
            Gizmos.matrix = defaultMatrix;
        }
    }
}
