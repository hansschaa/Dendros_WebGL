using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour 
{

	public AudioClip[] _musicList;

	public void updateMusic()
	{
		if(ViewController._gameState._state == GameState.States.MAINSCENE)
		{
			this.GetComponent<AudioSource>().clip = this._musicList[0];
			this.GetComponent<AudioSource>().Play();
		}

		else if(ViewController._gameState._state == GameState.States.GARAGE)
		{
			this.GetComponent<AudioSource>().Stop();
		}

		else if(ViewController._gameState._state == GameState.States.GAMESCENE)
		{
			this.GetComponent<AudioSource>().clip = this._musicList[1];
			this.GetComponent<AudioSource>().Play();
		}

		else if(ViewController._gameState._state == GameState.States.GAMEOVER)
		{
			this.GetComponent<AudioSource>().Stop();
		}

		else if(ViewController._gameState._state == GameState.States.FINALSCENE)
		{
			this.GetComponent<AudioSource>().Stop();
		}

	}

	
}
