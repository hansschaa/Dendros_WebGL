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
	// public GameObject _enemy;
	float deltaTime;


	public float _pixelsBeforeMove;
	public Vector2 _nextNodeCheckMove;
	private Animator _kaiAnimator;
	private bool _entry;

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
		this._entry = false;
		this._kaiAnimator = this.transform.GetChild(0).gameObject.GetComponent<Animator>();
		playerPath = new List<Vector2>();
		currentMove = Moves.NOTHING;
		speed = GlobalVariables._playerVelocity;
		this._nextNodeCheckMove = Vector2.zero;
		// this._enemy = GameObject.Find("BorisEnemy(Clone)").gameObject;
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


		if(Input.GetKeyDown(KeyCode.X))
		{
			print("X position Global: " + GlobalVariables._xPosPlayer);
			print("Y position Global: " + GlobalVariables._yPosPlayer );
		}
		
		// if(currentMove == Moves.NOTHING)
		// {
		// 	// _kaiAnimator.SetBool("walk",false);
		// 	// _kaiAnimator.SetFloat("movX",0);
		// 	// _kaiAnimator.SetFloat("movY",0);
		// }

		float x = Input.GetAxis("Horizontal");
		float y = Input.GetAxis("Vertical");

		#region Controls 
		if(!GlobalVariables._stageComplete)
		{
			if(y<0 && !GlobalVariables._changeDirection || y>0  && GlobalVariables._changeDirection)
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

			else if(y>0  && !GlobalVariables._changeDirection || y<0 && GlobalVariables._changeDirection)
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

			else if(x<0  && !GlobalVariables._changeDirection || x>0 && GlobalVariables._changeDirection)
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

			else if(x>0  && !GlobalVariables._changeDirection || x<0 && GlobalVariables._changeDirection)
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

		}
		

	}

    private void moveGameObject(int i, int j)
    {
        if((Vector2)this.transform.position != _nextPosition)
		{	
			this.transform.position = Vector2.MoveTowards(this.transform.position, this._nextPosition, time);
			// if(i==0)
			// {
			// 	if(Math.Abs(this.transform.position.x - _nextPositionToChange.x) <=0.04f && !_entry)
			// 	{
			// 		_entry = true;
			// 			GlobalVariables._xPosPlayer+=j;
			// 			GlobalVariables._yPosPlayer+=i;
			// 			// print("Posicion X: " + GlobalVariables._xPosPlayer);
			// 			// print("Posicion Y: " + GlobalVariables._yPosPlayer);
			// 			// print("Llegó de la nueva forma");

			// 	}

			// }

			// else {

			// 	if(Math.Abs(this.transform.position.y - _nextPositionToChange.y) <=0.04f && !_entry)
			// 	{
			// 		_entry = true;
			// 			GlobalVariables._xPosPlayer+=j;
			// 			GlobalVariables._yPosPlayer+=i;
			// 			// print("Posicion X: " + GlobalVariables._xPosPlayer);
			// 			// print("Posicion Y: " + GlobalVariables._yPosPlayer);
			// 			// print("Llegó de la nueva forma pero en y");

			// 	}


			// 	// if(Vector2.Distance(this.transform.position ,_nextPositionToChange) <= 0.03f && !_entry)
			// 	// {
					
			// 	// 		_entry = true;
			// 	// 		GlobalVariables._xPosPlayer+=j;
			// 	// 		GlobalVariables._yPosPlayer+=i;
			// 	// 		// print("Posicion X: " + GlobalVariables._xPosPlayer);
			// 	// 		// print("Posicion Y: " + GlobalVariables._yPosPlayer);
			// 	// 		print("Llegó de la otra forma");

			// 	// }
			// }

			if(i==0)
			{
				if(j==1 && this.transform.position.x >= _nextPositionToChange.x && !_entry)
				{
					_entry = true;
						GlobalVariables._xPosPlayer+=j;
						GlobalVariables._yPosPlayer+=i;
						// print("Posicion X: " + GlobalVariables._xPosPlayer);
						// print("Posicion Y: " + GlobalVariables._yPosPlayer);
						// print("Llegó de la nueva forma");

				}

				else if(j==-1 && this.transform.position.x <= _nextPositionToChange.x && !_entry)
				{
					_entry = true;
						GlobalVariables._xPosPlayer+=j;
						GlobalVariables._yPosPlayer+=i;
						// print("Posicion X: " + GlobalVariables._xPosPlayer);
						// print("Posicion Y: " + GlobalVariables._yPosPlayer);
						// print("Llegó de la nueva forma");

				}

			}

			else {

				if(i == 1 && this.transform.position.y <= _nextPositionToChange.y && !_entry)
				{
					_entry = true;
						GlobalVariables._xPosPlayer+=j;
						GlobalVariables._yPosPlayer+=i;
						// print("Posicion X: " + GlobalVariables._xPosPlayer);
						// print("Posicion Y: " + GlobalVariables._yPosPlayer);
						// print("Llegó de la nueva forma pero en y");

				}

				else if(i == -1 && this.transform.position.y >= _nextPositionToChange.y && !_entry)
				{
					_entry = true;
						GlobalVariables._xPosPlayer+=j;
						GlobalVariables._yPosPlayer+=i;
						// print("Posicion X: " + GlobalVariables._xPosPlayer);
						// print("Posicion Y: " + GlobalVariables._yPosPlayer);
						// print("Llegó de la nueva forma pero en y");

				}


				// if(Vector2.Distance(this.transform.position ,_nextPositionToChange) <= 0.03f && !_entry)
				// {
					
				// 		_entry = true;
				// 		GlobalVariables._xPosPlayer+=j;
				// 		GlobalVariables._yPosPlayer+=i;
				// 		// print("Posicion X: " + GlobalVariables._xPosPlayer);
				// 		// print("Posicion Y: " + GlobalVariables._yPosPlayer);
				// 		print("Llegó de la otra forma");

				// }
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

					// this._nextPosition = new Vector2( playerPath[currentNode].y* GlobalVariables._widthTile + (i * GlobalVariables._widthTile), playerPath[currentNode].x*-GlobalVariables._widthTile + j*GlobalVariables._widthTile - GlobalVariables._widthTile/2) - MapGeneratorController._offsetMap;

					// GlobalVariables._xPosPlayer+=j;
					// GlobalVariables._yPosPlayer+=i;
					// getNextPosition(2,i,j);
					
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
		// print("Posicion X en update value: " + GlobalVariables._xPosPlayer);
		// 		print("Posicion Y en update value: " + GlobalVariables._yPosPlayer);
				_entry = false;
		
		currentNode=0;
		this._nextPosition = new Vector2( playerPath[currentNode].y* GlobalVariables._widthTile, playerPath[currentNode].x*-GlobalVariables._widthTile) - MapGeneratorController._offsetMap;
		this._nextPositionToChange = new Vector2( playerPath[currentNode].y* GlobalVariables._widthTile-(j*GlobalVariables._widthTile/2), playerPath[currentNode].x*-GlobalVariables._widthTile +(i*GlobalVariables._widthTile/2)) - MapGeneratorController._offsetMap;
		// print(_nextPosition);
		// getNextPosition(i,j);
		// GlobalVariables._xPosPlayer+=j;
		// GlobalVariables._yPosPlayer+=i;
		
		currentMove = mov;
    }

	// public void getNextPosition(int i,int j)
	// {
	// 	// this._nextPosition = new Vector2( ((playerPath[currentNode].y* GlobalVariables._widthTile)),(playerPath[currentNode].x*-GlobalVariables._widthTile)) - MapGeneratorController._offsetMap;
	// 	this._nextPosition = new Vector2( ((playerPath[currentNode].y* GlobalVariables._widthTile)),(playerPath[currentNode].x*-GlobalVariables._widthTile)) - MapGeneratorController._offsetMap;
		
		
			
	// }

    private void generatePlayerPath(Moves mov)
    {
		// if(mov == Moves.DOWN)
		// {
			// for(int i = (GlobalVariables._yPosPlayer+1) ; i <ViewController._currentGameModel._map.GetLength(0) && ViewController._currentGameModel._map[i,GlobalVariables._xPosPlayer]!=30; i++)
			// {
			// 	playerPath.Add(new Vector2(i, GlobalVariables._xPosPlayer));
			// }
			// for(int i = Glo)
			// {

			// }
		// }
		int valor = ViewController._currentGameModel._map[GlobalVariables._yPosPlayer,GlobalVariables._xPosPlayer];
		
		// if(valor > 14)
		// {
		// 	do
		// 	{
		// 		valor-=15;
			
		// 	}while(valor > 14);
		// }

		// print("Este es el valor que buscas?: " + valor);

		if(mov == Moves.UP)
		{
			for(int i =GlobalVariables._yPosPlayer -1; i >=0 && GlobalVariables._allowedMovements[valor,0]==1; i--)
			{
				playerPath.Add(new Vector2(i, GlobalVariables._xPosPlayer));
				valor = ViewController._currentGameModel._map[i,GlobalVariables._xPosPlayer];
				// if(valor > 14)
				// {
				// 	do
				// 	{
				// 		valor-=15;
					
				// 	}while(valor > 14);
				// }
			}
		}	

		else if(mov == Moves.RIGHT)
		{
			for(int i =GlobalVariables._xPosPlayer +1; i < ViewController._currentGameModel._map.GetLength(1) && GlobalVariables._allowedMovements[valor,1]==1 ; i++)
			{
				playerPath.Add(new Vector2(GlobalVariables._yPosPlayer, i));
				valor = ViewController._currentGameModel._map[GlobalVariables._yPosPlayer,i];
				// if(valor > 14)
				// {
				// 	do
				// 	{
				// 		valor-=15;
					
				// 	}while(valor > 14);
				// }
			}
			// for(int i = (GlobalVariables._xPosPlayer+1) ; i < ViewController._currentGameModel._map.GetLength(1) && ViewController._currentGameModel._map[GlobalVariables._yPosPlayer,i]!=30; i++)
			// {
			// 	playerPath.Add(new Vector2(GlobalVariables._yPosPlayer, i));
			// }
		}

		else if(mov == Moves.DOWN)
		{
			for(int i =GlobalVariables._yPosPlayer +1; i <ViewController._currentGameModel._map.GetLength(0) && GlobalVariables._allowedMovements[valor,2]==1; i++)
			{
				playerPath.Add(new Vector2(i, GlobalVariables._xPosPlayer));
				valor = ViewController._currentGameModel._map[i,GlobalVariables._xPosPlayer];
				// if(valor > 14)
				// {
				// 	do
				// 	{
				// 		valor-=15;
					
				// 	}while(valor > 14);
				// }
			}
		}

		else if(mov == Moves.LEFT)
		{

			for(int i =GlobalVariables._xPosPlayer- 1; i >=0 && GlobalVariables._allowedMovements[valor,3]==1 ; i--)
			{
				playerPath.Add(new Vector2(GlobalVariables._yPosPlayer, i));
				valor = ViewController._currentGameModel._map[GlobalVariables._yPosPlayer,i];
				// if(valor > 14)
				// {
				// 	do
				// 	{
				// 		valor-=15;
					
				// 	}while(valor > 14);
				// }
			}
			// for(int i = (GlobalVariables._xPosPlayer+1) ; i < ViewController._currentGameModel._map.GetLength(1) && ViewController._currentGameModel._map[GlobalVariables._yPosPlayer,i]!=30; i++)
			// {
			// 	playerPath.Add(new Vector2(GlobalVariables._yPosPlayer, i));
			// }
		}

		// else if(mov == Moves.LEFT)
		// {
		// 	for(int i = (GlobalVariables._xPosPlayer-1) ; i >=0 && ViewController._currentGameModel._map[GlobalVariables._yPosPlayer,i]!=30; i--)
		// 	{
		// 		playerPath.Add(new Vector2(GlobalVariables._yPosPlayer, i));
		// 	}
		// }	

			
    }

    private void resetValues()
    {
        playerPath.Clear();
		currentNode = 0;
    }

    private bool canMove(int j, int i, Moves mov)
    {
		// print("GlobalVariables._xPosPlayer+y: " + (GlobalVariables._xPosPlayer+i));
		// print("GlobalVariables._yPosPlayer+x: " + (GlobalVariables._yPosPlayer+j));
		if(this.currentMove == mov)
			return false;
			

		// if((GlobalVariables._yPosPlayer+j == GlobalVariables._iMaxMatrix || GlobalVariables._yPosPlayer+j < 0) || 
		// (GlobalVariables._xPosPlayer+i == GlobalVariables._jMaxMatrix || GlobalVariables._xPosPlayer+i < 0))
		// 	return false;

		int valor = ViewController._currentGameModel._map[GlobalVariables._yPosPlayer,GlobalVariables._xPosPlayer];
		this._nextNodeCheckMove.x = (GlobalVariables._xPosPlayer* GlobalVariables._widthTile)- MapGeneratorController._offsetMap.x;
		this._nextNodeCheckMove.y = (GlobalVariables._yPosPlayer*-GlobalVariables._widthTile)- MapGeneratorController._offsetMap.y;
		// print("Valor: " + valor);
		// if(valor > 14)
		// {
		// 	do
		// 	{
		// 		valor-=15;
		// 	}while(valor > 14);
			
		// }

		switch(mov)
		{
			
			case(Moves.UP):
				if(GlobalVariables._allowedMovements[valor,0]==1)
				{
					
					return checkRange(mov);
				}
				break;
			case(Moves.RIGHT):
				if(GlobalVariables._allowedMovements[valor,1]==1)
				{
					
					return checkRange(mov);
				}
				break;
			case(Moves.DOWN):
				if(GlobalVariables._allowedMovements[valor,2]==1)
				{
					return checkRange(mov);
				}
				break;
			case(Moves.LEFT):
				if(GlobalVariables._allowedMovements[valor,3]==1)
				{
					return checkRange(mov);
				}
				break;
		}

		//14 es el numero max que puede tomar , porque el 15 es suelo , y hasta el 14 son tiles que representan obstaculos
		// if(ViewController._currentGameModel._map[GlobalVariables._yPosPlayer+j,GlobalVariables._xPosPlayer+i]!=30)
		// {
			
		// 	// print("Entra");
		// 	this._nextNodeCheckMove.x = (GlobalVariables._xPosPlayer* GlobalVariables._widthTile)- MapGeneratorController._offsetMap.x;
		// 	this._nextNodeCheckMove.y = (GlobalVariables._yPosPlayer*-GlobalVariables._widthTile)- MapGeneratorController._offsetMap.y;
		// 	// return checkRange(mov);
		// 	return true;
		// }

		// print("Retorno false");
		return false;
    }

    private bool checkRange(Moves mov)
    {
		// if(currentMove != Moves.NOTHING)
		// {
		// 	// print("Entro al check range");
		// 	// print("x: " + this._nextNodeCheckMove.x);
		// 	// print("y: " + this._nextNodeCheckMove.y);
		// 	// print("x Player: " + this.gameObject.transform.position.x);
		// 	// print("y Player: " + this.gameObject.transform.position.y);
		// 	// print("Resultado X: " + (this.transform.position.x-_nextNodeCheckMove.x));
		// 	// print("Resultado Y: " + (this.transform.position.y-_nextNodeCheckMove.y));

		// }
		
		//Comparando X Cuando hay un cambio de direccion
		if( currentMove == Moves.LEFT && (mov == Moves.UP || mov == Moves.DOWN) || currentMove == Moves.RIGHT && (mov == Moves.UP || mov == Moves.DOWN))
		{
			
			if(Math.Abs(this.transform.position.x-_nextNodeCheckMove.x) <= this._pixelsBeforeMove)
			{
				// print("Entro al de la X");
				return true;
			}
		}

		else if( currentMove == Moves.DOWN && (mov == Moves.RIGHT || mov == Moves.LEFT) || currentMove == Moves.UP && (mov == Moves.RIGHT || mov == Moves.LEFT))
		{
	
			if(Math.Abs(this.transform.position.y-_nextNodeCheckMove.y) <= this._pixelsBeforeMove)
			{
				// print("Entro al de la y");
				return true;
			}
		}
			
		
		else if(currentMove == Moves.NOTHING || (currentMove == Moves.DOWN && mov == Moves.UP) || (currentMove == Moves.UP && mov == Moves.DOWN) ||
		(currentMove == Moves.LEFT && mov == Moves.RIGHT) || (currentMove == Moves.RIGHT && mov == Moves.LEFT))
		{
			// print("nothig");
			return true;
		}
			

		return false;

    }

    /// <summary>
    /// Sent when another object enters a trigger collider attached to this
    /// object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    void OnTriggerEnter2D(Collider2D other)
	{
		if(other.gameObject.tag.Equals("Enemy"))
		{
			
			// _kaiAnimator.SetFloat("movX",0);
			// _kaiAnimator.SetFloat("movY",0.5f);
			// _kaiAnimator.SetFloat("movX",0);
			_kaiAnimator.SetBool("isDeath",true);
			GameObject.Find("SoundController").gameObject.GetComponent<SoundController>().playSound(4);
			this._gameController.GetComponent<MechanicController>().resetPlayerPosition();
			GlobalVariables._followPlayer=false;
		}

		if(other.gameObject.tag.Equals("Portal"))
		{
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
			// print("Toco portal");
		}

		else if(other.gameObject.tag.Equals("Bonus"))
		{ 
			GlobalVariables._followPlayer = false;
			GameObject.Find("SoundController").gameObject.GetComponent<SoundController>().playSound(1);
			this._gameController.GetComponent<BonusController>().bonusCollision(other.GetComponent<Bonus>()._myType, other.GetComponent<Bonus>()._position);
			// other.GetComponent<BoxCollider2D>().enabled = false;
			Destroy(other.gameObject);
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