using UnityEngine;
using System.Collections;
using StateMachineSystem;

namespace RopePhysics {

    public class TriStates : MonoBehaviour {
    	public States states;
    	public StateMachine fsm;

    	void OnEnable () {
    		fsm = GetComponent<StateMachine>();
    		fsm.Enqueue(states.downState);
    		fsm.Next();
    	}

    	[System.Serializable]
    	public class States {
    		public InitState initState;
    		public DownState downState;
    	}

    	public class State : MonoBehaviour {
    		private TriStates _parent;

    		public TriStates Parent {
    			get {
    				if (_parent == null)
    					_parent = GetComponent<TriStates>();
    				return _parent;
    			}
    		}
    	}
    }
}