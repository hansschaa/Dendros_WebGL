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
	public Sprite _redKai;
	public Sprite _greenKai;

	public SoundController _soundController;
	public Coroutine _portalTimeCoroutine;

	public void resetPlayerPosition()
	{
		int iReset=0;
		int jReset=0;
		do
		{
			iReset = UnityEngine.Random.Range(0, GlobalVariables._iMaxMatrix); 
			jReset = UnityEngine.Random.Range(0, GlobalVariables._jMaxMatrix); 
		} while(ViewController._currentGameModel._map[iReset,jReset] == -1);

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
		this._heartLifesList.transform.GetChild(GlobalVariables._currentLifes-1).gameObject.GetComponent<Image>().sprite = this._redKai;
		GlobalVariables._currentLifes-= 1;
		
		// this._heartLifesList.transform.GetChild(GlobalVariables._currentLifes).gameObject.SetActive(false);
		if(GlobalVariables._currentLifes == 0)
		{
			this.GetComponent<AudioSource>().enabled = false;
			_soundController.playSound(5);
			GlobalVariables._globalLifes -= 1;
			GlobalVariables._followPlayer = false;
			GlobalVariables._stageComplete = true;
			Invoke("partialGameOverMethod",3f);
		}

		else
		{
		
			this.GetComponent<BonusController>().resetStats();
		}	
	}

	public void initializatePortalCoroutine(int time)
	{
		this._portalTimeCoroutine = StartCoroutine(portalCoroutine());
	}

	public void stopPortalCoroutine()
	{

		this._heartLifesList.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = this._greenKai;
		this._heartLifesList.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = this._greenKai;
		this._heartLifesList.transform.GetChild(2).gameObject.GetComponent<Image>().sprite = this._greenKai;

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
		} while(ViewController._currentGameModel._map[iReset,jReset] == -1);

		
		this.gameObject.GetComponent<ViewController>().createPortal(iReset, jReset);
	}

	public void partialGameOverMethod()
	{
		GlobalVariables._followPlayer = true;
		this.GetComponent<ViewController>().pressEnter();
	}
}
