using System.Collections;

public enum CurrencyType 
{ 
	/// 金币
    Gold,
	/// 木材
    Wood, 
	/// 矿石
    Ore, 
	/// 钻石
    Gem
}

public struct Price
{
	/// 金币
    public int gold;
	/// 木材
    public int wood;
	/// 矿石
    public int ore;
	/// 钻石
    public int gem;

    public Price(int gold, int wood, int ore, int gem = 0)
    {
        this.gold = gold;
        this.wood = wood;
        this.ore = ore;
        this.gem = gem;
    }
}

public class CurrencyUtil 
{

}
