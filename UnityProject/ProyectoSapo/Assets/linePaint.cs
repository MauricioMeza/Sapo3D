using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class linePaint : MonoBehaviour
{
    public LineRenderer linea;

    //Paints the line from an array of Vector3 Points in space
    public void LinePaint(Vector3[] lineas) 
    {
        linea.positionCount = lineas.Length;
        linea.SetPositions(lineas);
    }
}
