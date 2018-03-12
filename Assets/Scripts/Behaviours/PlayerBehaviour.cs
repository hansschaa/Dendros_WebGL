using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour 
{

	public Vector2 _nextPosition;
	public Vector2 _nextPositionToChange;
	public List<Vector2> playerPath;
	public Moves currentMove;
	public float time;
	public float speed;

	public int currentNode;

	private GameObject _gameController;

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
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetKeyDown(KeyCode.X))
		{
			print("X position Global: " + GlobalVariables._xPosPlayer);
			print("Y position Global: " + GlobalVariables._yPosPlayer );
		}
		
		if(currentMove == Moves.NOTHING)
		{
			_kaiAnimator.SetFloat("movX",0);
			_kaiAnimator.SetFloat("movY",0);
		}

		#region Controls 
		if(Input.GetKeyDown(GlobalVariables._down) && !GlobalVariables._changeDirection || Input.GetKeyDown(GlobalVariables._up)  && GlobalVariables._changeDirection)
		{
			
			if(canMove(1,0,Moves.DOWN))
			{
				resetValues();

				generatePlayerPath(Moves.DOWN);

				updateValues(1,0,Moves.DOWN);
				_kaiAnimator.SetFloat("movX",0);
				_kaiAnimator.SetFloat("movY",-1);
			}
		}

		else if(Input.GetKeyDown(GlobalVariables._up)  && !GlobalVariables._changeDirection || Input.GetKeyDown(GlobalVariables._down) && GlobalVariables._changeDirection)
		{
			if(canMove(-1,0,Moves.UP))
			{
				resetValues();

				generatePlayerPath(Moves.UP);

				updateValues(-1,0,Moves.UP);

				_kaiAnimator.SetFloat("movX",0);
				_kaiAnimator.SetFloat("movY",1);
			}	
		}

		else if(Input.GetKeyDown(GlobalVariables._left)  && !GlobalVariables._changeDirection || Input.GetKeyDown(GlobalVariables._right) && GlobalVariables._changeDirection)
		{
			if(canMove(0,-1,Moves.LEFT))
			{
				resetValues();

				generatePlayerPath(Moves.LEFT);

				updateValues(0,-1,Moves.LEFT);

				_kaiAnimator.SetFloat("movX",-1);
				_kaiAnimator.SetFloat("movY",0);
			}	
		}

		else if(Input.GetKeyDown(GlobalVariables._right)  && !GlobalVariables._changeDirection || Input.GetKeyDown(GlobalVariables._left)  && GlobalVariables._changeDirection)
		{
			if(canMove(0,1,Moves.RIGHT))
			{
				resetValues();

				generatePlayerPath(Moves.RIGHT);

				updateValues(0,1,Moves.RIGHT);

				_kaiAnimator.SetFloat("movX",1);
				_kaiAnimator.SetFloat("movY",0);
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

    private void moveGameObject(int i, int j)
    {
        if((Vector2)this.transform.position != _nextPosition)
		{	
			this.transform.position = Vector2.MoveTowards(this.transform.position, this._nextPosition, time);
			if(i==0)
			{
				if(Math.Abs(this.transform.position.x - _nextPositionToChange.x) <=0.03f && !_entry)
				{
					_entry = true;
						GlobalVariables._xPosPlayer+=j;
						GlobalVariables._yPosPlayer+=i;
						// print("Posicion X: " + GlobalVariables._xPosPlayer);
						// print("Posicion Y: " + GlobalVariables._yPosPlayer);
						print("Llegó de la nueva forma");

				}

			}

			else {

				if(Math.Abs(this.transform.position.y - _nextPositionToChange.y) <=0.03f && !_entry)
				{
					_entry = true;
						GlobalVariables._xPosPlayer+=j;
						GlobalVariables._yPosPlayer+=i;
						// print("Posicion X: " + GlobalVariables._xPosPlayer);
						// print("Posicion Y: " + GlobalVariables._yPosPlayer);
						print("Llegó de la nueva forma pero en y");

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
		if(mov == Moves.DOWN)
		{
			for(int i = (GlobalVariables._yPosPlayer+1) ; i <ViewController._currentGameModel._map.GetLength(0) && ViewController._currentGameModel._map[i,GlobalVariables._xPosPlayer]==15; i++)
			{
				playerPath.Add(new Vector2(i, GlobalVariables._xPosPlayer));
			}
		}

		else if(mov == Moves.UP)
		{
			for(int i =(GlobalVariables._yPosPlayer-1) ; i >=0 && ViewController._currentGameModel._map[i,GlobalVariables._xPosPlayer]==15; i--)
			{
				playerPath.Add(new Vector2(i, GlobalVariables._xPosPlayer));
			}
		}	

		else if(mov == Moves.LEFT)
		{
			for(int i = (GlobalVariables._xPosPlayer-1) ; i >=0 && ViewController._currentGameModel._map[GlobalVariables._yPosPlayer,i]==15; i--)
			{
				playerPath.Add(new Vector2(GlobalVariables._yPosPlayer, i));
			}
		}	

		else if(mov == Moves.RIGHT)
		{
			for(int i = (GlobalVariables._xPosPlayer+1) ; i < ViewController._currentGameModel._map.GetLength(1) && ViewController._currentGameModel._map[GlobalVariables._yPosPlayer,i]==15; i++)
			{
				playerPath.Add(new Vector2(GlobalVariables._yPosPlayer, i));
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
		// print("GlobalVariables._xPosPlayer+y: " + (GlobalVariables._xPosPlayer+i));
		// print("GlobalVariables._yPosPlayer+x: " + (GlobalVariables._yPosPlayer+j));


		if((GlobalVariables._yPosPlayer+j == GlobalVariables._iMaxMatrix || GlobalVariables._yPosPlayer+j < 0) || 
		(GlobalVariables._xPosPlayer+i == GlobalVariables._jMaxMatrix || GlobalVariables._xPosPlayer+i < 0))
			return false;

		//14 es el numero max que puede tomar , porque el 15 es suelo , y hasta el 14 son tiles que representan obstaculos
		if(ViewController._currentGameModel._map[GlobalVariables._yPosPlayer+j,GlobalVariables._xPosPlayer+i]==15 && currentMove != mov)
		{
			
			// print("Entra");
			this._nextNodeCheckMove.x = (GlobalVariables._xPosPlayer* GlobalVariables._widthTile)- MapGeneratorController._offsetMap.x;
			this._nextNodeCheckMove.y = (GlobalVariables._yPosPlayer*-GlobalVariables._widthTile)- MapGeneratorController._offsetMap.y;
			return checkRange(mov);
		}

		// print("Retorno false");
		return false;
    }

    private bool checkRange(Moves mov)
    {
		if(currentMove != Moves.NOTHING)
		{
			// print("Entro al check range");
			// print("x: " + this._nextNodeCheckMove.x);
			// print("y: " + this._nextNodeCheckMove.y);
			// print("x Player: " + this.gameObject.transform.position.x);
			// print("y Player: " + this.gameObject.transform.position.y);
			// print("Resultado X: " + (this.transform.position.x-_nextNodeCheckMove.x));
			// print("Resultado Y: " + (this.transform.position.y-_nextNodeCheckMove.y));

		}
		
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
			
		
		//Comparando Y Cuando hay un cambio de direccion
		// if( currentMove == Moves.LEFT && (mov == Moves.UP || mov == Moves.UP) || currentMove == Moves.RIGHT && (mov == Moves.UP || mov == Moves.DOWN))
		// 	if(Math.Abs(this.transform.position.x-_nextNodeCheckMove.x) <= this._pixelsBeforeMove || currentMove == Moves.NOTHING)
		// 	{
		// 		print("Retorno true");
		// 		return true;
		// 	}

		return false;
		
        // switch(mov)
		// {
		// 	case Moves.LEFT:
		// 		if(this.)
				

		// 	break;

		// 	case Moves.RIGHT:
		// 	break;

		// 	case Moves.DOWN:
		// 	break;

		// 	case Moves.UP:
		// 	break;
		// }
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
			_kaiAnimator.SetBool("damage",true);
		
			GameObject.Find("SoundController").gameObject.GetComponent<SoundController>().playSound(4);
			
			this._gameController.GetComponent<MechanicController>().resetPlayerPosition();
			GlobalVariables._followPlayer=false;
		}

		else if(other.gameObject.tag.Equals("Portal"))
		{
			this._gameController.GetComponent<ViewController>().pressEnter();
			Destroy(other.gameObject);
			// print("Toco portal");
		}

		else if(other.gameObject.tag.Equals("Bonus"))
		{ 
			GameObject.Find("SoundController").gameObject.GetComponent<SoundController>().playSound(1);
			this._gameController.GetComponent<BonusController>().bonusCollision(other.GetComponent<Bonus>()._myType, other.GetComponent<Bonus>()._position);
			// other.GetComponent<BoxCollider2D>().enabled = false;
			Destroy(other.gameObject);
		}
	}

	
}