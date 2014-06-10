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

public interface IPoint {
	IPoint Parent { get; }
	void MoveNext(float dt, Vector3 gravity);
	void SatisfyConstraints();
}

public class RopePhysics {
	public int constraintIterations = 1;
	public float damping = 0.99f;
	public Vector3 axis = Vector3.up;
	public bool useGravity;

	private List<IPoint> _points = new List<IPoint>();
	private List<IPoint> _tips = new List<IPoint>();
	private Dictionary<IPoint, IPoint> _parent2child = new Dictionary<IPoint, IPoint>();
	private bool _changed = false;

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

		UpdateTips();
		Debug.Log("Num of tips : " + _tips.Count);
		for (var j = 0; j < constraintIterations; j++) {
			foreach (var tip in _tips) {
				var child = tip;
				while (child.Parent != null) {
					child.SatisfyConstraints();
					child = child.Parent;
				}
			}
		}
	}
	public void UpdateTips() {
		if (!_changed)
			return;
		_changed = false;

		_tips.Clear();
		foreach (var p in _points) {
			if (!_parent2child.ContainsKey(p))
				_tips.Add(p);
		}
	}
	public void Add(IPoint pmass) {
		_changed = true;
		_points.Add(pmass);
		if (pmass.Parent != null)
			_parent2child.Add(pmass.Parent, pmass);
	}
	public void Remove(IPoint pmass) {
		_changed = true;
		_points.Remove(pmass);
		if (pmass.Parent != null)
			_parent2child.Remove(pmass.Parent);
	}
}
