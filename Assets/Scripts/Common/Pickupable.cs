using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public abstract class Pickupable : MonoBehaviour {

	[SerializeField]
	private GameObject pickupParticle = null;

	public abstract void PickUp(PlayerMovement player);

	public virtual void OnPickUp(){
		if(pickupParticle != null)
			Instantiate (pickupParticle, transform.position, Quaternion.identity);
	}

    void OnTriggerEnter2D(Collider2D other){
		if (other.GetComponent<PlayerMovement> () != null && other is BoxCollider2D) {
			PickUp (other.GetComponent<PlayerMovement> ());
		}
	}
}
