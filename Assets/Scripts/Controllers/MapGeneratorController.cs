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


	/// <summary>
	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>
	void Start()
	{
		MapGeneratorController._offsetMap = new Vector2((GlobalVariables._iMaxMatrix* GlobalVariables._widthTile)/2 + GlobalVariables._widthTile,(GlobalVariables._jMaxMatrix*-GlobalVariables._widthTile)/2 + GlobalVariables._widthTile + _marginBottom);
	}

	public void drawMap()
	{
		GameObject currentTile;
        for(int i = 0 ; i < GlobalVariables._iMaxMatrix ; i++)
		{
			for(int j = 0 ; j < GlobalVariables._jMaxMatrix; j++)
			{
				int valor = ViewController._currentGameModel._map[i,j];
				currentTile = Instantiate(_tile,(new Vector2(j*GlobalVariables._widthTile, - i*GlobalVariables._widthTile)- _offsetMap), Quaternion.identity, _gameObjectMap) as GameObject;  
				currentTile.GetComponent<SpriteRenderer>().sprite = tilesGameObjects[valor];

				
					// case 0: Instantiate(_tile,(new Vector2(j*0.64f, - i* 0.64f)-offsetMap + new Vector2(0.32f,0.32f)), Quaternion.identity); break;
					// case 1: _tilemap.SetTile(_tilemap.WorldToCell(new Vector2(j*0.64f, - i* 0.64f)-offsetMap), tiles[1]); break;
					// case 2: _tilemap.SetTile(_tilemap.WorldToCell(new Vector2(j*0.64f, - i* 0.64f)-offsetMap), tiles[2]); break;
					// case 3: _tilemap.SetTile(_tilemap.WorldToCell(new Vector2(j*0.64f, - i* 0.64f)-offsetMap), tiles[3]); break;
					// case 4: _tilemap.SetTile(_tilemap.WorldToCell(new Vector2(j*0.64f, - i* 0.64f)-offsetMap), tiles[4]); break;
					// case 5: _tilemap.SetTile(_tilemap.WorldToCell(new Vector2(j*0.64f, - i* 0.64f)-offsetMap), tiles[5]); break;
					// case 6: _tilemap.SetTile(_tilemap.WorldToCell(new Vector2(j*0.64f, - i* 0.64f)-offsetMap), tiles[6]); break;
					// case 7: _tilemap.SetTile(_tilemap.WorldToCell(new Vector2(j*0.64f, - i* 0.64f)-offsetMap), tiles[7]); break;
					// case 8: _tilemap.SetTile(_tilemap.WorldToCell(new Vector2(j*0.64f, - i* 0.64f)-offsetMap), tiles[8]); break;
					// case 9: _tilemap.SetTile(_tilemap.WorldToCell(new Vector2(j*0.64f, - i* 0.64f)-offsetMap), tiles[9]); break;
					// case 10: _tilemap.SetTile(_tilemap.WorldToCell(new Vector2(j*0.64f, - i* 0.64f)-offsetMap), tiles[10]); break;
					// case 11: _tilemap.SetTile(_tilemap.WorldToCell(new Vector2(j*0.64f, - i* 0.64f)-offsetMap), tiles[11]); break;
					// case 12: _tilemap.SetTile(_tilemap.WorldToCell(new Vector2(j*0.64f, - i* 0.64f)-offsetMap), tiles[12]); break;
					// case 13: _tilemap.SetTile(_tilemap.WorldToCell(new Vector2(j*0.64f, - i* 0.64f)-offsetMap), tiles[13]); break;
			}
		}

	}
}
