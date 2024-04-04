using UnityEngine;

[ExecuteInEditMode]
public class PupilLook : MonoBehaviour {

	//Current transform
	private Transform transform;

	//Used as reference position
	private Transform parent;

	//Clamp to how closed eye is
	private ProceduralEye eye;
	public bool useLids;

	//2d mode
	public bool flat = true;

	public LookMode mode;
	public Transform targTrans;
	public Vector3 targPoint;

	public float maxDist;

	//Shake pupil (for emotional moments （ ;_; ）)
	[Range(0, 1)]
	public float randomOffset;

	//How fast pupil shakes
	public float randomSpeed;


	void Start () {
		//Set up references
		transform = GetComponent<Transform> ();
		parent = transform.parent;
		eye = parent.GetComponent<ProceduralEye> ();
	}

	void Update () {
		//Current position
		Vector3 pos;

		//Switch mode for calculating position
		Matrix4x4 trans = parent.worldToLocalMatrix;
		Vector3 target = Vector3.zero;
		if (mode == LookMode.Transform)
		{
			if (targTrans)
			{
				target = trans.MultiplyPoint3x4(targTrans.position);
			}
		}
		else
		{
			target = targPoint;
		}
		// Vector3 target = (mode == LookMode.Transform) ? trans.MultiplyPoint3x4 (targTrans.position) : targPoint;
		if (flat) {
			Vector2 in2d = (Vector2)target;
			float dist = in2d.magnitude / maxDist;
			pos = (Vector3)Vector2.Lerp (Vector2.zero, in2d.normalized, dist);
		} else
			pos = target.sqrMagnitude > 1 ? target.normalized : target;

		if (useLids) {
			//Clamp within eyelds
			float top = eye.topAt (pos.x);
			float bot = eye.bottomAt (pos.x);
			pos.y = Mathf.Clamp (pos.y, bot, top);
		}

		//Keep z position
		pos.z = transform.localPosition.z;

		//Shake
		if (randomSpeed > 0) {
			pos.x += ((Mathf.PerlinNoise (Time.time * randomSpeed, 0) * 2) - 1) * randomOffset;
			pos.y += ((Mathf.PerlinNoise (Time.time * randomSpeed, 1) * 2) - 1) * randomOffset;
		}

		transform.localPosition = pos;
	}

	public enum LookMode{
		Transform, Point
	}
}