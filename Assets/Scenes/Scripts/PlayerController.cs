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
    public float GravityScale = 3f;
    public LayerMask GeometryLayer;
    public float CollisionRadius = 0.25f;
    public Vector2 BottomOffset, RightOffset, LeftOffset;

    // components
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    // inputs
    public float InputX { get; set; }
    public float InputY { get; set; }
    public Vector2 InputDir { get; set; }
    public bool InputJump { get; set; }
    public bool InputJumpHeld { get; set; }
    public bool InputWallGrab { get; set; }

    // internal vars
    public bool FacingRight { get; set; }
    public bool OnGround { get; set; }
    public bool OnWall { get; set; }
    public WALL_SIDE WallSide { get; set; }
    public bool WallSliding { get; set; }
    public bool WallGrabbing { get; set; }
    public bool Dashing { get; set; }

    public enum WALL_SIDE {NONE, LEFT, RIGHT}

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update() {
        GetInput();
        CheckCollisions();
        Move();
        SetAnimation();
        SetFacing();
    }

    void CheckCollisions() {
        OnGround = Physics2D.OverlapCircle((Vector2)transform.position + BottomOffset, CollisionRadius, GeometryLayer);
        bool onRightWall = Physics2D.OverlapCircle((Vector2)transform.position + RightOffset, CollisionRadius, GeometryLayer);
        bool onLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + LeftOffset, CollisionRadius, GeometryLayer);

        OnWall = onRightWall || onLeftWall;
        WallSide = onRightWall ? WALL_SIDE.RIGHT : onLeftWall ? WALL_SIDE.LEFT : WALL_SIDE.NONE;
    }

    void GetInput() {
        InputX = Input.GetAxis("Horizontal");
        InputY = Input.GetAxis("Vertical");
        InputDir = new Vector2(InputX, InputY);
        InputJump = Input.GetButtonDown("Jump");
        InputJumpHeld = Input.GetButton("Jump");
        InputWallGrab = Input.GetButton("Grab");
    }

    void Move() {

        Walk(InputDir);       
       
        if (OnWall && !OnGround && !InputWallGrab && InputX != 0) {
            WallSlide();
        } else {
            WallSliding = false;
        }

        if (OnWall && InputWallGrab) {
            rb.gravityScale = 0;
            WallGrab(InputY);            
        } else {
            rb.gravityScale = GravityScale;
            WallGrabbing = false;
        }

        if (OnGround && InputJump) {
            Jump();
        }
                
        // change gravity during jump for better feel
        if (rb.velocity.y < 0) {
            rb.velocity += Vector2.up * Physics2D.gravity.y * FallMultiplier * Time.deltaTime;            
        } else if (rb.velocity.y > 0 && !InputJumpHeld) {            
            rb.velocity += Vector2.up * Physics2D.gravity.y * LowJumpMultiplier * Time.deltaTime;
        }  
        
    }

    private void SetAnimation() {
        animator.SetBool("OnGround", OnGround);
        animator.SetBool("OnWall", OnWall);
        animator.SetBool("Grabbing", WallGrabbing);
        animator.SetBool("Sliding", WallSliding);
        animator.SetFloat("XVelocity", rb.velocity.x);
        animator.SetBool("IsMovingX", !Mathf.Approximately(rb.velocity.x, 0.0f));
        animator.SetFloat("YVelocity", rb.velocity.y);
        animator.SetBool("IsMovingY", !Mathf.Approximately(rb.velocity.y, 0.0f));
    }

    void SetFacing() {
        if (InputX > 0) {
            FacingRight = true;
            spriteRenderer.flipX = false;
        }

        if (InputX < 0) {
            FacingRight = false;
            spriteRenderer.flipX = true;
        }
    }

    // movement
    void Walk(Vector2 dir) {        
        rb.velocity = new Vector2(dir.x * Speed, rb.velocity.y);        
    }

    void Jump() {        
        rb.velocity = new Vector2(rb.velocity.x, JumpVelocity);
    }

    private void WallGrab(float y) {
        rb.velocity = new Vector2(rb.velocity.x, y * Speed);
        WallGrabbing = true;
    }

    void WallSlide() {
        if (rb.velocity.y <= 0) {
            rb.velocity = new Vector2(rb.velocity.x, -WallSlideSpeed);
            WallSliding = true;
        }
    }

    // helpers
    void OnDrawGizmos() {
        Gizmos.color = Color.red;

        var positions = new Vector2[] { BottomOffset, RightOffset, LeftOffset };

        Gizmos.DrawWireSphere((Vector2)transform.position + BottomOffset, CollisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + RightOffset, CollisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + LeftOffset, CollisionRadius);
    }
}
