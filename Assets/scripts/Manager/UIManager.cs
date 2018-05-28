using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour {

    [SerializeField] UISoldier soldierPrint;
    [SerializeField] UIAtelier atelierPrint;

    [SerializeField] Text infoTextTranche;
    [SerializeField] float delayPrint = 2;
    [SerializeField] float delayFade;
    private bool canPrint = true;
    private List<string> listInfo;
    public string infoTranche
    {
        private get
        {
            string s = listInfo[0];
            listInfo.RemoveAt(0);
            return s;
        }
        set
        {
            if(value != null && value != "")
            {
                listInfo.Add(value);
            }
            else
            {
                print("pas d'info null ou vide !!!");
            }
        }
    }

    public UISoldier uISoldier { get; private set; }
    public UIAtelier uIAtelier { get; private set; }

    private void Awake()
    {
        uISoldier = soldierPrint;
        uIAtelier = atelierPrint;
        listInfo = new List<string>();
    }

    private void Update()
    {
        if(listInfo.Count > 0)
        {
            if (canPrint)
            {
                canPrint = false;
                StartCoroutine(PrintInfo(infoTranche));
            }
        }
    }

    private IEnumerator PrintInfo(string message)
    {
        infoTextTranche.text = message;
        infoTextTranche.DOFade(1, delayFade);
        yield return new WaitForSeconds(delayPrint);
        infoTextTranche.DOFade(0, delayFade).OnComplete(() =>
        {
            canPrint = true;
        });
    }
}
