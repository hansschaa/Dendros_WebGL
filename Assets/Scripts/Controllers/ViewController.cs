using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Prime31.TransitionKit;

public class ViewController : MonoBehaviour
{
	static public Game _currentGameModel;
	static public GameState _gameState;
	public GameObject _iconStagePrefab;
	public GameObject _portalPrefab;
	public GameObject _player;
	public GameObject _enemy;
	public SoundController _soundController;
	private bool _canPressEnter;

	[Header("LoadingScene")]
	public GameObject _loadingSceneCanvas;



	[Header("MainScene")]
	public GameObject _mainSceneCanvas;
	public GameObject _gameObjectsMainScene;


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
	public Transform _brain;
	public Material _lightSensitiveMaterial;
	public Transform _bonusGroup;
	public GameObject _leaveText;

	[HideInInspector]
	public GameObject _playerInstance;
	[HideInInspector]
	private GameObject _enemyInstance;
	[HideInInspector]
	public GameObject _portalInstance;


	[Header("FinalScene")]
	public GameObject _finalSceneCanvas;
	public GameObject _gameObjectsFinalView;
	// public Transform _camera;


	[Header("GameOverScene")]
	public GameObject _gameOverSceneCanvas;

	/// <summary>
	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>
	void Start()
	{
		this._canPressEnter = true;
		ViewController._gameState = new GameState(GameState.States.MAINSCENE);
	}

	/// <summary>
	/// Update is called every frame, if the MonoBehaviour is enabled.
	/// </summary>
	void Update()
	{
		if(Input.GetButtonDown("Fire1"))
		{
			if(!_loadingSceneCanvas.activeInHierarchy && !this._gameSceneCanvas.activeInHierarchy)
				pressEnter();
		}

		else if(Input.GetButtonDown("Fire2"))
		{
			Application.Quit();
		}

		// if(Input.GetKeyDown(KeyCode.Space))
		// {
		// 	pressSpace();
		// }
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
		
		if(ViewController._gameState._state == GameState.States.MAINSCENE && this._mainSceneCanvas.activeInHierarchy && this._canPressEnter)
		{
			this._canPressEnter = false;
			this._soundController.playSound(0);
			ViewController._gameState._state = GameState.States.GARAGE;
			this.GetComponent<AnimationController>().stopAnimations(GameState.States.MAINSCENE);
			
			if(!GlobalVariables._gameComplete && !GlobalVariables._iconStagesAdded)
			{
				addIconStagesGroup();
				GlobalVariables._iconStagesAdded= true;
			}

			var fader = new FadeTransition()
			{
				fadedDelay = 0.7f,
				fadeToColor = Color.black
			};
			TransitionKit.instance.transitionWithDelegate( fader );

			Invoke("updateView",0.7f);
		}

		else if(ViewController._gameState._state == GameState.States.GARAGE && this._canPressEnter)
		{
			this._soundController.playSound(0);
			this.gameObject.GetComponent<CountGarageController>().stopCoroutine();
			ViewController._gameState._state = GameState.States.GAMESCENE;
			this._garageSceneCanvas.SetActive(false);
			updateView();
		}

		else if(ViewController._gameState._state == GameState.States.GAMESCENE)
		{
			// this.gameObject.GetComponent<CountGarageController>().resetCoroutine();
			// setColorIconStageGroup();
			this._gameSceneCanvas.SetActive(false);
			this._gameObjectsGameScene.SetActive(false);
			
			if(GlobalVariables._currentLifes==0 && GlobalVariables._globalLifes==0)
			{
				resetGameInstances();
				ViewController._gameState._state = GameState.States.GAMEOVER;
			}

			else if(GlobalVariables._currentLifes==0 && GlobalVariables._globalLifes!=0)
			{
				resetGameInstances();
				ViewController._gameState._state = GameState.States.GARAGE;
			}

			else
			{
				GlobalVariables._currentLevel++;
				if(GlobalVariables._currentLevel == GlobalVariables._totallyStages)
				{
					GlobalVariables._currentLevel = 0;
					ViewController._gameState._state = GameState.States.FINALSCENE;
				}

				else
				{
					resetGameInstances();
					ViewController._gameState._state = GameState.States.GARAGE;
					updateIconStageGroup();

				}
			}

			updateView();
		}

		else if(ViewController._gameState._state == GameState.States.FINALSCENE)
		{
			this._soundController.GetComponent<AudioSource>().Stop();
			this._finalSceneCanvas.SetActive(false);
			this._gameObjectsFinalView.SetActive(false);
			// this._camera.position = new Vector3(0,0,-10);

			ViewController._gameState._state = GameState.States.MAINSCENE;
			GlobalVariables._gameComplete = false;
			// this._gameObjectsFinalView.GetComponent<Animator>().SetBool("fade",false); 
			resetGame();
			updateView();
		}

		else if(ViewController._gameState._state == GameState.States.GAMEOVER)
		{
			// this.GetComponent<AudioSource>().enabled = false;
			
			this._gameOverSceneCanvas.SetActive(false);
			ViewController._gameState._state = GameState.States.MAINSCENE;
			GlobalVariables._gameComplete = false;
			resetGame();
			// Invoke("updateView",f);
			updateView();

		}
    }

