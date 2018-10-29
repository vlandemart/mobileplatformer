using UnityEngine;

public class Projectile : MonoBehaviour {
	[SerializeField]
	private float speed = 5f;
	[SerializeField]
	private int damage = 1;
	[SerializeField]
	private float maxDistance = 10f;
	private Vector3 startPos;
	//Layers should be - platform, onewayplatform, player. Dont include enemy here
	[SerializeField]
	private LayerMask hitMask = 0;
	private Rigidbody2D myRB = null;

	public void SetStats(int projectileDamage, Vector3 startingPosition, Vector3 localScale){
		startPos = startingPosition;
		damage = projectileDamage;
		transform.localScale = localScale;
		myRB = GetComponent<Rigidbody2D> ();
	}

	void Update(){
		myRB.position = new Vector3 (myRB.position.x + (transform.localScale.x * speed * Time.deltaTime), myRB.position.y);
		if (Vector3.Distance (myRB.position, startPos) > maxDistance) {
			DestroyProjectile ();
		}
	}

	void OnTriggerEnter2D(Collider2D other){
		if (hitMask == (hitMask | (1 << other.gameObject.layer))) {
			OnHit (other);
		}
	}

	void OnHit(Collider2D hitCollider){
		if (hitCollider.GetComponent<PlayerMovement> () != null) {
			hitCollider.GetComponent<PlayerMovement> ().TakeHit (damage);
		}
		if (hitCollider.GetComponent<Damagable> () != null) {
			hitCollider.GetComponent<Damagable> ().TakeHit (damage, this.gameObject);
		}
		DestroyProjectile ();
	}

	void DestroyProjectile(){
		//destroy animation and etc
		Destroy (this.gameObject);
	}
}
