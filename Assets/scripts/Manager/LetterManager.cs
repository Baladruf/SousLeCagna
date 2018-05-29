using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterManager : MonoBehaviour {

    public enum TypeRecu
    {
        propre,
        censure,
        drop
    }

    [System.NonSerialized]
    public GameManager gameManager;
    public DepotLettre depot;

    public class LetterEffect
    {
        public ConsequenceLetter effect;
        public string nameSoldier;
        public TypeRecu typeRecu;

        public LetterEffect(string name, ConsequenceLetter effectL, TypeRecu type)
        {
            nameSoldier = name;
            effect = effectL;
            typeRecu = type;
            print("courrier pour " + nameSoldier + ", propriete : moral = " + effect.finalMoral + ", moralGrp = " + effectL.finalMoralGrp + ", commandement = " + effectL.finalCommandement+", type censure = "+typeRecu.ToString());
        }
    }
    public List<LetterEffect> courrierEffect;

    private void Awake()
    {
        courrierEffect = new List<LetterEffect>();
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void ClearLetter()
    {
        courrierEffect.Clear();
    }

    public void LetterEndDay()
    {
        var soldiers = gameManager.playerManager.soldiers;
        for(int i = 0; i < soldiers.Count; i++)
        {
            for(int j = 0; j < courrierEffect.Count; j++)
            {
                if(soldiers[i].nameSoldier == courrierEffect[j].nameSoldier)
                {
                    if(courrierEffect[j].typeRecu != TypeRecu.drop)
                    {
                        soldiers[i].SetAnimSpot(EnumDefine.AnimSpot.read_letter, true);
                    }
                    soldiers[i].moral += courrierEffect[j].effect.finalMoral;
                    gameManager.playerManager.moralGroup += courrierEffect[j].effect.finalMoralGrp;
                    gameManager.playerManager.commandement += courrierEffect[j].effect.finalCommandement;
                }
            }
        }
    }
}
