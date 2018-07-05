using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class AnswerButtonController : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
		if(this.gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text.Equals(QuestionCanvasController.answer))
		{
			GameObject.Find("KaiPlayer(Clone)").gameObject.GetComponent<PlayerBehaviour>().receiveAnswer(true);
		}

		else
		{
			GameObject.Find("KaiPlayer(Clone)").gameObject.GetComponent<PlayerBehaviour>().receiveAnswer(false);
		}
    }
}
