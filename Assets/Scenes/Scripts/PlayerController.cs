using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour
{

    // parameters
    public float Speed = 10f;
    public float JumpVelocity = 5f;
    public float WallSlideSpeed = 5f;
    public float FallMultiplier = 1.5f;
    public float LowJumpMultiplier = 1.5f;
    public LayerMask GeometryLayer;
    public float CollisionRadius = 0.25f;
    public Vector2 BottomOffset, RightOffset, LeftOffset;

    // components
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
  
    // inputs
    private float inputX;
    private float inputY;
    private Vector2 inputDir;
    private bool inputJump;
    private bool inputJumpHeld;

    // internal vars
    private bool facingRight;
    private bool onGround;
    private bool onWall;
    private WALL_SIDE wallSide;

    enum WALL_SIDE {NONE, LEFT, RIGHT}

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        CheckCollisions();
        Move();
        SetFacing();
    }

    void Move() {

        Walk(inputDir);       
       
        if (onWall && !onGround) {
            WallSlide();
        }

        if (onGround && inputJump) {
            Jump();
        }
        
        // change gravity during jump for better feel
        if (rb.velocity.y < 0) {
            rb.velocity += Vector2.up * Physics2D.gravity.y * FallMultiplier * Time.deltaTime;            
        } else if (rb.velocity.y > 0 && !inputJumpHeld) {            
            rb.velocity += Vector2.up * Physics2D.gravity.y * LowJumpMultiplier * Time.deltaTime;
        }   
    }

    void WallSlide() {
        if (rb.velocity.y <= 0) {
            rb.velocity = new Vector2(rb.velocity.x, -WallSlideSpeed);
        }
    }

    // movement
    void Walk(Vector2 dir) {
        rb.velocity = new Vector2(dir.x * Speed, rb.velocity.y);
        animator.SetBool("Moving", inputX != 0);
    }

    void Jump() {
        rb.velocity = new Vector2(rb.velocity.x, JumpVelocity);
    }

    void SetFacing() {
        if (inputX > 0) {
            facingRight = true;
            spriteRenderer.flipX = false;
        }

        if (inputX < 0) {
            facingRight = false;
            spriteRenderer.flipX = true;
        }
    }

    // helpers

    void CheckCollisions() {
        onGround = Physics2D.OverlapCircle((Vector2)transform.position + BottomOffset, CollisionRadius, GeometryLayer);
        bool onRightWall = Physics2D.OverlapCircle((Vector2)transform.position + RightOffset, CollisionRadius, GeometryLayer);
        bool onLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + LeftOffset, CollisionRadius, GeometryLayer);

        onWall = onRightWall || onLeftWall;
        wallSide = onRightWall ? WALL_SIDE.RIGHT : onLeftWall ? WALL_SIDE.LEFT : WALL_SIDE.NONE;
    }

    void GetInput() {
        inputX = Input.GetAxis("Horizontal");
        inputY = Input.GetAxis("Vertical");
        inputDir = new Vector2(inputX, inputY);
        inputJump = Input.GetButtonDown("Jump");
        inputJumpHeld = Input.GetButton("Jump");
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.red;

        var positions = new Vector2[] { BottomOffset, RightOffset, LeftOffset };

        Gizmos.DrawWireSphere((Vector2)transform.position + BottomOffset, CollisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + RightOffset, CollisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + LeftOffset, CollisionRadius);
    }
}
