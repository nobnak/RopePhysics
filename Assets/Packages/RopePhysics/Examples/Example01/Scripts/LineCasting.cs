using UnityEngine;
using System.Collections;

namespace RopePhysics {

    public class LineCasting : MonoBehaviour {
    	public enum State { Up = 0, Down }
    	public enum Transition { Nil = 0, ToDown, ToUp }

    	public Transform posUp;
    	public Transform posDown;
    	public float duration = 1f;

    	private State _lineState;
    	private Transition _lineTransition;
    	private float _startTime;
    	private System.Func<float, float> _easing;

    	void Start () {
    		_easing = (t) => t * t;
    	}

    	void Update () {
    		if (_lineTransition == Transition.Nil && Input.GetKeyDown(KeyCode.Alpha1)) {
    			_startTime = Time.time;
    			switch (_lineState) {
    			case State.Down:
    				_lineTransition = Transition.ToUp;
    				break;
    			case State.Up:
    				_lineTransition = Transition.ToDown;
    				break;
    			}				
    		}

    		switch (_lineTransition) {
    		case Transition.ToUp:
    			Up ();
    			break;
    		case Transition.ToDown:
    			Down ();
    			break;
    		}
    	}

    	void Up() {
    		var t = 1f - (Time.time - _startTime) / duration;
    		transform.position = Vector3.Lerp(posUp.position, posDown.position, _easing(t));
    		if (t <= 0f) {
    			_lineState = State.Up;
    			_lineTransition = Transition.Nil;
    		}
    	}
    	void Down() {
    		var t = (Time.time - _startTime) / duration;
    		transform.position = Vector3.Lerp(posUp.position, posDown.position, _easing(t));
    		if (t >= 1f) {
    			_lineState = State.Down;
    			_lineTransition = Transition.Nil;
    		}
    	}
    }
}