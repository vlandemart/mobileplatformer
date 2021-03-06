﻿using UnityEngine;

[RequireComponent(typeof(Animator), typeof(BoxCollider2D), typeof(Rigidbody2D))]
public class Enemy : Damagable {

	[SerializeField]
	protected float speed = 2f;
	[SerializeField]
	private float observationRange = 5f;
	[SerializeField]
	protected float attackRange = 1f;
	[SerializeField]
	private LayerMask playerMask = 0;
	[SerializeField]
	private LayerMask physicsMask = 0;
	[SerializeField]
	private GameObject hitParticle = null;

	protected BoxCollider2D myCol;
	protected Animator myAnim;
	protected PlayerMovement player;
	protected Rigidbody2D myRB;
    [SerializeField]
    protected float forceMultiply = 150;

	protected bool canMove = true;
    protected bool isGrounded;

	private float _skinWidth = 0.02f;
	protected bool rotated = false;

	protected CircleCollider2D attackCollider;
	protected int damage = 1;

    [SerializeField]
    protected bool isGoingRight;

	protected virtual void Start () {
        isGoingRight = transform.rotation.y == 0 ? true : false;
		myRB = GetComponent<Rigidbody2D> ();
		myAnim = GetComponent<Animator> ();
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerMovement>();
		attackCollider = GetComponent<CircleCollider2D> ();
		myCol = GetComponent<BoxCollider2D> ();
	}

	#region Behaviour bools
	protected void CheckGrounded(){
		if (isGoingRight) {
			RaycastHit2D rightcornerRay = Physics2D.Raycast (new Vector2 (myCol.bounds.max.x, myCol.bounds.min.y + _skinWidth), transform.up * -1, .15f, physicsMask);
			Debug.DrawRay (new Vector2 (myCol.bounds.max.x, myCol.bounds.min.y + _skinWidth), transform.up * -1, Color.red); //bottom right corner
			if(rightcornerRay.collider == null)
				isGrounded = false;
			else
				isGrounded = true;
		} else {
			RaycastHit2D leftcornerRay = Physics2D.Raycast (new Vector2 (myCol.bounds.min.x, myCol.bounds.min.y + _skinWidth), transform.up * -1f, .15f, physicsMask);
			Debug.DrawRay (new Vector2 (myCol.bounds.min.x, myCol.bounds.min.y + _skinWidth), transform.localScale.y * Vector3.up * -1f, Color.red); //bottom left corner	
			if(leftcornerRay.collider == null)
				isGrounded = false;
			else
				isGrounded = true;
		}
	}

	protected bool WayForwardIsClear(){
		RaycastHit2D rayForward = isGoingRight ? Physics2D.Raycast (new Vector2 (myCol.bounds.center.x, myCol.bounds.min.y), new Vector2(1, 0), speed * Time.deltaTime + myCol.bounds.size.x , physicsMask)
            : Physics2D.Raycast(new Vector2(myCol.bounds.center.x, myCol.bounds.min.y), new Vector2(-1, 0), speed * Time.deltaTime + myCol.bounds.size.x, physicsMask);

        if (isGoingRight) {
            Debug.DrawRay(new Vector2(myCol.bounds.center.x, myCol.bounds.min.y), new Vector2(1, 0));
        }else {
            Debug.DrawRay(new Vector2(myCol.bounds.center.x, myCol.bounds.min.y), new Vector2(-1, 0));
        }

		if (rayForward.collider == null) {
			return true;
		} else {
			return false;
		}
	}

	protected bool PlayerIsAhead(){
		RaycastHit2D rayForward = isGoingRight ? Physics2D.Raycast (myCol.bounds.center, new Vector2(1, 0), observationRange, playerMask) 
            : Physics2D.Raycast(myCol.bounds.center, new Vector2(-1, 0), observationRange, playerMask);

        if (isGoingRight)
        {
            Debug.DrawRay(myCol.bounds.center, new Vector2(1, 0), Color.white);
        }
        else
        {
            Debug.DrawRay(myCol.bounds.center, new Vector2(-1, 0), Color.white);
        }
		if (rayForward.collider != null && rayForward.collider.GetComponent<PlayerMovement>() != null && !rayForward.collider.GetComponent<PlayerMovement>().Dead) {
			return true;
		} else {
			return false;
		}
	}
	#endregion
	#region Attack colliders
	public void OpenAttackCollider(){
		attackCollider.enabled = true;
	}

	public void ClearAttackStatus(){
		myAnim.SetBool ("Attack", false);
		if(attackCollider != null)
			attackCollider.enabled = false;
	}

	protected virtual void Attack(PlayerMovement playerM){
		
	}

	protected virtual void OnTriggerEnter2D(Collider2D other)
	{
		PlayerMovement tmp = other.gameObject.GetComponent<PlayerMovement> ();
		if (tmp && other is BoxCollider2D)
		{
			tmp.TakeHit(damage);
			Attack (tmp);
		}
	}

	#endregion
	#region Damagable
	public override void TakeHit (int damageToTake, GameObject sender)
	{
        canMove = false;
        hp -= damageToTake;
        Vector2 knockBackVector;
		if (sender.transform.position.x > this.transform.position.x)
			knockBackVector = new Vector2 (-2, 1);
		else
			knockBackVector = new Vector2 (2, 1);
		myRB.AddForce (knockBackVector*forceMultiply);

		if (hp <= 0) {
			Death ();
			return;
		}
		myAnim.SetBool ("Hit", true);
		Invoke ("ClearHitStatus", .3f);
		if(hitParticle != null)
			Instantiate (hitParticle, transform.position, Quaternion.identity);
		//Слишком сильная механика, которая мешает нормально играть. Можно ее оставить на минибоссов типа минотавра, на обычных противников такое лучше не вешать.
		/*
		if (!PlayerIsAhead ()) {
			rotated = false;
			transform.localScale = new Vector3 (transform.localScale.x * -1, 1, 1);
		}
		*/
	}

	void ClearHitStatus(){
		myAnim.SetBool ("Hit", false);
		canMove = true;
	}

	public override void Death ()
	{
		myAnim.SetBool ("Death", true);
		StopAllCoroutines ();
		myCol.enabled = false;
		if(attackCollider != null)
			attackCollider.enabled = false;
		this.enabled = false;
	}

	#endregion
}
