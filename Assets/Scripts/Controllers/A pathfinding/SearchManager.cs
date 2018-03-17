using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchManager 
{
	private const int costoIrDerecho = 10;
	private const int costoIrDiagonal = 15;
	private List<Node> listaAbierta = new List<Node>();
	private List<Vector2> listaCerrada = new List<Vector2>();

	/// <summary>
	/// Adiciona un Nodo a la lista abierta, ordenadamente
	/// </summary>
	/// <param name="nodo"></param>
	private void adicionarNodoAListaAbierta(Node nodo)
	{
		int indice = 0;
		float costo = nodo._costoTotal;
		while ((listaAbierta.Count > indice) && (costo < listaAbierta[indice]._costoTotal))
		{
			indice++;
		}

		listaAbierta.Insert(indice, nodo);
	}

	public List<Vector2> encontrarCamino(Vector2 posTileInicial, Vector2 posTileFinal)
	{
		// print("Encontrar cmaino");
		listaAbierta.Clear();
		listaCerrada.Clear();
		
		Node nodoFinal = new Node(null, null, posTileFinal, 0);
		Node nodoInicial = new Node(null, nodoFinal, posTileInicial, 0);
 		

		// se adiciona el nodo inicial
 		adicionarNodoAListaAbierta(nodoInicial);

		while (listaAbierta.Count > 0)
		{
			// print("while lista abierta");
			Node nodoActual = listaAbierta[listaAbierta.Count - 1];
			// si es el nodo Final
			if (nodoActual.esIgual(nodoFinal))
			{
				List<Vector2> mejorCamino = new List<Vector2>();
				while (nodoActual != null)
				{
					mejorCamino.Insert(0, nodoActual._posicion);
					// print(nodoActual._posicion);
					nodoActual = nodoActual._nodoPadre;
				}

				
				return mejorCamino;
			}
			listaAbierta.Remove(nodoActual);

			foreach (Node posibleNodo in encontrarNodosAdyacentes(nodoActual, nodoFinal))
			{
				// si el nodo no se encuentra en la lista cerrada
				if (!listaCerrada.Contains(posibleNodo._posicion))
				{
				// si ya se encuentra en la lista abierta
					if (listaAbierta.Contains(posibleNodo))
					{
						if (posibleNodo._costoG >= nodoActual._costoG)
						{
							continue;
						}
					}

					adicionarNodoAListaAbierta(posibleNodo);
					}
			}
			// se cierra el nodo actual
			listaCerrada.Add(nodoActual._posicion);
			}//cierre whiile
			return null;
	}

	private List<Node> encontrarNodosAdyacentes(Node nodoActual, Node nodoFinal)
	{
		List<Node> nodosAdyacentes = new List<Node>();
		int J = nodoActual._grillaX;
		int I = nodoActual._grillaY;
		int valor = ViewController._currentGameModel._map[I,J];
		// if(valor > 14)
		// {
		// 	do
		// 	{
		// 		valor-=15;
		// 	}while(valor > 14);
			
		// }

		// Debug.Log("	i: " + I + " , " + "j: " + J);
		
	

		// Izquierda
		if ((J > 0) && GlobalVariables._allowedMovements[valor,3]==1)
		{
			nodosAdyacentes.Add(new Node(nodoActual, nodoFinal, new Vector2(J-1,I), costoIrDerecho + nodoActual._costoG));
		}
		

		//Derecha
		if ((J < ViewController._currentGameModel._map.GetLength(1)-1 && GlobalVariables._allowedMovements[valor,1]==1))
		{
			nodosAdyacentes.Add(new Node(nodoActual, nodoFinal,new Vector2(J+1, I), costoIrDerecho + nodoActual._costoG));
		}
		

		//Arriba
		if ((I > 0) && GlobalVariables._allowedMovements[valor,0]==1)
		{
			nodosAdyacentes.Add(new Node(nodoActual, nodoFinal, new Vector2(J, I-1), costoIrDerecho + nodoActual._costoG));
		}

		// Abajo
		if (I < ViewController._currentGameModel._map.GetLength(0)-1 && GlobalVariables._allowedMovements[valor,2]==1)
		{
			nodosAdyacentes.Add(new Node(nodoActual, nodoFinal, new Vector2(J, I+1 ), costoIrDerecho + nodoActual._costoG));
		}

		
		return nodosAdyacentes;
	}	
}
