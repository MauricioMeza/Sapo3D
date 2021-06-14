using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class linePaint : MonoBehaviour
{
    public LineRenderer linea;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LinePaint(Vector3[] lineas) 
    {
        linea.positionCount = lineas.Length;
        linea.SetPositions(lineas);
    }
}
