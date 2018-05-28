using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DepotLettre : MonoBehaviour {

    [SerializeField] Transform lettreTest;

    private List<Lettre> lettreScipts;
    private List<Transform> lettres;
    public Transform boiteLettre
    {
        private get
        {
            return lettres[0];
        }
        set
        {
            lettres.Add(value);
        }
    }
    public bool isRead
    {
        get;
        set;
    }

    private void Awake()
    {
        isRead = false;
        lettres = new List<Transform>();
        lettreScipts = new List<Lettre>();
        if(lettreTest != null)
        {
            boiteLettre = lettreTest;
            lettreScipts.Add(lettreTest.GetComponent<Lettre>());
        }
    }

    // Update is called once per frame
    void Update () {
		
	}

    private void OnMouseDown()
    {
        if (!isRead && lettres.Count > 0)
        {
            isRead = true;
            lettres[0].DOLocalMoveY(0.35f, 0.5f).OnComplete(()=>
            {
                lettres[0].DOLocalRotate(Vector3.right * 60, 0.5f);
                lettres[0].DOLocalMove(new Vector3(-0.001f, 0.465f, 0.007f), 0.5f);
            });
        }
    }
}
