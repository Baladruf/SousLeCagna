using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParagrapheInteract : MonoBehaviour {

    public Paragraphe paragraphe;

    private void OnMouseDown()
    {
        paragraphe.OnClick();
    }
}
