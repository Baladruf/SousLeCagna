using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Lettre : MonoBehaviour {

    private Paragraphe[] paragraphes;

    [SerializeField] int moral_drop;
    [SerializeField] int commandement_drop;
    [SerializeField] int moral_grp_drop;
    [SerializeField] string nameOfSoldier;

    private void Awake()
    {
        paragraphes = new Paragraphe[transform.childCount];
        for(int i = 0; i < transform.childCount; i++)
        {
            paragraphes[i] = transform.GetChild(i).GetComponent<ParagrapheInteract>().paragraphe;
        }
    }

    public void Drop()
    {
        GameManager.instance.letterManager.courrierEffect.Add(new LetterManager.LetterEffect(nameOfSoldier, new ConsequenceLetter(moral_drop, commandement_drop, moral_grp_drop)));
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
        }
        GameManager.instance.letterManager.courrierEffect.Add(new LetterManager.LetterEffect(nameOfSoldier, letterResult));
        Finish();
    }

    private void Finish()
    {
        transform.DOMoveY(-2, 0.5f);
        Destroy(gameObject, 1);
        GameManager.instance.letterManager.depot.isRead = false;
    }
}
