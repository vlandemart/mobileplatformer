public class Ammo : Pickupable {
	//private int ammoAmount = 1;

	public override void PickUp (PlayerMovement player)
	{
		if (player.CurrentAmmo < player.MaxAmmo) {
			//player.CurrentAmmo += ammoAmount; //Мне показалось это довольно слабым для баланса, так что теперь поднятие стрелы восстанавливает все стрелы
			player.CurrentAmmo = player.MaxAmmo;
			OnPickUp ();
			Destroy (this.gameObject);
		}
	}
}