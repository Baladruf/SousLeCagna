using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParagrapheInteract : MonoBehaviour {

    public Paragraphe paragraphe;

    private void Awake()
    {
        paragraphe.rature = transform.GetChild(1).gameObject;
    }

    public void OnClick()
    {
        paragraphe.OnClick();
    }
}
