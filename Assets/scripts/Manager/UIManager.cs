using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

    [SerializeField] UISoldier soldierPrint;
    [SerializeField] UIAtelier atelierPrint;

    public UISoldier uISoldier { get; private set; }
    public UIAtelier uIAtelier { get; private set; }

    private void Awake()
    {
        uISoldier = soldierPrint;
        uIAtelier = atelierPrint;
    }
}
