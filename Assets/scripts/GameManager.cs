using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance;
    [SerializeField] GameObject cursorSoldierSelected;
    public GameObject spotWaiting;
    private Soldier actualSoldier;
    public Soldier soldierSelected
    {
        get
        {
            return actualSoldier;
        }
        set
        {
            if(actualSoldier != null)
            {
                //action / UI desactiver
            }
            actualSoldier = value;
            cursorSoldierSelected.transform.position= new Vector3(actualSoldier.transform.position.x, 0, actualSoldier.transform.position.z);
            cursorSoldierSelected.transform.SetParent(actualSoldier.transform);
            cursorSoldierSelected.SetActive(true);

            // voir cursor spot
        }
    }

    private Spot actualSpot;
    public Spot spotSelected
    {
        get
        {
            return actualSpot;
        }
        set
        {
            if(actualSpot != null)
            {
                //action / UI desactiver
            }
            actualSpot = value;
            cursorSoldierSelected.SetActive(false);
            //cursor spot ?
        }
    }

    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