    internal void createPortal(int iReset, int jReset)
    {
		Vector2 vector = new Vector2(jReset,iReset);
		this._portalInstance = Instantiate(_portalPrefab, new Vector2((jReset*GlobalVariables._widthTile), (iReset * -GlobalVariables._widthTile)+ GlobalVariables._widthTile) - MapGeneratorController._offsetMap, Quaternion.identity, this._gameObjectsGameScene.transform) as GameObject;
		_portalInstance.GetComponent<Bonus>()._position = vector;
		try{ Destroy(_portalInstance,10f); }catch(Exception e){}
		
    }

    private void resetGame()
    {
		GlobalVariables._currentLevel = 0;
		GlobalVariables._globalLifes = 3;
		GlobalVariables._currentLifes = 3;
		if(GlobalVariables._gameComplete)
		{
			for(int i = 0 ; i < GlobalVariables._totallyStages ; i++)
			{
				updateIcon(1,i+1,_iconStageGroup.GetChild(i).gameObject);
				// this._iconStageGroup.GetChild(i).gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().color = this._stageCompleted;
			}
				

		}

		else
		{
			//Mejor borrarlas y crearlas nuevamente hasta la etapa 5
			for(int i = 0 ; i < GlobalVariables._totallyStages ; i++)
				updateIcon(3,i+1,_iconStageGroup.GetChild(i).gameObject);
		}

		resetGameInstances();
    }

    public void resetGameInstances()
    {
		this._leaveText.SetActive(false);
		this.gameObject.GetComponent<MechanicController>().stopPortalCoroutine();
		this.gameObject.GetComponent<BonusController>().stopCoroutine();
		
		this.gameObject.GetComponent<MechanicController>()._timeCountText.color = new Color32(0,255,0,255);
		// GlobalVariables._playerVelocity = GlobalVariables._playerVelocity;
		GlobalVariables._currentLifes = 3;
		GlobalVariables._yellowBonus = false;
		GlobalVariables._purpleBonus = false;
		GlobalVariables._changeDirection = false;
		GlobalVariables._followPlayer = true;
		GlobalVariables._stageComplete = false;
		GlobalVariables._allowPurpleBonus= false;
		this._canPressEnter = true;

        Destroy(_enemyInstance);
		Destroy(_playerInstance);
		Destroy(_portalInstance);

		//erase map
		eraseChildGameObjects(this._gameObjectsGameScene.transform.GetChild(0));

		eraseChildGameObjects(this._bonusGroup);
		eraseChildGameObjects(this.GetComponent<BonusController>()._portalGroupGameObject);

		foreach(Transform go in this.gameObject.GetComponent<MechanicController>()._heartLifesList.transform)
			go.gameObject.SetActive(true);

		foreach(Transform go in this._bonusList.transform)
			go.gameObject.SetActive(false);
    }

	public void eraseChildGameObjects(Transform parent)
	{
		foreach(Transform tr in parent)
				Destroy(tr.gameObject);
	}

	

