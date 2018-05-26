using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    private int actualScrap;
    public int scrap
    {
        get
        {
            return actualScrap;
        }
        set
        {
            actualScrap = Mathf.Max(0, actualScrap + value);
        }
    }

    private int actualAmmo;
    public int ammo
    {
        get
        {
            return actualAmmo;
        }
        set
        {
            actualAmmo = Mathf.Max(0, actualAmmo + value);
        }
    }

    private float actualMoralGrp;
    [SerializeField] float maxMoralGrp;
    public float moralGroup
    {
        get
        {
            return actualMoralGrp;
        }
        set
        {
            actualMoralGrp = Mathf.Max(0, Mathf.Min(maxMoralGrp, actualMoralGrp + value));
        }
    }

    private float actualCommandement;
    [SerializeField] float maxCommandement;
    public float commandement
    {
        get
        {
            return actualCommandement;
        }
        set
        {
            actualCommandement = Mathf.Max(0, Mathf.Min(maxCommandement, actualCommandement + value));
        }
    }


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
