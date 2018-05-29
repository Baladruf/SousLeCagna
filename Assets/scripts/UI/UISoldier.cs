using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISoldier : MonoBehaviour {

    [SerializeField] Image photoSoldier, rankIcon;
    private Soldier actualSoldier;

    [SerializeField] GameObject InfoSoldier;
    [SerializeField] Text[] description;
    [SerializeField] GameObject InfoBiography;
    private Text textBio;

    private bool activeDesciption = false;
    private bool activeBiography = false;

    public KeyCode Appear;
	public KeyCode Disappear;

    private void Awake()
    {
        textBio = InfoBiography.transform.GetChild(1).GetComponent<Text>();
    }

    // Update is called once per frame
    void Update () {
		if (Input.GetKeyDown(Appear)){
            activeDesciption = true;
            InfoSoldier.SetActive(activeDesciption);
        }

		if (Input.GetKeyDown(Disappear)){
            activeDesciption = false;
            InfoSoldier.SetActive(activeDesciption);
        }
	}

    public void SetIdentitySoldier(Sprite photo, Sprite rank, Text bio, Soldier soldier)
    {
        photoSoldier.sprite = photo;
        rankIcon.sprite = rank;
        actualSoldier = soldier;
        photoSoldier.transform.position = soldier.transform.position.WithRelativeY(1);
        photoSoldier.gameObject.SetActive(true);
        textBio.text = bio.text;
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

    public void ClickBio()
    {
        activeBiography = !activeBiography;
        InfoBiography.SetActive(activeBiography);
    }

    public void ResetUI()
    {
        activeDesciption = false;
        InfoSoldier.SetActive(false);
        photoSoldier.gameObject.SetActive(false);
    }
}
