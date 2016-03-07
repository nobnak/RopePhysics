using UnityEngine;
using System.Collections;

namespace RopePhysics {

    public class Empty : MonoBehaviour {
    	public int onCounter, offCounter;

    	void Awake() {
    		onCounter = 0;
    		offCounter = 0;
    	}

    	void OnEnable() {
    		onCounter++;
    	}

    	void OnDisable() {
    		offCounter++;
    	}
    }
}