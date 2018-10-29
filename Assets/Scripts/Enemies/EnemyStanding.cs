using UnityEngine;
using System.Collections;

public class EnemyStanding : Enemy {

	[SerializeField]
	private GameObject projectile = null;
	[SerializeField]
	private float secondsToAttack = 3f;

	protected override void Start () {
		base.Start ();
		StartCoroutine (Attack ());
    }
		
	#region Attack
	IEnumerator Attack(){
		myAnim.SetBool ("Attack", true);
		yield return new WaitForSeconds (secondsToAttack);
		StartCoroutine (Attack ());
	}

	public void Shoot(){
		Projectile arrow = Instantiate (projectile, transform.position, Quaternion.identity).GetComponent<Projectile>();
		arrow.SetStats (damage, transform.position, transform.localScale);
	}
	#endregion
}