    public void updateView()
    {
		if(ViewController._gameState._state == GameState.States.GAMESCENE)
		{
			

			this._canPressEnter = true;
			this.gameObject.GetComponent<MechanicController>().initializatePortalCoroutine(20);
			this.gameObject.GetComponent<MapGeneratorController>().drawMap();
			this._gameSceneCanvas.SetActive(true);
			this._gameObjectsGameScene.SetActive(true);
			updateBonusList();
			updateBrainZone();
			this.gameObject.GetComponent<BonusController>().initializeCorroutines();
			this._playerInstance = Instantiate(_player, getNewPosition(false) - MapGeneratorController._offsetMap, Quaternion.identity, this._gameObjectsGameScene.transform) as GameObject;
			this._enemyInstance = Instantiate(_enemy, getNewPosition(true) - MapGeneratorController._offsetMap, Quaternion.identity, this._gameObjectsGameScene.transform) as GameObject;
		}

		else if(ViewController._gameState._state == GameState.States.GARAGE)
		{
			if(!this.GetComponent<AudioSource>().enabled)
				this.GetComponent<AudioSource>().enabled = true;
				
			this._canPressEnter = true;
			this.gameObject.GetComponent<CountGarageController>().resetCoroutine();
			this._gameObjectsMainScene.SetActive(false);
			this._mainSceneCanvas.SetActive(false);
			updateGameModel();
			updateIconStageGroup();
			changeGarageTexts();
			// setColorIconStageGroup();
			this._garageSceneCanvas.SetActive(true);
			
		}

		else if(ViewController._gameState._state == GameState.States.MAINSCENE)
		{		
			if(!this.GetComponent<AudioSource>().enabled)
				this.GetComponent<AudioSource>().enabled = true;

			this._mainSceneCanvas.SetActive(true);
			this._gameObjectsMainScene.SetActive(true);
			//resetearcoroutina de los personajes
			this.GetComponent<AnimationController>().playAnimations(GameState.States.MAINSCENE);
		}

		else if(ViewController._gameState._state == GameState.States.FINALSCENE)
		{		
			this._soundController.playSound(3);
			this._gameObjectsFinalView.SetActive(true);
			this._finalSceneCanvas.SetActive(true);
		
		
			// StartCoroutine(waitThenCallback(2.45f, () => 
        //  { this._gameObjectsFinalView.transform.GetChild(1).gameObject.GetComponent<Animator>().SetBool("fade",true); }));
		}
		
		else if(ViewController._gameState._state == GameState.States.GAMEOVER)
		{		
			_soundController.playSound(5);
			this._gameOverSceneCanvas.SetActive(true);
		}

		this.gameObject.GetComponent<MusicController>().updateMusic();
    }

    private void updateBrainZone()
    {

		foreach(Transform tr in _brain)
			tr.gameObject.SetActive(false);

        if(ViewController._currentGameModel._bonusList.Count==1)
		{
			if(ViewController._currentGameModel._bonusList.Contains("green"))
			{
				_brain.GetChild(0).gameObject.SetActive(true);

			}

			if(ViewController._currentGameModel._bonusList.Contains("orange"))
			{
				_brain.GetChild(1).gameObject.SetActive(true);
			}

			if(ViewController._currentGameModel._bonusList.Contains("gray"))
			{
				_brain.GetChild(2).gameObject.SetActive(true);
			}

		}

		else if(ViewController._currentGameModel._bonusList.Count==2)
		{
			

		}

		else if(ViewController._currentGameModel._bonusList.Count==3)
		{
			if(ViewController._currentGameModel._bonusList.Contains("green") && ViewController._currentGameModel._bonusList.Contains("yellow") && ViewController._currentGameModel._bonusList.Contains("gray"))
			{
				_brain.GetChild(3).gameObject.SetActive(true);
			}

			else if(ViewController._currentGameModel._bonusList.Contains("green") && ViewController._currentGameModel._bonusList.Contains("orange") && ViewController._currentGameModel._bonusList.Contains("yellow"))
			{
				_brain.GetChild(4).gameObject.SetActive(true);
				
			}

		}

    }

