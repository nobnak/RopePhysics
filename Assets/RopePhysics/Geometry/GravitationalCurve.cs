using UnityEngine;
using System.Collections.Generic;

public class GravitationalCurve {
	private Vector3 _x0;
	private Vector3 _xt;
	private Vector3 _g;
	private float _duration;

	private Vector3 _v;

	public GravitationalCurve(Vector3 x0, Vector3 xt, float duration, Vector3 g) {
		this._x0 = x0;
		this._xt = xt;
		this._g = g;
		this._duration = duration;
		this._v = (1f / duration) * (xt - x0) - (0.5f * duration) * g;
	}

	public IEnumerable<Vector3> Go() {
		var startTime = Time.timeSinceLevelLoad;
		var t = 0f;
		while (t < _duration) {
			yield return _x0 + t * _v + (0.5f * t * t) * _g;
			t = (Time.timeSinceLevelLoad - startTime);
		}
		yield return _xt;
	}
	
	public IEnumerable<Vector3> GoBackward() {
		var endTime = Time.timeSinceLevelLoad + _duration;
		var t = _duration;
		while (0 < t) {
			yield return _x0 + t * _v + (0.5f * t * t) * _g;
			t = (endTime - Time.timeSinceLevelLoad);
		}
		yield return _x0;
	}
	
	public IEnumerable<Vector4> GoWithTime() {
		var startTime = Time.timeSinceLevelLoad;
		var t = 0f;
		while (t < _duration) {
			var x = _x0 + t * _v + (0.5f * t * t) * _g;
			yield return new Vector4(x.x, x.y, x.z, t);
			t = (Time.timeSinceLevelLoad - startTime);
		}
		yield return new Vector4(_xt.x, _xt.y, _xt.z, _duration);
	}
}
