using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerBehaviour : MonoBehaviour 
{
	//Path
	public Vector2 _nextPosition;
	public Vector2 _nextPositionToChange;
	public List<Vector2> playerPath;
	public Moves currentMove;
	public float time;
	public float speed;
	public int currentNode;

	//References
	private GameObject _gameController;
	private Transform _portalTransform;
	// public GameObject _enemy;
	float deltaTime;


	public float _pixelsBeforeMove;
	public Vector2 _nextNodeCheckMove;
	private Animator _kaiAnimator;
	private bool _entry;
	private float x,y;

	public bool _sleep;

	public enum Moves
	{
		DOWN,UP,LEFT,RIGHT,NOTHING
	}

	/// <summary>
	/// Awake is called when the script instance is being loaded.
	/// </summary>
	void Awake()
	{
		this._gameController = GameObject.Find("GameController").gameObject;
		
	}

	// Use this for initialization
	void Start () 
	{
		this.x= 0;
		this.y = 0;
		this._entry = false;
		this._kaiAnimator = this.transform.GetChild(0).gameObject.GetComponent<Animator>();
		playerPath = new List<Vector2>();
		currentMove = Moves.NOTHING;
		speed = GlobalVariables._playerVelocity;
		this._nextNodeCheckMove = Vector2.zero;
	}
	
	// Update is called once per frame
	void Update () 
	{
		deltaTime += Time.deltaTime;
		if(deltaTime>2)
		{
			if(this.transform.position.y > GlobalVariables._enemy.transform.position.y)
			{
				this.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingOrder = -2;
				GlobalVariables._enemy.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingOrder = -1;
			}

			else
			{
				this.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingOrder = -1;
				GlobalVariables._enemy.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingOrder = -2;
				

			}
			
			deltaTime = 0;
		}

		#region Controls 
		if(!GlobalVariables._stageComplete && !_sleep)
		{
			x = Input.GetAxis("Horizontal");
			y = Input.GetAxis("Vertical");

			if(y > 0 && !GlobalVariables._changeDirection || y < 0 && GlobalVariables._changeDirection)
			{
				if(canMove(1,0,Moves.DOWN))
				{
					_kaiAnimator.SetBool("walk",true);
					_kaiAnimator.SetBool("isDeath", false);
					_kaiAnimator.SetFloat("movX",0);
					_kaiAnimator.SetFloat("movY",-1);
					resetValues();

					generatePlayerPath(Moves.DOWN);

					updateValues(1,0,Moves.DOWN);
					
				}
			}

			else if(y < 0  && !GlobalVariables._changeDirection || y > 0  && GlobalVariables._changeDirection)
			{
				if(canMove(-1,0,Moves.UP))
				{
					_kaiAnimator.SetBool("walk",true);
					_kaiAnimator.SetBool("isDeath", false);
					_kaiAnimator.SetFloat("movX",0);
					_kaiAnimator.SetFloat("movY",1);

					resetValues();

					generatePlayerPath(Moves.UP);

					updateValues(-1,0,Moves.UP);
					
				}	
			}

			else if(x < 0  && !GlobalVariables._changeDirection || x > 0  && GlobalVariables._changeDirection)
			{
				if(canMove(0,-1,Moves.LEFT))
				{
					_kaiAnimator.SetBool("walk",true);
					_kaiAnimator.SetBool("isDeath", false);
					_kaiAnimator.SetFloat("movX",-1);
					_kaiAnimator.SetFloat("movY",0);
					resetValues();

					generatePlayerPath(Moves.LEFT);

					updateValues(0,-1,Moves.LEFT);
				}	
			}

			else if(x >0  && !GlobalVariables._changeDirection || x < 0  && GlobalVariables._changeDirection)
			{
				if(canMove(0,1,Moves.RIGHT))
				{
					_kaiAnimator.SetBool("walk",true);
					_kaiAnimator.SetBool("isDeath", false);
					_kaiAnimator.SetFloat("movX",1);
					_kaiAnimator.SetFloat("movY",0);
					resetValues();

					generatePlayerPath(Moves.RIGHT);

					updateValues(0,1,Moves.RIGHT);
					
				}
			}

			#endregion
			#region Move
			switch(currentMove)
			{
				case Moves.DOWN:
					time = Time.deltaTime * speed;
					moveGameObject(1,0);
					break;
				
				case Moves.UP:
					time = Time.deltaTime * speed;
					moveGameObject(-1,0);
					break;
				
				case Moves.RIGHT:
					time = Time.deltaTime * speed;
					moveGameObject(0, 1);
					break;
				
				case Moves.LEFT:
					time = Time.deltaTime * speed;
					moveGameObject(0,-1);
					break;

			}
			#endregion moving

			x= 0;
			y = 0;
		}


		else if(_sleep)
		{
			checkCurrentInput();
		}
	}

	public void checkCurrentInput()
	{
		if(Input.GetAxis("Horizontal") ==0 && Input.GetAxis("Vertical")==0)
		{
			_sleep= false;
			x = Input.GetAxis("Horizontal");
			y = Input.GetAxis("Vertical");

		if(y > 0 && !GlobalVariables._changeDirection || y < 0 && GlobalVariables._changeDirection)
		{
			if(canMove(1,0,Moves.DOWN))
			{
				_kaiAnimator.SetBool("walk",true);
				_kaiAnimator.SetBool("isDeath", false);
				_kaiAnimator.SetFloat("movX",0);
				_kaiAnimator.SetFloat("movY",-1);
				resetValues();

				generatePlayerPath(Moves.DOWN);

				updateValues(1,0,Moves.DOWN);
				
			}
		}

		else if(y < 0  && !GlobalVariables._changeDirection || y > 0  && GlobalVariables._changeDirection)
		{
			if(canMove(-1,0,Moves.UP))
			{
				_kaiAnimator.SetBool("walk",true);
				_kaiAnimator.SetBool("isDeath", false);
				_kaiAnimator.SetFloat("movX",0);
				_kaiAnimator.SetFloat("movY",1);

				resetValues();

				generatePlayerPath(Moves.UP);

				updateValues(-1,0,Moves.UP);
				
			}	
		}

		else if(x < 0  && !GlobalVariables._changeDirection || x > 0  && GlobalVariables._changeDirection)
		{
			if(canMove(0,-1,Moves.LEFT))
			{
				_kaiAnimator.SetBool("walk",true);
				_kaiAnimator.SetBool("isDeath", false);
				_kaiAnimator.SetFloat("movX",-1);
				_kaiAnimator.SetFloat("movY",0);
				resetValues();

				generatePlayerPath(Moves.LEFT);

				updateValues(0,-1,Moves.LEFT);
			}	
		}

		else if(x >0  && !GlobalVariables._changeDirection || x < 0  && GlobalVariables._changeDirection)
		{
			if(canMove(0,1,Moves.RIGHT))
			{
				_kaiAnimator.SetBool("walk",true);
				_kaiAnimator.SetBool("isDeath", false);
				_kaiAnimator.SetFloat("movX",1);
				_kaiAnimator.SetFloat("movY",0);
				resetValues();

				generatePlayerPath(Moves.RIGHT);

				updateValues(0,1,Moves.RIGHT);
				
			}
		}

		
		#region Move
		switch(currentMove)
		{
			case Moves.DOWN:
				time = Time.deltaTime * speed;
				moveGameObject(1,0);
				break;
			
			case Moves.UP:
				time = Time.deltaTime * speed;
				moveGameObject(-1,0);
				break;
			
			case Moves.RIGHT:
				time = Time.deltaTime * speed;
				moveGameObject(0, 1);
				break;
			
			case Moves.LEFT:
				time = Time.deltaTime * speed;
				moveGameObject(0,-1);
				break;

		}
		#endregion moving

		x= 0;
		y = 0;

		}

	}

    private void moveGameObject(int i, int j)
    {
        if((Vector2)this.transform.position != _nextPosition)
		{	
			this.transform.position = Vector2.MoveTowards(this.transform.position, this._nextPosition, time);

			if(i==0)
			{
				if(j==1 && this.transform.position.x >= _nextPositionToChange.x && !_entry)
				{
					_entry = true;
						GlobalVariables._xPosPlayer+=j;
						GlobalVariables._yPosPlayer+=i;

				}

				else if(j==-1 && this.transform.position.x <= _nextPositionToChange.x && !_entry)
				{
					_entry = true;
						GlobalVariables._xPosPlayer+=j;
						GlobalVariables._yPosPlayer+=i;

				}

			}

			else {

				if(i == 1 && this.transform.position.y <= _nextPositionToChange.y && !_entry)
				{
					_entry = true;
						GlobalVariables._xPosPlayer+=j;
						GlobalVariables._yPosPlayer+=i;
				}

				else if(i == -1 && this.transform.position.y >= _nextPositionToChange.y && !_entry)
				{
					_entry = true;
						GlobalVariables._xPosPlayer+=j;
						GlobalVariables._yPosPlayer+=i;

				}
			}
		}

		else
		{
			_entry = false;
			currentNode++;
			if(currentNode == playerPath.Count)
			{
				currentMove = Moves.NOTHING;
				currentNode = 0;
			}

			else
			{
				
				time = 0;
				if(playerPath.Count>0)
				{
					
					this._nextPositionToChange = new Vector2( playerPath[currentNode].y* GlobalVariables._widthTile-(j*GlobalVariables._widthTile/2), playerPath[currentNode].x*-GlobalVariables._widthTile +(i*GlobalVariables._widthTile/2)) - MapGeneratorController._offsetMap;
					
					this._nextPosition = new Vector2( playerPath[currentNode].y* GlobalVariables._widthTile, playerPath[currentNode].x*-GlobalVariables._widthTile) - MapGeneratorController._offsetMap;
					
				}
				
			}
		}
    }

    private void printPath()
    {
        for(int i = 0 ; i< playerPath.Count ; i++)
		{
			print(playerPath[i]);
		}
    }

    private void updateValues(int i, int j, Moves mov)
    {

		_entry = false;
		
		currentNode=0;
		this._nextPosition = new Vector2( playerPath[currentNode].y* GlobalVariables._widthTile, playerPath[currentNode].x*-GlobalVariables._widthTile) - MapGeneratorController._offsetMap;
		this._nextPositionToChange = new Vector2( playerPath[currentNode].y* GlobalVariables._widthTile-(j*GlobalVariables._widthTile/2), playerPath[currentNode].x*-GlobalVariables._widthTile +(i*GlobalVariables._widthTile/2)) - MapGeneratorController._offsetMap;
		
		currentMove = mov;
    }


    private void generatePlayerPath(Moves mov)
    {

		int valor = ViewController._currentGameModel._map[GlobalVariables._yPosPlayer,GlobalVariables._xPosPlayer];
		

		if(mov == Moves.UP)
		{
			for(int i =GlobalVariables._yPosPlayer -1; i >=0 && GlobalVariables._allowedMovements[valor,0]==1; i--)
			{
				playerPath.Add(new Vector2(i, GlobalVariables._xPosPlayer));
				valor = ViewController._currentGameModel._map[i,GlobalVariables._xPosPlayer];
			}
		}	

		else if(mov == Moves.RIGHT)
		{
			for(int i =GlobalVariables._xPosPlayer +1; i < ViewController._currentGameModel._map.GetLength(1) && GlobalVariables._allowedMovements[valor,1]==1 ; i++)
			{
				playerPath.Add(new Vector2(GlobalVariables._yPosPlayer, i));
				valor = ViewController._currentGameModel._map[GlobalVariables._yPosPlayer,i];
		
			}
		
		}

		else if(mov == Moves.DOWN)
		{
			for(int i =GlobalVariables._yPosPlayer +1; i <ViewController._currentGameModel._map.GetLength(0) && GlobalVariables._allowedMovements[valor,2]==1; i++)
			{
				playerPath.Add(new Vector2(i, GlobalVariables._xPosPlayer));
				valor = ViewController._currentGameModel._map[i,GlobalVariables._xPosPlayer];
			}
		}

		else if(mov == Moves.LEFT)
		{

			for(int i =GlobalVariables._xPosPlayer- 1; i >=0 && GlobalVariables._allowedMovements[valor,3]==1 ; i--)
			{
				playerPath.Add(new Vector2(GlobalVariables._yPosPlayer, i));
				valor = ViewController._currentGameModel._map[GlobalVariables._yPosPlayer,i];
			}
		}		
    }

    private void resetValues()
    {
        playerPath.Clear();
		currentNode = 0;
    }

    private bool canMove(int j, int i, Moves mov)
    {
		// if(playerPath.Count>0)
		// {
		// 	int lastX = (int)playerPath[playerPath.Count-1].x;
		// 	int lastY = (int)playerPath[playerPath.Count-1].y;
		// }
		

		if(this.currentMove == mov)
			return false;


		int valor = ViewController._currentGameModel._map[GlobalVariables._yPosPlayer,GlobalVariables._xPosPlayer];
		this._nextNodeCheckMove.x = (GlobalVariables._xPosPlayer* GlobalVariables._widthTile)- MapGeneratorController._offsetMap.x;
		this._nextNodeCheckMove.y = (GlobalVariables._yPosPlayer*-GlobalVariables._widthTile)- MapGeneratorController._offsetMap.y;

		switch(mov)
		{
			
			case(Moves.UP):
				if(GlobalVariables._allowedMovements[valor,0]==1)
				{
					// if(GlobalVariables._xPosPlayer == lastX && GlobalVariables._yPosPlayer == lastY)
					// 	return true;

					return checkRange(mov);
				}
				break;
			case(Moves.RIGHT):
				if(GlobalVariables._allowedMovements[valor,1]==1)
				{
					// if(GlobalVariables._xPosPlayer == lastX && GlobalVariables._yPosPlayer == lastY)
					// 	return true;

					return checkRange(mov);
				}
				break;
			case(Moves.DOWN):
				if(GlobalVariables._allowedMovements[valor,2]==1)
				{
					// if(GlobalVariables._xPosPlayer == lastX && GlobalVariables._yPosPlayer == lastY)
					// 	return true;

					return checkRange(mov);
				}
				break;
			case(Moves.LEFT):
				if(GlobalVariables._allowedMovements[valor,3]==1)
				{
					// if(GlobalVariables._xPosPlayer == lastX && GlobalVariables._yPosPlayer == lastY)
					// 	return true;
					return checkRange(mov);
				}
				break;
		}

		return false;
    }

    private bool checkRange(Moves mov)
    {
		
		//Comparando X Cuando hay un cambio de direccion
		if( currentMove == Moves.LEFT && (mov == Moves.UP || mov == Moves.DOWN) || currentMove == Moves.RIGHT && (mov == Moves.UP || mov == Moves.DOWN))
		{
			
			if(Math.Abs(this.transform.position.x-_nextNodeCheckMove.x) <= this._pixelsBeforeMove)
			{
				return true;
			}
		}

		else if( currentMove == Moves.DOWN && (mov == Moves.RIGHT || mov == Moves.LEFT) || currentMove == Moves.UP && (mov == Moves.RIGHT || mov == Moves.LEFT))
		{
	
			if(Math.Abs(this.transform.position.y-_nextNodeCheckMove.y) <= this._pixelsBeforeMove)
			{
				return true;
			}
		}
			
		
		else if(currentMove == Moves.NOTHING || (currentMove == Moves.DOWN && mov == Moves.UP) || (currentMove == Moves.UP && mov == Moves.DOWN) ||
		(currentMove == Moves.LEFT && mov == Moves.RIGHT) || (currentMove == Moves.RIGHT && mov == Moves.LEFT))
		{
			return true;
		}
			

		return false;

    }

    void OnTriggerEnter2D(Collider2D other)
	{
		if(other.gameObject.tag.Equals("Enemy"))
		{
			
			_kaiAnimator.SetBool("isDeath",true);
			GameObject.Find("SoundController").gameObject.GetComponent<SoundController>().playSound(4);
			this._gameController.GetComponent<MechanicController>().resetPlayerPosition();
			GlobalVariables._followPlayer=false;
		}

		if(other.gameObject.tag.Equals("Portal"))
		{
			other.gameObject.GetComponent<Collider2D>().enabled = false;
			//GameObject.Find("SoundController").gameObject.GetComponent<SoundController>().playSound(3);
			//_gameController.GetComponent<AudioSource>().enabled = false;
			_gameController.GetComponent<ViewController>()._questionsCanvas.SetActive(true);
			_gameController.GetComponent<ViewController>()._gameSceneCanvas.SetActive(false);
			_portalTransform = other.gameObject.transform;

			Time.timeScale = 0;


			/* 
			_gameController.GetComponent<ViewController>()._leaveText.SetActive(true);
			_kaiAnimator.SetBool("isWin",true);
			GameObject.Find("SoundController").gameObject.GetComponent<SoundController>().playSound(3);
			_gameController.GetComponent<AudioSource>().enabled = false;
			GameObject.Find("exitDone").GetComponent<Text>().enabled = false;

			
			other.gameObject.GetComponent<Collider2D>().enabled = false;
			GameObject.Find("Fade").GetComponent<SpriteRenderer>().enabled = true;
			
			GlobalVariables._stageComplete = true;
			GlobalVariables._followPlayer = false;
			Destroy(other.gameObject,3.5f);
			
			Invoke("stageCompleteEvent",3.5f);
			*/
		}

		else if(other.gameObject.tag.Equals("Bonus"))
		{ 
			GlobalVariables._followPlayer = false;
			GameObject.Find("SoundController").gameObject.GetComponent<SoundController>().playSound(1);
			this._gameController.GetComponent<BonusController>().bonusCollision(other.GetComponent<Bonus>()._myType, other.GetComponent<Bonus>()._position);
			
			if(other.gameObject.GetComponent<Bonus>()._myType != BonusTypes.Types.PORTAL && other.gameObject.GetComponent<Bonus>()._myType != BonusTypes.Types.TELEPORT)
				other.GetComponent<QuadraticInterpolation>().GoToTarget();
			
			else
				Destroy(other.gameObject);
		}
	}

	public void receiveAnswer(bool answerBool)
	{
		print("Esto llego...:" + answerBool);
		_gameController.GetComponent<ViewController>()._questionsCanvas.SetActive(false);
		_gameController.GetComponent<ViewController>()._gameSceneCanvas.SetActive(true);
		Time.timeScale = 1;

		if(answerBool)
		{
			Destroy(_portalTransform.gameObject,3.5f);
			GameObject.Find("SoundController").gameObject.GetComponent<SoundController>().playSound(3);
			_gameController.GetComponent<AudioSource>().enabled = false;
			_gameController.GetComponent<ViewController>()._leaveText.SetActive(true);
			_kaiAnimator.SetBool("isWin",true);
			GameObject.Find("exitDone").GetComponent<Text>().enabled = false;
			GlobalVariables._stageComplete = true;
			GlobalVariables._followPlayer = false;

			Invoke("stageCompleteEvent",3.5f);
		}

		else
		{
			Destroy(_portalTransform.gameObject);
		}

	}

	public void stageCompleteEvent()
	{
		GameObject.Find("Fade").GetComponent<SpriteRenderer>().enabled = false;
		GameObject.Find("exitDone").GetComponent<Text>().enabled = false;
		_gameController.GetComponent<ViewController>()._leaveText.SetActive(false);

		_kaiAnimator.SetBool("isWin",false);
		GlobalVariables._stageComplete = false;
		GlobalVariables._followPlayer = true;
		this._gameController.GetComponent<ViewController>().pressEnter();
	}

	
}