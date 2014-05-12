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


}
