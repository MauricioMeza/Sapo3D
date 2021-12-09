using System;
using System.Collections.Generic;

[Serializable]
public class Puntaje
{
    public string name;
    public int pts;
}

[Serializable]
public class PuntajeLista
{
    public bool success;
    public List<Puntaje> data;
}
