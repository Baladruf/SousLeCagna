using System.Collections;
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
		
	}

    public void SetIdentitySoldier(Sprite photo, Sprite rank, Soldier soldier)
    {
        photoSoldier.sprite = photo;
        rankIcon.sprite = rank;
        actualSoldier = soldier;
        photoSoldier.transform.position = soldier.transform.position.WithRelativeY(3);
        photoSoldier.gameObject.SetActive(true);

        description[0].text = soldier.name;
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
