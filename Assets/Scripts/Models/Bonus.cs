using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus: MonoBehaviour
{
	public Vector2 _position;
	public BonusTypes.Types _myType;

	/// <summary>
	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>
	void Start()
	{
		Invoke("secondAnimation",1f);
	}

	public void secondAnimation()
	{
		try
		{
			this.transform.GetChild(0).gameObject.GetComponent<Animator>().SetBool("fly", true);

		}

		catch(Exception e)
		{
			print("Exception: " + e);
		}
	}
}
