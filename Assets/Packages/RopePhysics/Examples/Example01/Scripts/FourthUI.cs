using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace RopePhysics {

    public class FourthUI : MonoBehaviour {
    	public string title;
    	public float width;
    	public Canon canon;

    	void OnGUI() {
    		GUILayout.BeginVertical(GUILayout.Width(width));

    		GUILayout.Label(title);
            if (GUILayout.Button("Next")) {
                var currScene = SceneManager.GetActiveScene ();
                SceneManager.LoadScene((currScene.buildIndex + 1) % SceneManager.sceneCountInBuildSettings);
    			return;
    		}
    		
    		GUILayout.Label(string.Format("Throwing duration : {0}", canon.duration));
    		canon.duration = GUILayout.HorizontalSlider(canon.duration, 0.1f, 10f);
    		if (GUILayout.Button("Shoot"))
    			canon.Shoot();

    		GUILayout.EndVertical();
    	}
    }
}