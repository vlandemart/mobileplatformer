using UnityEngine;

public class Spike : MonoBehaviour {

    [SerializeField]
	private int damage = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponent<PlayerMovement>();
        var enemy = collision.GetComponent<Enemy>();

		if (player != null && collision is BoxCollider2D) { //До этого шипы регистрировали атак коллайдер и били игрока
            player.TakeHit(damage);
        } else if(enemy != null) {
            enemy.Death();
        }
    }
}
