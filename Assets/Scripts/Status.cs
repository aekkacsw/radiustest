[System.Serializable]
public struct Status
{
    public int MaxHP;
    public int HP;
    public int Atk;
    public int AtkRange;
    public float Speed;
}

public enum State 
{
    Low,
    Medium,
    High
}