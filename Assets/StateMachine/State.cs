using UnityEngine;
using System.Collections;

public class State : MonoBehaviour {
	public State[] next;
	
	public System.Action<StateMachine, State> Enter;
	public System.Action<StateMachine, State> Stay;
	public System.Action<StateMachine, State> Exit;
	
	public override string ToString () { return string.Format("{0}(State)", name); }

}
