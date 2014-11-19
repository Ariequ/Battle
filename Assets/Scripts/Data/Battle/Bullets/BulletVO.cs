using UnityEngine;
using System.Collections;

public class BulletVO
{
    private BulletMeta bulletMeta;

    public BulletVO(BulletMeta bulletMeta, Faction faction, AttackValue attackValue)
    {
        this.bulletMeta = bulletMeta;
        this.faction = faction;
        this.attackValue = attackValue;
    }

    public Faction faction;

    public Vector3 fromPosition;
    public Vector3 toPosition;

    public Vector3 currentPosition;
    public Vector3 currentDirection;
    
    public AttackValue attackValue;

	public float MaxSpeed
	{
		get
		{
            return bulletMeta.maxSpeed;
		}
	}

	public float MinSpeed
	{
		get
		{
            return bulletMeta.minSpeed;
		}
	}

	public float Drag
	{
		get
		{
            return bulletMeta.drag;
		}
	}

	public bool IsAreaAttack
	{
		get
		{
            return bulletMeta.isAreaAttack;
		}
	}

	public float AttackRadius
	{
		get
		{
            return bulletMeta.attackRadius;
		}
	}
}
