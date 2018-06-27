using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class PlayerController : MonoBehaviour
{
	[Header("Movement Properties")]
	public float speed = 50;

    [Header("Jump Properties")]
	public float jumpForce = 50;
	public float checkRadius = 1;
	public Transform groundCheck;
	public LayerMask groudMask;
	public int numberOfJumps = 1;

	[Header("Dash Properties")]
	public float dashDistance;
	public float dashDuration;

	[Header("Game Manager")]
	public StatsManager stats;

	[Header("Attack Properties")]
	public Transform left;
	public Transform right;
	public float attackRange;

    [HideInInspector] public int maxJumpAmount = 1;
	private float moveInput, dashTimer;
    private float currentHealth, currentMagic, currentStamina;
    private bool isGrounded, attacking;
	private Rigidbody2D playerBody;
	private SpriteRenderer playerRenderer;
	private Animator playerAnimator;
	
	private void Start()
	{
        maxJumpAmount = numberOfJumps;
		
		attacking = false;

		playerBody = GetComponent<Rigidbody2D>();
		playerRenderer = GetComponent<SpriteRenderer>();
		playerAnimator = GetComponent<Animator>();

	}

	private void Update()
	{
        Jump();
		Dash();
		if (Input.GetButtonDown("Fire1"))
			Attack();
	}

	private void FixedUpdate()
	{
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groudMask);
		playerAnimator.SetBool("isGrounded", isGrounded);
		HorizontalMovement();
	}

	private void HorizontalMovement()
	{
		moveInput = Input.GetAxis("Horizontal");

		if (moveInput > 0)
			playerRenderer.flipX = false;
		else if (moveInput < 0)
			playerRenderer.flipX = true;

		playerAnimator.SetBool("isRunning", moveInput != 0);
		playerBody.velocity = new Vector2((moveInput * speed), playerBody.velocity.y);
	}

	private void Jump()
	{
		if (isGrounded)
		{
			numberOfJumps = maxJumpAmount;
			playerAnimator.SetBool("midAirJump", false);
		}

		if (Input.GetButtonDown("Jump") && (numberOfJumps > 0
			|| playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Jump")
			|| playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Falling")))
		{
			playerBody.velocity = new Vector2(playerBody.velocity.x, jumpForce);
			numberOfJumps--;
            StartCoroutine(WaitForBoolSwitch("midAirJump"));
		}
		else if (Input.GetButtonDown("Jump") && isGrounded)
		{
			isGrounded = false;
			playerBody.velocity = new Vector2(playerBody.velocity.x, jumpForce);
		}
	}

	private void Dash()
	{
		int direction = (int)Mathf.Sign(moveInput);
		if (Input.GetButtonDown("Fire3"))
		{
			transform.position += new Vector3((dashDistance * direction), 0, 0);
		}
	}

	private void Attack()
	{
		attacking = true;
		int atkHitRight, atkHitLeft;
		StartCoroutine(WaitForBoolSwitch("attacking"));
        ContactFilter2D filterRight = new ContactFilter2D();
        ContactFilter2D filterLeft = new ContactFilter2D();
        RaycastHit2D[] hitInfoRight = new RaycastHit2D[1];
		RaycastHit2D[] hitInfoLeft = new RaycastHit2D[1];
		if (!playerRenderer.flipX)
		{
            atkHitRight = Physics2D.BoxCast(
				right.position,
				new Vector2(attackRange, attackRange),
				0,
				Vector2.right,
				filterRight,
				hitInfoRight,
				attackRange
			);
		}
		else if (playerRenderer.flipX)
		{
            atkHitLeft = Physics2D.BoxCast(
				left.position,
				new Vector2(attackRange, attackRange),
				0,
				Vector2.right,
                filterLeft,
				hitInfoLeft,
				-attackRange
			);
		}
	}

	private IEnumerator WaitForBoolSwitch(string parameter)
	{
        playerAnimator.SetBool(parameter, true);
		yield return new WaitForSeconds(.0001f);
        playerAnimator.SetBool(parameter, false);
	}
}
