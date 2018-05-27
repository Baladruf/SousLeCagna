using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ConsequenceLetter {
    public int finalMoral;
    public int finalCommandement;
    public int finalMoralGrp;

    public ConsequenceLetter(int m, int c, int mg)
    {
        finalMoral = m;
        finalCommandement = c;
        finalMoralGrp = mg;
    }
}
