using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotDeTest : Spot {

    protected override void specificiteAtelier()
    {
        print("OSEF");
    }

    private delegate void Test();
    private Test t;

    // Use this for initialization
    void Start () {
        /*t += TestEndDay;
        GameManager.instance.eventManager.AddEvent(t);*/
        GameManager.instance.eventManager.eventEndDay += TestEndDay;
    }

    private void TestEndDay()
    {
        print("fin de journee");
    }
}
