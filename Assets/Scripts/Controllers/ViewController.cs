using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Prime31.TransitionKit;
using TMPro;

public class ViewController : MonoBehaviour
{
	static public Game _currentGameModel;
	static public GameState _gameState;
	static public bool OverLeftAnswer = false;
	public GameObject _iconStagePrefab;
	public GameObject _portalPrefab;
	public GameObject _player;
	public GameObject _enemy;
	public SoundController _soundController;
	private bool _canPressEnter;

	private float x;
	private float y;

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
	const int ICONGROUPSPACING = 70;

	
	[Header("MenuPausaGarage")]
	public GameObject _menuPausaGarage;
	public Sprite _noOptionImage; 
	public Sprite _yesOptionImage; 
	public Button _musicButtonMenuGarage;
	public Button _soundButtonMenuGarage;

	[Header("MenuPausaGarage")]
	public GameObject _menuPausaGameScene;
	public Button _musicButtonMenuGameScene;
	public Button _soundButtonMenuGameScene;



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
	public GameObject _questionsCanvas;

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

	[Header("QuestionCanvas")]
	public TextMeshProUGUI _questionText;
	public TextMeshProUGUI _continueText;

	[Header("ExitCanvas")]
	public GameObject _exitCanvas;

	

	/// <summary>
	/// Awake is called when the script instance is being loaded.
	/// </summary>
	/* 
	void Awake()
	{
	 	PlayerPrefs.DeleteAll();
	}
	*/
	
	void Start()
	{
		#if UNITY_STANDALONE
		print(PlayerPrefs.GetInt("lastStageCompleted"));
		if(PlayerPrefs.GetInt("GameComplete")==0)
		{
		 	GlobalVariables._currentLevel = PlayerPrefs.GetInt("lastStageCompleted");
		 	GlobalVariables._userLevel = PlayerPrefs.GetInt("lastStageCompleted");
		}

		else
		{
		 	GlobalVariables._currentLevel = 0;
		 	GlobalVariables._userLevel = 0;
		}
		#endif

		#if UNITY_WEBGL || UNITY_EDITOR
		GlobalVariables._currentLevel = 0;
		GlobalVariables._userLevel = 0;
		#endif
		
		this._canPressEnter = true;
		ViewController._gameState = new GameState(GameState.States.MAINSCENE);
	}

	/// <summary>
	/// Update is called every frame, if the MonoBehaviour is enabled.
	/// </summary>
	void Update()
	{
		if(!this._exitCanvas.activeInHierarchy && !this._questionsCanvas.activeInHierarchy && Input.GetButtonDown("Enter") || (Input.GetMouseButton(0) && !this._garageSceneCanvas.activeInHierarchy && !this._questionsCanvas.activeInHierarchy) )
		{
			if(!_loadingSceneCanvas.activeInHierarchy && !this._gameSceneCanvas.activeInHierarchy && !this._menuPausaGarage.activeInHierarchy )
				pressEnter();
		}

		#if UNITY_STANDALONE || UNITY_EDITOR
		else if(Input.GetButtonDown("Exit") && !_exitCanvas.activeInHierarchy)
			this._exitCanvas.SetActive(true);
		#endif

		else if(this._questionsCanvas.activeInHierarchy)
		{
			if(Input.GetButtonDown("LeftArrow") && !GlobalVariables._answeredAnswer)
			{
				OverLeftAnswer = true;
				_questionsCanvas.GetComponent<QuestionCanvasController>().updateUI();
				this._soundController.playSound(6);

			}

			else if(Input.GetButtonDown("RightArrow") && !GlobalVariables._answeredAnswer)
			{
				OverLeftAnswer = false;
				_questionsCanvas.GetComponent<QuestionCanvasController>().updateUI();
				this._soundController.playSound(6);
			}
				

			else if(Input.GetButtonDown("Enter") && !GlobalVariables._answeredAnswer)
			{
				answeredAnswer();
			}

			else if(Input.GetButtonDown("Enter") && GlobalVariables._answeredAnswer)
			{
				exitQuestionary();
			}
		}
	}

