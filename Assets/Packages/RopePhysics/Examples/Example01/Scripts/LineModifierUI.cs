using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace RopePhysics {

    [RequireComponent(typeof(LineLengthModifier))]
    public class LineModifierUI : MonoBehaviour {
    	private LineLengthModifier _mod;
    	private float _length;

    	// Use this for initialization
    	void Start () {
    		_mod = GetComponent<LineLengthModifier>();
    		_length = _mod.length;
    	}
    	
    	void OnGUI() {
    		GUILayout.BeginVertical(GUILayout.Width(200));

            if (GUILayout.Button("Next")) {
                var currScene = SceneManager.GetActiveScene ();
                SceneManager.LoadScene((currScene.buildIndex + 1) % SceneManager.sceneCountInBuildSettings);
    			return;
    		}

    		GUILayout.Label(string.Format("Line length : {0}", _length));
    		var tmpLength = GUILayout.HorizontalSlider(_length, 1e-3f, 40f);
    		if (tmpLength != _length)
    			_mod.length = _length = tmpLength;

    		GUILayout.EndVertical();
    	}
    }
}