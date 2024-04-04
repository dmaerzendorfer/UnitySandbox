using UnityEngine;
using System.Collections;

public class BlinkController : MonoBehaviour {

	//y value that lids close towards
	public ProceduralEye.BlinkPointMode blinkPointMode;
	[Range(0, 1)]
	public float closedPoint = 0.5f;

	//Eye for blinking
	public ProceduralEye eye;

	//Self explanatory
	public float minTimeBetweenBlinks = .25f;
	public float maxTimeBetweenBlinks = 2f;

	public float minClosedTime = .125f;
	public float maxClosedTime = .2f;

	public float timeToClose = .05f;
	public float timeToOpen = .05f;

	//Minimum amt closed
	[Range(0, 1)]
	public float minClosed = 0f;

	void Start(){
		StartCoroutine (waitForBlink ());
	}

	private IEnumerator waitForBlink(){
		eye.percentClosed = minClosed;
		eye.blinkPointMode = blinkPointMode;
		eye.closedPoint = closedPoint;
		//Time when blink will start
		float blinkMoment = Time.time + Mathf.Lerp (minTimeBetweenBlinks, maxTimeBetweenBlinks, Random.value);
		while (Time.time < blinkMoment) {
			if (!enabled)
				yield break;

			yield return null;
		}
		//Debug.Log ("wait");
		//Start blink
		StartCoroutine (close ());
	}

	private IEnumerator close(){

		//Begin closing eye
		float endTime = Time.time + timeToClose;
		while (Time.time < endTime) {
			eye.percentClosed = Mathf.Lerp (minClosed, 1f, Mathf.SmoothStep(0, 1, 1-((endTime-Time.time)/timeToClose)));
			yield return null;
		}

		//Debug.Log ("close");
		StartCoroutine (waitForOpen ());
	}

	private IEnumerator waitForOpen(){
		eye.percentClosed = 1f;

		float waitTime = Mathf.Lerp (minClosedTime, maxClosedTime, Random.value);
		float endBlink = Time.time + waitTime;
		while (Time.time < endBlink) {
			yield return null;
		}
		//Debug.Log ("close");
		//Open eye
		StartCoroutine (open ());
	}

	private IEnumerator open(){
		//Begin opening eye
		float endTime = Time.time + timeToOpen;
		while (Time.time < endTime) {
			eye.percentClosed = Mathf.Lerp (minClosed, 1f, Mathf.SmoothStep(0, 1, (endTime-Time.time)/timeToOpen));
			yield return null;
		}
		//Debug.Log ("we good");
		StartCoroutine (waitForBlink ());
	}
		
}