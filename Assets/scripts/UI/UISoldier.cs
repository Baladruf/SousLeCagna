﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISoldier : MonoBehaviour {

    [SerializeField] Image photoSoldier, rankIcon;
    private Soldier actualSoldier;

    [SerializeField] GameObject InfoSoldier;
    [SerializeField] Text[] description;

    private bool activeDesciption = false;

    private void Awake()
    {
        
    }

    // Update is called once per frame
    void Update () {
        if (actualSoldier != null)
        {
            photoSoldier.transform.position = actualSoldier.transform.position.WithRelativeY(1);
        }
	}

    public void SetIdentitySoldier(Sprite photo, Sprite rank, string[] soldierDes)
    {
        photoSoldier.sprite = photo;
        rankIcon.sprite = rank;
        //actualSoldier = soldier;
        photoSoldier.gameObject.SetActive(true);

        for(int i = 0; i < soldierDes.Length; i++)
        {
            description[i].text = soldierDes[i];
        }
        //description[0].text = soldier.name;
        //description[1].text = soldier.perk;
        //description[0].text = soldier.moral;
        //stat

    }

    public void ClickPhoto()
    {
        activeDesciption = !activeDesciption;
        InfoSoldier.SetActive(activeDesciption);
    }

    public void ResetUI()
    {
        activeDesciption = false;
        InfoSoldier.SetActive(false);
        photoSoldier.gameObject.SetActive(false);
    }
}
