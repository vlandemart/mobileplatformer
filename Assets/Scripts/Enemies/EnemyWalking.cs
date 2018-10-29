using UnityEngine;

public class EnemyWalking : Enemy {

	protected override void Start ()
	{
		base.Start ();
	}

	void Update(){
        isGoingRight = transform.rotation.y == 0 ? true : false;
        SimpleMovement ();
	}

	void SimpleMovement(){
		CheckGrounded ();
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
}
