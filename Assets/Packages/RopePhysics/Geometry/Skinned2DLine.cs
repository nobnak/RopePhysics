using UnityEngine;
using System.Collections;

namespace RopePhysics {

    [ExecuteInEditMode]
    public class Skinned2DLine : MonoBehaviour {
    	public Material mat;
    	public float width = 0.1f;
    	public Vector3 axis = Vector3.right;
    	[HideInInspector]
    	public Transform[] bones;

    	public void Build() {
    		var halfwidth = 0.5f * width;
    		var nSegments = bones.Length - 1;

    		var vertices = new Vector3[bones.Length * 2];
    		var weights = new BoneWeight[vertices.Length];
    		var poses = new Matrix4x4[bones.Length];
    		var counter = 0;
    		for (var i = 0; i < bones.Length; i++) {
    			var bone = bones[i];
    			poses[i] = bone.worldToLocalMatrix * transform.localToWorldMatrix;
    			var center = transform.InverseTransformPoint(bone.position);
    			var right = halfwidth * bone.InverseTransformDirection(axis);
    			vertices[counter] = center - right;
    			vertices[counter + 1] = center + right;
    			for (var j = 0; j < 2; j++) {
    				weights[counter + j].boneIndex0 = i;
    				weights[counter + j].weight0 = 1f;
    			}
    			counter += 2;
    		}

    		counter = 0;
    		var triangles = new int[6 * nSegments];
    		for (var i = 0; i < nSegments; i++) {
    			var baseVertex = 2 * i;
    			triangles[counter++] = baseVertex;
    			triangles[counter++] = baseVertex + 1;
    			triangles[counter++] = baseVertex + 3;
    			triangles[counter++] = baseVertex;
    			triangles[counter++] = baseVertex + 3;
    			triangles[counter++] = baseVertex + 2;
    		}

    		var skin = GetComponent<SkinnedMeshRenderer>();
    		if (skin == null)
    			skin = gameObject.AddComponent<SkinnedMeshRenderer>();

    		var bounds = new Bounds();
    		foreach (var bone in bones)
    			bounds.Encapsulate(bone.transform.localPosition);

    		var mesh = skin.sharedMesh;
    		if (mesh == null)
    			mesh = skin.sharedMesh = new Mesh();
    		mesh.vertices = vertices;
    		mesh.triangles = triangles;
    		mesh.boneWeights = weights;
    		mesh.bindposes = poses;
    		mesh.bounds = bounds;

    		skin.bones = bones;
    		skin.localBounds = bounds;
    		skin.sharedMaterial = mat;
    	}
    }
}