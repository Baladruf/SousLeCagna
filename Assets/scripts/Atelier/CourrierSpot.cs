using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CourrierSpot : Spot {

    [SerializeField] GameObject[] prefabLettres;
    [SerializeField] Transform positionLettre;
    private bool isDispo;
    private bool courrierDispo
    {
        get
        {
            return isDispo;
        }
        set
        {
            isDispo = value;
            if (isDispo)
            {
                nbPlaceSoldier = 1;
            }
            else
            {
                nbPlaceSoldier = 0;
            }
        }
    }

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        TimeManager.instance.eventManager.eventLetter += CourrierDispo;
    }

    private void CourrierDispo()
    {
        courrierDispo = true;
        //Affiche UI lettre peut etre recup
    }

    protected override void specificiteAtelier()
    {
        courrierDispo = false;
        int i;
        for(i = 0; i < prefabLettres.Length; i++)
        {
            Instantiate(prefabLettres[i], positionLettre.position, Quaternion.identity);
        }
        GameManager.instance.uiManager.infoTranche = (i == 1 ? "1 lettre est arrivé" : i + " lettres sont arrivés");
        //ajout 3 lettres
    }

    protected override void FeedbackStatSpot()
    {
        if (nbPlaceSoldier > 0)
        {
            base.FeedbackStatSpot();
        }
        else
        {
            meshRenderer.material.color = Color.black;
        }
    }

    protected override void SetAnimeSoldierEndTask(Soldier soldier)
    {
        soldier.SetAnimSpot(animSpot, false);
    }

    public override void SetTaskSoldier(Soldier soldier)
    {
        base.SetTaskSoldier(soldier);
        soldier.SetAnimSpot(animSpot, true);
    }
}
