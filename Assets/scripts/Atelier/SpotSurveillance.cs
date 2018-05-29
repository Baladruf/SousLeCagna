using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotSurveillance : Spot {

    [SerializeField] float reduceProbaInvasion = 40;

    // Use this for initialization
    void Start () {
		
	}

    protected override void specificiteAtelier()
    {
        GameManager.instance.eventManager.ReduceProbaEventInvasionAllemand(reduceProbaInvasion);
    }

    protected override void SetAnimeSoldierEndTask(Soldier soldier)
    {
        soldier.SetAnimSpot(EnumDefine.AnimSpot.mitrailleuse, true);
    }
}
