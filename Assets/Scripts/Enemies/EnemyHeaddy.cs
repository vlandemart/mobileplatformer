using UnityEngine;


public class EnemyHeaddy : EnemyWalking {

    [SerializeField]
    protected float deathFromAboveMultiply;

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        PlayerMovement tmp = other.gameObject.GetComponent<PlayerMovement>();
		if(tmp && other.transform.position.y > transform.position.y + myCol.bounds.size.y/2 && !tmp._controller.isGrounded)
        {
			DeathFromAbove ();
        }
        else if (tmp && other is BoxCollider2D)
        {
            tmp.TakeHit(damage);
            Attack(tmp);
        }            
    }

    public override void Death()
    {
        StopAllCoroutines();
        myCol.enabled = false;
        if (attackCollider != null)
            attackCollider.enabled = false;
		myAnim.SetBool ("Death", true);
        this.enabled = false;
    }

    protected void DeathFromAbove()
    {
        Death();
        myRB.AddForce(Vector2.up * deathFromAboveMultiply);
    }
}
