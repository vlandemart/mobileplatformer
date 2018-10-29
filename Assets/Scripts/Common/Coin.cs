public class Coin : Pickupable {
	private int coinWorth = 1;

	public override void PickUp (PlayerMovement player)
	{
		player.levelStats.Coins += coinWorth;
		OnPickUp ();
		Destroy (this.gameObject);
	}
}
