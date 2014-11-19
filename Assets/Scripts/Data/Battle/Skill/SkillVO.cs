using System;
using Battle;

public class SkillVO
{
    public SkillMeta Meta { get; private set; }

    public Faction casterFaction;

    public Vector2 centerPosition;

    public int anger;

    public SkillVO(SkillMeta meta, Faction faction)
    {
        this.Meta = meta;
		this.casterFaction = faction;
    }
}

