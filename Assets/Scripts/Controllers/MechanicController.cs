using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MechanicController : MonoBehaviour 
{
	[Header("Prefabs")]
	public GameObject _playerPrefab;
	

	[Header("Game Scene UI")]
	public GameObject _heartLifesList;
	public Text _timeCountText;


	public Coroutine _portalTimeCoroutine;

	public void resetPlayerPosition()
	{
		int iReset=0;
		int jReset=0;
		do
		{
			iReset = UnityEngine.Random.Range(0, GlobalVariables._iMaxMatrix); 
			jReset = UnityEngine.Random.Range(0, GlobalVariables._jMaxMatrix); 
		} while(ViewController._currentGameModel._map[iReset,jReset] != 15);

		GlobalVariables._xPosPlayer = jReset;
		GlobalVariables._yPosPlayer = iReset;


		if(GlobalVariables._currentLifes>1)
		{
			this.gameObject.GetComponent<ViewController>().createPlayer();
		}

		updateLifes();
	}

	public void updateLifes()
	{
		// GlobalVariables._currentLifes-= 1;
		// this._heartLifesList.transform.GetChild(GlobalVariables._currentLifes).gameObject.SetActive(false);
		// if(GlobalVariables._currentLifes == 0)
		// 	this.gameObject.GetComponent<ViewController>().pressSpace();
	}

	public void initializatePortalCoroutine(int time)
	{
		this._portalTimeCoroutine = StartCoroutine(portalCoroutine());
	}

	public void stopPortalCoroutine()
	{
		StopCoroutine(this._portalTimeCoroutine);
		
	}

	IEnumerator portalCoroutine()
	{

		int contador = ViewController._currentGameModel._entryTime;
		while(contador > 0)
		{
			contador--;
			_timeCountText.text = contador.ToString();
			yield return new WaitForSeconds(1f);
		}

		

		while(true)
		{
			contador = ViewController._currentGameModel._postEntryTime;
			while(contador > 0)
			{
				contador--;
				_timeCountText.text = contador.ToString();
				yield return new WaitForSeconds(1f);
			}

			putPortal();
			_timeCountText.color = new Color(255,0,0,255);

			contador = 10;
			while(contador > 0)
			{
				contador--;
				_timeCountText.text = contador.ToString();
				yield return new WaitForSeconds(1f);
			}
			_timeCountText.color = new Color(0,255,0,255);
			Destroy(this.gameObject.GetComponent<ViewController>()._portalInstance); 

		}
	}

	public void putPortal()
	{
		int iReset=0;
		int jReset=0;
		do
		{
			iReset = UnityEngine.Random.Range(0, GlobalVariables._iMaxMatrix); 
			jReset = UnityEngine.Random.Range(0, GlobalVariables._jMaxMatrix); 
		} while(ViewController._currentGameModel._map[iReset,jReset] != 15);

		
		this.gameObject.GetComponent<ViewController>().createPortal(iReset, jReset);
	}
}
