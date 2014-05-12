using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StateMachine : MonoBehaviour {
	private Queue<MonoBehaviour> _sequence = new Queue<MonoBehaviour>();

	public void Change(MonoBehaviour next) {
		_sequence.Enqueue(next);
	}

	void OnEnable() {
		StartCoroutine(Process());
	}

	IEnumerator Process() {
		while (enabled) {
			if (_sequence.Count == 0) {
				yield return null;
				continue;
			}
			var next = _sequence.Dequeue();

			next.enabled = true;
			while (_sequence.Count == 0)
				yield return null;
			next.enabled = false;
		}
	}
}

