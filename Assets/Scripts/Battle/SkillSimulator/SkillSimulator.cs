using System.Collections.Generic;
using System;

namespace Battle
{
	public class SkillSimulator
	{
		private Dictionary<string, SkillAgent> skillAgentDic = new Dictionary<string, SkillAgent> ();
        
		public SkillSimulator()
		{
		}
        
		public void Destroy()
		{
		}

		public void AddSkillAgent(SkillAgent skillAgent)
		{
			skillAgentDic.Add(skillAgent.Name, skillAgent);
		}

		public void RemoveSkillAgent(string skillAgentName)
		{
			skillAgentDic.Remove(skillAgentName);
		}

		public void ExecuteSkill(int skillID, ILimitFuncitonContext context, Action<int, ILimitFuncitonContext> callBack)
		{
			LimitFunctionNode node = LimitFunctionTreeFactory.Instance.Create(skillID);
			node.Execute(context, callBack);
		}

		public void Update(float deltaTime)
		{
			List<string> removeAgentKey = new List<string> ();
            
			foreach (KeyValuePair<string, SkillAgent> kvp in skillAgentDic)
			{
				SkillAgent skillAgent = kvp.Value;
                
				if (skillAgent.Status == SkillAgentStatus.DESTROY)
				{
					removeAgentKey.Add(kvp.Key);
				}
				else
				{
					skillAgent.Update(deltaTime);
				}
			}

			removeAgentKey.ForEach(delegate(String name)
			{
				RemoveSkillAgent(name);
			});
		}
	}
}
