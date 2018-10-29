using UnityEngine;

[RequireComponent (typeof(BoxCollider2D),typeof(Rigidbody2D))]
public class Bomb : MonoBehaviour {
	[SerializeField]
	private float speed = 5f;
	[SerializeField]
	private int damage = 1;
	private float destroyTimer = 0f;

	public void SetStats(int projectileDamage, Vector3 localScale){
		damage = projectileDamage;
		transform.localScale = localScale;
		GetComponent<Rigidbody2D> ().AddForce(new Vector2(transform.localScale.x * speed, speed/2));
	}

	void Update(){
		destroyTimer += Time.deltaTime;
		if (destroyTimer > 0.9f) {
			DestroyProjectile ();
		}
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other is BoxCollider2D && other.GetComponent<PlayerMovement> () != null) {
			OnHit (other);
		} else {
			//DestroyProjectile ();
			GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
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
