using UnityEngine;
using System.Collections.Generic;

public class RopePhysics : MonoBehaviour {
	public float timeStep = 1f / 60f;
	public int constraintIterations = 1;
	public float damping = 0.99f;
	public Vector3 axis = Vector3.up;

	private List<PointMass> _points = new List<PointMass>();
	private float _timeResidue = 0f;

	void Update () {
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
		var elapsed = Time.deltaTime + _timeResidue;
		var nSteps = Mathf.FloorToInt(elapsed / timeStep);
		_timeResidue = elapsed - nSteps * timeStep;
		return nSteps;
	}
}
