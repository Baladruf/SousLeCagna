using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CamPP : MonoBehaviour {

	public Material camPP;
	private Material camPP2;
	
	
	// Use this for initialization
	void Awake () {
		camPP2 = camPP;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnRenderImage (RenderTexture source, RenderTexture destination){
		Graphics.Blit (source, destination,camPP2);
	}
}
