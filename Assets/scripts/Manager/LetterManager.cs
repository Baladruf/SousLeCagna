using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterManager : MonoBehaviour {

    public GameManager gameManager;

    public class LetterEffect
    {
        ConsequenceLetter effect;
        string nameSoldier;

        public LetterEffect(string name, ConsequenceLetter effectL)
        {
            nameSoldier = name;
            effect = effectL;
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
