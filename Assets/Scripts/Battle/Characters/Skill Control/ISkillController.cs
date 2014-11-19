using System;
using Battle;

public interface ISkillController
{
    void Initialize (BattleAgentManager agentManager, ISkillContainerDelegate skillContainerDelegate, BattleValueCalculator valueCalculator);

    void Execute (SkillVO skillVO);
}

