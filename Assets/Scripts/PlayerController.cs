using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour
{

    // parameters
    public float Speed = 10f;
    public float JumpVelocity = 5f;
    public float WallSlideSpeed = 5f;
    public float WallJumpLerp = 10;
    public float FallMultiplier = 1.5f;
    public float LowJumpMultiplier = 1.5f;
    public float GravityScale = 3f;
    public float DashSpeed = 20f;
    public float DashCooldownTime = .3f;
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
    public bool InputDash { get; set; }

    // internal vars
    public bool FacingRight { get; set; }
    public bool OnGround { get; set; }
    public bool OnWall { get; set; }
    public WALL_SIDE WallSide { get; set; }
    public bool WallSliding { get; set; }
    public bool WallGrabbing { get; set; }
    public bool Dashing { get; set; }
    public bool Jumping { get; set; }
    public bool WallJumping { get; set; }
    public bool CanMove { get; set; }
    
    public bool CanDash { get; set; }

    public bool DashLockout { get; set; }

    public enum WALL_SIDE {NONE, LEFT, RIGHT}

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        CanMove = true;
        CanDash = true;
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
        InputWallGrab = Input.GetButton("Grab") || Input.GetAxis("GrabTrigger") != 0.0f;
        InputDash = Input.GetButtonDown("Dash");
    }

    void Move() {

        Walk(InputDir);       
       
        if (OnGround) {
            // reset any params for touching the ground (jumping, etc)
            Jumping = false;
            WallJumping = false;
            if (!DashLockout) {
                CanDash = true;
            }
        }

        if (OnWall && !OnGround && !InputWallGrab && InputX != 0) {
            WallSlide();
        } else {
            WallSliding = false;
        }

        if (OnWall && InputWallGrab && CanMove) {
            rb.gravityScale = 0;
            WallGrab(InputY);            
        } else if (!Dashing) {
            rb.gravityScale = GravityScale;
            WallGrabbing = false;
        }

        if (InputDash && CanDash) {
            Dash(InputDir);
        }

        if (InputJump) {
            if (OnGround) {
                Jump(Vector2.up);
                Jumping = true;
            } else if (OnWall) {
                WallJump();
                WallJumping = true;
            }
        }

        // change gravity during jump for better feel
        if (!Dashing) {
            if (rb.velocity.y < 0) {
                rb.velocity += Vector2.up * Physics2D.gravity.y * FallMultiplier * Time.deltaTime;
            }
            else if (rb.velocity.y > 0 && !InputJumpHeld) {
                rb.velocity += Vector2.up * Physics2D.gravity.y * LowJumpMultiplier * Time.deltaTime;
            }
        }        
    }

    private void SetAnimation() {
        animator.SetBool("OnGround", OnGround);
        animator.SetBool("OnWall", OnWall);
        animator.SetBool("Grabbing", WallGrabbing);
        animator.SetBool("Sliding", WallSliding);
        animator.SetBool("Jumping", Jumping);
        animator.SetBool("WallJumping", WallJumping);
        animator.SetBool("Dashing", Dashing);
        animator.SetBool("CanMove", CanMove);
        animator.SetBool("CanDash", CanDash);
        animator.SetBool("DashLockout", DashLockout);
        animator.SetFloat("XVelocity", rb.velocity.x);
        animator.SetBool("IsMovingX", !Mathf.Approximately(rb.velocity.x, 0.0f));
        animator.SetFloat("YVelocity", rb.velocity.y);
        animator.SetBool("IsMovingY", !Mathf.Approximately(rb.velocity.y, 0.0f));        
    }

    void SetFacing() {
        if (rb.velocity.x > 0) {
            FacingRight = true;
            spriteRenderer.flipX = false;
        }

        if (rb.velocity.x < 0) {
            FacingRight = false;
            spriteRenderer.flipX = true;
        }
    }

    // movement
    void Walk(Vector2 dir) {

        if (!CanMove) {
            return;
        }

        if (WallGrabbing) {
            return;
        }

        if (WallJumping) {
            rb.velocity = Vector2.Lerp(rb.velocity, (new Vector2(dir.x * Speed, rb.velocity.y)), WallJumpLerp * Time.deltaTime);
        } else {
            rb.velocity = new Vector2(dir.x * Speed, rb.velocity.y);
        }
        
    }

    void Jump(Vector2 direction) {
        rb.velocity = direction * JumpVelocity;
    }

    private void WallJump() {
        StopCoroutine(DisableMovement(0));
        StartCoroutine(DisableMovement(.1f));

        // jump the opposite direction of the wall we are on
        Vector2 wallDir = WallSide == WALL_SIDE.LEFT ? Vector2.right : Vector2.left;        
        Jump(Vector2.up / 1.5f + wallDir / 1.5f);
    }

    private void WallGrab(float y) {
        rb.velocity = new Vector2(0.0f, y * Speed);
        WallGrabbing = true;
    }

    void WallSlide() {
        if (rb.velocity.y <= 0) {
            rb.velocity = new Vector2(rb.velocity.x, -WallSlideSpeed);
            WallSliding = true;
        }
    }

    void Dash(Vector2 direction) {
        Vector2 dashDirection = direction;
        if (direction == Vector2.zero) {
            dashDirection = FacingRight ? Vector2.right : Vector2.left;
        }

        dashDirection = normalizeDashDirection(dashDirection);
        
        rb.velocity = dashDirection * DashSpeed;
        CanDash = false;
        StartCoroutine(DashWait());
        StartCoroutine(DashCooldown(DashCooldownTime));
    }

    void RigidBodyDrag(float drag) {
        rb.drag = drag;
    }

    // enumerators
    IEnumerator DisableMovement(float time) {
        CanMove = false;
        yield return new WaitForSeconds(time);
        CanMove = true;
    }

    IEnumerator DashWait() {
        DOVirtual.Float(8, 0, .8f, RigidBodyDrag);        
        rb.gravityScale = 0;
        rb.drag = 14.0f;
        Dashing = true;
        CanMove = false;        

        yield return new WaitForSeconds(.3f);

        Dashing = false;
        rb.drag = 0.0f;
        rb.gravityScale = 3;
        CanMove = true;               
    }

    // make sure you can't dash forever on the ground
    IEnumerator DashCooldown(float cooldown) {
        DashLockout = true;
        yield return new WaitForSeconds(cooldown);
        DashLockout = false;
    }

    // helpers
    private Vector2 normalizeDashDirection(Vector2 dashDirection) {        
        return new Vector2(Mathf.Round(dashDirection.normalized.x), Mathf.Round(dashDirection.normalized.y));        
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.red;

        var positions = new Vector2[] { BottomOffset, RightOffset, LeftOffset };

        Gizmos.DrawWireSphere((Vector2)transform.position + BottomOffset, CollisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + RightOffset, CollisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + LeftOffset, CollisionRadius);
    }
}
