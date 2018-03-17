using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour 
{
	Coroutine _titleScreenAnimationCoroutine;

	[Header("Title Animation Parameters")]
	public float _firstAnimationTime;
	public float _secondAnimationTime;
	public float _thirdAnimationTime;
	public float _fourthAnimationTime;
	public GameObject _kaiTextAnimation_2;
	public GameObject _reimiTextAnimation_3;
	public GameObject titleGameImage;

	[Header("Animators")]
	public Animator _kaiAnimator;
	public Animator _borisAnimator;



	// Use this for initialization
	void Start () {
		
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator titleScreenAnimation()
	{
		this._kaiAnimator.SetBool("walk",true);
		while(true)
		{
			playTitleScreenAnimation(1);
			yield return new WaitForSeconds(this._firstAnimationTime);
			playTitleScreenAnimation(2);
			yield return new WaitForSeconds(this._secondAnimationTime);
			playTitleScreenAnimation(3);
			yield return new WaitForSeconds(this._thirdAnimationTime);
			playTitleScreenAnimation(4);
			yield return new WaitForSeconds(this._fourthAnimationTime);
		}

	}

	public void playTitleScreenAnimation(int idAnimation)
	{
		
		switch(idAnimation)
		{
			case 1 :

				this.titleGameImage.SetActive(false);
				this._borisAnimator.SetFloat("movX",-1);
				this._borisAnimator.SetFloat("movY",0);
				this._kaiAnimator.SetFloat("movX",-1);
				this._kaiAnimator.SetFloat("movY",0);
			break;

			case 2:
				this._kaiTextAnimation_2.SetActive(true);
				this._kaiAnimator.SetFloat("movX",0);
				this._kaiAnimator.SetFloat("movY",-1);
			break;

			case 3:
				this._kaiAnimator.SetFloat("movX",-1);
				this._kaiAnimator.SetFloat("movY",0);
				this._kaiTextAnimation_2.SetActive(false);
				this._reimiTextAnimation_3.SetActive(true);
			break;

			case 4:
				this._reimiTextAnimation_3.SetActive(false);
				this.titleGameImage.SetActive(true);
				break;
		}
	}

	public void playAnimations(GameState.States state)
	{
		switch(state)
		{
			case GameState.States.MAINSCENE:
				this._kaiTextAnimation_2.SetActive(false);
				this._reimiTextAnimation_3.SetActive(false);
				this.titleGameImage.SetActive(false);
				this._titleScreenAnimationCoroutine = StartCoroutine(titleScreenAnimation());
			break;
		}
	}

	public void stopAnimations(GameState.States state)
	{
		switch(state)
		{
			case GameState.States.MAINSCENE:
				print("parar coroutine");
				StopCoroutine(this._titleScreenAnimationCoroutine);
				// this._titleScreenAnimationCoroutine = StartCoroutine(titleScreenAnimation());
			break;
		}
	}
}
