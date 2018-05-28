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
}
