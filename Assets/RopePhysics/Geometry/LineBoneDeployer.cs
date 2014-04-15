using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent(typeof(Skinned2DLine))]
public class LineBoneDeployer : MonoBehaviour {
	public bool build = false;
	public float length = 20f;
	public int segmentCount = 40;
	public float segmentMass = 1f;
	public Vector3 direction = Vector3.down;

	void Start() {
		if (Application.isPlaying)
			enabled = false;
	}

	void Update () {
		if (!build)
			return;
		build = false;

		foreach (var pmass in GetComponentsInChildren<PointMass>())
			DestroyImmediate(pmass.gameObject);

		var segmentLength = length / segmentCount;
		var pos = transform.position;
		var bones = new Transform[segmentCount + 1];
		PointMass parent = null;
		for (var i = 0; i <= segmentCount; i++) {
			var bone = bones[i] = new GameObject().transform;
			bone.name = string.Format("{0:d3}", i);
			bone.parent = transform;
			bone.position = pos;
			pos += segmentLength * direction;

			var pmass = bone.gameObject.AddComponent<PointMass>();
			if (i == 0)
				pmass.Kinematic = true;
			pmass.parent = parent;
			pmass.restLength = segmentLength;
			pmass.Mass = segmentMass;
			parent = pmass;
		}

		var lineSkinning = GetComponent<Skinned2DLine>();
		if (lineSkinning != null) {
			lineSkinning.bones = bones;
			lineSkinning.Build();
		}
	}
}
