using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Censure : MonoBehaviour
{
	public GameObject[] Rature;

	public void clicked(int button)
	{
		if (Rature[button].activeSelf)
			Rature[button].SetActive(false);
		else
			Rature[button].SetActive(true);

	}
}