	public void answeredAnswer()
	{
		print("Aprietas enter para averiguar respuesta");
		GlobalVariables._answeredAnswer = true;
		_questionsCanvas.GetComponent<QuestionCanvasController>().checkAnswer();
		buttonsDisabled();
		updateQuestionText();
	}

	public void exitQuestionary()
	{
		print("Aprietas enter para enviar respuesta a kai");
		_questionsCanvas.SetActive(false);
		this._continueText.gameObject.SetActive(false);
		_gameSceneCanvas.SetActive(true);
		this._playerInstance.GetComponent<PlayerBehaviour>().check();
		playGame();
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
			
			if(PlayerPrefs.GetInt("GameComplete")==0 || !GlobalVariables._iconStagesAdded)
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
				//Jugador Logra completar la etapa seleccionada
				if(GlobalVariables._userLevel == GlobalVariables._currentLevel && GlobalVariables._currentLevel != GlobalVariables._totallyStages)
				{
					GlobalVariables._currentLevel++;
					if(GlobalVariables._currentLevel > PlayerPrefs.GetInt("lastStageCompleted") && GlobalVariables._currentLevel != GlobalVariables._totallyStages)
					{
						PlayerPrefs.SetInt("lastStageCompleted",GlobalVariables._currentLevel );
						print(PlayerPrefs.GetInt("lastStageCompleted"));

					}
						
				}
				
				if(GlobalVariables._currentLevel == GlobalVariables._totallyStages)
				{
					print("Aparecer escena final");
					GlobalVariables._currentLevel = 0;
					GlobalVariables._userLevel=0;
					ViewController._gameState._state = GameState.States.FINALSCENE;
					this._iconStageGroup.transform.GetChild(_iconStageGroup.transform.childCount-1).gameObject.GetComponent<Image>().color = this._stageCompleted;
					this._iconStageGroup.GetChild(_iconStageGroup.transform.childCount-1).transform.localScale = new Vector2(1f,1f);
				}

				else
				{
					resetGameInstances();
					ViewController._gameState._state = GameState.States.GARAGE;
					updateIconStageGroup();
				}

				GlobalVariables._userLevel = GlobalVariables._currentLevel;
			}

