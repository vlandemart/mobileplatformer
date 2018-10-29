using UnityEngine;

public abstract class Damagable : MonoBehaviour {

	public int hp;

	public abstract void TakeHit(int damageToTake, GameObject sender);

	public abstract void Death ();
}
