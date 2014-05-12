using UnityEngine;
using System.Collections;

public class Canon : MonoBehaviour {
	public Transform bullet;
	public Transform destination;
	public float duration;

	private GravitationalCurve _curve;

	void Update () {
		if (_curve == null && Input.GetMouseButtonDown(0)) {
			StartCoroutine(Shoot());
		}
	}

	IEnumerator Shoot() {
		_curve = new GravitationalCurve(transform.position, destination.position, duration, Physics.gravity);
		foreach (var p in _curve.Go()) {
			bullet.position = p;
			yield return null;
		}
		_curve = null;
	}
}
