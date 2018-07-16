using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestionCanvasController : MonoBehaviour 
{
	static public string answer;

	[Header("Text Variables")]
	public TextMeshProUGUI _questionText;
	public TextMeshProUGUI _answer1;
	public TextMeshProUGUI _answer2;

	public Sprite _currentAnswerSprite;
	public Sprite _normalAnswerSprite;

	void OnEnable()
	{
		setup();
		updateUI();
		loadData();
	}

    private void loadData()
    {
        int idPregunta = UnityEngine.Random.Range(0, JsonController.jsonDataQuestions["stages"][ViewController._currentGameModel._missionNumber].Count);
		
		string question = JsonController.jsonDataQuestions["stages"][ViewController._currentGameModel._missionNumber][idPregunta]["question"].ToString();
		this._questionText.text = question;
		this._answer1.text = JsonController.jsonDataQuestions["stages"][ViewController._currentGameModel._missionNumber][idPregunta]["options"][0].ToString();
		this._answer2.text = JsonController.jsonDataQuestions["stages"][ViewController._currentGameModel._missionNumber][idPregunta]["options"][1].ToString();
		
		answer = JsonController.jsonDataQuestions["stages"][ViewController._currentGameModel._missionNumber][idPregunta]["answer"].ToString();
    }

    private void setup()
    {
		ViewController.OverLeftAnswer = true;
		GlobalVariables._answeredAnswer = false;
	   	GlobalVariables._answerGood = false;

		this._answer1.transform.parent.gameObject.SetActive(true);
		this._answer2.transform.parent.gameObject.SetActive(true);

    }

	public void checkAnswer()
	{
		string a1 = _answer1.text;
		string a2 = _answer2.text;


		if(ViewController.OverLeftAnswer)
		{
			if(a1.Equals(answer))
			{
				GlobalVariables._answerGood = true;
			}


			else
				GlobalVariables._answerGood = false;
			
		}

		else
		{
			if(a2.Equals(answer))
				GlobalVariables._answerGood = true;


			else
				GlobalVariables._answerGood = false;
		}

	}

	public void updateUI()
	{
		if(ViewController.OverLeftAnswer)
		{
			_answer1.transform.parent.gameObject.GetComponent<Image>().sprite = this._currentAnswerSprite;
			_answer2.transform.parent.gameObject.GetComponent<Image>().sprite = this._normalAnswerSprite;
		}

		else
		{
			_answer2.transform.parent.gameObject.GetComponent<Image>().sprite = this._currentAnswerSprite;
			_answer1.transform.parent.gameObject.GetComponent<Image>().sprite = this._normalAnswerSprite;
		}

	}
}
