using UnityEngine;
using System.Collections;

namespace RopePhysics {

    public class EnabledTest : MonoBehaviour {
    	public Empty empty;

    	// Use this for initialization
    	void Start () {
    		for (var i = 0; i < 20; i++) {
    			empty.enabled = ((i % 2) == 0);
    		}

    		Debug.Log(string.Format("Enabled:{0}, Disabled:{1}", empty.onCounter, empty.offCounter));
    	}
    }
}