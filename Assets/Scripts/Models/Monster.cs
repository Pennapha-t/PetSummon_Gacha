using System;
[Serializable]
public class MonsterRequest
{
    public int[] monster_ids;
    public MonsterRequest(int[] _ids)
    {
        this.monster_ids = _ids;
    }
}

public class MonsterData
{
    public int monster_id;
    public int hp;
    public int damage;
    public string name;
    public ItemDrop[] item_drops;
    public CoinDrop coin_drops;
}
