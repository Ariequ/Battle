using System;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

namespace Battle
{
    public class BattleSimulator
    {
		public static float deltaTime;
		private static float lastUpdateTime;

		public volatile Dictionary<string, BattleAgent> agentsOutputInfo;
		private bool agentsOutputInfoDirty;

		private BattleAgentManager battleAgentManager;

        private Dictionary<string, SideAgent> sideDictionary = new Dictionary<string, SideAgent>();
		private Dictionary<string, BattleAgent> agentDictionary = new Dictionary<string, BattleAgent>();
		private Dictionary<string, TroopAgent> troopDictionary = new Dictionary<string, TroopAgent>();

		private List<SkillVO> skillList = new List<SkillVO> ();
		
		private FameManager fameManager;
		private Thread threadHold;

		private float hpSumCalculatetTimer;
        
		internal BattleSimulator(GameMessageRoute messageRoute, int battlefieldSize)
        {
			this.battleAgentManager = new BattleAgentManager(messageRoute, this, battlefieldSize);
			this.fameManager = FameManager.Singleton;

			fameManager.GetInfoFromUnityTerrain = false;
			fameManager.Resolution = 1;
			fameManager.TerrainWidth = battlefieldSize;
			fameManager.TerrainHeight = 1;
			fameManager.TerrainLength = battlefieldSize;
        }

        internal BattleAgentManager BattleAgentManager
        {
            get
            {
                return this.battleAgentManager;
            }
        }

		internal void Start ()
		{
			BattleSimulator.lastUpdateTime = BattleTime.time;

			this.threadHold = new Thread (ThreadUpdate);
			threadHold.Start ();
		}

		private void ThreadUpdate ()
		{
			battleAgentManager.CalculateInitialHPSum ();

			while (battleAgentManager != null)
			{
				Thread.Sleep(Mathf.RoundToInt(Mathf.Max(0, GameGlobal.BATTLE_SIMULATION_STEP - BattleSimulator.deltaTime) * 1000));

				for (int i=0; i< 1; i++)
				{
				BattleSimulator.deltaTime = BattleTime.time - BattleSimulator.lastUpdateTime;
				BattleSimulator.lastUpdateTime = BattleTime.time;

				battleAgentManager.Tick(BattleSimulator.deltaTime);
				fameManager.Tick(BattleSimulator.deltaTime);

				lock (this.skillList) 
				{
					foreach(SkillVO skillVO in this.skillList)
					{
						battleAgentManager.ExecuteSkill(skillVO);
					}

					skillList.Clear();
				}

				List<string> removeTroopKey = new List<string>();
				
				foreach (KeyValuePair<string, TroopAgent> kvp in troopDictionary)
				{
					TroopAgent troop = kvp.Value;
					
					if (troop.CurrentStatus == BattleAgentStatus.DIE)
					{
						removeTroopKey.Add(kvp.Key);
					}
					else
					{
						troop.Tick();
					}
				}

				foreach (string key in removeTroopKey)
				{
					TroopAgent troopAgent = troopDictionary[key];
					battleAgentManager.RemoveTroop(troopAgent, troopAgent.Faction);
					sideDictionary[troopAgent.SideName].RemoveTroop(key);
					troopDictionary.Remove(key);
				}

				List<string> removeAgentKey = new List<string>();

				foreach (KeyValuePair<string, BattleAgent> kvp in agentDictionary)
				{
					BattleAgent agent = kvp.Value;

					if (agent.CurrentStatus == BattleAgentStatus.DIE)
					{
						removeAgentKey.Add(agent.Name);
					}
					else   			// if (RandomUtil.Range(0, 10) < 1)
					{
						agent.Tick();
						
						//                    Debug.DrawLine(MathUtil.ParseToVector3(agent.Position), MathUtil.ParseToVector3(agent.TroopPostion), agent.m_color);
						
						//						if (agent.TracingTarget != null && agent.TracingTarget.IsAlive)
						//						{
						//							Debug.DrawLine(MathUtil.ParseToVector3(agent.Position), MathUtil.ParseToVector3(agent.TracingTarget.Position), agent.m_color);
						//						}
					}
				}

				foreach (string key in removeAgentKey)
				{
					agentsOutputInfoDirty = true;

					BattleAgent agent = agentDictionary[key];
					troopDictionary[agent.TroopName].RemoveSoldier(key);
					battleAgentManager.RemoveSoldier(agent);
					agentDictionary.Remove(key);
				}

				foreach(KeyValuePair<string, SideAgent> kvp in sideDictionary)
				{
					kvp.Value.Tick();
				}

				if (agentsOutputInfoDirty)
				{
					Dictionary<string, BattleAgent> agentsOutputBuffer = new Dictionary<string, BattleAgent>();

					foreach (KeyValuePair<string, BattleAgent> kvp in agentDictionary)
					{
						agentsOutputBuffer.Add(kvp.Key, kvp.Value);
					}

					agentsOutputInfo = agentsOutputBuffer;
					agentsOutputInfoDirty = false;
				}

				if (hpSumCalculatetTimer > 0.5f)
				{
					battleAgentManager.CalculateHPSum();
					hpSumCalculatetTimer = 0;;
				}
				else
				{
					hpSumCalculatetTimer += BattleSimulator.deltaTime;
				}
				}
			}

			Debug.Log ("Battle Simulation Terminated");
		}
		
		public SideAgent AddSide(SideDefinition side)
		{
			SideAgent sideAgent = battleAgentManager.AddSide(side);
			
			foreach(KeyValuePair<string, TroopAgent> kvp in sideAgent.troopDictionary)
			{
				AddTroop(kvp.Value);
			}
			
			sideDictionary.Add(sideAgent.sideName, sideAgent);
			return sideAgent;
		}

        public TroopAgent AddTroop(TroopAgent troopAgent)
		{
			troopDictionary.Add(troopAgent.Name, troopAgent);

            foreach (KeyValuePair<string, BattleAgent> kvp in  troopAgent.Soldiers)
            {
                BattleAgent battleAgent = kvp.Value;
				agentDictionary.Add(battleAgent.Name, battleAgent);
            }

			agentsOutputInfoDirty = true;

            return troopAgent;
        }

		public void ExecuteSkill (SkillVO skillVO)
		{
			lock (this.skillList) 
			{
				skillList.Add(skillVO);
			}
		}

        public Dictionary<string, TroopDefinition> AllTombStones()
        {
            return battleAgentManager.tombStoneDic;
        }

		public void Dispose()
		{
			this.battleAgentManager = null;

			threadHold.Interrupt ();

			FameManager.ResetScene ();

			Debug.Log ("Battle Simulator Stopped !");
		}
    }
}
