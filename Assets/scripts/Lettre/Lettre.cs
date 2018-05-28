using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Lettre : MonoBehaviour {

    [SerializeField] ParagrapheInteract[] paraS;
    private Paragraphe[] paragraphes;

    [SerializeField] int moral_drop;
    [SerializeField] int commandement_drop;
    [SerializeField] int moral_grp_drop;
    [SerializeField] string nameOfSoldier;

    private void Awake()
    {
        paragraphes = new Paragraphe[paraS.Length];
        for(int i = 0; i < paraS.Length; i++)
        {
            paragraphes[i] = paraS[i].paragraphe;
        }
    }

    public void Drop()
    {
        GameManager.instance.letterManager.courrierEffect.Add(new LetterManager.LetterEffect(nameOfSoldier, new ConsequenceLetter(moral_drop, commandement_drop, moral_grp_drop, true), LetterManager.TypeRecu.drop));
        Finish();
    }

    public void Accept()
    {
        var letterResult = new ConsequenceLetter();
        for(int i = 0; i < paragraphes.Length; i++)
        {
            var typePara = paragraphes[i].SendInfo();
            letterResult.finalMoral += typePara.finalMoral;
            letterResult.finalCommandement += typePara.finalCommandement;
            letterResult.finalMoralGrp += typePara.finalMoralGrp;
            if (typePara.isCensor)
            {
                letterResult.isCensor = true;
            }
        }
        GameManager.instance.letterManager.courrierEffect.Add(new LetterManager.LetterEffect(nameOfSoldier, letterResult, letterResult.isCensor ? LetterManager.TypeRecu.censure : LetterManager.TypeRecu.propre));
        Finish();
    }

    private void Finish()
    {
        transform.DOMoveY(-2, 1f);
        Destroy(gameObject, 1);
        GameManager.instance.letterManager.depot.isRead = false;
    }
}
