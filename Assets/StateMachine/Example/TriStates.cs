using UnityEngine;
using System.Collections;

public class TriStates : MonoBehaviour {
	public InitState initState;
	public DownState downState;

	private StateMachine _fsm;

	void Awake() {
		initState.nextState = downState;
		downState.nextState = initState;
	}

	void OnEnable () {
		_fsm = GetComponent<StateMachine>();
		_fsm.Change(initState);
	}

	void Update () {
	
	}

	[System.Serializable]
	public class InitState : StateMachine.AbstractState {
		public Transform targetPos;
		public float durationEnter = 1f;
		[HideInInspector]
		public StateMachine.AbstractState nextState;

		private bool _firstTimeEnter = true;

		public override IEnumerator Enter (StateMachine sm) {
			Debug.Log("Enter InitState");
			if (_firstTimeEnter) {
				_firstTimeEnter = false;
				sm.transform.position = targetPos.position;
				yield break;
			}

			var tr = sm.transform;
			var startPos = tr.position;
			var startTime = Time.time;
			for (var t = 0f; t <= durationEnter; t = Time.time - startTime) {
				t = PennerEasing.QuadEaseOut(t, 0f, 1f, durationEnter);
				tr.position = Vector3.Lerp(startPos, targetPos.position, t);
				yield return null;
			}
			tr.position = targetPos.position;
		}
		public override IEnumerator Stay (StateMachine sm) {
			if (Input.GetKeyDown(KeyCode.Space)) {
				sm.Change(nextState);
				yield break;
			}
		}
	}

	[System.Serializable]
	public class DownState : StateMachine.AbstractState {
		public Transform targetPos;
		public float durationEnter = 1f;
		[HideInInspector]
		public StateMachine.AbstractState nextState;

		public override IEnumerator Enter (StateMachine sm) {
			Debug.Log("Enter DownState");
			var tr = sm.transform;
			var startPos = tr.position;
			var startTime = Time.time;
			for (var t = 0f; t <= durationEnter; t = Time.time - startTime) {
				t = PennerEasing.QuadEaseIn(t, 0f, 1f, durationEnter);
				tr.position = Vector3.Lerp(startPos, targetPos.position, t);
				yield return null;
			}
			tr.position = targetPos.position;
		}
		public override IEnumerator Stay (StateMachine sm) {
			if (Input.GetKeyDown(KeyCode.Space)) {
				sm.Change(nextState);
				yield break;
			}
		}
	}

}
