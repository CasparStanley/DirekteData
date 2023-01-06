using System.Collections.Generic;

[System.Serializable]
public class DataSet
{
    public string Name;
    public string Description;
    public readonly int Id;
    public List<DataStructure> Recordings = new List<DataStructure>();

    private static int _currentId = 0;

    public DataSet(string name, string description, List<DataStructure> recordings)
    {
        Name = name;
        Description = description;
        Id = _currentId;
        Recordings = recordings;

        // TODO: Bro this is handled by the database
        _currentId++;
    }
}
