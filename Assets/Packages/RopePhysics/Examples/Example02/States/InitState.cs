using UnityEngine;
using System.Collections;
using StateMachineSystem;

namespace RopePhysics {

    public class InitState : TriStates.State {
    	public Transform targetPos;
    	public float durationEnter = 1f;
    	[HideInInspector]
    	public MonoBehaviour nextState;

    	void OnEnable() {
    		StartCoroutine(Play());
    	}

    	IEnumerator Play() {
    		yield return null;
    		var startPos = transform.position;
    		var startTime = Time.time;
    		for (var t = 0f; t < durationEnter; t = Time.time - startTime) {
    			t = PennerEasing.QuadEaseOut(t, 0f, 1f, durationEnter);
    			transform.position = Vector3.Lerp(startPos, targetPos.position, t);
    			yield return null;
    		}
    		transform.position = targetPos.position;

    		yield return new WaitForSeconds(0.5f);
    		Parent.fsm.Enqueue(Parent.states.downState);
    		Parent.fsm.Next();
    	}
    }
}