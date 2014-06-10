using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Point : MonoBehaviour, IPoint {
	public const float EPSILON = 1e-3f;
	
	public Point parent;
	public float restLength;

	private RopePhysics _ropePhysics;
	[SerializeField]
	private bool _kinematic;
	private Vector3 _prevPosition;
	private Vector3 _axis;
	private Vector3 _accel;

	public IPoint Parent { get { return parent; } }
	public bool Kinematic {
		get { return _kinematic; }
		set { _kinematic = value; }
	}
	public Vector3 Axis { get { return _axis; } }

	public void AddAccel(Vector3 accel) {
		_accel += accel;
	}

	public void MoveNext(float dt, Vector3 gravity) {
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
	
	public void SatisfyConstraints() {
		if (parent == null)
			return;

		var posParent = parent.transform.position;
		var posMe = transform.position;
		var dx = posParent - posMe;
		var length = dx.magnitude;
		if (length <= EPSILON) {
			_axis = EPSILON * Random.onUnitSphere;
		} else {
			_axis = dx / length;
		}

		if (!parent.Kinematic)
			parent.transform.position = restLength * _axis + posMe;
		transform.rotation = Quaternion.FromToRotation(_ropePhysics.axis, _axis);
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
