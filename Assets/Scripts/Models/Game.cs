public class Game 
{
	public string _missionNumber;
	public string _descriptionMission;
    public int _entryTime;
    public int _postEntryTime;
    public string[] _zones;
	public int[,] _map;

    public Game(string missionNumber, string descriptionMission, int entryTime, int postEntryTime, string[] zones, int[,] map)
    {
        _missionNumber = missionNumber;
        _descriptionMission = descriptionMission;
        _entryTime = entryTime;
        _postEntryTime = postEntryTime;
        _zones = zones;
        _map = map;
    }
}
