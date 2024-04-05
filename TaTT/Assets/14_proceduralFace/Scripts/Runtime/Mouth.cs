using System;
using UnityEngine;

namespace _14_proceduralFace.Scripts.Runtime
{
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshFilter))]
    [ExecuteInEditMode]
    public class Mouth : MonoBehaviour
    {
    //Send mesh data
	private MeshFilter mf;
	private Mesh mesh;

	//Vertices
	private Vector3[] verts;
	//Store innerColor
	private Color[] inColors;
	//Store borderColor
	private Vector4[] outColors;
	//Pass dist from top, bottom, and borderPercent
	private Vector3[] data;


	//Radial mask
	private Vector2[] uv1;
	//Points
	private Vector2[] uv2;

	[Range(-45, 45)]
	public float topAngle;
	[Range(-1, 1)]
	public float topOffset = 1;

	[Range(-45, 45)]
	public float bottomAngle = 0f;
	[Range(-1, 1)]
	public float bottomOffset = -1;

	[Range(0, 1)]
	public float borderPercent;

	public Color innerColor;
	public Color borderColor;

	//Triangle indices
	private int[] tris = new int[6]{
		0, 1, 2,
		1, 2, 3
	};

	//Is eye closed (used by blink controller)
	[Range(0, 1)]
	public float percentClosed;

	public BlinkPointMode blinkPointMode;

	//Point for closing eye towards
	[Range(0, 1)]
	public float closedPoint;

	//Access clipping info (Hidden in inspector)
	public float topSlope{get{ return slope (verts [0], verts [1]);}}
	public float topCenter{get{ return (verts [0].y+verts [1].y)*0.5f;}}
	public float topMax{get{ return Mathf.Max (verts [0].y, verts [1].y);}}
	public float topMin{get{ return Mathf.Min (verts [0].y, verts [1].y);}}

	public float botSlope{get{ return slope (verts [2], verts [3]);}}
	public float botCenter{get{ return (verts [2].y + verts [3].y) * 0.5f;}}
	public float botMax{get{ return Mathf.Max (verts [2].y, verts [3].y);}}
	public float botMin{get{ return Mathf.Min (verts [2].y, verts [3].y);}}

	void Start(){
		mf = GetComponent<MeshFilter> ();
		mesh = new Mesh ();
		mf.mesh = mesh;
		verts = new Vector3[4];
	}

	void Update(){
		Vector3 topOff = new Vector3 (0, topOffset);
		Vector3 botOff = new Vector3 (0, bottomOffset);

		Quaternion topRot = Quaternion.Euler (0, 0, topAngle);
		Quaternion botRot = Quaternion.Euler (0, 0, bottomAngle);

		verts = new Vector3[4] {
			(topRot*Vector3.left+topOff),
			(topRot*Vector3.right+topOff),
			(botRot*Vector3.left+botOff),
			(botRot*Vector3.right+botOff)
		};

		//Initialize mesh data arrays
		uv1 = new Vector2[4];
		uv2 = new Vector2[4];
		outColors = new Vector4[4];
		inColors = new Color[4];
		data = new Vector3[4];

		//Extend points to x bounds
		for(int i = 0; i < verts.Length; i++){
			Vector3 norm = verts [i].normalized;
			float xFact = 1 / norm.x;
			verts [i] = norm * xFact * Mathf.Sign (norm.x);
		}


		//Clamp y vals to offset

		float maxDiff = topMax - topOffset;
		float minDiff = botMin - bottomOffset;
		for (int i = 0; i < 4; i++) {
			float y = verts [i].y;

			//Flipped y counterpart
			float k = verts[(i+2)%4].y;

			verts [i].y = i<2 ? Mathf.Clamp (y-maxDiff, k, y) : Mathf.Clamp (y-minDiff, y, k);

			//Set color
			inColors[i] = innerColor;
				
			outColors [i] = borderColor;

			data [i] = new Vector3 (1-borderPercent, 0, 0);
		}


		//Close eye by averaging top and bottom vertices
		float avg, leftAvg, rightAvg;
		if (blinkPointMode == BlinkPointMode.Interpolate) {
			avg = closedPoint;
			leftAvg = Mathf.Lerp (verts [0].y, verts [2].y, closedPoint);
			rightAvg = Mathf.Lerp (verts [1].y, verts [3].y, closedPoint);
		} else
			leftAvg = rightAvg = Mathf.Lerp (topOffset, bottomOffset, closedPoint);

		verts [0].y =  Mathf.Lerp (verts[0].y, leftAvg+borderPercent, percentClosed);
		verts [1].y =  Mathf.Lerp (verts[1].y, rightAvg+borderPercent, percentClosed);

		verts [2].y =  Mathf.Lerp (verts[2].y, leftAvg-borderPercent, percentClosed);
		verts [3].y =  Mathf.Lerp (verts[3].y, rightAvg-borderPercent, percentClosed);


		//Set border/uv data
		uv1 [0] = (Vector2)verts [0];
		uv1 [1] = (Vector2)verts [1];
		uv1 [2] = (Vector2)verts [2];
		uv1 [3] = (Vector2)verts [3];
			

		float border = 1 - borderPercent;
		data [0] = new Vector3 (verts [0].y, verts [2].y, border);
		data [1] = new Vector3 (verts [1].y, verts [3].y, border);
		data [2] = new Vector3 (verts [0].y, verts [2].y, border);
		data [3] = new Vector3 (verts [1].y, verts [3].y, border);

		uv2 [0] = new Vector2 (verts[0].y, verts[2].y);
		uv2 [1] = new Vector2 (verts[1].y, verts[3].y);
		uv2 [2] = new Vector2 (verts[0].y, verts[2].y);
		uv2 [3] = new Vector2 (verts[1].y, verts[3].y);
			

		mesh.MarkDynamic ();
		mesh.vertices = verts;
		mesh.triangles = tris;
		mesh.colors = inColors;
		mesh.uv = uv1;
		mesh.uv2 = uv2;
		mesh.tangents = outColors;
		mesh.normals = data;
	}

	//Get clipping area
	public Vector2 GetMask(Vector2 point){
		return new Vector2 (point.x, point.y)/(1-borderPercent);
	}

	public float topAt(float x){
		return ((topSlope * x) + topCenter);
	}

	public float bottomAt(float x){
		return ((botSlope * x) + botCenter);
	}

	public float slope(Vector2 a, Vector2 b){
		return (a.y - b.y) / (a.x - b.x);
	}


	public enum BlinkPointMode{
		Interpolate,
		Fixed
	}
}
}