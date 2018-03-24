using System.Collections.Generic;

public class Game 
{
	public string _missionNumber;
	public string _descriptionMission;
    public int _entryTime;
    public int _postEntryTime;
	public int[,] _map;
    public List <string> _bonusList;

    public Game(string missionNumber, string descriptionMission, int entryTime, int postEntryTime, List<string> bonus, int[,] map)
    {
        _missionNumber = missionNumber;
        _descriptionMission = descriptionMission;
        _entryTime = entryTime;
        _postEntryTime = postEntryTime;
        _bonusList = bonus;
        _map = map;
    }
}
