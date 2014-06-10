using UnityEngine;
using System.Collections.Generic;

[ExecuteInEditMode]
public class RopePhysicsBehaviour : MonoBehaviour {
	public int constraintIterations = 1;
	public float damping = 0.99f;
	public Vector3 axis = Vector3.up;
	public bool executeInEditor = false;
	public bool useGravity = true;

	private RopePhysics _physics;
	private ITimeHelper _timeHelper;
	private float _timeResidue;

	void OnEnable() {
		_physics = RopePhysics.Instance();
		_physics.constraintIterations = constraintIterations;
		_physics.damping = damping;
		_physics.axis = axis;
		_physics.useGravity = useGravity;
		_timeResidue = 0f;
		if (Application.isPlaying)
			executeInEditor = false;
	}

	void Update() {
		if (executeInEditor) {
			if (_timeHelper == null)
				_timeHelper = TimeHelper.Create();
			_timeHelper.Update();
			var dt = _timeHelper.DeltaTime + _timeResidue;
			var fixedDelta = Time.fixedDeltaTime;
			var nSteps = Mathf.FloorToInt(dt / fixedDelta);
			_timeResidue = dt - nSteps * fixedDelta;

			nSteps = Mathf.Min(100, nSteps);
			for (var i = 0; i < nSteps; i++)
				_physics.FixedUpdate(fixedDelta);
		} else {
			_timeHelper = null;
		}
	}

	void FixedUpdate() {
		_physics.FixedUpdate(Time.fixedDeltaTime);
	}
}

public abstract class Point : MonoBehaviour {
	public abstract Point Parent { get; }
	public abstract void MoveNext(float dt, Vector3 gravity);
	public abstract void SatisfyConstraints();
}

public class RopePhysics {
	public int constraintIterations = 1;
	public float damping = 0.99f;
	public Vector3 axis = Vector3.up;
	public bool useGravity;

	private List<Point> _points = new List<Point>();
	private List<Point> _roots = new List<Point>();
	private bool _pointsChanged = true;

	private static RopePhysics _instance;
	
	public RopePhysics() {
	}

	public static RopePhysics Instance() {
		if (_instance == null)
			_instance = new RopePhysics();
		return _instance;
	}

	public void FixedUpdate (float dt) {
		var gravity = (useGravity ? Physics.gravity : Vector3.zero);
		foreach (var point in _points)
			point.MoveNext(dt, gravity);
		for (var j = 0; j < constraintIterations; j++)
			foreach (var point in _points)
				point.SatisfyConstraints();
	}
	
	public void Add(Point pmass) {
		_pointsChanged = true;
		_points.Add(pmass);
	}
	public void Remove(Point pmass) {
		_pointsChanged = true;
		_points.Remove(pmass);
	}
}
