using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGeneratorController : MonoBehaviour 
{
	static public Vector2 _offsetMap;
	public Sprite[] tilesGameObjects;
	public GameObject _tile;
	public Transform _gameObjectMap;
	public float _marginBottom;
	public float _marginLeft;


	void Start()
	{
		MapGeneratorController._offsetMap = new Vector2((GlobalVariables._iMaxMatrix* GlobalVariables._widthTile)/2 + GlobalVariables._widthTile + _marginLeft,(GlobalVariables._jMaxMatrix*-GlobalVariables._widthTile)/2 + _marginBottom);
		print("OffsetMap: " + MapGeneratorController._offsetMap);
	}

	public void drawMap()
	{
		
		GameObject currentTile;
        for(int i = 0 ; i < GlobalVariables._iMaxMatrix ; i++)
		{
			for(int j = 0 ; j < GlobalVariables._jMaxMatrix; j++)
			{
				int valor = ViewController._currentGameModel._map[i,j];
				
				if(valor != -1)
				{
					currentTile = Instantiate(_tile,(new Vector2(j*GlobalVariables._widthTile, - i*GlobalVariables._widthTile)-MapGeneratorController._offsetMap), Quaternion.identity, _gameObjectMap) as GameObject;  
					currentTile.GetComponent<SpriteRenderer>().sprite = tilesGameObjects[valor];
				}

			}
		}

		print("Cantidad de go: " + _gameObjectMap.transform.childCount);

	}
}
