using UnityEngine;

public class EnemyRanged : Enemy {

	[SerializeField]
	private GameObject projectile = null;
	//[SerializeField]
	//private float secondsToAttack = 2f;

	private bool attacking = false;
	private float attackTimer = 0f;

	protected override void Start () {
		base.Start ();
    }

	void Update(){
        isGoingRight = transform.rotation.y == 0 ? true : false;
        if (!PlayerIsAhead () && !attacking) {
			SimpleMovement ();
		} else if (Mathf.Abs (myRB.position.x - player.transform.position.x) > attackRange && !attacking) {
			WalkTowardsPlayer ();
		} else if (!attacking) {
			Attack ();
		} else {
			if (attackTimer >= 2f) {
				attackTimer = 0;
				attacking = false;
			} else {
				attackTimer += Time.deltaTime;
			}
		}
	}

	void SimpleMovement(){
		CheckGrounded ();
		myAnim.SetBool ("Walking", true);
        if (canMove)
        {
            if ((!isGrounded && !rotated) || (!WayForwardIsClear() && !rotated))
            {
                rotated = true;
                transform.rotation = isGoingRight ? new Quaternion(transform.rotation.x, 180, transform.rotation.z, transform.rotation.w)
                    : new Quaternion(transform.rotation.x, 0, transform.rotation.z, transform.rotation.w);
            }
            else
            {
                myRB.position = isGoingRight ? new Vector3(myRB.position.x + (1 * speed * Time.deltaTime), myRB.position.y)
                   : new Vector3(myRB.position.x + (-1 * speed * Time.deltaTime), myRB.position.y);
            }
        }		
		if (isGrounded) {
			rotated = false;
		}
	}
		
	void WalkTowardsPlayer(){
		myAnim.SetBool ("Walking", true);
        if (canMove)
        {
            if (myRB.position.x < player.transform.position.x)
            {
                transform.rotation = new Quaternion(transform.rotation.x, 0, transform.rotation.z, transform.rotation.w);
            }
            else
            {
                transform.rotation = new Quaternion(transform.rotation.x, 180, transform.rotation.z, transform.rotation.w);
            }

            if (Mathf.Abs(transform.position.x - player.transform.position.x) > attackRange)
            {
                myRB.position = isGoingRight ? new Vector3(myRB.position.x + (1 * speed * Time.deltaTime), myRB.position.y)
                    : new Vector3(myRB.position.x + (-1 * speed * Time.deltaTime), myRB.position.y);
            }
        }
	}

	void Attack(){
		myAnim.SetBool("Walking", false);
		attacking = true;
		myAnim.SetBool ("Attack", true);
	}

	public void Shoot(){
		Projectile arrow = Instantiate (projectile, transform.position, Quaternion.identity).GetComponent<Projectile>();
        var scale = isGoingRight ? new Vector3(1, 1, 1) : new Vector3(-1, 1, 1);
		arrow.SetStats (damage, transform.position, scale);
	}
}
