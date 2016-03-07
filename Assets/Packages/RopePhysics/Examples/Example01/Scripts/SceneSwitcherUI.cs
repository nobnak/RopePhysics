using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace RopePhysics {

    public class SceneSwitcherUI : MonoBehaviour {
    	public string title;
    	public int width = 100;

    	void OnGUI() {
    		GUILayout.BeginVertical(GUILayout.Width(width));

    		GUILayout.Label(title);
            if (GUILayout.Button("Next")) {
                var currScene = SceneManager.GetActiveScene ();
                SceneManager.LoadScene((currScene.buildIndex + 1) % SceneManager.sceneCountInBuildSettings);
    			return;
    		}

    		GUILayout.EndVertical();
    	}
    }
}