using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour 
{
	static Vector2 _currentPositionHolder;
	static Vector2 _backPosition;
	public List<Vector2> _currentPath;
	public float _speed;
	public float _time;
	
	int _currentNode;
	SearchManager _gestorBusqueda;

	private Animator _borisAnimator;

	public float _frecuency;

	/// <summary>
	/// Awake is called when the script instance is being loaded.
	/// </summary>
	void Awake()
	{
		this._borisAnimator = this.transform.GetChild(0).gameObject.GetComponent<Animator>();
		
		this._currentPath = new List<Vector2>();

		this._gestorBusqueda = new SearchManager();

		GlobalVariables._enemy = this.gameObject;
	}
	
	

	void Start()
	{
		GlobalVariables._followPlayer = true;
		GlobalVariables._stageComplete = false;
		StartCoroutine(search());
	}

	void Update()
	{
		if(GlobalVariables._runUpdateEnemy)
		{
			this._time = Time.deltaTime * this._speed;
			if(_currentPath!=null && GlobalVariables._followPlayer)
			{
				if((Vector2)this.transform.position != _currentPositionHolder)
				{
					this.transform.position = Vector2.MoveTowards(this.transform.position, _currentPositionHolder, this._time);
				}

				else
				{
					this._currentNode++;
					if(this._currentNode == _currentPath.Count)
					{
						this._currentNode = 0;
					}

					else
					{
						this._time = 0;
						if(this._currentPath.Count>0)
							_currentPositionHolder = new Vector2( _currentPath[this._currentNode].x*GlobalVariables._widthTile,_currentPath[this._currentNode].y*-GlobalVariables._widthTile) - MapGeneratorController._offsetMap;

						else
							return;
						
					
						//Izquierda
						if  (_currentPath[_currentNode-1].x> _currentPath[_currentNode].x)
						{
							if(GlobalVariables._xPosEnemy > 0)
								GlobalVariables._xPosEnemy-=1;
					
							this._borisAnimator.SetFloat("movX",-1);
							this._borisAnimator.SetFloat("movY",0);
						}

						//Derecha
						else if  (_currentPath[_currentNode-1].x< _currentPath[_currentNode].x)
						{
							
							if(GlobalVariables._xPosEnemy < 13)
								GlobalVariables._xPosEnemy+=1;
							
							this._borisAnimator.SetFloat("movX",1);
							this._borisAnimator.SetFloat("movY",0);
						}

						//ARRIBA
						else if  (_currentPath[_currentNode-1].y > _currentPath[_currentNode].y)
						{
							if(GlobalVariables._yPosEnemy > 0)
								GlobalVariables._yPosEnemy-=1; 
								
							this._borisAnimator.SetFloat("movX",0);
							this._borisAnimator.SetFloat("movY",1);
						}

						//ABAJO
						else if  (_currentPath[_currentNode-1].y< _currentPath[_currentNode].y)
						{
							if(GlobalVariables._yPosEnemy < 13)
								GlobalVariables._yPosEnemy+=1;  

							this._borisAnimator.SetFloat("movX",0);
							this._borisAnimator.SetFloat("movY",-1);
						}
					}
				}
			}
		
			else if(!GlobalVariables._followPlayer && !GlobalVariables._stageComplete)
			{
				this._currentPath.Clear();
				GlobalVariables._followPlayer = true;
			}

			else if(!GlobalVariables._followPlayer && GlobalVariables._stageComplete)
			{
				this._currentPath.Clear();
			}

		}
		
	}

	IEnumerator search()
	{
		while(GlobalVariables._followPlayer)
		{
			this._currentNode = 0;
			_currentPath = this._gestorBusqueda.encontrarCamino(new Vector2(GlobalVariables._xPosEnemy,GlobalVariables._yPosEnemy), new Vector2(GlobalVariables._xPosPlayer,GlobalVariables._yPosPlayer));
			if(_currentPath!= null)
			{
				if(_currentPath.Count>0)
					_currentPositionHolder = new Vector2( ((_currentPath[this._currentNode].x*GlobalVariables._widthTile)),(_currentPath[this._currentNode].y*-GlobalVariables._widthTile))- MapGeneratorController._offsetMap;
			}
			yield return new WaitForSeconds(_frecuency);
		}
			
	}

	public void imprimir()
	{
		for(int i = 0 ; i < _currentPath.Count;i++)
		{
			print(_currentPath[i]);
		}
	}
}