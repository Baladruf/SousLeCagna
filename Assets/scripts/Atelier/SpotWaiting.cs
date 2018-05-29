using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotWaiting : MonoBehaviour {

    public Transform[] posSoldier;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Soldier soldier = GameManager.instance.soldierSelected;
            if (soldier != null)
            {
                soldier.BackWaitingZone();
            }
        }
    }
}
