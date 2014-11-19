using System;
using System.Xml;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Battle
{
    public class BattleAgentImage
    {
        
        public BattleAgent agent;
        public float distance;
        public List<String> attackers;
    }

    internal class Tile : LinkedList<BattleAgent>
    {
    }

    internal class DistanceSnapshot
    {
        
        public float distance;
        public float timestamp;
        
        public DistanceSnapshot(float distance, float timestamp)
        {
            this.distance = distance;
            this.timestamp = timestamp;
        }
    }

    internal class BattleAgentIndex
    {
        public BattleAgent agent;
        public float boundsRadius;
        public Faction faction;
        public string troopName;
        public float movementTimestamp;

        public BattleAgentIndex(BattleAgent agent, string troopName)
        {
            this.agent = agent;
            this.boundsRadius = agent.UnitController.Meta.boundsRadius;
            this.faction = agent.UnitController.Faction;
            this.troopName = troopName;
        }
    }

    internal class PendingAttackResult
    {
        private BattleAgent attacker;
        private BattleAgent receiver;
        private Vector2 searchPoint;
        private float delayTime;
        
        public PendingAttackResult(BattleAgent attacker, BattleAgent receiver, float delayTime, Vector2 searchPoint)
        {
            this.attacker = attacker;
            this.receiver = receiver;
            this.delayTime = delayTime;
            this.searchPoint = searchPoint;
        }
        
        public bool CheckDueTime(float elapsedTime)
        {
            this.delayTime -= elapsedTime;
            
            return this.delayTime <= 0;
        }
        
        public void CalculateResult(BattleValueCalculator battleValueCalculator, BattleAgentManager agentManager, GameMessageRoute messageRoute)
        {
            Dictionary<String, System.Object> data = new Dictionary<String, System.Object>();
            data.Add(AttackParamKey.CURRENT_ENEMY, attacker.Name);
            
            if (Vector2.IsZero(searchPoint))
            {
                data.Add(MessageParamKey.TARGET, this.receiver);
                messageRoute.SendGameMessage(new MessageContext(Messages.ATTACK_OCCUR, 0, data));
                
                battleValueCalculator.Attack(attacker.UnitController.CreateAttackValue(), receiver.UnitController);
            }
            else
            {
                Faction faction = FactionUtil.Revert(attacker.UnitController.Faction);
                List<BattleAgent> searchResultList = agentManager.SearchAroundPoint(searchPoint, attacker.UnitController.Meta.bulletMeta.attackRadius, faction); 
                
                data.Add(MessageParamKey.TARGETS, searchResultList);
                messageRoute.SendGameMessage(new MessageContext(Messages.ATTACK_OCCUR, 0, data));
                
                foreach (BattleAgent target in searchResultList)
                {
                    battleValueCalculator.Attack(attacker.UnitController.CreateAttackValue(), target.UnitController);
                }
            }
        }
    }

    public class BattleAgentManager
    {
		public const string SoldierBehaviorTreeName = "SoliderBehaviorTree";
		public const string TroopBehaviorTreeName = "TroopBehaviorTree";

        private static Type[] analyzerDefinitions = new Type[] {
            typeof(AttackLockedSensorAnalyzer),
            typeof(AttackUnlockedSensorAnalyzer),
            typeof(AttackOccurSensorAnalyzer),
            typeof(EnemyDetectSensorAnalyzer)
        };
        public Dictionary<Faction, int> hpSumDic = new Dictionary<Faction, int>();
        public float bigCharacterRadius = 1.0f;
        public float searchGridSize = 1.0f;
		public Dictionary<string, TroopDefinition> tombStoneDic = new Dictionary<string, TroopDefinition>();
	
		private int battlefieldSize;
        private List<List<Tile>> searchGrid;
        private Dictionary<String, Tile> searchGridSnapshot = new Dictionary<String, Tile>();
        private float bySearchGridSize;
        private Dictionary<String, ISearchOrderLogic> orderLogicIndex = new Dictionary<String, ISearchOrderLogic>();
        private ISearchOrderLogic[] orderLogicInstances = new ISearchOrderLogic[] { new AttackerAscDistanceAsc(), new AttackerAscDistanceDesc(), 
            new AttackerDescDistanceAsc(), new AttackerDescDistanceDesc(), new DistanceAscAttackerAsc(), new DistanceAscAttackerDesc(), 
            new DistanceDescAttackerAsc(), new DistanceDescAttackerDesc() };
        private Dictionary<String, BattleAgentIndex> characterIndex = new Dictionary<string, BattleAgentIndex>();
        private Dictionary<String, Dictionary<String, BattleAgentIndex>> troopIndex = new Dictionary<String, Dictionary<String, BattleAgentIndex>>();
        private Dictionary<String, TroopAgent> troopAgentIndex = new Dictionary<String, TroopAgent>();
        private Dictionary<Faction, List<TroopAgent>> troopFactionIndex = new Dictionary<Faction, List<TroopAgent>>();
        private Dictionary<String, Vector2> troopLastSoldierPositionRecord = new Dictionary<String, Vector2>();
        private Dictionary<String, DistanceSnapshot> distanceSnapshots = new Dictionary<String, DistanceSnapshot>();
        private Dictionary<String, List<String>> attackerSnapshots = new Dictionary<String, List<String>>();
        private Dictionary<String, String> attackingSnapshots = new Dictionary<String, String>();
        private Dictionary<Faction, List<BattleAgentIndex>> bigCharacterIndex = new Dictionary<Faction, List<BattleAgentIndex>>();
        private IList<PendingAttackResult> pendingAttackResults = new List<PendingAttackResult>();

		private GameMessageRoute messageRoute = new GameMessageRoute();
		private GameSenseRoute senseRoute = new GameSenseRoute();
		private BattleValueCalculator battleValueCalculator = new BattleValueCalculator();

		private GameMessageRoute outerMessageRoute;
        private SkillControllerContainer skillControllerContainer;

        private Dictionary<string, SideAgent> sideDic = new Dictionary<string, SideAgent>();
		private Dictionary<string, XmlDocument> behaviorTreeCache = new Dictionary<string, XmlDocument> ();
  
        private BattleSimulator battleSimulator;

		public BattleAgentManager(GameMessageRoute outerMessageRoute, BattleSimulator battleSimulator, int battlefieldSize)
        {
			this.outerMessageRoute = outerMessageRoute;
			this.battleSimulator = battleSimulator;
			this.battlefieldSize = battlefieldSize;

			this.skillControllerContainer = new SkillControllerContainer(this, outerMessageRoute);

            foreach (Type analyzerInstance in analyzerDefinitions)
            {
                ISensorAnalyzer sensorAnalyzer = (ISensorAnalyzer)Activator.CreateInstance(analyzerInstance, new object[] { this });
                senseRoute.RegisterSensorAnalyzer(sensorAnalyzer);
            }

            messageRoute.RegistryMessageDrivenBehaviour(this.senseRoute);

            foreach (ISearchOrderLogic logic in orderLogicInstances)
            {
                orderLogicIndex.Add(logic.GetSearchOrderPairs(), logic);
            }

			int gridWidth = (int)Mathf.Ceil(this.battlefieldSize / this.searchGridSize);
			int gridHeight = (int)Mathf.Ceil(this.battlefieldSize / this.searchGridSize);
            
            this.searchGrid = new List<List<Tile>>(gridWidth);
            this.bySearchGridSize = 1.0f / this.searchGridSize;
            
            for (int w = 0; w < gridWidth; w++)
            {
                List<Tile> tiles = new List<Tile>(gridHeight);
                searchGrid.Add(tiles);
                
                for (int h = 0; h < gridHeight; h++)
                {
                    tiles.Add(new Tile());
                }
            }
        }

		public void Tick(float deltaTime)
        {
			messageRoute.Tick (deltaTime);

            for (int i = 0; i < pendingAttackResults.Count; i++)
            {
                PendingAttackResult attackResult = pendingAttackResults[i];

				if (attackResult.CheckDueTime(deltaTime))
                {
                    pendingAttackResults.RemoveAt(i--);
                    attackResult.CalculateResult(this.battleValueCalculator, this, this.messageRoute);
                }
            }
        }

        public void Destroy()
        {
            messageRoute.UnregistryMessageDrivenBehaviour(this.senseRoute);
        }

        public BattleSimulator BattleSimulator
		{
            get
            {
                return battleSimulator;
            }
        }

        public GameMessageRoute MessageRoute
        {
            get
            {
                return this.messageRoute;
            }
        }

        public BattleValueCalculator ValueCalculator
        {
            get
            {
                return this.battleValueCalculator;
            }
        }

        public BattleAgent GetCharacterByName(String name)
        {
            BattleAgentIndex agentIndex;
            characterIndex.TryGetValue(name, out agentIndex);
            
            return agentIndex != null ? agentIndex.agent : null;
        }
        
        public float GetCharacterRadiusByName(String name)
        {
            BattleAgentIndex agentIndex;
            characterIndex.TryGetValue(name, out agentIndex);
            
            return agentIndex != null ? agentIndex.boundsRadius : 0;
        }
        
        public List<String> GetAttackersByName(String name)
        {
            List<String> attackers = null;
            attackerSnapshots.TryGetValue(name, out attackers);

            return attackers;
        }

        public float GetDistanceForTarget(String soldierNameA, String soldierNameB)
        {
            String snapshotKey = soldierNameA + soldierNameB;
            DistanceSnapshot snapshot = null;
            distanceSnapshots.TryGetValue(snapshotKey, out snapshot);
            
            if (snapshot == null)
            {
                snapshot = new DistanceSnapshot(float.MaxValue, -1.0f);
                distanceSnapshots.Add(snapshotKey, snapshot);
                distanceSnapshots.Add(soldierNameB + soldierNameA, snapshot);
            }
            
            BattleAgentIndex agentIndexA = null; 
            BattleAgentIndex agentIndexB = null;
            characterIndex.TryGetValue(soldierNameA, out agentIndexA);
            characterIndex.TryGetValue(soldierNameB, out agentIndexB);
            
            if (agentIndexA == null || agentIndexB == null)
            {
                return float.MaxValue;
            }
            
            if (snapshot.timestamp < agentIndexA.movementTimestamp || snapshot.timestamp < agentIndexB.movementTimestamp)
            {
                Vector2 positionA = agentIndexA.agent.Position;
                Vector2 positionB = agentIndexB.agent.Position;
                
                snapshot.timestamp = BattleTime.time;
                snapshot.distance 
                    = Mathf.Max(0, Mathf.Sqrt(Mathf.Pow(positionA.x_ - positionB.x_, 2.0f) + Mathf.Pow(positionA.y_ - positionB.y_, 2.0f)) 
                    - (agentIndexA.boundsRadius + agentIndexB.boundsRadius));
            }
            
            return snapshot.distance;
        }

        public int SearchAroundByDistance(List<BattleAgentImage> searchResult, BattleAgent soldier, Faction faction, SearchOrder searchOrder,
                                           float maxSearchRadious, float minSearchRadious = 0, String targetTroopName = null, int maxResult = -1)
        {
            if (maxResult < 0) 
                maxResult = searchResult.Count;
            else 
                maxResult = Mathf.Min(maxResult, searchResult.Count);
            
            int searchResultLength = 0;
            
            String soldierName = soldier.Name;
            ISearchOrderLogic searchOrderLogic = maxResult == 1 || searchOrder == null ? null : orderLogicIndex[searchOrder.ToString()];
            
            BattleAgentIndex agentIndex = null;
            characterIndex.TryGetValue(soldierName, out agentIndex);

			float halfBattlefieldSize = this.battlefieldSize * 0.5f;
            
            Vector2 soldierPosition = agentIndex.agent.Position;
            int searchSize = (int)Mathf.Ceil((maxSearchRadious + agentIndex.boundsRadius) * this.bySearchGridSize);
			int gridX = (int)Mathf.Floor((soldierPosition.x_ + halfBattlefieldSize) * this.bySearchGridSize);
			int gridY = (int)Mathf.Floor((soldierPosition.y_ + halfBattlefieldSize) * this.bySearchGridSize);
            
            Dictionary<BattleAgent, bool> foundRecord = new Dictionary<BattleAgent, bool>();
            String agentName;
            int i;
            
            for (i = 0; i <= searchSize; i++)
            {
                int maxX = Mathf.Min(searchGrid.Count - 1, gridX + i);
                
                for (int x = Mathf.Max(0, gridX - i); x <= maxX; x++)
                {
                    List<Tile> tiles = searchGrid[x];
                    int maxY = Mathf.Min(tiles.Count - 1, gridY + i);
                    
                    for (int y = Mathf.Max(0, gridY - i); y <= maxY;)
                    {
                        Tile tile = tiles[y];
                        
                        foreach (BattleAgent battleAgent in tile)
                        {
                            if (equalsCompareForAgentFaction(battleAgent, faction))
                            {
                                agentName = battleAgent.Name;
                                foundRecord[battleAgent] = true;
                                
                                if (agentName != soldierName)
                                {
                                    float distance = GetDistanceForTarget(agentName, soldierName);
                                    
                                    if (distance <= maxSearchRadious && distance >= minSearchRadious)
                                    {
                                        List<String> attackers = GetAttackersByName(agentName);
                                        BattleAgentImage agentImage;
                                        
                                        if (searchOrderLogic != null)
                                        {
                                            agentImage = searchOrderLogic.Order(searchResult, searchResultLength, distance, attackers != null ? attackers.Count : 0);
                                        }
                                        else
                                        {
                                            agentImage = searchResult[searchResultLength];
                                        }
                                        
                                        agentImage.agent = battleAgent;
                                        agentImage.distance = distance;
                                        agentImage.attackers = attackers;
                                        
                                        searchResultLength++;
                                        
                                        if (maxResult > 0 && searchResultLength >= maxResult)
                                        {
                                            return searchResultLength;
                                        }
                                    }
                                }
                            }
                        }
                        
                        y = Mathf.Abs(gridX - x) < i && y < maxY ? maxY : y + 1;
                    }
                }
            }
            
            List<BattleAgentIndex> bigCharacters;
            bigCharacterIndex.TryGetValue(faction, out bigCharacters);
            
            BattleAgent currentAgent;
            
            if (bigCharacters != null)
            {
                for (i = 0; i < bigCharacters.Count; i++)
                {
                    agentIndex = bigCharacters[i];
                    currentAgent = agentIndex.agent;
                    agentName = currentAgent.Name;
                    
                    if (!foundRecord.ContainsKey(currentAgent) && !ReferenceEquals(currentAgent, soldier))
                    {
                        float distance = GetDistanceForTarget(agentName, soldierName);
                        
                        if (distance <= maxSearchRadious && distance >= minSearchRadious)
                        {
                            List<String> attackers = GetAttackersByName(agentName);
                            BattleAgentImage agentImage;
                            
                            if (searchOrderLogic != null)
                            {
                                agentImage = searchOrderLogic.Order(searchResult, searchResultLength, distance, attackers != null ? attackers.Count : 0);
                            }
                            else
                            {
                                agentImage = searchResult[searchResultLength];
                            }
                            
                            agentImage.agent = currentAgent;
                            agentImage.distance = distance;
                            agentImage.attackers = attackers;
                            
                            searchResultLength++;
                            
                            if (maxResult > 0 && searchResultLength >= maxResult)
                            {
                                return searchResultLength;
                            }
                        }
                    }
                }
            }
            
            return searchResultLength;
        }

        public bool DetectAvailablePoint(Vector2 point, float radius, int layer)
        {
			float halfBattlefieldSize = this.battlefieldSize * 0.5f;

            int searchSize = (int)Mathf.Ceil(radius * this.bySearchGridSize);
			int gridX = (int)Mathf.Floor((point.x_ + halfBattlefieldSize) * this.bySearchGridSize);
			int gridY = (int)Mathf.Floor((point.y_ + halfBattlefieldSize) * this.bySearchGridSize);
            
            int maxX = Mathf.Min(searchGrid.Count - 1, gridX + searchSize);
            int inRangeCount = 0;
            
            for (int x = Mathf.Max(0, gridX - searchSize); x <= maxX; x++)
            {
                List<Tile> tiles = searchGrid[x];
                int maxY = Mathf.Min(tiles.Count - 1, gridY + searchSize);
                
                for (int y = Mathf.Max(0, gridY - searchSize); y <= maxY; y++)
                {
                    Tile tile = tiles[y];
                    
                    foreach (BattleAgent battleAgent in tile)
                    {
                        if (equalsCompareForAgentLayer(battleAgent, layer))
                        {
                            BattleAgentIndex agentIndex = null;
                            characterIndex.TryGetValue(battleAgent.Name, out agentIndex);
                            
                            if (agentIndex != null)
                            {
                                Vector2 position = agentIndex.agent.Position;
                                float sqrDistance = Mathf.Pow(point.x_ - position.x_, 2.0f) + Mathf.Pow(point.y_ - position.y_, 2.0f);
                                
                                if (Mathf.Pow(radius + agentIndex.boundsRadius, 2.0f) > sqrDistance && ++inRangeCount > 1)
                                {
                                    return false;
                                }
                            }
                        }
                    }
                }
            }
            
            return true;
        }
    
        public List<BattleAgent> SearchAroundPoint(Vector2 point, float radius, Faction faction = Faction.All, int maxCount = int.MaxValue)
        {
            List<BattleAgent> selectedGroup = new List<BattleAgent>();
            Vector3 center = MathUtil.ParseToVector3(point);
            float SOLDIER_SEARCH_RADIUS = radius;
            Vector3[] selectionPoints = new Vector3[]{
                new Vector3(-SOLDIER_SEARCH_RADIUS, 0, -SOLDIER_SEARCH_RADIUS) + center,
                new Vector3(-SOLDIER_SEARCH_RADIUS, 0, SOLDIER_SEARCH_RADIUS) + center,
                new Vector3(SOLDIER_SEARCH_RADIUS, 0, SOLDIER_SEARCH_RADIUS) + center,
                new Vector3(SOLDIER_SEARCH_RADIUS, 0, -SOLDIER_SEARCH_RADIUS) + center
                };
           
            int[] selectedAgents = FameManager.QueryAgents(selectionPoints);
             
            foreach (int i in selectedAgents)
            {
                BattleAgent unit = FameManager.GetFlockMember(i);
            
                if (Faction.All == faction || equalsCompareForAgentFaction(unit, faction))
                {
                    selectedGroup.Add(unit);

                    if (selectedGroup.Count >= maxCount)
                    {
                        return selectedGroup;
                    }
                }
            }

            return selectedGroup;
        }

//        public List<BattleAgent> SearchAroundPoint(Vector2 point, float radius, Faction faction = Faction.All, int maxCount = int.MaxValue)
//        {
//            List<BattleAgent> searchResult = new List<BattleAgent>();
//            float sqrRadius = Mathf.Pow(radius, 2.0f);
//            
//            int searchSize = (int)Mathf.Ceil(radius * this.bySearchGridSize);
//            int gridX = (int)Mathf.Floor(point.x_ * this.bySearchGridSize);
//            int gridY = (int)Mathf.Floor(point.y_ * this.bySearchGridSize);
//            
//            for (int i = 0; i < searchSize; i++)
//            {
//                int maxX = Mathf.Min(searchGrid.Count - 1, gridX + i);
//                
//                for (int x = Mathf.Max(0, gridX - i); x <= maxX; x++)
//                {
//                    List<Tile> tiles = searchGrid[x];
//                    int maxY = Mathf.Min(tiles.Count - 1, gridY + i);
//                    
//                    for (int y = Mathf.Max(0, gridY - i); y <= maxY;)
//                    {
//                        Tile tile = tiles[y];
//                        
//                        foreach (BattleAgent battleAgent in tile)
//                        {
//                            if (Faction.All == faction || equalsCompareForAgentFaction(battleAgent, faction))
//                            {
//                                if (i < searchSize - 1)
//                                {
//                                    searchResult.Add(battleAgent);
//                                    
//                                    if (searchResult.Count >= maxCount)
//                                    {
//                                        return searchResult;
//                                    }
//                                }
//                                else            // the outermost circle
//                                {
//                                    Vector2 position = battleAgent.Position;
//                                    float sqrDistance = Mathf.Pow(point.x_ - position.x_, 2.0f) + Mathf.Pow(point.y_ - position.y_, 2.0f);
//                                    
//                                    if (sqrRadius > sqrDistance)
//                                    {
//                                        searchResult.Add(battleAgent);
//                                        
//                                        if (searchResult.Count >= maxCount)
//                                        {
//                                            return searchResult;
//                                        }
//                                    }
//                                }
//                            }
//                        }
//                        
//                        y = Mathf.Abs(gridX - x) < i && y < maxY ? maxY : y + 1;
//                    }
//                }
//            }
//            
//            return searchResult;
//        }

        public List<String> LockForAttackTarget(BattleAgent soldier, BattleAgent targetSoldier)
        {
            String soldierName = soldier.Name;
            
            UnlockAttackTarget(soldier);
            
            attackingSnapshots.Add(soldierName, targetSoldier.Name);
            
            List<String> attackers = null;
            attackerSnapshots.TryGetValue(targetSoldier.Name, out attackers);
            
            if (attackers == null)
            {
                attackers = new List<String>();
                attackers.Add(soldierName);
            }
            else
            {
                attackers.Add(soldierName);
            }
            
            Dictionary<String, System.Object> data = new Dictionary<String, System.Object>();
            data.Add(MessageParamKey.TARGET, targetSoldier);
            data.Add(AttackParamKey.LOCKED_ATTACKERS, attackers);
            data.Add(AttackParamKey.CURRENT_ENEMY, soldierName);
            messageRoute.SendGameMessage(new MessageContext(Messages.ATTACK_LOCKED, 0, data));
            
            return attackers;
        }
        
        public void UnlockAttackTarget(BattleAgent soldier)
        {
            String soldierName = soldier.Name;
            
            String attackTargetName = null;
            attackingSnapshots.TryGetValue(soldierName, out attackTargetName);
            
            if (attackTargetName != null)
            {
                attackingSnapshots.Remove(soldierName);
                
                List<String> attackers = null;
                attackerSnapshots.TryGetValue(attackTargetName, out attackers);
                
                if (attackers != null)
                {
                    attackers.Remove(soldierName);
                }
            }
        }

        public void DoAttack(BattleAgent soldier, BattleAgent attackTarget, BulletMeta bulletMeta)
        {
            if (attackTarget != null)
            {
                PendingAttackResult pendingAttackResult;
                
                if (bulletMeta != null)
                {
                    float distance = Vector2.Distance(soldier.Position, attackTarget.Position);
                    float buttleFlyingTime = 1.1f * distance / ((bulletMeta.minSpeed + bulletMeta.maxSpeed) * 0.5f);                        // 1.1 for time adjustment
                    
                    pendingAttackResult = new PendingAttackResult(soldier, attackTarget, buttleFlyingTime, bulletMeta.isAreaAttack ? attackTarget.Position : Vector2.zero);

                    Dictionary<string, object> messageData = new Dictionary<string, object>();
                    messageData.Add(MessageParamKey.METAID, bulletMeta.id);
                    messageData.Add(MessageParamKey.FACTION, soldier.UnitController.Faction);
                    messageData.Add(MessageParamKey.CURRENT_SOLDIER, soldier.Name);
                    messageData.Add(MessageParamKey.TARGET, attackTarget.Name);

					if (bulletMeta.isAreaAttack)
					{
						List<BattleAgent> searchResultList = SearchAroundPoint(attackTarget.Position, bulletMeta.attackRadius, attackTarget.UnitController.Faction, 5); 
						List<SoldierMeta> metaList = new List<SoldierMeta> (searchResultList.Count);
						
						for (int i = 0; i < searchResultList.Count; i++)
						{
							metaList.Add(searchResultList[i].Metadata);
						}

						messageData.Add(MessageParamKey.DETECTED_ENEMYS, metaList);
					}

					outerMessageRoute.SendGameMessage(new MessageContext(Messages.SHOW_BULLET_EFFECT, 0, messageData));
                }
                else
                {
                    pendingAttackResult = new PendingAttackResult(soldier, attackTarget, 0, Vector2.zero);
                }
                
                pendingAttackResults.Add(pendingAttackResult);
            }
        }

		public void ExecuteSkill (SkillVO skill)
		{
			skillControllerContainer.ExecuteSkill (skill);

            if (skill.Meta.metaName == "Resurrection")
            {
                List<TroopDefinition> result = GetTombStonesByFaction(Faction.Self);

                foreach(TroopDefinition troop in result)
                {
                    if (Vector2.Distance(skill.centerPosition, troop.position) < 2)
                    {
                        RemoveTombStone(troop.name);
                        troop.name = troop.name + "_Resurrection" + BattleTime.time;
                        TroopAgent troopTarget = AddTroop(troop);
                        battleSimulator.AddTroop(troopTarget);
                        return;
                    }
                }
            }
		}

        public TroopAgent GetTroopByName(String name)
        {
            TroopAgent troop = null;
            troopAgentIndex.TryGetValue(name, out troop);
            
            return troop;
        }
        
        public List<TroopAgent> GetTroopsByFaction(Faction faction)
        {
            List<TroopAgent> troops = null;
            troopFactionIndex.TryGetValue(faction, out troops);
            
            return troops;
        }

        public SideAgent AddSide(SideDefinition side)
        {
			XmlDocument behaviorTree = (side.behaviorTreeName != null ? GetBehaviorTree(side.behaviorTreeName) : null);

			SideAgent sideAgent = new SideAgent(side.skillMetas, side.faction, side.sideName, this, behaviorTree, this.skillControllerContainer);
            sideDic.Add(sideAgent.sideName, sideAgent);
         
            foreach(TroopDefinition definition in side.troops)
            {
                AddTroop(definition);
//                TroopAgent troop = AddTroop(definition);
//                sideAgent.AddTroop(troop);
            }

            return sideAgent;
        } 

        public TroopAgent AddTroop(TroopDefinition troopDefinition)
        {
			XmlDocument behaviorTree = GetBehaviorTree(TroopBehaviorTreeName);

			TroopAgent troopAgent = new TroopAgent(troopDefinition, this, behaviorTree);

            if (!sideDic[troopAgent.SideName].troopDictionary.ContainsKey(troopAgent.Name))
            {
                sideDic[troopAgent.SideName].troopDictionary.Add(troopAgent.Name, troopAgent);
            }

            troopAgent.InitFlock();

			behaviorTree = GetBehaviorTree(SoldierBehaviorTreeName);

            for (int i = 0; i < troopDefinition.soliderAnchors.Length; i++)
            {
                Vector2 position = troopDefinition.position + troopDefinition.soliderAnchors[i];
                UnitController unitController = UnitControllerFactory.Singleton.CreateUnitController(troopDefinition.soldlerMeta, troopDefinition.faction, skillControllerContainer);
				BattleAgent agent = new BattleAgent(i + "_" + troopAgent.Name, position, troopAgent.Name, unitController, this, behaviorTree);
                int agentID = FameManager.CreateAgent(FlockType.Ground, MathUtil.ParseToVector3(position));
                agent.Init(agentID, troopAgent.FlockID, FlockType.Ground);
                troopAgent.AddSoldier(agent);
            }

            List<TroopAgent> troops = null;
            troopFactionIndex.TryGetValue(troopDefinition.faction, out troops);

            if (troops == null)
            {
                troops = new List<TroopAgent>();
                troopFactionIndex.Add(troopDefinition.faction, troops);
            }

            troops.Add(troopAgent);       

            troopAgentIndex.Add(troopAgent.Name, troopAgent);
            
            Dictionary<String, BattleAgentIndex> troopCharacters = new Dictionary<string, BattleAgentIndex>();
            troopIndex.Add(troopAgent.Name, troopCharacters);
            
            foreach (KeyValuePair<string, BattleAgent> kvp in troopAgent.Soldiers)
            {
                BattleAgent soldierAgent = kvp.Value;
                BattleAgentIndex soldierIndex = new BattleAgentIndex(soldierAgent, troopAgent.Name);
                
                characterIndex.Add(soldierAgent.Name, soldierIndex);
                troopCharacters.Add(soldierAgent.Name, soldierIndex);
                
                if (soldierIndex.boundsRadius > this.bigCharacterRadius)
                {
                    AddToBigCharacterIndex(soldierIndex);
                }
                
                senseRoute.RegistryGameSensor(soldierAgent);
            }
            
            foreach (BattleAgentIndex soldierIndex in troopCharacters.Values)
            {
                UpdatePosition(soldierIndex.agent, soldierIndex.agent.Position);
            }

            return troopAgent;
        }
        
        public Vector2 RemoveTroop(TroopAgent troopAgent, Faction faction)
        {
            FameManager.RemoveFlock(troopAgent.FlockID);

            String troopName = troopAgent.Name;

            TroopDefinition defination = troopAgent.OriginalTroopDefinition;
            defination.position = troopLastSoldierPositionRecord[troopName];
            AddTombStone(defination);
            
            Dictionary<String, BattleAgentIndex> troopCharacters = null;
            troopIndex.TryGetValue(troopName, out troopCharacters);
            troopIndex.Remove(troopName);
            
            troopAgentIndex.Remove(troopName);
            troopFactionIndex[faction].Remove(troopAgent);

            troopAgent.RemoveAllSoldier();
                
            troopAgent = null;
            return troopLastSoldierPositionRecord[troopName];
        }

        private void AddTombStone(TroopDefinition troopDefinition)
        {
            tombStoneDic.Add(troopDefinition.name, troopDefinition);
        }

        public void RemoveTombStone(string tombStoneName)
        {
            tombStoneDic.Remove(tombStoneName);
        }

        public List<TroopDefinition> GetTombStonesByFaction(Faction faction)
        {
            List<TroopDefinition> result = new List<TroopDefinition>();
            foreach(KeyValuePair<string, TroopDefinition> kvp in tombStoneDic)
            {
                if (kvp.Value.faction == faction)
                {
                    result.Add(kvp.Value);
                }
            }
            return result;
        }

        public List<TroopDefinition> GetTombStonesByRadius(Vector2 center, float radious)
        {
            List<TroopDefinition> result = new List<TroopDefinition>();
            foreach(KeyValuePair<string, TroopDefinition> kvp in tombStoneDic)
            {
                if (Vector2.Distance(center, kvp.Value.position) <= radious)
                {
                    result.Add(kvp.Value);
                }
            }

            return result;
        }

        public void RemoveSoldier(BattleAgent soldier)
        {
            FameManager.RemoveAgent(soldier.AgentID);

            senseRoute.UnregistryGameSensor(soldier);

            String soldierName = soldier.Name;
            BattleAgentIndex agentIndex = characterIndex[soldierName];

            RemoveFromBigCharacterIndex(agentIndex);
            
            Dictionary<String, BattleAgentIndex> troopCharacters = null;
            troopIndex.TryGetValue(agentIndex.troopName, out troopCharacters);
            
            characterIndex.Remove(soldierName);
            troopCharacters.Remove(soldierName);
            troopLastSoldierPositionRecord[agentIndex.troopName] = soldier.Position;
            
            List<String> attackers = null;
            attackerSnapshots.TryGetValue(soldierName, out attackers);
            
            if (attackers != null)
            {
                foreach (String attacker in attackers)
                {
                    attackingSnapshots.Remove(attacker);
                }
                
                attackerSnapshots.Remove(soldierName);
            }
            
            UnlockAttackTarget(soldier);
            
            foreach (String sName in characterIndex.Keys)
            {
                distanceSnapshots.Remove(sName + soldierName);
                distanceSnapshots.Remove(soldierName + sName);
            }
            
            Tile searchTile = null;
            searchGridSnapshot.TryGetValue(soldierName, out searchTile);
            
            if (searchTile != null)
            {
                searchGridSnapshot.Remove(soldierName);
                searchTile.Remove(soldier);
            }
            soldier = null;
        }

        public void UpdatePosition(BattleAgent soldier, Vector2 position)
        {
            String soldierName = soldier.Name;  
            Tile searchTile = null;
            searchGridSnapshot.TryGetValue(soldierName, out searchTile);

			float halfBattlefieldSize = this.battlefieldSize * 0.5f;
            
			int gridX = (int)Mathf.Floor((position.x_ + halfBattlefieldSize) * this.bySearchGridSize);
			int gridY = (int)Mathf.Floor((position.y_ + halfBattlefieldSize) * this.bySearchGridSize);
            Tile newSearchTile = searchGrid[gridX][gridY];
            
            if (!ReferenceEquals(searchTile, newSearchTile))
            {
                if (searchTile != null)
                {
                    searchTile.Remove(soldier);
                }
                
                newSearchTile.AddFirst(soldier);
                searchGridSnapshot[soldierName] = newSearchTile;
            }

            BattleAgentIndex agentIndex;
            characterIndex.TryGetValue(soldierName, out agentIndex);

            if (agentIndex != null)
            {
                agentIndex.movementTimestamp = BattleTime.time;
            }
        }

		public BattleResult CheckGameState()
        {
            int selfTroopCount = GetTroopsByFaction(Faction.Self).Count;
            int opponentTroopCount = GetTroopsByFaction(Faction.Opponent).Count;

			if (selfTroopCount > 0 && opponentTroopCount > 0) 
			{
				return BattleResult.NotOver;
			} 
			else 
			{
				return selfTroopCount > 0 ? BattleResult.Win : BattleResult.Lose;
			}
        }

        private void AddToBigCharacterIndex(BattleAgentIndex soldierIndex)
        {
            List<BattleAgentIndex> bigCharacters = null;
            bigCharacterIndex.TryGetValue(soldierIndex.faction, out bigCharacters);
            
            if (bigCharacters == null)
            {
                bigCharacters = new List<BattleAgentIndex>();
                bigCharacterIndex.Add(soldierIndex.faction, bigCharacters);
            }
            
            for (int i = 0; i < bigCharacters.Count; i++)
            {
                if (soldierIndex.boundsRadius >= bigCharacters[i].boundsRadius)
                {
                    bigCharacters.Insert(i, soldierIndex);
                    
                    return;
                }
            }
            
            bigCharacters.Add(soldierIndex);
        }
        
        private void RemoveFromBigCharacterIndex(BattleAgentIndex soldierIndex)
        {
            List<BattleAgentIndex> bigCharacters;
            bigCharacterIndex.TryGetValue(soldierIndex.faction, out bigCharacters);
            if (bigCharacters != null)
                bigCharacters.Remove(soldierIndex);
        }

        private static bool equalsCompareForAgentFaction(BattleAgent agent, Faction faction)
        {
            return agent.UnitController.Faction == faction;
        }

        private static bool equalsCompareForAgentLayer(BattleAgent agent, int layer)
        {
            return agent.UnitController.Meta.layer == layer;
        }

		private XmlDocument GetBehaviorTree (string behaviorName)
		{
			XmlDocument behaviorTree = null;
			behaviorTreeCache.TryGetValue (behaviorName, out behaviorTree);

			if (behaviorTree == null)
			{
				behaviorTree = BehaviorTreeParser.loadXml(behaviorName);
				behaviorTreeCache.Add(behaviorName, behaviorTree);
			}

			return behaviorTree;
		}

		internal void CalculateInitialHPSum()
		{
			hpSumDic[Faction.Self] = 0;
			hpSumDic[Faction.Opponent] = 0;
			
			foreach (BattleAgentIndex agentIndex in characterIndex.Values)
			{
				Faction faction = agentIndex.agent.UnitController.Faction;
				hpSumDic[faction] += agentIndex.agent.UnitController.HP;
			}
		}
		
		internal void CalculateHPSum()
		{
			int selfHPSum = 0;
			int opponentHPSum = 0;
			
			foreach (BattleAgentIndex agentIndex in characterIndex.Values)
			{
				Faction faction = agentIndex.agent.UnitController.Faction;
				
				if (faction == Faction.Self)
					selfHPSum += agentIndex.agent.UnitController.HP;
				else
					opponentHPSum += agentIndex.agent.UnitController.HP;
			}
			
			Dictionary<string, object> data = new Dictionary<string, object>();
			data[Faction.Self.ToString()] = 1f * selfHPSum / hpSumDic[Faction.Self];
			data[Faction.Opponent.ToString()] = 1f * opponentHPSum / hpSumDic[Faction.Opponent];
			outerMessageRoute.SendGameMessage(new MessageContext(Messages.UPDATE_BATTLE_FORCE, 0, data));
		}
    }
}
