using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lettre : MonoBehaviour {

    private Paragraphe[] paragraphes;

    [SerializeField] int moral_drop;
    [SerializeField] int commandement_drop;
    [SerializeField] int moral_grp;

    private void Awake()
    {
        paragraphes = new Paragraphe[transform.childCount];
        for(int i = 0; i < transform.childCount; i++)
        {
            paragraphes[i] = transform.GetChild(i).GetComponent<ParagrapheInteract>().paragraphe;
        }
    }

    public void Drop()
    {

    }

    public void Accept()
    {

    }
}
