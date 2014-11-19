using System;
using Battle;

public interface ISkillContainerDelegate
{
    void ExecuteSkill(SkillVO skillVO);

    void ExecuteSkillsBehind(UnitController unitController, Vector2 position, SkillLinkMeta[] skillLinkMetaArray);
}

