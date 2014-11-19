using System;

public class SoldierVO
{
	public int level = 1;

    public SoldierMeta Meta { get; private set; }

    private Faction faction;

    public SoldierVO(SoldierMeta meta, Faction faction)
    {
        this.Meta = meta;
        this.faction = faction;
    }

	public Faction Faction 
	{ 
		get
		{
			return this.faction;
		}
	}

    public int Attack
    {
        get
        {
            return Meta.initialAttack + (int)(Meta.attackIncrement * level);
        }
    }

    public int Defense
    {
        get
        {
            return Meta.initialDefense + (int)(Meta.defenseIncrement * level);
        }
    }

    public int HP
    {
        get
        {
            return Meta.initialHP + (int)(Meta.hpIncrement * level);
        }
    }

    public int Luck
    {
        get
        {
            return Meta.initialLuck + (int)(Meta.luckIncrement * level);
        }
    }

    public int AttackFrequency
    {
        get
        {
            return Meta.attackFrequency;
        }
    }

    public int AttackMinRange
    {
        get
        {
            return Meta.attackMinRange;
        }
    }

    public int AttackMaxRange
    {
        get
        {
            return Meta.attackMaxRange;
        }
    }

    public int Dodge
    {
        get
        {
            return Meta.dodge;
        }
    }
}

