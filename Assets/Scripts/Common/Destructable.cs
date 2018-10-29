using UnityEngine;
public class Destructable : Damagable {

	[SerializeField]
	private Pickupable[] itemsToDrop = null; //if it has 4 coins and one potion, the chance to drop potion will be 20%
	[SerializeField]
	private GameObject hitParticle = null; //might as well add death particle later

    [SerializeField]
    private float forceMultiply = 150;

    [SerializeField]
    private float min = -0.5f;
    [SerializeField]
    private float max = 0.5f;

    public override void TakeHit (int damageToTake, GameObject sender)
	{
		hp -= damageToTake;
		if(hitParticle != null)
			Instantiate (hitParticle, transform.position, Quaternion.identity);
		if (hp <= 0) {
			Death ();
			return;
		}
	}

	public override void Death ()
	{
		Instantiate (itemsToDrop [Random.Range (0, itemsToDrop.Length)], transform.position, Quaternion.identity).GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(min, max), 1) * forceMultiply); ;
		Destroy (this.gameObject);
	}

}