    private void updateBonusList()
    {
        for(int i = 0 ; i < ViewController._currentGameModel._bonusList.Count; i++)
		{
			switch(ViewController._currentGameModel._bonusList[i])
			{
				case "all":
					this._bonusList.transform.GetChild(0).gameObject.SetActive(true);
					this._bonusList.transform.GetChild(1).gameObject.SetActive(true);
					this._bonusList.transform.GetChild(2).gameObject.SetActive(true);
					GlobalVariables._yellowBonus = true;
					GlobalVariables._purpleBonus = true;
				break;
				case "green":
					this._bonusList.transform.GetChild(0).gameObject.SetActive(true);
				break;

				case "orange":
					this._bonusList.transform.GetChild(1).gameObject.SetActive(true);
				break;

				case "purple":
					GlobalVariables._purpleBonus = true;
				break;

				case "gray":
					this._bonusList.transform.GetChild(2).gameObject.SetActive(true);
				break;

				case "yellow":
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
		} while(ViewController._currentGameModel._map[iReset,jReset] == -1);

		if(isForEnemy)
		{
			GlobalVariables._xPosEnemy = jReset;
			GlobalVariables._yPosEnemy = iReset;
			return new Vector2((GlobalVariables._xPosEnemy*GlobalVariables._widthTile), (- GlobalVariables._yPosEnemy* GlobalVariables._widthTile));
		}
			
		
		else
		{
			GlobalVariables._xPosPlayer = jReset;
			GlobalVariables._yPosPlayer = iReset;
			// GlobalVariables._xPosPlayer = 2;
			// GlobalVariables._yPosPlayer = 2;
			return new Vector2((GlobalVariables._xPosPlayer*GlobalVariables._widthTile), (GlobalVariables._yPosPlayer * -GlobalVariables._widthTile));
			// return new Vector2((2*GlobalVariables._widthTile), (2 * -GlobalVariables._widthTile));
		}
			
    }

    private void addIconStagesGroup()
    {
		GameObject iconStage;
        for(int i = 0 ; i < GlobalVariables._totallyStages && i < 5 ; i++)
		{
			iconStage = Instantiate(_iconStagePrefab, _iconStageGroup);
			iconStage.transform.GetChild(1).gameObject.GetComponent<Text>().text = (i + 1).ToString();
		}
    }

	private void updateIconStageGroup()
	{
		if(GlobalVariables._totallyStages < 6)
		{
			for (int i = 0 ; i < GlobalVariables._currentLevel; i++)
			{
				
				this._iconStageGroup.GetChild(i).gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().color = this._stageCompleted;
			}

			this._iconStageGroup.GetChild(GlobalVariables._currentLevel).gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().color = this._currentStageColor;
		}

		else if(GlobalVariables._totallyStages >= 6)
		{
			if(GlobalVariables._currentLevel<3)
			{
				for (int i = 0 ; i < GlobalVariables._currentLevel; i++)
				{
					updateIcon(1,i+1,_iconStageGroup.GetChild(i).gameObject);
				}

				updateIcon(2,GlobalVariables._currentLevel+1,_iconStageGroup.GetChild(GlobalVariables._currentLevel).gameObject);
			}

			else if(GlobalVariables._currentLevel>=3 && GlobalVariables._currentLevel < GlobalVariables._totallyStages-2)
			{
				updateIcon(1,GlobalVariables._currentLevel-1,_iconStageGroup.GetChild(0).gameObject);
				updateIcon(1,GlobalVariables._currentLevel,_iconStageGroup.GetChild(1).gameObject);
				updateIcon(2,GlobalVariables._currentLevel+1,_iconStageGroup.GetChild(2).gameObject);
				if(GlobalVariables._gameComplete)
				{
					updateIcon(1,GlobalVariables._currentLevel+2,_iconStageGroup.GetChild(3).gameObject);
					updateIcon(1,GlobalVariables._currentLevel+3,_iconStageGroup.GetChild(4).gameObject);

				}

				else
				{
					updateIcon(3,GlobalVariables._currentLevel+2,_iconStageGroup.GetChild(3).gameObject);
					updateIcon(3,GlobalVariables._currentLevel+3,_iconStageGroup.GetChild(4).gameObject);
				}
				
			}

			else if(GlobalVariables._currentLevel == GlobalVariables._totallyStages-2)
			{
				updateIcon(1,GlobalVariables._currentLevel,_iconStageGroup.GetChild(2).gameObject);
				updateIcon(2,GlobalVariables._currentLevel+1,_iconStageGroup.GetChild(3).gameObject);
			}

			else if(GlobalVariables._currentLevel == GlobalVariables._totallyStages-1)
			{
				updateIcon(1,GlobalVariables._currentLevel,_iconStageGroup.GetChild(3).gameObject);
				updateIcon(2,GlobalVariables._currentLevel+1,_iconStageGroup.GetChild(4).gameObject);
			}

		}

	}

	public void updateIcon(int iconType,int stageNumber,GameObject go)
	{
		switch(iconType)
		{
			case 1:
				go.transform.GetChild(0).GetComponent<Image>().color = this._stageCompleted;
				go.transform.GetChild(1).GetComponent<Text>().text = stageNumber.ToString();
				break;
			case 2:
				go.transform.GetChild(0).GetComponent<Image>().color = this._currentStageColor;
				go.transform.GetChild(1).GetComponent<Text>().text = stageNumber.ToString();
				break;
			case 3:
				go.transform.GetChild(0).GetComponent<Image>().color = this._stageNotComplete;
				go.transform.GetChild(1).GetComponent<Text>().text = stageNumber.ToString();
				break;
		}
	}

	// private void setColorIconStageGroup()
	// {
	// 	//For stages Completed
	// 	for (int i = 0 ; i < GlobalVariables._currentLevel; i++)
	// 	{
	// 		this._iconStageGroup.GetChild(i).gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().color = this._stageCompleted;
	// 	}

	// 	this._iconStageGroup.GetChild(GlobalVariables._currentLevel).gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().color = this._currentStageColor;
	// }

    private void changeGarageTexts()
    {
		this._lifesCountText.text = GlobalVariables._globalLifes.ToString();
		this._missionNumberDescriptionText.text = ViewController._currentGameModel._missionNumber;
		this._descriptionMissionText.text = ViewController._currentGameModel._descriptionMission;
    }

	private void updateGameModel()
    {
		string missionNumber = JsonController.jsonDataStages["Stages"][GlobalVariables._currentLevel]["missionNumber"].ToString();
		string descriptionMission = JsonController.jsonDataStages["Stages"][GlobalVariables._currentLevel]["descriptionMission"].ToString();
		int entryTime = (int)JsonController.jsonDataStages["Stages"][GlobalVariables._currentLevel]["entryTime"];
		int postEntryTime = (int)JsonController.jsonDataStages["Stages"][GlobalVariables._currentLevel]["postEntryTime"];
		List<string> _bonusList = addBonusToArray();
		int[,] map = generateMap();

        ViewController._currentGameModel = new Game(missionNumber, descriptionMission,entryTime, postEntryTime,_bonusList,map);
    }

    private List<string> addBonusToArray()
    {
		List<String> _bonusList = new List<string>();
		for(int i = 0 ; i < JsonController.jsonDataStages["Stages"][GlobalVariables._currentLevel]["bonus"].Count; i++)
			_bonusList.Add(JsonController.jsonDataStages["Stages"][GlobalVariables._currentLevel]["bonus"][i].ToString());

		return _bonusList;
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

	public void createPlayer(bool borisHit)
    {
		
        Destroy(this._playerInstance);
		this._playerInstance = Instantiate(_player, this.getNewPosition(false)- MapGeneratorController._offsetMap, Quaternion.identity, this._gameObjectsGameScene.transform) as GameObject;
		if(borisHit)
			_playerInstance.transform.GetChild(0).gameObject.GetComponent<Animator>().SetBool("isDeath",true);
    }

	private IEnumerator waitThenCallback(float time, Action callback)
	{
	yield return new WaitForSeconds(time);
	callback();
	}
}
