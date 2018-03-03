using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Node {

	#region declaraciones
    public Node _nodoPadre;
    public Node _nodoFinal;

    //World position
    public Vector2 _posicion;
    public float _costoTotal;
    public float _costoG;
    public bool _cerrado = false;
    #endregion

    
    public int _grillaX
    {
    get { return (Int32)_posicion.x; }
    }
    
    public int _grillaY
    {
    get { return (Int32)_posicion.y; }
    }

    public Node(Node nodoPadre, Node nodoFinal, Vector2 posicion, float costo)
    {
        this._nodoPadre = nodoPadre;
        this._nodoFinal = nodoFinal;
        this._posicion = posicion;
        this._costoG = costo;
        if (nodoFinal != null)
        {
            this._costoTotal = this._costoG + Calcularcosto();
        }
    }
    
    public float Calcularcosto()
    {
        return Math.Abs(this._grillaX - _nodoFinal._grillaX) + Math.Abs(_grillaY - _nodoFinal._grillaY);
    }
    
    public Boolean esIgual(Node nodo)
    {
        return (_posicion == nodo._posicion);
    }
}
