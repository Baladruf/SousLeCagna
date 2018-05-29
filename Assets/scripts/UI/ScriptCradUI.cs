using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptCradUI : MonoBehaviour
{
	public GameObject Target;

	public KeyCode Appear;
	public KeyCode Disappear;
	private void Update()
	{
		if (Input.GetKeyDown(Appear))
			Target.SetActive(true);

		if (Input.GetKeyDown(Disappear))
			Target.SetActive(false);
	}
}
