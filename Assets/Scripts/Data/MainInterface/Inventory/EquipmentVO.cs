using System;

public class EquipmentVO
{
    public EquipmentMeta meta;

    public int level;

    public string ownerHeroName;

    public int Attack
    {
        get
        {
            return meta.initialAttack + (int)(meta.attackIncrement * level);
        }
    }

    public int Defense
    {
        get
        {
            return meta.initialDefense + (int)(meta.defenseIncrement * level);
        }
    }

    public int HP
    {
        get
        {
            return meta.initialHP + (int)(meta.hpIncrement * level);
        }
    }
}

