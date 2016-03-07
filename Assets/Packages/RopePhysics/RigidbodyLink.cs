using UnityEngine;
using System.Collections;

namespace RopePhysics {

    [ExecuteInEditMode]
    [RequireComponent(typeof(Rigidbody))]
    public class RigidbodyLink : MonoBehaviour {
    	public PointMass link;
    	public float kp;
    	public float ki;
    	public float kd;

    	private Vector3 _accumDx = Vector3.zero;
    	private Vector3 _prevDx = Vector3.zero;

    	void FixedUpdate() {
    		var dx = link.transform.position - GetComponent<Rigidbody>().position;
    		var ddx = (dx - _prevDx) / Time.fixedDeltaTime;
    		_accumDx = 0.99f * _accumDx + dx;
    		_prevDx = dx;
    		var f = kp * dx + ki * _accumDx + kd * ddx;
    		link.AddForce(-f);
    		GetComponent<Rigidbody>().AddForce(f);
    	}
    }
}