using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour 
{

	public Vector2 currentPositionHolder;
	public List<Vector2> playerPath;
	public Moves currentMove;
	public float time;
	public float speed;

	public int currentNode;

	private GameObject _gameController;
	

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
		playerPath = new List<Vector2>();
		currentMove = Moves.NOTHING;
		speed = GlobalVariables._playerVelocity;
	}
	
	// Update is called once per frame
	void Update () 
	{

		if(currentMove == Moves.NOTHING)
		{
			this.gameObject.GetComponent<Animator>().SetFloat("movX",0);
			this.gameObject.GetComponent<Animator>().SetFloat("movY",0);
		}

		#region Controls 
		if(Input.GetKeyDown(KeyCode.S) && !GlobalVariables._changeDirection || Input.GetKeyDown(KeyCode.W)  && GlobalVariables._changeDirection)
		{
			if(canMove(1,0,Moves.DOWN))
			{
				resetValues();

				generatePlayerPath(Moves.DOWN);

				updateValues(1,0,Moves.DOWN);
				this.gameObject.GetComponent<Animator>().SetFloat("movX",0);
				this.gameObject.GetComponent<Animator>().SetFloat("movY",-1);
			}
		}

		else if(Input.GetKeyDown(KeyCode.W)  && !GlobalVariables._changeDirection || Input.GetKeyDown(KeyCode.S) && GlobalVariables._changeDirection)
		{
			if(canMove(-1,0,Moves.UP))
			{
				resetValues();

				generatePlayerPath(Moves.UP);

				updateValues(-1,0,Moves.UP);

				this.gameObject.GetComponent<Animator>().SetFloat("movX",0);
				this.gameObject.GetComponent<Animator>().SetFloat("movY",1);
			}	
		}

		else if(Input.GetKeyDown(KeyCode.A)  && !GlobalVariables._changeDirection || Input.GetKeyDown(KeyCode.D) && GlobalVariables._changeDirection)
		{
			if(canMove(0,-1,Moves.LEFT))
			{
				resetValues();

				generatePlayerPath(Moves.LEFT);

				updateValues(0,-1,Moves.LEFT);

				this.gameObject.GetComponent<Animator>().SetFloat("movX",-1);
				this.gameObject.GetComponent<Animator>().SetFloat("movY",0);
			}	
		}

		else if(Input.GetKeyDown(KeyCode.D)  && !GlobalVariables._changeDirection || Input.GetKeyDown(KeyCode.A)  && GlobalVariables._changeDirection)
		{
			if(canMove(0,1,Moves.RIGHT))
			{
				resetValues();

				generatePlayerPath(Moves.RIGHT);

				updateValues(0,1,Moves.RIGHT);

				this.gameObject.GetComponent<Animator>().SetFloat("movX",1);
				this.gameObject.GetComponent<Animator>().SetFloat("movY",0);
			}
		}
		#endregion

		// if(Input.GetKeyDown(KeyCode.S) && GlobalVariables._changeDirection)
		// {
		// 	if(canMove(-1,0,Moves.UP))
		// 		return;
			
		// 	resetValues();

		// 	generatePlayerPath(Moves.UP);

		// 	updateValues(-1,0,Moves.UP);

		// 	this.gameObject.GetComponent<Animator>().SetFloat("movX",0);
		// 	this.gameObject.GetComponent<Animator>().SetFloat("movY",1);
		// }

		// else if(Input.GetKeyDown(KeyCode.W)  && GlobalVariables._changeDirection)
		// {
		// 	if(canMove(1,0,Moves.DOWN))
		// 		return;
			
		// 	resetValues();

		// 	generatePlayerPath(Moves.DOWN);

		// 	updateValues(1,0,Moves.DOWN);


		// 	this.gameObject.GetComponent<Animator>().SetFloat("movX",0);
		// 	this.gameObject.GetComponent<Animator>().SetFloat("movY",-1);
		// }

		// else if(Input.GetKeyDown(KeyCode.A)  && GlobalVariables._changeDirection)
		// {
		// 	if(canMove(0,1,Moves.RIGHT))
		// 		return;
			
		// 	resetValues();

		// 	generatePlayerPath(Moves.RIGHT);

		// 	updateValues(0,1,Moves.RIGHT);

		// 	this.gameObject.GetComponent<Animator>().SetFloat("movX",1);
		// 	this.gameObject.GetComponent<Animator>().SetFloat("movY",0);
		// }

		// else if(Input.GetKeyDown(KeyCode.D)  && GlobalVariables._changeDirection)
		// {
		// 	if(canMove(0,-1,Moves.LEFT))
		// 		return;
			
		// 	resetValues();

		// 	generatePlayerPath(Moves.LEFT);

		// 	updateValues(0,-1,Moves.LEFT);

		// 	this.gameObject.GetComponent<Animator>().SetFloat("movX",-1);
		// 	this.gameObject.GetComponent<Animator>().SetFloat("movY",0);
		// }
	


		#region Move
		if(currentMove == Moves.DOWN)
		{
			time = Time.deltaTime * speed;
			moveGameObject(1,0);
		}

		if(currentMove == Moves.UP)
		{
			time = Time.deltaTime * speed;
			moveGameObject(-1,0);
		}

		if(currentMove == Moves.LEFT)
		{
			time = Time.deltaTime * speed;
			moveGameObject(0,-1);
		}

		if(currentMove == Moves.RIGHT)
		{
			time = Time.deltaTime * speed;
			moveGameObject(0, 1);
		}
		#endregion moving


	}

    private void moveGameObject(int i, int j)
    {
        if((Vector2)this.transform.position != currentPositionHolder)
		{
			this.transform.position = Vector2.MoveTowards(this.transform.position, currentPositionHolder, time);
		}

		else
		{
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
					GlobalVariables._xPosPlayer+=j;
					GlobalVariables._yPosPlayer+=i;
					currentPositionHolder = new Vector2( ((playerPath[currentNode].y*0.16f)),(playerPath[currentNode].x*-0.16f)) - MapGeneratorController._offsetMap;
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
		
		currentPositionHolder = new Vector2( ((playerPath[0].y*0.16f)),(playerPath[0].x*-0.16f)) - MapGeneratorController._offsetMap;
		GlobalVariables._xPosPlayer+=j;
		GlobalVariables._yPosPlayer+=i;
		currentMove = mov;
		
		
    }

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

    private bool canMove(int x, int y, Moves mov)
    {
		// print("GlobalVariables._xPosPlayer+y: " + (GlobalVariables._xPosPlayer+y));
		// print("GlobalVariables._yPosPlayer+x: " + (GlobalVariables._yPosPlayer+x));


		if((GlobalVariables._yPosPlayer+x == GlobalVariables._iMaxMatrix || GlobalVariables._yPosPlayer+x < 0) || 
		(GlobalVariables._xPosPlayer+y == GlobalVariables._jMaxMatrix || GlobalVariables._xPosPlayer+y < 0))
			return false;

		//14 es el numero max que puede tomar , porque el 15 es suelo , y hasta el 14 son tiles que representan obstaculos
		return ViewController._currentGameModel._map[GlobalVariables._yPosPlayer+x,GlobalVariables._xPosPlayer+y]==15 && currentMove != mov;
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
			this._gameController.GetComponent<MechanicController>().resetPlayerPosition();
		}

		else if(other.gameObject.tag.Equals("Portal"))
		{
			this._gameController.GetComponent<ViewController>().pressEnter();
			Destroy(other.gameObject);
			print("Toco portal");
		}

		else if(other.gameObject.tag.Equals("Bonus"))
		{ 
			this._gameController.GetComponent<BonusController>().bonusCollision(other.GetComponent<Bonus>()._myType, other.GetComponent<Bonus>()._position);
			// other.GetComponent<BoxCollider2D>().enabled = false;
			Destroy(other.gameObject);
		}
	}

	
}