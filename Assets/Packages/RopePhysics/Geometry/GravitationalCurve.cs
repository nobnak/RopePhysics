using UnityEngine;
using System.Collections.Generic;

namespace RopePhysics {

    public class GravitationalCurve {
    	private Vector3 _x0;
    	private Vector3 _xt;
    	private Vector3 _g;
    	private float _duration;
    	private float _durationInv;

    	private Vector3 _v;

    	public float T { get; private set; }
    	public float NormT { get; private set; }

    	public GravitationalCurve(Vector3 x0, Vector3 xt, float duration, Vector3 g) {
    		this._x0 = x0;
    		this._xt = xt;
    		this._g = g;
    		this._duration = duration;
    		this._durationInv = 1f / duration;
    		this._v = _durationInv * (xt - x0) - (0.5f * duration) * g;
    		this.NormT = this.T = 0f;
    	}

    	public IEnumerable<Vector3> Go() {
    		var startTime = Time.timeSinceLevelLoad;
    		NormT = T = 0f;
    		while (T < _duration) {
    			yield return _x0 + T * _v + (0.5f * T * T) * _g;
    			T = (Time.timeSinceLevelLoad - startTime);
    			NormT = T * _durationInv;
    		}
    		T = _duration;
    		NormT = 1f;
    		yield return _xt;
    	}
    	
    	public IEnumerable<Vector3> GoBackward() {
    		var endTime = Time.timeSinceLevelLoad + _duration;
    		T = _duration;
    		NormT = 1f;
    		while (0 < T) {
    			yield return _x0 + T * _v + (0.5f * T * T) * _g;
    			T = (endTime - Time.timeSinceLevelLoad);
    			NormT = T * _durationInv;
    		}
    		T = 0f;
    		NormT = 0f;
    		yield return _x0;
    	}
    	
    	public IEnumerable<Vector4> GoWithTime() {
    		var startTime = Time.timeSinceLevelLoad;
    		T = 0f;
    		while (T < _duration) {
    			var x = _x0 + T * _v + (0.5f * T * T) * _g;
    			yield return new Vector4(x.x, x.y, x.z, T);
    			T = (Time.timeSinceLevelLoad - startTime);
    		}
    		yield return new Vector4(_xt.x, _xt.y, _xt.z, _duration);
    	}
    }
}