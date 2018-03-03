using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusController : MonoBehaviour 
{
	Coroutine _changeParametersCoroutine;
	Coroutine _bonusRespawnCoroutine;
	SearchManager _searchManager;
	public GameObject _shape;
	public List<Vector2> _portalPath;

	[Header("Bonus GameObjects")]
	public RectTransform _lightBonus;
	public RectTransform _velocityBonus;
	public RectTransform _coordinationBonus;
	public GameObject _bonusPrefab;
	public GameObject _lightBonusInstance;
	public GameObject _velocityBonusInstance;
	public GameObject _coordinationBonusInstance;
	public GameObject _teleportBonusInstance;
	public GameObject _portalBonusInstance;
	public Transform _portalGroupGameObject;
	public Sprite[] _bonusTextures;
	public Material _material;

	[Header("Parameters")]
	public float _decreaseValue;
	public float _initializeChangeParametersDelay;
	public float _respawnDelay;
	private float _playerVelocity; 
	[HideInInspector]
	public Vector2 _bar;
	
	/// <summary>
	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>
	void Start()
	{
		this._portalPath = new List<Vector2>();
		this._searchManager = new SearchManager();
		
		this._decreaseValue = this._lightBonus.sizeDelta.x/50;
		this._bar = this._velocityBonus.sizeDelta;
		this._playerVelocity = GlobalVariables._playerVelocity;
		this._material.color = new Color32(255,255,255,255);
	}

	/// <summary>
	/// Update is called every frame, if the MonoBehaviour is enabled.
	/// </summary>
	void Update()
	{
		// if(_portalPath.Count>0)
		// {
		// 	// Debug.DrawLine(p)
		// 	for(int i = 0 ; i < _portalPath.Count-1; i++)
		// 		Debug.DrawLine(new Vector2(_portalPath[i].x * 0.16f,_portalPath[i].y*-0.16f) - MapGeneratorController._offsetMap, new Vector2(_portalPath[i+1].x * 0.16f, _portalPath[i+1].y*-0.16f) - MapGeneratorController._offsetMap);

		// }
	}

    private IEnumerator changeParametersBonus()
    {
		while(true)
		{
			
			if(this._lightBonus.transform.parent.gameObject.activeInHierarchy && this._lightBonus.sizeDelta.x-_decreaseValue>=0)
			{
				this._lightBonus.sizeDelta -= new Vector2(_decreaseValue,0);
				if(this._lightBonus.sizeDelta.x <= 5)
					_material.color = new Color32(20,20,20,255);

				else if(this._lightBonus.sizeDelta.x < (_bar.x/3))
					_material.color = new Color32(80,80,80,255);

				else if(this._lightBonus.sizeDelta.x < (_bar.x*2)/3)
					_material.color = new Color32(172,172,172,255);
			}
			
			if(this._velocityBonus.transform.parent.gameObject.activeInHierarchy && this._velocityBonus.sizeDelta.x-_decreaseValue>=0)
			{
				this._velocityBonus.sizeDelta -= new Vector2(_decreaseValue,0);
				this._playerVelocity -=0.002f;
				// GameObject.Find("Player(Clone)").gameObject.GetComponent<PlayerBehaviour>().speed = this._playerVelocity;
			}

			if(this._coordinationBonus.transform.parent.gameObject.activeInHierarchy && this._coordinationBonus.sizeDelta.x-_decreaseValue>=0)
			{
				this._coordinationBonus.sizeDelta -= new Vector2(_decreaseValue,0);				
			}

			if(this._coordinationBonus.sizeDelta.x-_decreaseValue<=0 && !GlobalVariables._changeDirection)
			{
				GlobalVariables._changeDirection = true;
			}

			yield return new WaitForSeconds(_initializeChangeParametersDelay);

		}
		
    }

	internal void stopCoroutine()
    {
		StopCoroutine(_changeParametersCoroutine);
		this._lightBonus.sizeDelta = _bar;
		this._velocityBonus.sizeDelta = _bar;
		this._coordinationBonus.sizeDelta = _bar;
		_material.color = new Color32(255,255,255,255);
       
	
    }

	internal void initializeCorroutines()
    {
		this._changeParametersCoroutine = StartCoroutine(changeParametersBonus());
        this._bonusRespawnCoroutine = StartCoroutine(bonusRespawn());
		
    }

    private IEnumerator bonusRespawn()
    {
		while(true)
		{
			if(this._lightBonus.transform.parent.gameObject.activeInHierarchy)
			{
				createBonus(BonusTypes.Types.LIGHT);
			}
			
			if(this._velocityBonus.transform.parent.gameObject.activeInHierarchy)
			{
				createBonus(BonusTypes.Types.VELOCITY);
			}

			if(this._coordinationBonus.transform.parent.gameObject.activeInHierarchy)
			{
				createBonus(BonusTypes.Types.COORDINATION);		
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
		Vector2 position = searchForPosition();
		
		if(type == BonusTypes.Types.LIGHT && _lightBonusInstance == null)
		{
				_lightBonusInstance = Instantiate(_bonusPrefab, new Vector2(position.x * 0.16f, position.y*-0.16f) - MapGeneratorController._offsetMap, Quaternion.identity, this.gameObject.GetComponent<ViewController>()._bonusGroup.transform) as GameObject;
				_lightBonusInstance.GetComponent<Bonus>()._position = position;
				_lightBonusInstance.GetComponent<Bonus>()._myType = BonusTypes.Types.LIGHT;
				_lightBonusInstance.GetComponent<SpriteRenderer>().sprite = this._bonusTextures[0];
		}

		else if(type == BonusTypes.Types.VELOCITY && _velocityBonusInstance == null)
		{
				_velocityBonusInstance = Instantiate(_bonusPrefab, new Vector2(position.x * 0.16f, position.y*-0.16f)- MapGeneratorController._offsetMap, Quaternion.identity, this.gameObject.GetComponent<ViewController>()._bonusGroup.transform) as GameObject;
				_velocityBonusInstance.GetComponent<Bonus>()._position = position;
				_velocityBonusInstance.GetComponent<Bonus>()._myType = BonusTypes.Types.VELOCITY;
				_velocityBonusInstance.GetComponent<SpriteRenderer>().sprite = this._bonusTextures[1];
		}

		else if(type == BonusTypes.Types.COORDINATION && _coordinationBonusInstance == null)
		{
				_coordinationBonusInstance = Instantiate(_bonusPrefab, new Vector2(position.x * 0.16f, position.y*-0.16f)- MapGeneratorController._offsetMap, Quaternion.identity, this.gameObject.GetComponent<ViewController>()._bonusGroup.transform) as GameObject;
				_coordinationBonusInstance.GetComponent<Bonus>()._position = position;
				_coordinationBonusInstance.GetComponent<Bonus>()._myType = BonusTypes.Types.COORDINATION;
				_coordinationBonusInstance.GetComponent<SpriteRenderer>().sprite = this._bonusTextures[2];
		}

		else if(type == BonusTypes.Types.TELEPORT && _teleportBonusInstance == null)
		{
				_teleportBonusInstance = Instantiate(_bonusPrefab, new Vector2(position.x * 0.16f, position.y*-0.16f) - MapGeneratorController._offsetMap, Quaternion.identity, this.gameObject.GetComponent<ViewController>()._bonusGroup.transform) as GameObject;
				_teleportBonusInstance.GetComponent<Bonus>()._position = position;
				_teleportBonusInstance.GetComponent<Bonus>()._myType = BonusTypes.Types.TELEPORT;
				_teleportBonusInstance.GetComponent<SpriteRenderer>().sprite = this._bonusTextures[3];
		}

		else if(type == BonusTypes.Types.PORTAL && _portalBonusInstance == null && this.GetComponent<ViewController>()._portalInstance == null)
		{
				_portalBonusInstance = Instantiate(_bonusPrefab, new Vector2(position.x * 0.16f, position.y*-0.16f) - MapGeneratorController._offsetMap, Quaternion.identity, this.gameObject.GetComponent<ViewController>()._bonusGroup) as GameObject;
				_portalBonusInstance.GetComponent<Bonus>()._position = position;
				_portalBonusInstance.GetComponent<Bonus>()._myType = BonusTypes.Types.PORTAL;
				_portalBonusInstance.GetComponent<SpriteRenderer>().sprite = this._bonusTextures[4];
		}
		
    }

    private Vector2 searchForPosition()
    {

		int iReset=0;
		int jReset=0;
		do
		{
			iReset = UnityEngine.Random.Range(0, GlobalVariables._iMaxMatrix); 
			jReset = UnityEngine.Random.Range(0, GlobalVariables._jMaxMatrix); 
		} while(ViewController._currentGameModel._map[iReset,jReset] != 15);

		return new Vector2(jReset,iReset);
    }

    public void resetBonusInstances()
	{
		_lightBonusInstance = null;
		_velocityBonusInstance = null;
		_coordinationBonusInstance = null;
		_teleportBonusInstance = null;
		_portalBonusInstance = null;
		this._playerVelocity = GlobalVariables._playerVelocity;
	}

	public void bonusCollision(BonusTypes.Types type, Vector2 position)
	{
		if(type == BonusTypes.Types.LIGHT)
		{
			this._material.color = new Color32(255,255,255,255);
			this._lightBonus.sizeDelta = _bar;
		}

		else if(type == BonusTypes.Types.VELOCITY)
		{
			this._playerVelocity = GlobalVariables._playerVelocity;
			GameObject.Find("Player(Clone)").gameObject.GetComponent<PlayerBehaviour>().speed = this._playerVelocity;
			this._velocityBonus.sizeDelta = _bar;
		}

		else if(type == BonusTypes.Types.COORDINATION)
		{
			GlobalVariables._changeDirection= false;
			this._coordinationBonus.sizeDelta = _bar;
		}

		else if(type == BonusTypes.Types.TELEPORT)
		{
			this.GetComponent<ViewController>().createPlayer();
		}

		else if(type == BonusTypes.Types.PORTAL)
		{
			
			this._portalPath.Clear();
			GetComponent<MechanicController>().stopPortalCoroutine();
			GetComponent<MechanicController>().putPortal();
			Vector2 portalPosition = GetComponent<ViewController>()._portalInstance.GetComponent<Bonus>()._position;
			this._portalPath = _searchManager.encontrarCamino(position, portalPosition);
			imprimir();
			drawShapePath();
		}

	}

    private void drawShapePath()
    {
        for(int i = 0 ; i < this._portalPath.Count ; i++)
			Destroy(Instantiate(_shape,new Vector2(_portalPath[i].x * 0.16f,_portalPath[i].y*-0.16f) - MapGeneratorController._offsetMap, Quaternion.identity,this._portalGroupGameObject) as GameObject,10f);
    }

	public void imprimir()
	{
		for(int i = 0 ; i < _portalPath.Count;i++)
		{
			print(_portalPath[i]);
		}
	}
}
