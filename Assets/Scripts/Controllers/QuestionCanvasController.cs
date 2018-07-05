using UnityEngine;
using UnityEngine.UI;

public class QuestionCanvasController : MonoBehaviour 
{
	static public string answer;

	[Header("Text Variables")]
	public Text _questionText;
	public Text _answer1;
	public Text _answer2;

	void OnEnable()
	{
		int idPregunta = Random.Range(0, JsonController.jsonDataQuestions["stages"][ViewController._currentGameModel._missionNumber].Count);
		
		string question = JsonController.jsonDataQuestions["stages"][ViewController._currentGameModel._missionNumber][idPregunta]["question"].ToString();
		this._questionText.text = question;
		this._answer1.text = JsonController.jsonDataQuestions["stages"][ViewController._currentGameModel._missionNumber][idPregunta]["options"][0].ToString();
		this._answer2.text = JsonController.jsonDataQuestions["stages"][ViewController._currentGameModel._missionNumber][idPregunta]["options"][1].ToString();
		
		answer = JsonController.jsonDataQuestions["stages"][ViewController._currentGameModel._missionNumber][idPregunta]["answer"].ToString();
	}

	public void checkAnswerKeyboard()
	{
		string a1 = _answer1.text;
		string a2 = _answer2.text;

		if(ViewController.OverLeftAnswer)
		{
			if(a1.Equals(answer))
			{
				print("1..");
				GameObject.Find("KaiPlayer(Clone)").gameObject.GetComponent<PlayerBehaviour>().receiveAnswer(true);
			}


			else
			{
				print("2..");
				GameObject.Find("KaiPlayer(Clone)").gameObject.GetComponent<PlayerBehaviour>().receiveAnswer(false);
			}
		}

		else
		{
			if(a2.Equals(answer))
			{
				print("3..");
				GameObject.Find("KaiPlayer(Clone)").gameObject.GetComponent<PlayerBehaviour>().receiveAnswer(true);
			}


			else
			{
				print("4..");
				GameObject.Find("KaiPlayer(Clone)").gameObject.GetComponent<PlayerBehaviour>().receiveAnswer(false);
			}
		}
	}

	public void updateUI()
	{
		if(ViewController.OverLeftAnswer)
		{
			_answer1.transform.parent.gameObject.GetComponent<Outline>().enabled = true;
			_answer2.transform.parent.gameObject.GetComponent<Outline>().enabled = false;
		}

		else
		{
			_answer1.transform.parent.gameObject.GetComponent<Outline>().enabled = false;
			_answer2.transform.parent.gameObject.GetComponent<Outline>().enabled = true;
		}

	}
}
