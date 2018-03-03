using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountGarageController : MonoBehaviour 
{
	public Text _beginCountGarage;
	public Coroutine _BC;

	// Use this for initialization
	void Start () 
	{
		// this._BC = StartCoroutine(beginCount());	
	}

	public IEnumerator beginCount()
	{
		int contador = 30;
		while(contador != 0)
		{
			this._beginCountGarage.text = contador.ToString();
			contador--;
			yield return new WaitForSeconds(1f);
		}

		this.gameObject.GetComponent<ViewController>().pressEnter();
	}

    internal void stopCoroutine()
    {
		StopCoroutine(this._BC);
    }

    internal void resetCoroutine()
    {
        this._BC = StartCoroutine(beginCount());
    }
}
