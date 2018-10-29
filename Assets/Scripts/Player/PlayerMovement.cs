using UnityEngine;
using Player;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
	#region Variables
    //events for HUD
    public delegate void HealthChange();
    public event HealthChange OnHealthChange;

    public delegate void AmmoChange();
    public event AmmoChange OnAmmoChange;

    //public delegate void CoinChange();
    //public event CoinChange OnCoinChange;

    //HUD controls handling
    private bool leftArrow;
    private bool rightArrow;
    private bool jumping;
    private bool meleeAttack;
    private bool rangedAttack;
    private bool jumpDown;
    private bool jumpState;


    //movement config
    public float gravity = -25f;
    public float runSpeed = 8f;
    public float groundDamping = 20f; // how fast do we change direction? higher means faster
    public float inAirDamping = 5f;
    public float jumpHeight = 3f;
    [HideInInspector]
    private float normalizedHorizontalSpeed = 0;

    public CharacterController2D _controller;
    private Animator _animator;
    private RaycastHit2D _lastControllerColliderHit;
    private Vector3 _velocity;
	private CircleCollider2D attackCollider;
	private BoxCollider2D myCollider;
	[SerializeField]
	private GameUIHandler gameUIHandler = null;

	//attack stuff
	private bool attacking;
	[SerializeField]
	private int damage = 1;
	[SerializeField]
	private Projectile projectile = null;
	[SerializeField]
	private int maxAmmo = 3;
	[SerializeField] //just to somehow visualize it without the hud
	private int currentAmmo = 3;
	
	private Collider2D attackedEnemy;

	//health stuff
    [SerializeField]
    private int maxHP = 3;
	[SerializeField]
	private int hp;
	private bool dead = false;
    [SerializeField]
    private bool invincible = false;
    [SerializeField]
    private float invincibilityTimer = 2f;
	[SerializeField]
	private LayerMask movingPlatformMask = 0;

    public struct Stats
    {
        private int coins;
        //Overall level time
        private float time;
        //Taken damage counter
        private int damageCount;

        private const int maxDamageCounts = 3;

        public enum Score
        {
            C = 0, // Not in time, got hit 3+ times
            B = 1, // Not in time, got hit 3 times or less
            A = 2, // Not in time, got hit 1 time or less
            S = 3, // In time, got hit 1 times
            SS = 4 // In time, got hit 0 times
        }

        private Score levelScore;

        public Score LevelScore
        {
            get
            {
                return levelScore;
            }
        }

        public int Coins
        {
            get
            {
                return coins;
            }

            set
            {
                coins = value;
            }
        }

        public float Time
        {
            get
            {
                return time;
            }

            set
            {
                time = value;
            }
        }

        public int DamageCount
        {
            get
            {
                return damageCount;
            }

            set
            {
                damageCount = value;
            }
        }

        public static int MaxDamageCounts
        {
            get
            {
                return maxDamageCounts;
            }
        }

        public void CalculateLevelScore(float levelTime)
        {
            if(time > levelTime)
            {
                if(damageCount <= 1)
                {
                    levelScore = Score.A;
                }else if(damageCount > 1 && damageCount <= MaxDamageCounts)
                {
                    levelScore = Score.B;
                }
                else
                {
                    levelScore = Score.C;
                }
            }else if(damageCount == 0)
            {
                levelScore = Score.SS;
            }else if(damageCount == 1)
            {
                levelScore = Score.S;
            }
        }
    }
    public Stats levelStats;
	#endregion

    void Awake()
    {
        hp = maxHP;
		currentAmmo = maxAmmo;
        _animator = GetComponent<Animator>();
		myCollider = GetComponent<BoxCollider2D> ();
        _controller = GetComponent<CharacterController2D>();
		attackCollider = GetComponent<CircleCollider2D> ();
		attackCollider.enabled = false;
        // listen to some events for illustration purposes
		_controller.onControllerCollidedEvent += onControllerCollider;
        SwipeDetection.OnSwipeDown += JumpDownSwipe;
    }


    #region Event Listeners

    void onControllerCollider(RaycastHit2D hit)
    {
        // bail out on plain old ground hits cause they arent very interesting
        if (hit.normal.y == 1f)
            return;

        // logs any collider hits if uncommented. it gets noisy so it is commented out for the demo
        //Debug.Log( "flags: " + _controller.collisionState + ", hit.normal: " + hit.normal );
    }


    #endregion

    // the Update loop contains a very simple example of moving the character around and controlling the animation
    // Animator disabled until we have sprytes
    void Update()
    {
        levelStats.Time += Time.deltaTime;
        if (_controller.isGrounded) {
			_velocity.y = 0;
			_animator.SetBool ("Jumping", false);
            jumpState = false;
		} else {
			_animator.SetBool ("Jumping", true);
		}

		//Moving platform parenting
		RaycastHit2D movingPlatformRayLeft = Physics2D.Raycast(new Vector2(myCollider.bounds.min.x, myCollider.bounds.min.y + 0.02f), transform.up * -1f, .15f, movingPlatformMask);
		RaycastHit2D movingPlatformRayRight = Physics2D.Raycast(new Vector2(myCollider.bounds.max.x, myCollider.bounds.min.y + 0.02f), transform.up * -1f, .15f, movingPlatformMask);
		if (movingPlatformRayLeft.collider != null || movingPlatformRayRight.collider != null) {
			if (transform.parent == null) {
				if (movingPlatformRayLeft.collider != null) {
					transform.parent = movingPlatformRayLeft.collider.gameObject.transform;
				} else {
					transform.parent = movingPlatformRayRight.collider.gameObject.transform;
				}
			}
		} else {
			transform.parent = null;
		}
		//Moving platform parenting ended

        if (!dead)
        {
            InputHandle();
            // apply horizontal speed smoothing it. dont really do this with Lerp. Use SmoothDamp or something that provides more control
            var smoothedMovementFactor = _controller.isGrounded ? groundDamping : inAirDamping; // how fast do we change direction?
            _velocity.x = Mathf.SmoothDamp(_velocity.x, normalizedHorizontalSpeed * runSpeed, ref _velocity.x, Time.deltaTime * smoothedMovementFactor);
            _velocity.x = Mathf.Lerp(_velocity.x, normalizedHorizontalSpeed * runSpeed, Time.deltaTime * smoothedMovementFactor);

            // apply gravity before moving
            _velocity.y += gravity * Time.deltaTime;

            // if holding down bump up our movement amount and turn off one way platform detection for a frame.
            // this lets us jump down through one way platforms
            if (_controller.isGrounded && (Input.GetKey(KeyCode.DownArrow) || JumpDown) && !jumpState)
            {
                JumpDown = false;
                _velocity.y *= 3f;
                _controller.ignoreOneWayPlatformsThisFrame = true;
            }

            _controller.move(_velocity * Time.deltaTime);

            // grab our current _velocity to use as a base for all calculations
            _velocity = _controller.velocity;
        }
        else
        {
            _velocity.x = 0;
            _velocity.y += gravity * Time.deltaTime;
            _controller.move(_velocity * Time.deltaTime);
            _velocity = _controller.velocity;

            
        }
        
    }

	#region Movement

    private void InputHandle()
    {

        if (Input.GetKey(KeyCode.RightArrow) || RightArrow)
        {
            MoveRight();
        }
        else if (Input.GetKey(KeyCode.LeftArrow) || LeftArrow)
        {
            MoveLeft();
        }
        else
        {
            normalizedHorizontalSpeed = 0;

            if (_controller.isGrounded)
            {
                _animator.SetBool("Walking", false);
            }

        }

        //attack motion
        if (Input.GetAxisRaw("Fire") != 0 || MeleeAttack)
        {
            if (!attacking)
            {
				Attack ();
            }
        }

		if (Input.GetButtonDown("Fire2") || RangedAttack)
		{
			if (!attacking)
			{
				Shoot ();
			}
		}

        // we can only jump whilst grounded
        if (Input.GetKeyDown(KeyCode.UpArrow) || Jumping)
        {
            Jump();
        }
    }

    private void MoveLeft()
    {
        normalizedHorizontalSpeed = -1;

        if (transform.rotation.y == 0f && !attacking)
            transform.rotation = new Quaternion(transform.rotation.x, 180, transform.rotation.z, transform.rotation.w);

        if (_controller.isGrounded)
        {
            _animator.SetBool("Walking", true);
        }
    }

    private void Jump()
    {
        if (_controller.isGrounded)
        {
            Jumping = false;
            jumpState = true;
            _velocity.y = Mathf.Sqrt(2f * jumpHeight * -gravity);
            _animator.SetBool("Jumping", true);
        }
    }

    private void MoveRight()
    {
        normalizedHorizontalSpeed = 1;

        if (transform.rotation.y != 0 && !attacking)
            transform.rotation = new Quaternion(transform.rotation.x, 0, transform.rotation.z, transform.rotation.w);

        if (_controller.isGrounded)
        {
            _animator.SetBool("Walking", true);
        }
    }

    private void JumpDownSwipe()
    {
        JumpDown = true;
    }

    #endregion
    #region Taking damage, death
    private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.GetComponent<Damagable>() && other is BoxCollider2D && attackCollider.enabled && other != attackedEnemy){
			attackedEnemy = other;
			other.gameObject.GetComponent<Damagable>().TakeHit(damage, this.gameObject);
		}
	}


	public void TakeHit(int damageToTake){
		if (!invincible && !dead)
		{
			HP -= damageToTake;
			if (HP <= 0)
				Death();
			StartCoroutine(Invincibility());
		}
	}

	IEnumerator Invincibility()
	{
		_animator.SetBool("TakeHit", true);
		invincible = true;
		yield return new WaitForSeconds(invincibilityTimer);
		_animator.SetBool("TakeHit", false);
		invincible = false;
	}

	void Death(){
		Debug.Log ("You died");
		Invoke ("DeathUI", 1f);
		_animator.SetBool ("Dead", true);
		dead = true;
		//this.enabled = false;
	}

	void DeathUI(){
		GameUIHandler.DeathScreen ();
	}
	#endregion
	#region Attacking

	void Attack(){
		attackedEnemy = null;
        MeleeAttack = false;
        attacking = true;
		_animator.SetInteger("AttackType", Random.Range(0, 2));
		_animator.SetBool("Attacking", true);
	}

	public void EnableAttackCollider(){
		attackCollider.enabled = true;
	}

	public void ClearAttackStatus(){
		_animator.SetBool ("Attacking", false);
		attacking = false;
		attackCollider.enabled = false;
	}

	void Shoot(){
		if (CurrentAmmo > 0) {
            RangedAttack = false;
            CurrentAmmo--;
			Projectile arrow = Instantiate (projectile, transform.position, Quaternion.identity).GetComponent<Projectile> ();
            var scale = transform.rotation.y == 0 ? new Vector3(1, 1, 1) : new Vector3(-1, 1, 1);
            arrow.SetStats (damage, transform.position, scale);
		} else {
			return;
		}
	}
	#endregion
	#region Accessors

	public int MaxHP
    {
        set
        {
            maxHP = value;
        }
        get
        {
            return maxHP;
        }
    }

	public int HP
    {
        set
        {
            if(value <= MaxHP && value >= 0)
            {
                hp = value;
                levelStats.DamageCount++;
            }else if(value > MaxHP)
            {
                hp = MaxHP;
            }
            if(OnHealthChange != null)
                OnHealthChange();
        }
        get
        {
            return hp;
        }
    }


	public int MaxAmmo
	{
		set
		{
			maxAmmo = value;
		}
		get
		{
			return maxAmmo;
		}
	}

	public int CurrentAmmo
	{
		set
		{
            if (value <= MaxAmmo && value >= 0)
                currentAmmo = value;
            if (OnAmmoChange != null)
                OnAmmoChange();
        }
		get
		{
			return currentAmmo;
		}
	}

    public bool Invincible
    {
        set
        {
            invincible = value;
        }
        get
        {
            return invincible;
        }
    }

	public bool Dead
	{
		set
		{
			dead = value;
		}
		get
		{
			return dead;
		}
	}

    public bool Jumping
    {
        get
        {
            return jumping;
        }

        set
        {
            if(_controller.isGrounded)
                jumping = value;
        }
    }

    public bool RightArrow
    {
        get
        {
            return rightArrow;
        }

        set
        {
            rightArrow = value;
        }
    }

    public bool LeftArrow
    {
        get
        {
            return leftArrow;
        }

        set
        {
            leftArrow = value;
        }
    }

    public bool MeleeAttack
    {
        get
        {
            return meleeAttack;
        }

        set
        {
            if(!attacking)
                meleeAttack = value;
        }
    }

    public bool RangedAttack
    {
        get
        {
            return rangedAttack;
        }

        set
        {
            rangedAttack = value;
        }
    }

    public bool JumpDown
    {
        get
        {
            return jumpDown;
        }

        set
        {
            jumpDown = value;
        }
    }

    public BoxCollider2D MyCollider
    {
        get
        {
            return myCollider;
        }

        set
        {
            myCollider = value;
        }
    }

    public GameUIHandler GameUIHandler
    {
        get
        {
            return gameUIHandler;
        }

        set
        {
            gameUIHandler = value;
        }
    }

    #endregion
}
