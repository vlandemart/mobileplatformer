using System.Collections;
using UnityEngine;

public class HiddenSpike : MonoBehaviour {

    [SerializeField]
    private float startOffset = 0f;
	[SerializeField]
	private float delay = 1f;
	[SerializeField]
	private float attackTime = 1f;
    [SerializeField]
	private Animator animator = null;

	void Start(){
		Invoke ("StartDelay", startOffset);
	}

	void StartDelay(){
		StartCoroutine (Rise ());
	}

    IEnumerator Rise()
    {
		animator.SetBool("Fire", true);
		yield return new WaitForSeconds(attackTime);
		animator.SetBool("Fire", false);
		yield return new WaitForSeconds(delay);
		StartCoroutine (Rise ());
    }
}
