using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVariables 
{

	#region General Global Variables

	internal static bool _isSoundOn = true;
	internal static bool _isMusicOn = true;
	public static int _iMaxMatrix = 14;
	public static int _jMaxMatrix =14;
	public static float _widthTile = 0.3f;
	public static bool _answeredAnswer = false;
	public static bool _answerGood = false;

	#endregion


	#region Global Garage Variables

	public static int _currentLevel;
	//Level choose for the user in the garage View
	public static int _userLevel;
	public static int _totallyStages;
	public static bool _gameComplete = false;
	public static bool _iconStagesAdded= false;
	public static bool _stageComplete = false;

	#endregion


	#region Global Player Game Variables

	public static bool _runUpdatePlayer = true;
	public static bool _runUpdateEnemy = true;

	public static int _xPosPlayer;
	public static int _yPosPlayer;
	public static int _currentLifes = 3;
	public static int _globalLifes = 3;
	public static bool _yellowBonus = false;
	public static bool _purpleBonus = false;
	public static bool _changeDirection = false;
	public static bool _allowPurpleBonus = false;
	public static float _playerVelocity = 1f;

	#endregion
	

	#region Enemy Global Variables
	public static int _xPosEnemy;
	public static int _yPosEnemy;
	public static bool _followPlayer = true;
	public static GameObject _enemy;
	#endregion
		
	//public static List<int> _walkablesTiles = new List<int>();
	
	// [arriba, derecha, abajo, izquierda] : 1 / 0 segun se permita, 1 para permitir, 0 para no permitir
	public static int[,] _allowedMovements =   {{0,0,0,1},
												{1,0,0,0},
												{0,1,0,0},
												{0,0,1,0},
												{1,0,0,1},
												{1,1,0,0},
												{0,1,1,0},
												{0,0,1,1},
												{1,0,1,1},
												{1,1,0,1},
												{1,1,1,0},
												{0,1,1,1},
												{1,0,1,0},
												{0,1,0,1},
												{1,1,1,1},
												{0,0,0,1},
												{1,0,0,0},
												{0,1,0,0},
												{0,0,1,0},
												{1,0,0,1},
												{1,1,0,0},
												{0,1,1,0},
												{0,0,1,1},
												{1,0,1,1},
												{1,1,0,1},
												{1,1,1,0},
												{0,1,1,1},
												{1,0,1,0},
												{0,1,0,1},
												{1,1,1,1},
												{0,0,0,1},
												{1,0,0,0},
												{0,1,0,0},
												{0,0,1,0},
												{1,0,0,1},
												{1,1,0,0},
												{0,1,1,0},
												{0,0,1,1},
												{1,0,1,1},
												{1,1,0,1},
												{1,1,1,0},
												{0,1,1,1},
												{1,0,1,0},
												{0,1,0,1},
												{1,1,1,1},
												{0,0,0,1},
												{1,0,0,0},
												{0,1,0,0},
												{0,0,1,0},
												{1,0,0,1},
												{1,1,0,0},
												{0,1,1,0},
												{0,0,1,1},
												{1,0,1,1},
												{1,1,0,1},
												{1,1,1,0},
												{0,1,1,1},
												{1,0,1,0},
												{0,1,0,1},
												{1,1,1,1},
												{0,0,0,1},
												{1,0,0,0},
												{0,1,0,0},
												{0,0,1,0},
												{1,0,0,1},
												{1,1,0,0},
												{0,1,1,0},
												{0,0,1,1},
												{1,0,1,1},
												{1,1,0,1},
												{1,1,1,0},
												{0,1,1,1},
												{1,0,1,0},
												{0,1,0,1},
												{1,1,1,1},
												{0,0,0,1},
												{1,0,0,0},
												{0,1,0,0},
												{0,0,1,0},
												{1,0,0,1},
												{1,1,0,0},
												{0,1,1,0},
												{0,0,1,1},
												{1,0,1,1},
												{1,1,0,1},
												{1,1,1,0},
												{0,1,1,1},
												{1,0,1,0},
												{0,1,0,1},
												{1,1,1,1},
												{0,0,0,1},
												{1,0,0,0},
												{0,1,0,0},
												{0,0,1,0},
												{1,0,0,1},
												{1,1,0,0},
												{0,1,1,0},
												{0,0,1,1},
												{1,0,1,1},
												{1,1,0,1},
												{1,1,1,0},
												{0,1,1,1},
												{1,0,1,0},
												{0,1,0,1},
												{1,1,1,1}};

    
}
