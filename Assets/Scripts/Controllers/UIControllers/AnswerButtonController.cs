using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class AnswerButtonController : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
		if(this.gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text.Equals(QuestionCanvasController.answer))
		{
			GlobalVariables._answerGood = true;
			GameObject.Find("GameController").GetComponent<ViewController>().answeredAnswer();
		}

		else
		{
			GlobalVariables._answerGood = false;
			GameObject.Find("GameController").GetComponent<ViewController>().answeredAnswer();
		}

		GlobalVariables._answeredAnswer = true;
    }



}
