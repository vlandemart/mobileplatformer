using System.Collections;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {

	[SerializeField]
	private Transform[] waypoints = null;
	[SerializeField]
	private float waitTime = 0;
	[SerializeField]
	private float speed = 2f;

	private int currentWaypoint = 0;
	private Vector3 dir;
	private Transform target;

	void Start(){
		UpdateWaypoint ();
		StartCoroutine (Moving ());
	}

	void UpdateWaypoint(){
		if (currentWaypoint + 1 < waypoints.Length) {
			currentWaypoint++;
		} else {
			currentWaypoint = 0;
		}
		target = waypoints [currentWaypoint];
		dir = target.position - transform.position;
	}

	IEnumerator Moving(){
        //What does 0.2f stands for? No magic numbers pls
		if (Vector3.Distance (transform.position, target.position) <= 0.2f) {
			UpdateWaypoint ();
			yield return new WaitForSeconds (waitTime);
		} else {
			transform.Translate (dir.normalized * speed * Time.deltaTime, Space.World);
		}
		yield return null;
		StartCoroutine (Moving ());
	}
}
