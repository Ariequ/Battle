using System;

public class BattleValueConst
{
	public enum AttackResult { Fail = 0, Normal, Critical, Evasion };

	public const int MAX_HERO_COUNT = 3;

	public const int MAX_BUFF_COUNT = 32;

    public const int MAX_EQUIPMENT_COUNT = 4;

}

public enum BattleResult {Win = 0, Lose, Draw, NotOver};
