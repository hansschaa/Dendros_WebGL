using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState
{
	public States _state;

    public GameState(States state)
    {
        this._state = state;
    }

    public enum States
	{
		MAINSCENE, GARAGE, GAMESCENE, FINALSCENE, GAMEOVER
	}
}
