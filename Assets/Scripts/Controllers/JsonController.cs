using LitJson;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class JsonController : MonoBehaviour
{
	[Header("Views")]
	public GameObject _mainSceneCanvas;
	public GameObject _gameObjectsMainScene;
	public GameObject _loadingSceneCanvas;
	private string jsonString;
	public static JsonData jsonDataStages;
	public static JsonData jsonDataQuestions;
	public string _stageUrl;
	public string _questionsUrl;

	private WWW _wwwStageUrl;
	private WWW _wwwQuestionsUrl;
	
	
	#if UNITY_STANDALONE
	void Start()
	{
	 	    jsonDataStages = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/GameData.txt").Trim());
	 	    jsonDataQuestions = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/QuestionsData.txt").Trim());

		    loadGlobalVariablesFromJson();
	 	    this._mainSceneCanvas.SetActive(true);
	 	    this._gameObjectsMainScene.SetActive(true);
	 	    this.GetComponent<AnimationController>().playAnimations(GameState.States.MAINSCENE);
	 	    this._loadingSceneCanvas.SetActive(false);       
	}
	#endif
	


	#if UNITY_WEBGL || UNITY_EDITOR
	IEnumerator Start()
	{
		_wwwStageUrl = new WWW(_stageUrl);
		_wwwQuestionsUrl = new WWW(_questionsUrl);

		yield return _wwwStageUrl;
		yield return _wwwQuestionsUrl;

		if (_wwwStageUrl.error == null && _wwwQuestionsUrl.error == null)
		{
			print(_wwwStageUrl.text);
	
			jsonDataStages = JsonMapper.ToObject(_wwwStageUrl.text.Trim());
			jsonDataQuestions = JsonMapper.ToObject(_wwwQuestionsUrl.text.Trim());

			loadGlobalVariablesFromJson();
			yield return new WaitForSeconds(1f);
			this._mainSceneCanvas.SetActive(true);
			this._gameObjectsMainScene.SetActive(true);
			this.GetComponent<AnimationController>().playAnimations(GameState.States.MAINSCENE);
			this._loadingSceneCanvas.SetActive(false);
		}

		else
		{
			Debug.Log("ERROR: " +  _wwwStageUrl.error);
		}        
	}
	#endif

	

    private void loadGlobalVariablesFromJson()
    {
        GlobalVariables._totallyStages = jsonDataStages["Stages"].Count;
    }
}
