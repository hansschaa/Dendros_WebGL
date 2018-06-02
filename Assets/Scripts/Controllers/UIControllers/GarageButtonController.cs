using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GarageButtonController : MonoBehaviour , IPointerDownHandler
{
	public int _idStage;

	public void OnPointerDown(PointerEventData e)
	{
		if(_idStage <= PlayerPrefs.GetInt("lastStageCompleted"))
		{
			GameObject.Find("GameController").gameObject.GetComponent<ViewController>().refreshContext(_idStage);
		}

	}
}
