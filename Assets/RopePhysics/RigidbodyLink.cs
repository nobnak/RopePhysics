using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent(typeof(Rigidbody))]
public class RigidbodyLink : MonoBehaviour {
	public PointMass link;
	public float k;

	void FixedUpdate() {
		var toLink = link.transform.position - rigidbody.position;
		var f = k * toLink;
		link.AddForce(-f);
		rigidbody.AddForce(f);
	}
}
