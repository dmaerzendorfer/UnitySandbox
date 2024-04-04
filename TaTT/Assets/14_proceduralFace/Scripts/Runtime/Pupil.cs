using UnityEngine;
using System;
using System.Security.AccessControl;
using UnityEngine.SocialPlatforms;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
[ExecuteInEditMode]
public class Pupil : MonoBehaviour {

	//Each pupil is a quad
	public PupilData[] pupils;

	//Mesh data references
	private MeshRenderer rend;
	private MeshFilter mf;
	private ProceduralEye eye;

	//Transform points
	private Transform parent;
	private Transform trans;

	//Mesh data
	private Mesh mesh;

	//Vertex positions
	private Vector3[] verts;
	//Pass in color data
	private Color[] colors;
	//UV data
	private Vector2[] uvs;

	//Triangles
	private int[] tris;

	//Radial cutoff
	//private Vector3[] rad;

	//Mask
	private Vector2[] mask1;
	private Vector2[] mask2;
	private Vector4[] mask;

	//Set up references
	void Start(){
		//Assume eye data is in the parent object
		trans = GetComponent<Transform> ();
		parent = trans.parent;
		eye = parent.GetComponent<ProceduralEye> ();

		//Set up mesh basics
		rend = GetComponent<MeshRenderer> ();
		mf = GetComponent<MeshFilter> ();
		mesh = new Mesh ();
		mf.mesh = mesh;
		mesh.MarkDynamic ();
	}

	void LateUpdate () {
		//Initialize arrays
		verts = new Vector3[pupils.Length*4];
		colors = new Color[verts.Length];
		uvs = new Vector2[verts.Length];
		mask1 = new Vector2[verts.Length];
		mask2 = new Vector2[verts.Length];
		tris = new int[pupils.Length * 6];
		mask = new Vector4[verts.Length];
		//rad = new Vector3[verts.Length];



		Matrix4x4 parentL2W = parent.localToWorldMatrix;
		Matrix4x4 parentW2L = parent.worldToLocalMatrix;
		Matrix4x4 l2W = trans.localToWorldMatrix;
		Matrix4x4 w2L = trans.worldToLocalMatrix;

		//TopCenter/BotCenter in relation to pupil
		float topCenter = w2L.MultiplyPoint3x4(parentL2W.MultiplyPoint3x4 (new Vector3(0,eye.topCenter, 0))).y;
		float botCenter = w2L.MultiplyPoint3x4(parentL2W.MultiplyPoint3x4 (new Vector3(0,eye.botCenter, 0))).y;

		//Loop through pupil data
		for(int i = 0; i < pupils.Length; i++){
			int j = i * 4;
			int k = i * 6;

			PupilData pup = pupils [i];
			float size = pup.size;
			Color col = pup.color;
			Vector3 pos = (Vector3)pup.offset;
			pos.z = pup.depth;

			//Set vertex positions
			verts[j] = new Vector3(-size, size) + pos;
			verts[j+1] = new Vector3(size, size) + pos;
			verts[j+2] = new Vector3(-size, -size) + pos;
			verts[j+3] = new Vector3(size, -size) + pos;

			//Set color data
			colors[j] = col;
			colors[j+1] = col;
			colors[j+2] = col;
			colors[j+3] = col;


			//UVs
			uvs[j] = new Vector2(-1, 1);
			uvs[j+1] = new Vector2(1, 1);
			uvs[j+2] = new Vector2(-1, -1);
			uvs[j+3] = new Vector2(1, -1);




			//Clip vertices/Assign mask
			for(int l = 0; l < 4; l++){
				Vector2 vert = verts [j + l];
				float y = vert.y;

				//Get top and bottom of eye to clip vertically
				Vector2 point = (Vector2)parentW2L.MultiplyPoint3x4(l2W.MultiplyPoint3x4 (vert));
				float eyeTop = ((eye.topSlope * point.x) + topCenter);
				float eyeBot = ((eye.botSlope * point.x) + botCenter);

				//Clip based on radius
				Vector2 radMask = eye.GetMask (point);


				Vector2 vertMask = (new Vector2 (vert.y - eyeTop, eyeBot - vert.y) + Vector2.one) / (1 - eye.borderPercent);
				mask [j + l] = new Vector4 (radMask.x, radMask.y, vertMask.x, vertMask.y);
				//rad [j + l] = new Vector3 (pup.cutoffPoint.x, pup.cutoffPoint.y, pup.radialCutoff);
			}

			tris [k] = j;
			tris [k + 1] = j + 1;
			tris [k + 2] = j + 2;
			tris [k + 3] = j + 1;
			tris [k + 4] = j + 3;
			tris [k + 5] = j + 2;
		}

		mesh.MarkDynamic ();
		mesh.vertices = verts;
		mesh.colors = colors;
		mesh.uv = uvs;
		mesh.triangles = tris;
		mesh.tangents = mask;
		//mesh.normals = rad;
	}
}

[Serializable]
public struct PupilData{
	[Range(0, 1)]
	public float size;

	[Range(0, 3)]
	public float depth;

	public Color color;

	public Vector2 offset;

	/*
	[Range(0, 2)]
	public float radialCutoff;
	public Vector2 cutoffPoint;*/
}