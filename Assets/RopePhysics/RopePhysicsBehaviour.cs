using UnityEngine;
using System.Collections.Generic;

[ExecuteInEditMode]
public class RopePhysicsBehaviour : MonoBehaviour {
	public float timeStep = 1f / 60f;
	public int constraintIterations = 1;
	public float damping = 0.99f;
	public Vector3 axis = Vector3.up;
	public bool executeInEditor = false;

	private RopePhysics _physics;

	void OnEnable() {
		_physics = RopePhysics.Instance();
		_physics.timeStep = timeStep;
		_physics.constraintIterations = constraintIterations;
		_physics.damping = damping;
		_physics.axis = axis;
	}

	void Update() {
		if (executeInEditor || Application.isPlaying)
		_physics.Update();
	}
}

public class RopePhysics {
	public float timeStep = 1f / 60f;
	public int constraintIterations = 1;
	public float damping = 0.99f;
	public Vector3 axis = Vector3.up;

	private List<PointMass> _points = new List<PointMass>();
	private float _timeResidue = 0f;
	private ITimeHelper _timeHelper;

	private static RopePhysics _instance;
	
	public RopePhysics() {
		_timeHelper = TimeHelper.Create();
	}

	public static RopePhysics Instance() {
		if (_instance == null)
			_instance = new RopePhysics();
		return _instance;
	}

	public void Update () {
		_timeHelper.Update();
		var gravity = Physics.gravity;
		var accelBySqrTime = gravity * timeStep * timeStep;
		var nSteps = CountSteps();
		for (var i = 0; i < nSteps; i++) {
			foreach (var point in _points)
				point.MoveNext(accelBySqrTime);
			for (var j = 0; j < constraintIterations; j++) {
				foreach (var point in _points)
					point.SatisfyConstraints();
			}
		}
	}
	
	public void Add(PointMass pmass) {
		_points.Add(pmass);
	}
	public void Remove(PointMass pmass) {
		_points.Remove(pmass);
	}

	int CountSteps() {
		var dt = _timeHelper.DeltaTime;
		var elapsed = dt + _timeResidue;
		var nSteps = Mathf.FloorToInt(elapsed / timeStep);
		_timeResidue = elapsed - nSteps * timeStep;
		return nSteps;
	}
}
