using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName ="ScriptableObjects/Paragraphe")]
[System.Serializable]
public class Paragraphe : ScriptableObject {

    public int moral;
    public int commandement;
    public int moralGrp;

    public int moral_censor;
    public int commandement_censor;
    public int moralGrp_censor;

    public GameObject rature;

    [System.NonSerialized]
    public bool isCensor = false;

    public void OnClick()
    {
        isCensor = !isCensor;
        if (isCensor)
        {
            rature.SetActive(true);
        }
        else
        {
            rature.SetActive(false);
        }
        //animation rayure
    }

    public ConsequenceLetter SendInfo()
    {
        if (isCensor)
        {
            return new ConsequenceLetter(moral_censor, commandement_censor, moralGrp_censor, true);
        }
        else
        {
            return new ConsequenceLetter(moral, commandement, moralGrp, false);
        }
    }
}
