using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPS : MonoBehaviour
{

	string label = "";
	int avgFrameRate = 0;

    private void Awake()
    {
		DontDestroyOnLoad(this.gameObject);
		GUI.depth = 2;
	}

    private void Update()
	{
		float current = 0;
		current = (int)(1f / Time.unscaledDeltaTime);
		avgFrameRate = (int)current;
		label = "FPS " +avgFrameRate.ToString();
	}

	void OnGUI()
	{
		GUI.Label(new Rect(5, 100, 300, 200), label);
	}
}
