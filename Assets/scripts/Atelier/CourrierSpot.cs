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
        for(int i = 0; i < prefabLettres.Length; i++)
        {
            Instantiate(prefabLettres[i], positionLettre.position, Quaternion.identity);
        }
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
}
