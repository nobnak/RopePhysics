using UnityEngine;
using System.Collections;

public class InitState : MonoBehaviour {
	public Transform targetPos;
	public float durationEnter = 1f;
	[HideInInspector]
	public MonoBehaviour nextState;
	
	private bool _firstTimeEnter = true;

	void OnEnable() {
		StartCoroutine(Play());
	}
	
	IEnumerator Play() {
		var sm = GetComponent<StateMachine>();
		
		if (_firstTimeEnter) {
			_firstTimeEnter = false;
			transform.position = targetPos.position;
		}
		
		var tr = sm.transform;
		var startPos = tr.position;
		var startTime = Time.time;
		for (var t = 0f; t < durationEnter; t = Time.time - startTime) {
			t = PennerEasing.QuadEaseOut(t, 0f, 1f, durationEnter);
			tr.position = Vector3.Lerp(startPos, targetPos.position, t);
			yield return null;
		}
		tr.position = targetPos.position;

		yield return new WaitForSeconds(0.5f);
		sm.Change(nextState);
	}
}
