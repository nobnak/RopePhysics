using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent(typeof(Rigidbody))]
public class RigidbodyLink : MonoBehaviour {
	public Hair link;
	public float k;

	void FixedUpdate() {
		var toLink = link.transform.position - rigidbody.position;
		var f = k * toLink;
		var a = f / rigidbody.mass;
		link.AddAccel(-a);
		rigidbody.AddForce(a, ForceMode.Acceleration);
	}
}
