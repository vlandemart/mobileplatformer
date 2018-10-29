using UnityEngine;
using System.Collections;

public class EnemyBombThrower: Enemy {

	[SerializeField]
	private GameObject bombProjectile = null;
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
		Bomb bomb = Instantiate (bombProjectile, new Vector3(transform.position.x + transform.localScale.x, transform.position.y + .5f, 0), Quaternion.identity).GetComponent<Bomb>();
		bomb.SetStats (damage, transform.localScale);
	}
	#endregion
}
