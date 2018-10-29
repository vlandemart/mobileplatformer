using UnityEngine;
public class HealthPotion : Pickupable {
	[SerializeField]
	private int healAmount = 1;

	public override void PickUp (PlayerMovement player)
	{
		if (player.HP < player.MaxHP) {
			player.HP += healAmount;
			OnPickUp ();
			Destroy (this.gameObject);
		}
	}
}
