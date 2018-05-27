using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="ScriptableObjects/Paragraphe")]
[System.Serializable]
public class Paragraphe : ScriptableObject {

    public int moral;
    public int commandement;
    public int moralGrp;

    public int moral_censor;
    public int commandement_censor;
    public int moralGrp_censor;

    [System.NonSerialized]
    public bool isCensor = false;

    public void OnClick()
    {
        isCensor = !isCensor;
    }

    public ConsequenceLetter SendInfo()
    {
        if (isCensor)
        {
            return new ConsequenceLetter(moral_censor, commandement_censor, moralGrp_censor);
        }
        else
        {
            return new ConsequenceLetter(moral, commandement, moralGrp);
        }
    }
}
