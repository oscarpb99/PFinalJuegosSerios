using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Situation 
{
    // Stat1 es Social
    // Stat2 es Bienestar
    // Stat3 es Responsabilidad Academica
    // Stat4 es Dinero
    [SerializeField]
	public string situation;
    [SerializeField]
	public string elec1;
    [SerializeField]
    public string elec2;
    [SerializeField]
    public string elec3;
	public int stat1Left;
    public int stat2Left;
    public int stat3Left;
    public int stat4Left;
    public int stat1Right;
    public int stat2Right;
    public int stat3Right;
    public int stat4Right;
    public int stat1Down;
    public int stat2Down;
    public int stat3Down;
    public int stat4Down;
    public int spawnRate;
    public string image;
    public string tag;
    public int elecAcu;
    public int acumulativeStat1Left;
    public int acumulativeStat2Left;
    public int acumulativeStat3Left;
    public int acumulativeStat4Left;
    public int acumulativeStat1Right;
    public int acumulativeStat2Right;
    public int acumulativeStat3Right;
    public int acumulativeStat4Right;
    public int acumulativeStat1Down;
    public int acumulativeStat2Down;
    public int acumulativeStat3Down;
    public int acumulativeStat4Down;
}

[System.Serializable]
public class TutorialCard
{
    public string situation;
    public string elec1;
    public string elec2;
    public string elec3;
    public int stat1Left;
    public int stat2Left;
    public int stat3Left;
    public int stat4Left;
    public int stat1Right;
    public int stat2Right;
    public int stat3Right;
    public int stat4Right;
    public int stat1Down;
    public int stat2Down;
    public int stat3Down;
    public int stat4Down;
    public string image;

}