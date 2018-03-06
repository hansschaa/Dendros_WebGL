using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViewController : MonoBehaviour
{
	static public Game _currentGameModel;
	static public GameState _gameState;
	public GameObject _iconStagePrefab;
	public GameObject _portalPrefab;
	public GameObject _player;
	public GameObject _enemy;

	[Header("LoadingScene")]
	public GameObject _loadingSceneCanvas;



	[Header("MainScene")]
	public GameObject _mainSceneCanvas;



	[Header("GarageScene")]
	public GameObject _garageSceneCanvas;
	public Text _lifesCountText;
	public Text _missionNumberDescriptionText;
	public Text _subTitleMissionText;
	public Text _descriptionMissionText;
	public Transform _iconStageGroup;



	[Header("ColourGarageScene")]
	public Color _stageNotComplete;
	public Color _stageCompleted;
	public Color _currentStageColor;



	[Header("GameScene")]
	public GameObject _gameSceneCanvas;
    public GameObject _gameObjectsGameScene;
	public GameObject _bonusList;
	public SpriteRenderer _brainImage;
	public Material _lightSensitiveMaterial;
	public Transform _bonusGroup;

	[HideInInspector]
	public GameObject _playerInstance;

	[HideInInspector]
	private GameObject _enemyInstance;

	[HideInInspector]
	public GameObject _portalInstance;

	[Header("FinalScene")]
	public GameObject _finalSceneCanvas;

	[Header("GameOverScene")]
	public GameObject _gameOverSceneCanvas;


	/// <summary>
	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>
	void Start()
	{
		ViewController._gameState = new GameState(GameState.States.MAINSCENE);
	}

	/// <summary>
	/// Update is called every frame, if the MonoBehaviour is enabled.
	/// </summary>
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Return))
		{
			if(!_loadingSceneCanvas.activeInHierarchy)
				pressEnter();
		}

		if(Input.GetKeyDown(KeyCode.Space))
		{
			pressSpace();
		}
	}

    public void pressSpace()
    {
        ViewController._gameState._state = GameState.States.GAMEOVER;
		this._gameSceneCanvas.SetActive(false);
		this._gameObjectsGameScene.SetActive(false);
		updateView();
    }

    public void pressEnter()
    {
		if(ViewController._gameState._state == GameState.States.MAINSCENE && this._mainSceneCanvas.activeInHierarchy)
		{
			ViewController._gameState._state = GameState.States.GARAGE;
			this._mainSceneCanvas.SetActive(false);
			if(!GlobalVariables._gameComplete && !GlobalVariables._iconStagesAdded)
			{
				addIconStagesGroup();
				GlobalVariables._iconStagesAdded= true;
			}
				

			this.gameObject.GetComponent<CountGarageController>().resetCoroutine();

		}

		else if(ViewController._gameState._state == GameState.States.GARAGE)
		{
			this.gameObject.GetComponent<CountGarageController>().stopCoroutine();
			ViewController._gameState._state = GameState.States.GAMESCENE;
			this._garageSceneCanvas.SetActive(false);
		}

		else if(ViewController._gameState._state == GameState.States.GAMESCENE)
		{
			this.gameObject.GetComponent<CountGarageController>().resetCoroutine();
			setColorIconStageGroup();
			this._gameSceneCanvas.SetActive(false);
			this._gameObjectsGameScene.SetActive(false);
			

			GlobalVariables._currentLevel++;
			

			if(GlobalVariables._currentLevel == GlobalVariables._totallyStages)
			{
				ViewController._gameState._state = GameState.States.FINALSCENE;
			}

			else
			{
				
				resetGameInstances();
				ViewController._gameState._state = GameState.States.GARAGE;
			}

			
		}

		else if(ViewController._gameState._state == GameState.States.FINALSCENE)
		{
			this._finalSceneCanvas.SetActive(false);
			ViewController._gameState._state = GameState.States.MAINSCENE;
			GlobalVariables._gameComplete = true;
			resetGame();
		}

		else if(ViewController._gameState._state == GameState.States.GAMEOVER)
		{
			this._gameOverSceneCanvas.SetActive(false);
			ViewController._gameState._state = GameState.States.MAINSCENE;
			GlobalVariables._gameComplete = false;
			resetGame();
		}

		updateView();
    }

    internal void createPortal(int iReset, int jReset)
    {
		Vector2 vector = new Vector2(jReset,iReset);
		this._portalInstance = Instantiate(_portalPrefab, new Vector2((jReset*0.16f), (iReset * -0.16f)) - MapGeneratorController._offsetMap, Quaternion.identity, this._gameObjectsGameScene.transform) as GameObject;
		_portalInstance.GetComponent<Bonus>()._position = vector;
		Destroy(_portalInstance,10f);
    }

    private void resetGame()
    {
		GlobalVariables._currentLevel = 0;
		if(GlobalVariables._gameComplete)
		{
			for(int i = 0 ; i < GlobalVariables._totallyStages ; i++)
				this._iconStageGroup.GetChild(i).gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().color = this._stageCompleted;

		}

		else
		{
			for(int i = 0 ; i < GlobalVariables._totallyStages ; i++)
				this._iconStageGroup.GetChild(i).gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().color = this._stageNotComplete;
		}

		
		
		resetGameInstances();
    }

    private void resetGameInstances()
    {
		this.gameObject.GetComponent<MechanicController>().stopPortalCoroutine();
		this.gameObject.GetComponent<BonusController>().stopCoroutine();
		this.gameObject.GetComponent<MechanicController>()._timeCountText.color = new Color32(0,255,0,255);
		GlobalVariables._playerVelocity = 0.5f;
		GlobalVariables._currentLifes = 3;
		GlobalVariables._yellowBonus = false;
		GlobalVariables._purpleBonus = false;
		GlobalVariables._changeDirection = false;
        Destroy(_enemyInstance);
		Destroy(_playerInstance);
		Destroy(_portalInstance);
		// Transform map = this._gameObjectsGameScene.transform.GetChild(0);

		//erase map
		eraseChildGameObjects(this._gameObjectsGameScene.transform.GetChild(0));

		//erase hearts
		// eraseChildGameObjects(this.gameObject.GetComponent<MechanicController>()._heartLifesList.transform);

		// eraseChildGameObjects(this._bonusList.transform);
		eraseChildGameObjects(this._bonusGroup);
		eraseChildGameObjects(this.GetComponent<BonusController>()._portalGroupGameObject);

		// foreach(Transform go in map.transform)
		// 	Destroy(go.gameObject);
		

		foreach(Transform go in this.gameObject.GetComponent<MechanicController>()._heartLifesList.transform)
			go.gameObject.SetActive(true);

		foreach(Transform go in this._bonusList.transform)
			go.gameObject.SetActive(false);

		// foreach(Transform tr in _bonusGroup)
		// 		Destroy(tr.gameObject);
		
		// foreach(Transform tr in _bonusGroup)
		// 		Destroy(tr.gameObject);
    }

	public void eraseChildGameObjects(Transform parent)
	{
		foreach(Transform tr in parent)
				Destroy(tr.gameObject);
	}

	

    private void updateView()
    {
		if(ViewController._gameState._state == GameState.States.GAMESCENE)
		{
			this.gameObject.GetComponent<MechanicController>().initializatePortalCoroutine(20);
			this.gameObject.GetComponent<MapGeneratorController>().drawMap();
			this._gameSceneCanvas.SetActive(true);
			this._gameObjectsGameScene.SetActive(true);
			updateBonusList();
			this.gameObject.GetComponent<BonusController>().initializeCorroutines();
			this._playerInstance = Instantiate(_player, getNewPosition(false) - MapGeneratorController._offsetMap, Quaternion.identity, this._gameObjectsGameScene.transform) as GameObject;
			// this._enemyInstance = Instantiate(_enemy, getNewPosition(true) - MapGeneratorController._offsetMap, Quaternion.identity, this._gameObjectsGameScene.transform) as GameObject;

		}

		else if(ViewController._gameState._state == GameState.States.GARAGE)
		{
			updateGameModel();
			changeGarageTexts();
			setColorIconStageGroup();
			this._garageSceneCanvas.SetActive(true);
			
		}

		else if(ViewController._gameState._state == GameState.States.MAINSCENE)
		{		
			this._mainSceneCanvas.SetActive(true);
		}

		else if(ViewController._gameState._state == GameState.States.FINALSCENE)
		{		
			this._finalSceneCanvas.SetActive(true);
		}
		
		else if(ViewController._gameState._state == GameState.States.GAMEOVER)
		{		
			this._gameOverSceneCanvas.SetActive(true);
		}

		this.gameObject.GetComponent<MusicController>().updateMusic();
    }

    private void updateBonusList()
    {
        for(int i = 0 ; i < ViewController._currentGameModel._zones.Length; i++)
		{
			switch(ViewController._currentGameModel._zones[i])
			{
				case "Occipital":
					this._bonusList.transform.GetChild(0).gameObject.SetActive(true);
				break;

				case "Premotora":
					this._bonusList.transform.GetChild(1).gameObject.SetActive(true);
				break;

				case "Brain":
					this._bonusList.transform.GetChild(0).gameObject.SetActive(true);
					this._bonusList.transform.GetChild(1).gameObject.SetActive(true);
					this._bonusList.transform.GetChild(2).gameObject.SetActive(true);
					GlobalVariables._purpleBonus = true;
				break;

				case "Cerebelo":
					this._bonusList.transform.GetChild(2).gameObject.SetActive(true);
				break;

				case "Teleport":
					GlobalVariables._yellowBonus = true;
				break;
			}
		}
    }

    public Vector2 getNewPosition(bool isForEnemy)
    {
		int iReset=0;
		int jReset=0;
		do
		{
			iReset = UnityEngine.Random.Range(0, GlobalVariables._iMaxMatrix); 
			jReset = UnityEngine.Random.Range(0, GlobalVariables._jMaxMatrix); 
		} while(ViewController._currentGameModel._map[iReset,jReset] != 15);

		if(isForEnemy)
		{
			GlobalVariables._xPosEnemy = jReset;
			GlobalVariables._yPosEnemy = iReset;
			return new Vector2((GlobalVariables._xPosEnemy*0.16f), (- GlobalVariables._yPosEnemy * 0.16f));
		}
			
		
		else
		{
			GlobalVariables._xPosPlayer = jReset;
			GlobalVariables._yPosPlayer = iReset;
			return new Vector2((GlobalVariables._xPosPlayer*0.16f), (- GlobalVariables._yPosPlayer * 0.16f));
		}
			
    }

    private void addIconStagesGroup()
    {
		GameObject iconStage;
        for(int i = 0 ; i < GlobalVariables._totallyStages ; i++)
		{
			iconStage = Instantiate(_iconStagePrefab, _iconStageGroup);
			iconStage.transform.GetChild(1).gameObject.GetComponent<Text>().text = (i + 1).ToString();
		}
    }

	private void setColorIconStageGroup()
	{
		//For stages Completed
		for (int i = 0 ; i < GlobalVariables._currentLevel; i++)
		{
			this._iconStageGroup.GetChild(i).gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().color = this._stageCompleted;
		}

		this._iconStageGroup.GetChild(GlobalVariables._currentLevel).gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().color = this._currentStageColor;
	}

    private void changeGarageTexts()
    {
		this._lifesCountText.text = GlobalVariables._currentLifes.ToString();
		this._missionNumberDescriptionText.text = ViewController._currentGameModel._missionNumber;
		this._descriptionMissionText.text = ViewController._currentGameModel._descriptionMission;
    }

	private void updateGameModel()
    {
		string missionNumber = JsonController.jsonDataStages["Stages"][GlobalVariables._currentLevel]["missionNumber"].ToString();
		string descriptionMission = JsonController.jsonDataStages["Stages"][GlobalVariables._currentLevel]["descriptionMission"].ToString();
		int entryTime = (int)JsonController.jsonDataStages["Stages"][GlobalVariables._currentLevel]["entryTime"];
		int postEntryTime = (int)JsonController.jsonDataStages["Stages"][GlobalVariables._currentLevel]["postEntryTime"];
		string[] zones = addBonusToArray();
		int[,] map = generateMap();

        ViewController._currentGameModel = new Game(missionNumber, descriptionMission,entryTime, postEntryTime,zones,map);
    }

    private string[] addBonusToArray()
    {
		string[] zones = new string[JsonController.jsonDataStages["Stages"][GlobalVariables._currentLevel]["brainZone"].Count];
		for(int i = 0 ; i < JsonController.jsonDataStages["Stages"][GlobalVariables._currentLevel]["brainZone"].Count; i++)
			zones[i] = JsonController.jsonDataStages["Stages"][GlobalVariables._currentLevel]["brainZone"][i].ToString();

		return zones;
    }

    private int[,] generateMap()
    {

		int[,] map = new int[GlobalVariables._iMaxMatrix, GlobalVariables._jMaxMatrix];
		int contador = 0;

        for(int i = 0 ; i < GlobalVariables._iMaxMatrix; i++)
			for(int j = 0 ; j < GlobalVariables._jMaxMatrix; j++)
			{
				map[i,j] =  (int) JsonController.jsonDataStages["Stages"][GlobalVariables._currentLevel]["map"][contador];
				contador++;
			}

		// showArray(map);

		return map;
    }

	public void showArray(int[,] array)
	{
		for(int i = 0 ; i < GlobalVariables._iMaxMatrix; i++)
		{
			for(int j = 0 ; j < GlobalVariables._jMaxMatrix; j++)
			{
				Debug.Log(array[i,j]);

			}

			print("\n");

		}		
	}	

	public void createPlayer()
    {
        Destroy(this._playerInstance);
		this._playerInstance = Instantiate(_player, this.getNewPosition(false)- MapGeneratorController._offsetMap, Quaternion.identity, this._gameObjectsGameScene.transform) as GameObject;
    }
}
