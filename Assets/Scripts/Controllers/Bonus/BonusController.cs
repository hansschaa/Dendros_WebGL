using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class BonusController : MonoBehaviour 
{
	Coroutine _changeParametersCoroutine;
	Coroutine _bonusRespawnCoroutine;
	SearchManager _searchManager;
	//public GameObject _shape;
	public List<Vector2> _portalPath;
	public SoundController _soundController;

	[Header("Reimi")]
	public TextMeshProUGUI _reimiText;

	[Header("Bonus GameObjects")]
	public Transform _lightBonus;
	public Transform _velocityBonus;
	public Transform _coordinationBonus;
	public Transform _hipocampoBonus;

	//public GameObject _bonusPrefab;

	[HideInInspector]
	public GameObject _lightBonusInstance;

	[HideInInspector]
	public GameObject _velocityBonusInstance;

	[HideInInspector]
	public GameObject _coordinationBonusInstance;

	[HideInInspector]
	public GameObject _teleportBonusInstance;

	[HideInInspector]
	public GameObject _portalBonusInstance;

	[HideInInspector]
	public GameObject _hipocampoBonusInstance;

	public Transform _portalGroupGameObject;

	public GameObject[] _bonusList;

	public GameObject[] _portalPathTiles;
	public Material _materialLightSensitive;
	//public Material _materialHipocampo;
	public Transform _map;
	public int[] _tilesHipocampo;
	private int _tileCountHipocampo;

	[Header("Parameters")]
	public float _decreaseValue;
	public float _initializeChangeParametersDelay;
	public float _respawnDelay;
	
	
	[HideInInspector]
	public Vector2 _bar;


	//SEARCH FOR POSITION VARIABLES
	int max;
	int min;
	int iReset;
	int jReset;


	public int _idActiveChildLight;
	public int _idActiveChildVelocity;
	public int _idActiveChildCoordination;
	public int _idActiveChildHipocampo;

    internal void resetStats()
    {

		if(this._lightBonus.gameObject.activeInHierarchy)
		{
			for(int i = 0 ; i < 3;i++)
				this._lightBonus.transform.GetChild(i).gameObject.GetComponent<RectTransform>().sizeDelta = this._bar;

			_materialLightSensitive.color = new Color32(255,255,255,255);
			this._idActiveChildLight = 0;
		}
			

		if(this._velocityBonus.gameObject.activeInHierarchy)
		{
			for(int i = 0 ; i < 3;i++)
				this._velocityBonus.transform.GetChild(i).gameObject.GetComponent<RectTransform>().sizeDelta = this._bar;

			// this._playerVelocity = GlobalVariables._playerVelocity;
			GameObject.Find("KaiPlayer(Clone)").gameObject.GetComponent<PlayerBehaviour>().speed = GlobalVariables._playerVelocity;
			this._idActiveChildVelocity = 0;
		}
		
		if(this._coordinationBonus.gameObject.activeInHierarchy)
		{
			GlobalVariables._changeDirection = false;
			for(int i = 0 ; i < 3;i++)
				this._coordinationBonus.transform.GetChild(i).gameObject.GetComponent<RectTransform>().sizeDelta = this._bar;
			
			this._idActiveChildCoordination = 0;
		}

		if(this._hipocampoBonus.gameObject.activeInHierarchy)
		{
			for(int i = 0 ; i < 3;i++)
				this._hipocampoBonus.transform.GetChild(i).gameObject.GetComponent<RectTransform>().sizeDelta = this._bar;
			
			for(int i = 0 ; i < _tileCountHipocampo ; i++)
			{
				_map.GetChild(this._tilesHipocampo[i]).gameObject.GetComponent<Renderer>().material = _materialLightSensitive;   
				_map.GetChild(this._tilesHipocampo[i]).gameObject.GetComponent<SpriteRenderer>().color = new Color(255,255,255,255);   

				_tilesHipocampo[i] = 0;
			}

			this._idActiveChildHipocampo = 0;
			this._tileCountHipocampo = 0;
		}
        
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
	{
		this._portalPath = new List<Vector2>();
		this._searchManager = new SearchManager();
		this._tilesHipocampo = new int[27];
		this._decreaseValue = this._lightBonus.GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta.x/10;
		this._bar = this._lightBonus.GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta;
		this._materialLightSensitive.color = new Color32(255,255,255,255);
	}

    private IEnumerator changeParametersBonus()
    {
		while(true)
		{
			
			if(this._lightBonus.gameObject.activeInHierarchy && this._idActiveChildLight!=3)
			{
				if(this._lightBonus.transform.GetChild(this._idActiveChildLight).gameObject.GetComponent<RectTransform>().sizeDelta.x - this._decreaseValue>=0)
				{
					this._lightBonus.transform.GetChild(this._idActiveChildLight).gameObject.GetComponent<RectTransform>().sizeDelta -= new Vector2(this._decreaseValue,0);
				}

				else
				{
					
					this._soundController.playSound(2);
					switch(this._idActiveChildLight)
					{
						case 2:
						_materialLightSensitive.color = new Color32(255,255,255,30);
					
						_reimiText.text = "KAI estás por perder tu visión ¡Busca el bonus VERDE!";
						break;
						case 1:
						_materialLightSensitive.color = new Color32(255,255,255,100);
						_reimiText.text = "KAI estás por perder tu visión ¡Busca el bonus VERDE!";
						break;
						case 0:
						_materialLightSensitive.color = new Color32(255,255,255,160);
						_reimiText.text = "KAI estás por perder tu visión ¡Busca el bonus VERDE!";
						break;
					}

					this._idActiveChildLight++;
				}
			}

			if(this._velocityBonus.gameObject.activeInHierarchy && this._idActiveChildVelocity!=3)
			{
				if(this._velocityBonus.transform.GetChild(this._idActiveChildVelocity).gameObject.GetComponent<RectTransform>().sizeDelta.x - this._decreaseValue>=0)
				{
					this._velocityBonus.transform.GetChild(this._idActiveChildVelocity).gameObject.GetComponent<RectTransform>().sizeDelta -= new Vector2(this._decreaseValue,0);
				}

				else
				{
					this._soundController.playSound(2);
					switch(this._idActiveChildVelocity)
					{
						case 2:
						GameObject.Find("KaiPlayer(Clone)").gameObject.GetComponent<PlayerBehaviour>().speed = GlobalVariables._playerVelocity-0.3f;
						_reimiText.text = "KAI vas a perder tu movimiento ¡Busca el bonus NARANJO!";
						break;
						case 1:
						GameObject.Find("KaiPlayer(Clone)").gameObject.GetComponent<PlayerBehaviour>().speed = GlobalVariables._playerVelocity-0.2f;
						_reimiText.text = "KAI vas a perder tu movimiento ¡Busca el bonus NARANJO!";
						break;
						case 0:
						
						GameObject.Find("KaiPlayer(Clone)").gameObject.GetComponent<PlayerBehaviour>().speed = GlobalVariables._playerVelocity-0.1f;
						_reimiText.text = "KAI vas a perder tu movimiento ¡Busca el bonus NARANJO!";
						break;
					}
					

					this._idActiveChildVelocity++;
				}
			}

			if(this._coordinationBonus.gameObject.activeInHierarchy && this._idActiveChildCoordination!=3)
			{
				if(this._coordinationBonus.transform.GetChild(this._idActiveChildCoordination).gameObject.GetComponent<RectTransform>().sizeDelta.x - this._decreaseValue>=0)
				{
					this._coordinationBonus.transform.GetChild(this._idActiveChildCoordination).gameObject.GetComponent<RectTransform>().sizeDelta -= new Vector2(this._decreaseValue,0);
				}

				else
				{
					this._soundController.playSound(2);
					
					this._idActiveChildCoordination++;
					if(this._idActiveChildCoordination == 3)
					{
						GlobalVariables._changeDirection = true;
					}

					else if(this._idActiveChildCoordination == 1 || this._idActiveChildCoordination == 2)
					{
						_reimiText.text = "KAI vas a perder tu Coordinación ¡Busca el bonus GRIS!";
					}
				}
			}

			if(this._hipocampoBonus.gameObject.activeInHierarchy && this._idActiveChildHipocampo!=3)
			{
				if(this._hipocampoBonus.transform.GetChild(this._idActiveChildHipocampo).gameObject.GetComponent<RectTransform>().sizeDelta.x - this._decreaseValue>=0)
				{
					this._hipocampoBonus.transform.GetChild(this._idActiveChildHipocampo).gameObject.GetComponent<RectTransform>().sizeDelta -= new Vector2(this._decreaseValue,0);
				}

				else
				{
					this._soundController.playSound(2);
					

					switch(this._idActiveChildHipocampo)
					{
						case 0:
							searchForTiles(6);
							StartCoroutine(paintTilesInCascade(0));
							break;

						case 1:
							searchForTiles(15);
							StartCoroutine(paintTilesInCascade(6));
							break;

						case 2:
							searchForTiles(27);
							StartCoroutine(paintTilesInCascade(15));
							break;	
					}

					this._reimiText.text = "KAI cuidado con el bonus hipocampo";
					this._idActiveChildHipocampo++;
				}
			}


			yield return new WaitForSeconds(_initializeChangeParametersDelay);

		}	
    }

	private void searchForTiles(int amount)
	{
		int i,j;

		do
		{
			i = UnityEngine.Random.Range(0, _map.childCount); 
			

			this._tilesHipocampo[_tileCountHipocampo] = i;
			_tileCountHipocampo++;
		}
		while(_tileCountHipocampo < amount);
	}

    IEnumerator paintTilesInCascade(int init)
    {
		for(int i = init; i < this._tileCountHipocampo; i++)
		{
			//_map.GetChild(this._tilesHipocampo[i]).gameObject.GetComponent<Renderer>().material = this._materialHipocampo; 
			_map.GetChild(this._tilesHipocampo[i]).gameObject.GetComponent<SpriteRenderer>().material.DOColor(new Color32(255,255,255,10),2f);
			yield return new WaitForSeconds(0.6f);		
		}
			  
    }

    internal void stopCoroutine()
    {
		StopCoroutine(_changeParametersCoroutine);
		//Light Childs
		for(int i = 0 ; i < 3;i++)
			this._lightBonus.transform.GetChild(i).gameObject.GetComponent<RectTransform>().sizeDelta = this._bar;

		for(int i = 0 ; i < 3;i++)
			this._velocityBonus.transform.GetChild(i).gameObject.GetComponent<RectTransform>().sizeDelta = this._bar;
		
		for(int i = 0 ; i < 3;i++)
			this._coordinationBonus.transform.GetChild(i).gameObject.GetComponent<RectTransform>().sizeDelta = this._bar;

		for(int i = 0 ; i < 3;i++)
			this._hipocampoBonus.transform.GetChild(i).gameObject.GetComponent<RectTransform>().sizeDelta = this._bar;
		
		this._idActiveChildLight = 0;
		this._idActiveChildVelocity = 0;
		this._idActiveChildCoordination = 0;
		this._idActiveChildHipocampo = 0;
		_materialLightSensitive.color = new Color32(255,255,255,255);
		
		for(int i = 0 ; i < _tileCountHipocampo ; i++)
		{
			_map.GetChild(this._tilesHipocampo[i]).gameObject.GetComponent<Renderer>().material = _materialLightSensitive;   
			_map.GetChild(this._tilesHipocampo[i]).gameObject.GetComponent<SpriteRenderer>().color = new Color(255,255,255,255);   

			_tilesHipocampo[i] = 0;
		}

		_tileCountHipocampo = 0;
    }

	public void pauseCoroutines()
	{
		StopCoroutine(_changeParametersCoroutine);
		StopCoroutine(_bonusRespawnCoroutine);
	}

	internal void initializeCorroutines()
    {
		this._changeParametersCoroutine = StartCoroutine(changeParametersBonus());
        this._bonusRespawnCoroutine = StartCoroutine(bonusRespawn());
    }

    private IEnumerator bonusRespawn()
    {
		yield return new WaitForSeconds(_respawnDelay);
		while(true)
		{
			if(this._lightBonus.gameObject.activeInHierarchy)
			{
				createBonus(BonusTypes.Types.LIGHT);

			}
			
			if(this._velocityBonus.gameObject.activeInHierarchy)
			{
				createBonus(BonusTypes.Types.VELOCITY);
			}

			if(this._coordinationBonus.gameObject.activeInHierarchy)
			{
				createBonus(BonusTypes.Types.COORDINATION);		
			}

			if(this._hipocampoBonus.gameObject.activeInHierarchy)
			{
				createBonus(BonusTypes.Types.HIPOCAMPO);
			}

			if(GlobalVariables._yellowBonus)
			{
				createBonus(BonusTypes.Types.TELEPORT);
			}

			if(GlobalVariables._purpleBonus)
			{
				createBonus(BonusTypes.Types.PORTAL);
			}

			

			yield return new WaitForSeconds(_respawnDelay);

		}
    }

    private void createBonus(BonusTypes.Types type)
	{
		Vector2 position = searchForPosition(type);
		Vector2 bonusMatrixPosition = new Vector2(position.x,position.y);
		position.x = (position.x *  GlobalVariables._widthTile) - MapGeneratorController._offsetMap.x;
		position.y = (position.y *  -GlobalVariables._widthTile) - MapGeneratorController._offsetMap.y;
		
		
		
		if(type == BonusTypes.Types.LIGHT && _lightBonusInstance == null)
		{
			_lightBonusInstance = Instantiate(_bonusList[0],position, Quaternion.identity, this.gameObject.GetComponent<ViewController>()._bonusGroup.transform) as GameObject;
			_lightBonusInstance.GetComponent<Bonus>()._position = position;
			//_lightBonusInstance.GetComponent<QuadraticInterpolation>().target = _lightBonus.gameObject;

		}

		else if(type == BonusTypes.Types.VELOCITY && _velocityBonusInstance == null)
		{
	
			_velocityBonusInstance = Instantiate(_bonusList[1], position, Quaternion.identity, this.gameObject.GetComponent<ViewController>()._bonusGroup.transform) as GameObject;
			_velocityBonusInstance.GetComponent<Bonus>()._position = position;
		}

		else if(type == BonusTypes.Types.COORDINATION && _coordinationBonusInstance == null)
		{
			_coordinationBonusInstance = Instantiate(_bonusList[2],position, Quaternion.identity, this.gameObject.GetComponent<ViewController>()._bonusGroup.transform) as GameObject;
			_coordinationBonusInstance.GetComponent<Bonus>()._position = position;
		}

		else if(type == BonusTypes.Types.TELEPORT && _teleportBonusInstance == null)
		{
			_reimiText.text = "KAI puedes escapar ¡Busca el bonus AMARILLO!";
			_teleportBonusInstance = Instantiate(_bonusList[3], position, Quaternion.identity, this.gameObject.GetComponent<ViewController>()._bonusGroup.transform) as GameObject;
			_teleportBonusInstance.GetComponent<Bonus>()._position = position;
		}

		else if(type == BonusTypes.Types.PORTAL && _portalBonusInstance == null && GlobalVariables._allowPurpleBonus )
		{
			_reimiText.text = "KAI resuelve la salida ¡Busca el bonus MORADO!";
			_portalBonusInstance = Instantiate(_bonusList[4], position, Quaternion.identity, this.gameObject.GetComponent<ViewController>()._bonusGroup) as GameObject;
			_portalBonusInstance.GetComponent<Bonus>()._position = bonusMatrixPosition;
		}

		else if(type == BonusTypes.Types.HIPOCAMPO && _hipocampoBonusInstance == null)
		{
			_reimiText.text = "KAI hay zonas oscuras !Busca el bonus AZUL!";
			_hipocampoBonusInstance = Instantiate(_bonusList[5], position, Quaternion.identity, this.gameObject.GetComponent<ViewController>()._bonusGroup) as GameObject;
			_hipocampoBonusInstance.GetComponent<Bonus>()._position = position;
		}	
    }

    private Vector2 searchForPosition(BonusTypes.Types type)
    {
		

		switch(type)
		{
			case BonusTypes.Types.LIGHT:
				min=0;
				max=14;
			break;

			case BonusTypes.Types.VELOCITY:
				min=75;
				max=89;
			break;

			case BonusTypes.Types.COORDINATION:
				min=30;
				max=44;
			break;

			case BonusTypes.Types.TELEPORT:
				min=15;
				max=29;
			break;

			case BonusTypes.Types.PORTAL:
				min=90;
				max=104;
			break;

			case BonusTypes.Types.HIPOCAMPO:
				min=45;
				max=59;
			break;
		}

		do
		{
			iReset = UnityEngine.Random.Range(0, GlobalVariables._iMaxMatrix); 
			jReset = UnityEngine.Random.Range(0, GlobalVariables._jMaxMatrix); 
		} while((ViewController._currentGameModel._map[iReset,jReset] < min || ViewController._currentGameModel._map[iReset,jReset] > max) && checkPortalPosition(iReset,jReset));

		return new Vector2(jReset,iReset);
    }

	public bool checkPortalPosition(int i, int j)
	{
		if(this.gameObject.GetComponent<ViewController>()._portalInstance != null)
		{
			float x = this.gameObject.GetComponent<ViewController>()._portalInstance.GetComponent<Bonus>()._position.x;
			if(j == x)
				return false;
		}

		return true;
	}

    public void resetBonusInstances()
	{
		_lightBonusInstance = null;
		_velocityBonusInstance = null;
		_coordinationBonusInstance = null;
		_teleportBonusInstance = null;
		_portalBonusInstance = null;
		_hipocampoBonusInstance = null;

		// GlobalVariables._playerVelocity = this._playerVelocity;
	}

	public void bonusCollision(BonusTypes.Types type, Vector2 position)
	{
		if(type == BonusTypes.Types.LIGHT)
		{
			this._materialLightSensitive.color = new Color32(255,255,255,255);
			
			for(int i = 0 ; i < 3;i++)
				this._lightBonus.transform.GetChild(i).gameObject.GetComponent<RectTransform>().sizeDelta = this._bar;

			this._idActiveChildLight = 0;
		}

		else if(type == BonusTypes.Types.VELOCITY)
		{
			for(int i = 0 ; i < 3;i++)
				this._velocityBonus.transform.GetChild(i).gameObject.GetComponent<RectTransform>().sizeDelta = this._bar;
			
			GameObject.Find("KaiPlayer(Clone)").gameObject.GetComponent<PlayerBehaviour>().speed = GlobalVariables._playerVelocity;
			this._idActiveChildVelocity = 0;
		}

		else if(type == BonusTypes.Types.COORDINATION)
		{
			for(int i = 0 ; i < 3;i++)
				this._coordinationBonus.transform.GetChild(i).gameObject.GetComponent<RectTransform>().sizeDelta = this._bar;

			GlobalVariables._changeDirection= false;

			this._idActiveChildCoordination = 0;
			// this._coordinationBonus.sizeDelta = _bar;
		}

		else if(type == BonusTypes.Types.TELEPORT)
		{
			this.GetComponent<ViewController>().createPlayer(false);
		}

		else if(type == BonusTypes.Types.PORTAL)
		{	
			if(GetComponent<ViewController>()._portalInstance!= null)
			{
				Destroy(GetComponent<ViewController>()._portalInstance);
				foreach(Transform tr in _portalGroupGameObject)
				{
					Destroy(tr.gameObject);
				}

			}
				

			this._portalPath.Clear();
			// GetComponent<MechanicController>().stopPortalCoroutine();
			GetComponent<MechanicController>().putPortal();
			Vector2 portalPosition = GetComponent<ViewController>()._portalInstance.GetComponent<Bonus>()._position;
			this._portalPath = _searchManager.encontrarCamino(position, portalPosition);
			// imprimir();
			drawShapePath();
		}

		else if(type == BonusTypes.Types.HIPOCAMPO)
		{
			for(int i = 0 ; i < 3;i++)
				this._hipocampoBonus.transform.GetChild(i).gameObject.GetComponent<RectTransform>().sizeDelta = this._bar;

			print("TilesCountHipocampo: " + _tileCountHipocampo);
			for(int i = 0 ; i < _tileCountHipocampo ; i++)
			{
				_map.GetChild(this._tilesHipocampo[i]).gameObject.GetComponent<Renderer>().material = _materialLightSensitive;   
				_map.GetChild(this._tilesHipocampo[i]).gameObject.GetComponent<SpriteRenderer>().color = new Color(255,255,255,255);   

				_tilesHipocampo[i] = 0;
			}

			this._tileCountHipocampo = 0;
			this._idActiveChildHipocampo = 0;			
		}

	}

    private void drawShapePath()
    {
		try{
			for(int i = 0 ; i < this._portalPath.Count-1; i++)
			{
				//Move to Up
				if(this._portalPath[i].y < this._portalPath[i+1].y)
				{
					Destroy(Instantiate(_portalPathTiles[0],new Vector2(_portalPath[i].x * GlobalVariables._widthTile,(_portalPath[i].y*-GlobalVariables._widthTile)-GlobalVariables._widthTile/2) - MapGeneratorController._offsetMap, Quaternion.identity,this._portalGroupGameObject) as GameObject,10f);
				}

				//Move to right
				else if(this._portalPath[i].x<this._portalPath[i+1].x)
				{
					Destroy(Instantiate(_portalPathTiles[1],new Vector2((_portalPath[i].x * GlobalVariables._widthTile) + GlobalVariables._widthTile/2,_portalPath[i].y*-GlobalVariables._widthTile) - MapGeneratorController._offsetMap, Quaternion.identity,this._portalGroupGameObject) as GameObject,10f);
				}

				//Move to Down
				else if(this._portalPath[i].y>this._portalPath[i+1].y)
				{
					Destroy(Instantiate(_portalPathTiles[0],new Vector2(_portalPath[i].x * GlobalVariables._widthTile,(_portalPath[i].y*-GlobalVariables._widthTile) + GlobalVariables._widthTile/2) - MapGeneratorController._offsetMap, Quaternion.identity,this._portalGroupGameObject) as GameObject,10f);
				}

				//Move to left
				else if(this._portalPath[i].x>this._portalPath[i+1].x)
				{
					Destroy(Instantiate(_portalPathTiles[1],new Vector2((_portalPath[i].x * GlobalVariables._widthTile)- GlobalVariables._widthTile/2,_portalPath[i].y*-GlobalVariables._widthTile) - MapGeneratorController._offsetMap, Quaternion.identity,this._portalGroupGameObject) as GameObject,10f);
				}
			}
		}

		catch(Exception e){}

		
    }

	public void imprimir()
	{
		for(int i = 0 ; i < _portalPath.Count;i++)
		{
			print(_portalPath[i]);
		}
	}
}
