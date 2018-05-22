using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spot : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnMouseDown()
    {
        GameManager.instance.spotSelected = this;
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Soldier soldier = GameManager.instance.soldierSelected;
            if(soldier != null)
            {

            }
        }
    }
}
