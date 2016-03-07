using UnityEngine;
using System.Collections;

namespace RopePhysics {

    [ExecuteInEditMode]
    public class PointMass : Point {
    	public const float EPSILON = 1e-3f;
    	
    	public PointMass parent;
    	public float restLength;

    	private RopePhysics _ropePhysics;
    	[SerializeField]
    	private float _mass;
    	[SerializeField]
    	private float _invMass;
    	[SerializeField]
    	private bool _kinematic;
    	private Vector3 _prevPosition;
    	private Vector3 _axis;
    	private Vector3 _accel;

    	public float Mass {
    		get { return _mass; }
    		set {
    			_mass = value;
    			if (_kinematic || _mass >= float.MaxValue)
    				_invMass = 0f;
    			else
    				_invMass = 1f / _mass;
    		}
    	}
    	public bool Kinematic {
    		get { return _kinematic; }
    		set {
    			_kinematic = value;
    			if (_kinematic || _mass >= float.MaxValue)
    				_invMass = 0f;
    			else
    				_invMass = 1f / _mass;
    		}
    	}
    	public Vector3 Axis { get { return _axis; } }

    	public void AddForce(Vector3 f) {
    		_accel += _invMass * f;
    	}

    	public override Point Parent { get { return parent; } }
    	public override void MoveNext(float dt, Vector3 gravity) {
    		if (_kinematic) {
    			_prevPosition = transform.position;
    			return;
    		}

    		var accelBySqrTime = (dt * dt) * (_accel + gravity);
    		var curr = transform.position;
    		var v = curr - _prevPosition;
    		var next = curr + _ropePhysics.damping * v + accelBySqrTime;
    		_prevPosition = curr; transform.position = next;
    		_accel = Vector3.zero;
    	}
    	
    	public override float SatisfyConstraints() {
    		if (parent == null)
    			return 0f;
    		
    		var totalInvMass = _invMass + parent._invMass;
    		if (totalInvMass <= 0f)
    			return 0f;

    		var posParent = parent.transform.position;
    		var posMe = transform.position;
    		var dx = posParent - posMe;
    		var length = dx.magnitude;
    		if (length <= EPSILON) {
    			_axis = EPSILON * Random.onUnitSphere;
    		} else {
    			_axis = dx / length;
    		}

    		var error = length - restLength;
    		dx = error / (totalInvMass) * _axis;
    		transform.position = posMe + dx * _invMass;
    		parent.transform.position = posParent - dx * parent._invMass;
    		transform.rotation = Quaternion.FromToRotation(_ropePhysics.axis, _axis);
    		return error * error;
    	}
    	public float SqrError () {
    		if (parent == null)
    			return 0f;
    		var length = (parent.transform.position - transform.position).magnitude;
    		var dx = (restLength - length);
    		return dx * dx;
    	}
    		
    	void OnEnable() {
    		_ropePhysics = RopePhysics.Instance();
    		_ropePhysics.Add(this);
    		
    		_prevPosition = transform.position;		
    		_axis = _ropePhysics.axis;
    		_accel = Vector3.zero;
    	}
    	void OnDisable() {
    		_ropePhysics.Remove(this);
    	}
    	
    	void OnDrawGizmos() {
    		if (parent != null) {
    			Gizmos.color = Color.green;
    			Gizmos.DrawLine(transform.position, parent.transform.position);
    		}
    	}
    	void OnDrawGizmosSelected() {
    		var pos = transform.position;
    		Gizmos.color = Color.red;
    		Gizmos.DrawLine(pos, 0.5f * restLength * _axis + pos);
    	}
    }
}