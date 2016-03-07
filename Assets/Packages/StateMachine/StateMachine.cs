using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace StateMachineSystem {

    public class StateMachine : MonoBehaviour {
    	private Queue<MonoBehaviour> _sequence = new Queue<MonoBehaviour>();
    	private MonoBehaviour _next;

    	public void Enqueue(MonoBehaviour next) {
    		_sequence.Enqueue(next);
    	}
    	public bool Next() {
    		if (_sequence.Count == 0)
    			return false;
    		_next = _sequence.Dequeue();
    		return true;
    	}

    	void OnEnable() {
    		StartCoroutine(Process());
    	}

    	IEnumerator Process() {
    		while (enabled) {
    			while (_next == null)
    				yield return null;

    			var curr = _next;
    			_next = null;
    			curr.enabled = true;
    			while (_next == null)
    				yield return null;
    			curr.enabled = false;
    		}
    	}
    }
}