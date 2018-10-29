using UnityEngine;
using System.Collections;

public class EnemyCharger : Enemy {

	[SerializeField]
	private float chargingSpeed = 5f;
    [SerializeField]
    private float knockbackForce = 50f;
    [SerializeField]
	private bool chargeEnded = true;
	[SerializeField]
	private float ignoreTime = 1f;

	private bool ignorePlayer = false;
	private bool attacking = false;

	protected override void Start ()
	{
		base.Start ();
        transform.GetChild(0).GetComponent<Collider2D>();
	}

	void Update(){
        isGoingRight = transform.rotation.y == 0 ? true : false;
        if (!PlayerIsAhead () && !attacking) {
			SimpleMovement ();
		} else if (Mathf.Abs (myRB.position.x - player.transform.position.x) > attackRange && !attacking) {
			WalkTowardsPlayer ();
		} else if(!attacking) {
			StartCoroutine (Attack ());
		}
	}

	void SimpleMovement(){
        CheckGrounded ();
		myAnim.SetBool ("Walking", true);
        if ((!isGrounded && !rotated) || (!WayForwardIsClear() && !rotated))
        {
            rotated = true;
            transform.rotation = isGoingRight ? new Quaternion(transform.rotation.x, 180, transform.rotation.z, transform.rotation.w)
                       : new Quaternion(transform.rotation.x, 0, transform.rotation.z, transform.rotation.w);
        }
        else if (canMove)
        {
            myRB.position = isGoingRight ? new Vector3(myRB.position.x + (1 * speed * Time.deltaTime), myRB.position.y)
                       : new Vector3(myRB.position.x + (-1 * speed * Time.deltaTime), myRB.position.y);
        }

        if (isGrounded)
        {
                rotated = false;
        }	
	}

	void WalkTowardsPlayer(){
		myAnim.SetBool ("Walking", true);

        if (canMove)
        {
            if (transform.position.x < player.transform.position.x)
            {
                transform.rotation = new Quaternion(transform.rotation.x, 0, transform.rotation.z, transform.rotation.w);
            }
            else
            {
                transform.rotation = new Quaternion(transform.rotation.x, 180, transform.rotation.z, transform.rotation.w);
            }

            if (Mathf.Abs(myRB.position.x - player.transform.position.x) > attackRange)
            {
                myRB.position = isGoingRight ? new Vector3(myRB.position.x + (1 * speed * Time.deltaTime), myRB.position.y)
                    : new Vector3(myRB.position.x + (-1 * speed * Time.deltaTime), myRB.position.y);
            }
            else
            {
				myAnim.SetBool("Walking", false);
            }
        }
	}

	IEnumerator Attack(){
		attacking = true;
		if (chargeEnded) {
			myAnim.SetBool ("Taunting", true);
			yield return new WaitForSeconds (1f);
			chargeEnded = false;
			myAnim.SetBool ("Taunting", false);
			myAnim.SetBool ("Walking", true);
		}
		if (WayForwardIsClear ()) {
			transform.position = transform.position = isGoingRight ? new Vector3(transform.position.x + (1 * chargingSpeed * Time.deltaTime), transform.position.y)
                    : new Vector3(transform.position.x + (-1 * chargingSpeed * Time.deltaTime), transform.position.y);
        } else {
			chargeEnded = true;
		}

		if (chargeEnded) {
			myAnim.SetBool ("Walking", false);
			yield return new WaitForSeconds (1f);
			attacking = false;
		} else {
			yield return null;
			StartCoroutine (Attack ());
		}
	}

	protected override void Attack (PlayerMovement playerM)
	{

        if (!ignorePlayer && !chargeEnded)
        {
            Vector2 knockBackVector;
            if (playerM.transform.position.x > transform.position.x)
                knockBackVector = new Vector2(2, 1);
            else
                knockBackVector = new Vector2(-2, 1);
            //the problem with move function is that it's applying force vector for the current frame
            //and the impact is a continious process
            playerM.GetComponent<ImpactReceiver>().AddImpact(knockBackVector, knockbackForce);
            StartCoroutine(IgnorePlayer());
        }
        //playerM.gameObject.GetComponent<Rigidbody2D> ().AddForce (knockBackVector*150);
    }

    IEnumerator IgnorePlayer()
    {
        ignorePlayer = true;
        yield return new WaitForSeconds(ignoreTime);
        ignorePlayer = false;
    }
}
