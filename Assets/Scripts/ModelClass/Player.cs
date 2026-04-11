using UnityEngine;

public class Player
{
    protected int id;
    protected string username;

    public Player(int id,string username)
    {
        this.id         = id;
        this.username   = username;
    }

    public int getPlayerID() {
        return this.id;
    }
    public string getUsername() {
        return this.username;
    }
}

