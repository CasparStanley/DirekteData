using System.Collections.Generic;

[System.Serializable]
public class DataSet
{
    public readonly int Id;
    public string Name;
    public string Description;
    public List<DataStructure> Recordings = new List<DataStructure>();

    public DataSet() { }

    public DataSet(int id, string name, string description)
    {
        Id = id;
        Name = name;
        Description = description;
    }

    public DataSet(int id, string name, string description, List<DataStructure> recordings)
    {
        Id = id;
        Name = name;
        Description = description;
        Recordings = recordings;
    }

    public override string ToString()
    {
        return $"Id: {Id}, Name: {Name}, Description: {Description}, Recordings: {Recordings.Count}";
    }
}
