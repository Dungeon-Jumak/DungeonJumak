using System;
using System.Collections.Generic;

[Serializable]
public class EncyclopediaData
{
    private Dictionary<string, bool> monsters = new Dictionary<string, bool>();
    private Dictionary<string, bool> customers = new Dictionary<string, bool>();

    public Dictionary<string, bool> Monsters
    {
        get { return monsters; }
        set { monsters = value; }
    }

    public Dictionary<string, bool> Customers
    {
        get { return customers; }
        set { customers = value; }
    }

    public EncyclopediaData()
    {
        monsters["0"] = false;
        monsters["1"] = false;
        monsters["2"] = false;
        monsters["3"] = false;

        customers["11"] = false;
    }
}
