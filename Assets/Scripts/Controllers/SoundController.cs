using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour 
{
	public AudioClip[] _gameSounds;

	public void playSound(int idSound)
	{
		this.GetComponent<AudioSource>().PlayOneShot(this._gameSounds[idSound]);
	}
}
