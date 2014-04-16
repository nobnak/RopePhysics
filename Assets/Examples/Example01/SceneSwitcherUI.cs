using UnityEngine;
using System.Collections;

public class SceneSwitcherUI : MonoBehaviour {
	public string title;
	public int width = 100;

	void OnGUI() {
		GUILayout.BeginVertical(GUILayout.Width(width));

		GUILayout.Label(title);
		if (GUILayout.Button("Next")) {
			Application.LoadLevel((Application.loadedLevel + 1) % Application.levelCount);
			return;
		}

		GUILayout.EndVertical();
	}
}
