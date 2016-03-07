using UnityEngine;
using System.Collections;

namespace RopePhysics {

    public class Canon : MonoBehaviour {
    	public Transform bullet;
    	public Transform destination;
    	public float duration;

    	private GravitationalCurve _curve;

    	public void Shoot() {
    		StartCoroutine(Shooting());
    	}

    	IEnumerator Shooting() {
    		_curve = new GravitationalCurve(transform.position, destination.position, duration, Physics.gravity);
    		foreach (var p in _curve.Go()) {
    			bullet.position = p;
    			yield return null;
    		}
    		_curve = null;
    	}
    }
}