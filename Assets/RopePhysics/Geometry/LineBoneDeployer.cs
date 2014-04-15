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

		var prevPointMasses = GetComponentsInChildren<PointMass>();
		var bones = new Transform[segmentCount + 1];
		for (var i = 0; i < prevPointMasses.Length; i++) {
			if (i < bones.Length)
				bones[i] = prevPointMasses[i].transform;
			else
				DestroyImmediate(prevPointMasses[i].gameObject);
		}

		var segmentLength = length / segmentCount;
		var pos = transform.position;
		PointMass parent = null;
		for (var i = 0; i <= segmentCount; i++) {
			var bone = bones[i];
			if (bone == null)
				bones[i] = bone = new GameObject().transform;

			bone.name = string.Format("{0:d3}", i);
			bone.parent = transform;
			bone.position = pos;
			pos += segmentLength * direction;

			var pmass = bone.GetComponent<PointMass>();
			if (pmass == null)
				pmass = bone.gameObject.AddComponent<PointMass>();
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