			updateView();
		}

		else if(ViewController._gameState._state == GameState.States.FINALSCENE)
		{
			this._soundController.GetComponent<AudioSource>().Stop();
			this._finalSceneCanvas.SetActive(false);
			this._gameObjectsFinalView.SetActive(false);

			ViewController._gameState._state = GameState.States.MAINSCENE;
			PlayerPrefs.SetInt("GameComplete", 1);
			resetGame();
			
			updateView();
		}

		else if(ViewController._gameState._state == GameState.States.GAMEOVER)
		{	
			this._gameOverSceneCanvas.SetActive(false);
			ViewController._gameState._state = GameState.States.MAINSCENE;

			// GlobalVariables._gameComplete = false;
			resetGame();
			
			updateView();

		}
    }

	public void exitGame()
	{
		Application.Quit();
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
		GlobalVariables._userLevel = 0;
		GlobalVariables._globalLifes = 3;
		GlobalVariables._currentLifes = 3;
	
		// for(int i = 0 ; i < this._iconStageGroup.childCount;i++)
		// {
		// 	this._iconStageGroup.transform.GetChild(i).gameObject.GetComponent<Image>().color = this._stageNotComplete;
		// 	this._iconStageGroup.transform.GetChild(i).gameObject.transform.localScale = new Vector2(1,1);
		// }

		this._iconStageGroup.transform.GetChild(0).gameObject.GetComponent<Image>().color = this._currentStageColor;	
        
		resetGameInstances();
    }

    public void resetGameInstances()
    {
		this._leaveText.SetActive(false);
		this.gameObject.GetComponent<MechanicController>().stopPortalCoroutine();
		this.gameObject.GetComponent<BonusController>().stopCoroutine();
		
		this.gameObject.GetComponent<MechanicController>()._timeCountText.color = new Color32(0,255,0,255);
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
			this.gameObject.GetComponent<MechanicController>().startInitTime();
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
			this.gameObject.GetComponent<CountGarageController>().resetCoroutine(30);
			this._gameObjectsMainScene.SetActive(false);
			this._mainSceneCanvas.SetActive(false);
			updateGameModel(GlobalVariables._currentLevel);
			updateIconStageGroup();
			changeGarageTexts();
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
					this._bonusList.transform.GetChild(3).gameObject.SetActive(true);

					GlobalVariables._yellowBonus = true;
					GlobalVariables._purpleBonus = true;
				break;

				case "green":
					this._bonusList.transform.GetChild(0).gameObject.SetActive(true);
				break;

				case "orange":
					this._bonusList.transform.GetChild(1).gameObject.SetActive(true);
				break;

				case "gray":
					this._bonusList.transform.GetChild(2).gameObject.SetActive(true);
				break;

				case "blue":
					this._bonusList.transform.GetChild(3).gameObject.SetActive(true);
				break;

				case "purple":
					GlobalVariables._purpleBonus = true;
				break;

				case "yellow":
					GlobalVariables._yellowBonus = true;
				break;
			}
		}
    }

    internal void stopGame()
    {	
		GlobalVariables._enemy.GetComponent<EnemyBehaviour>()._currentPath.Clear();
		GlobalVariables._runUpdateEnemy = false;
		
		GlobalVariables._runUpdatePlayer = false;

		this.GetComponent<BonusController>().pauseCoroutines();
		this.GetComponent<MechanicController>().stopOnlyPortal();
	}

	public void playGame()
	{
		GlobalVariables._runUpdateEnemy = true;
		GlobalVariables._runUpdatePlayer = true;
		
		this.GetComponent<BonusController>().initializeCorroutines();
		this.GetComponent<MechanicController>().startPortalCoroutine(10);
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
			return new Vector2((GlobalVariables._xPosPlayer*GlobalVariables._widthTile), (GlobalVariables._yPosPlayer * -GlobalVariables._widthTile));
		}
			
    }

    internal void buttonsDisabled()
    {
        this._questionsCanvas.transform.Find("Answers").transform.GetChild(0).gameObject.SetActive(false);
		this._questionsCanvas.transform.Find("Answers").transform.GetChild(1).gameObject.SetActive(false);
		this._continueText.gameObject.SetActive(true);
    }

    private void addIconStagesGroup()
    {
		GameObject iconStage;
        for(int i = 0 ; i < GlobalVariables._totallyStages; i++)
		{
			if(i%5 ==0 && i != 0)
			{
				//Add spacing to content
				this._iconStageGroup.GetComponent<RectTransform>().sizeDelta += new Vector2(0,ICONGROUPSPACING);
			}

			iconStage = Instantiate(_iconStagePrefab, _iconStageGroup);
			iconStage.GetComponent<GarageButtonController>()._idStage = i;
			iconStage.transform.GetChild(0).gameObject.GetComponent<Text>().text = (i + 1).ToString();
		}
    }

    internal void updateQuestionText()
    {
        if(GlobalVariables._answerGood)
		{
			this._soundController.playSound(8);
			this._questionText.text = "<color=#00ff00ff><b>Excelente Kay, logré hacer la conexión</b>, te paso al siguiente nivel";
		}
			

		else
		{
			this._soundController.playSound(7);
			this._questionText.text = "<color=#ff0000ff><b>Kay lo que me dijiste es incorrecto!</b>, abro otro portal en 10 sgs";
		}
		
    }

    private void updateIconStageGroup()
	{

		#if UNITY_EDITOR || UNITY_STANDALONE
		for(int i = 0 ; i < PlayerPrefs.GetInt("lastStageCompleted") ;i++)
		{
			this._iconStageGroup.GetChild(i).transform.localScale = new Vector2(1,1);
			this._iconStageGroup.GetChild(i).gameObject.GetComponent<Image>().color = this._stageCompleted;
		}
		#endif

		#if UNITY_WEBGL
		for(int i = 0 ; i < GlobalVariables._currentLevel ;i++)
		{
			this._iconStageGroup.GetChild(i).transform.localScale = new Vector2(1,1);
			this._iconStageGroup.GetChild(i).gameObject.GetComponent<Image>().color = this._stageCompleted;
		}
		#endif

		this._iconStageGroup.GetChild(GlobalVariables._currentLevel).GetComponent<Image>().color = this._currentStageColor;
		this._iconStageGroup.GetChild(GlobalVariables._currentLevel).gameObject.transform.localScale = new Vector2(1.2f, 1.2f);
		
	}

	public void refreshContext(int idStage)
	{
		_iconStageGroup.GetChild(GlobalVariables._userLevel).gameObject.transform.localScale = new Vector2(1, 1);
		_iconStageGroup.GetChild(idStage).gameObject.transform.localScale = new Vector2(1.2f, 1.2f);
		GlobalVariables._userLevel = idStage;
		updateGameModel(idStage);
		changeGarageTexts();
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

    private void changeGarageTexts()
    {
		this._lifesCountText.text = GlobalVariables._globalLifes.ToString();
		this._missionNumberDescriptionText.text = ViewController._currentGameModel._missionNumber;
		this._descriptionMissionText.text = ViewController._currentGameModel._descriptionMission;
    }

	private void updateGameModel(int level)
    {
		string missionNumber = JsonController.jsonDataStages["Stages"][level]["missionNumber"].ToString();
		string descriptionMission = JsonController.jsonDataStages["Stages"][level]["descriptionMission"].ToString();
		int entryTime = (int)JsonController.jsonDataStages["Stages"][level]["entryTime"];
		int postEntryTime = (int)JsonController.jsonDataStages["Stages"][level]["postEntryTime"];
		List<string> _bonusList = addBonusToArray(level);
		int[,] map = generateMap(level);

        ViewController._currentGameModel = new Game(missionNumber, descriptionMission,entryTime, postEntryTime,_bonusList,map);
    }

    private List<string> addBonusToArray(int level)
    {
		List<String> _bonusList = new List<string>();
		for(int i = 0 ; i < JsonController.jsonDataStages["Stages"][level]["bonus"].Count; i++)
			_bonusList.Add(JsonController.jsonDataStages["Stages"][level]["bonus"][i].ToString());

		return _bonusList;
    }

    private int[,] generateMap(int level)
    {

		int[,] map = new int[GlobalVariables._iMaxMatrix, GlobalVariables._jMaxMatrix];
		int contador = 0;

        for(int i = 0 ; i < GlobalVariables._iMaxMatrix; i++)
			for(int j = 0 ; j < GlobalVariables._jMaxMatrix; j++)
			{
				map[i,j] =  (int) JsonController.jsonDataStages["Stages"][level]["map"][contador];
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
		{
			_playerInstance.transform.GetChild(0).gameObject.GetComponent<Animator>().SetBool("isDeath",true);
			_playerInstance.GetComponent<PlayerBehaviour>()._sleep = true;

		}
    }

	private IEnumerator waitThenCallback(float time, Action callback)
	{
		yield return new WaitForSeconds(time);
		callback();
	}


	#region "PauseMenu"
	public void onPressMusicButton()
	{
		GlobalVariables._isMusicOn = !GlobalVariables._isMusicOn;
		if(GlobalVariables._isMusicOn)
		{
			this.GetComponent<AudioSource>().volume = 1;
			if(this._menuPausaGarage.activeInHierarchy)
			{
				changeImage(this._musicButtonMenuGarage, this._yesOptionImage);
			}
			else
			{
				changeImage(this._musicButtonMenuGameScene, this._yesOptionImage);
			}
		}

		else
		{
			this.GetComponent<AudioSource>().volume = 0;
			if(this._menuPausaGarage.activeInHierarchy)
			{
				changeImage(this._musicButtonMenuGarage, this._noOptionImage);
			}
			else
			{
				changeImage(this._musicButtonMenuGameScene, this._noOptionImage);
			}
		}
	}

	public void onPressSoundButton()
	{
		GlobalVariables._isSoundOn = !GlobalVariables._isSoundOn;
		if(GlobalVariables._isSoundOn)
		{
			this._soundController.GetComponent<AudioSource>().volume =1;
			if(this._menuPausaGarage.activeInHierarchy)
			{
				changeImage(this._soundButtonMenuGarage, this._yesOptionImage);
			}
			else
			{
				changeImage(this._soundButtonMenuGameScene, this._yesOptionImage);
			}
		}

		else
		{
			this._soundController.GetComponent<AudioSource>().volume =0;

			if(this._menuPausaGarage.activeInHierarchy)
			{
				changeImage(this._soundButtonMenuGarage, this._noOptionImage);
			}
			else
			{
				changeImage(this._soundButtonMenuGameScene, this._noOptionImage);
			}
			
		}
	}

	public void changeOverColorText(Text text)
	{
		text.color = new Color(0,255,0,1);
	}

	public void changeExitColorText(Text text)
	{
		if(Time.timeScale == 0)
			Time.timeScale = 1;
		text.color = new Color(255,255,255,1);
	}

	

	//Bool para diferenciar entre los canvas del menu pausa, int para saber a que boton pertenece
	public void changeImage(Button button,Sprite image)
	{
		button.GetComponent<Image>().sprite = image;

	}

	public void closePanel(GameObject panel)
	{
		panel.SetActive(false);
		Time.timeScale = 1;
	}
	
	public void openPanel(GameObject panel)
	{
		
		Time.timeScale = 0;
		panel.SetActive(true);
		updatePausePanel();
	}

	public void updatePausePanel()
	{
		if(GlobalVariables._isSoundOn)
		{
			if(this._menuPausaGarage.activeInHierarchy)
			{	
				changeImage(this._soundButtonMenuGarage, this._yesOptionImage);
			}

			else
			{
				changeImage(this._soundButtonMenuGameScene, this._yesOptionImage);
			}
			
		}
		
		if(!GlobalVariables._isSoundOn)
		{
			if(this._menuPausaGarage.activeInHierarchy)
			{	
				changeImage(this._soundButtonMenuGarage, this._noOptionImage);
			}

			else
			{
				changeImage(this._soundButtonMenuGameScene, this._noOptionImage);
			}	
		}

		if(GlobalVariables._isMusicOn)
		{
			if(this._menuPausaGarage.activeInHierarchy)
			{	
				changeImage(this._musicButtonMenuGarage, this._yesOptionImage);
			}

			else
			{
				changeImage(this._musicButtonMenuGameScene, this._yesOptionImage);
			}
			
		}
		
		if(!GlobalVariables._isMusicOn)
		{
			if(this._menuPausaGarage.activeInHierarchy)
			{	
				changeImage(this._musicButtonMenuGarage, this._noOptionImage);
			}

			else
			{
				changeImage(this._musicButtonMenuGameScene, this._noOptionImage);
			}	
		}
	}

	public void returnToGarage( GameObject panel)
	{
		Time.timeScale = 1;
		panel.SetActive(false);
		this._gameSceneCanvas.SetActive(false);
		this._gameObjectsGameScene.SetActive(false);
		resetGameInstances();
		ViewController._gameState._state = GameState.States.GARAGE;
		GlobalVariables._currentLevel = PlayerPrefs.GetInt("lastStageCompleted");
		GlobalVariables._userLevel = PlayerPrefs.GetInt("lastStageCompleted");
		
		updateView();
	}

	public void deleteProgress(GameObject panel)
	{
		PlayerPrefs.DeleteAll();
		Time.timeScale = 1;
		
		GlobalVariables._currentLevel = PlayerPrefs.GetInt("lastStageCompleted");
		GlobalVariables._userLevel = PlayerPrefs.GetInt("lastStageCompleted");
		resetGame();
		resetGameInstances();
		
		for(int i = 0 ; i < this._iconStageGroup.childCount;i++)
		{
			Destroy(_iconStageGroup.GetChild(i).gameObject);
		}

		
		this._gameSceneCanvas.SetActive(false);
		this._gameObjectsGameScene.SetActive(false);
		panel.SetActive(false);
		ViewController._gameState._state = GameState.States.MAINSCENE;
		updateView();
	}

	#endregion
}
