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

	/// <summary>
	/// Awake is called when the script instance is being loaded.
	/// </summary>
	void Awake()
	{
		
		this._currentPath = new List<Vector2>();

		this._gestorBusqueda = new SearchManager();
		
	}
	

	void Start()
	{
		
		
		StartCoroutine(search());
		
		
	}

	void Update()
	{

		this._time = Time.deltaTime * this._speed;
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
					_currentPositionHolder = new Vector2( ((_currentPath[this._currentNode].x*0.16f)),(_currentPath[this._currentNode].y*-0.16f)) - MapGeneratorController._offsetMap;

				else
					return;
				
			
				//Izquierda
				if  (_currentPath[_currentNode-1].x> _currentPath[_currentNode].x)
				{
					if(GlobalVariables._xPosEnemy != 0)
						GlobalVariables._xPosEnemy-=1;
			
					// print("Izquierda: " + GlobalVariables._xPosEnemy);
					this.gameObject.GetComponent<Animator>().SetFloat("movX",-1);
					this.gameObject.GetComponent<Animator>().SetFloat("movY",0);
				}

				//Derecha
				else if  (_currentPath[_currentNode-1].x< _currentPath[_currentNode].x)
				{
					
					if(GlobalVariables._xPosEnemy != 14)
						GlobalVariables._xPosEnemy+=1;
					
					// print("derecha: " + GlobalVariables._xPosEnemy);
					this.gameObject.GetComponent<Animator>().SetFloat("movX",1);
					this.gameObject.GetComponent<Animator>().SetFloat("movY",0);
				}

				//ARRIBA
				else if  (_currentPath[_currentNode-1].y > _currentPath[_currentNode].y)
				{
					if(GlobalVariables._yPosEnemy != 0)
						GlobalVariables._yPosEnemy-=1; 
						
					// print("arriba: " + GlobalVariables._yPosEnemy);
					this.gameObject.GetComponent<Animator>().SetFloat("movX",0);
					this.gameObject.GetComponent<Animator>().SetFloat("movY",1);
				}

				//ABAJO
				else if  (_currentPath[_currentNode-1].y< _currentPath[_currentNode].y)
				{
				
					GlobalVariables._yPosEnemy+=1; 
					// print("Abajo: " + GlobalVariables._yPosEnemy);
					this.gameObject.GetComponent<Animator>().SetFloat("movX",0);
					this.gameObject.GetComponent<Animator>().SetFloat("movY",-1);
				}

			}
		}
	}

	IEnumerator search()
	{
		while(true)
		{
			this._currentNode = 0;
			_currentPath.Clear();
			_currentPath = this._gestorBusqueda.encontrarCamino(new Vector2(GlobalVariables._xPosEnemy,GlobalVariables._yPosEnemy), new Vector2(GlobalVariables._xPosPlayer,GlobalVariables._yPosPlayer));
			if(_currentPath.Count>0)
				_currentPositionHolder = new Vector2( ((_currentPath[this._currentNode].x*0.16f)),(_currentPath[this._currentNode].y*-0.16f))- MapGeneratorController._offsetMap;
				
			yield return new WaitForSeconds(0.5f);
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