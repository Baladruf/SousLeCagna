using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ConsequenceLetter {
    public int finalMoral;
    public int finalCommandement;
    public int finalMoralGrp;
    public bool isCensor;

    public ConsequenceLetter(int m, int c, int mg, bool censor)
    {
        finalMoral = m;
        finalCommandement = c;
        finalMoralGrp = mg;
        isCensor = censor;
    }
}
