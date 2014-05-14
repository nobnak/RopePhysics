using UnityEngine;
using System.Collections;

public class FourthUI : MonoBehaviour {
	public string title;
	public float width;
	public Canon canon;

	void OnGUI() {
		GUILayout.BeginVertical(GUILayout.Width(width));

		GUILayout.Label(title);
		if (GUILayout.Button("Next")) {
			Application.LoadLevel((Application.loadedLevel + 1) % Application.levelCount);
			return;
		}
		
		GUILayout.Label(string.Format("Throwing duration : {0}", canon.duration));
		canon.duration = GUILayout.HorizontalSlider(canon.duration, 0.1f, 10f);
		if (GUILayout.Button("Shoot"))
			canon.Shoot();

		GUILayout.EndVertical();
	}
}
