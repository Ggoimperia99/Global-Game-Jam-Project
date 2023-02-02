using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementClass : MonoBehaviour
{
    //state machine
    public enum PlayerStates { Standing, onLadder, Gliding, Empty };
    public PlayerStates myPlayerStates; 
    
    [Header("Physics")]
    public Rigidbody2D rigBod;
    public bool isGrounded;

    [Header ("LR Move")]
    public float horizInput, speed;

    // [Header ("jump")]
    //jump
    //jump
    Vector3 jump;
    public float jumpForce;
    public bool isJumping;
    public bool isDoubleJumping;
    public float jumpWindow, doubleJumpWindow;
    public float jumpWindowTimer, doubleJumpWindowTimer;
    //Ladder
    public bool onLadderBool, touchingLadder, tryingtoClimb;
    float VertInput;

    //Glider
    public bool touchingGlider, isGliding, tryingtoGlide; 

    void Start()
    {
        //jump
        jumpForce = 4;
        jump = new Vector3(0, 6f, 0);
    }

    private void FixedUpdate()
    {
        if (myPlayerStates == PlayerStates.Standing)
        {
            LRMovement();
            Jump();
            //from Standing --> onLadder
            if (touchingLadder == true && tryingtoClimb == true)
            {
                myPlayerStates = PlayerStates.onLadder;
                isGrounded = false;
            }

            if (touchingGlider == true && isGliding == true)
            {
                myPlayerStates = PlayerStates.Gliding;
                isGrounded = false;
            }

            if (isGrounded == false)
            {
                myPlayerStates = PlayerStates.Empty;
            }

        }
        else if (myPlayerStates == PlayerStates.onLadder)
        {
            //maybe remove LR movement on ladders?
            LRMovement();
            UDMovement();
            //from onLadder --> Standing
            if (isGrounded == true && touchingLadder == true && tryingtoClimb == false)
            {
                //print("on ladder but standing");
                myPlayerStates = PlayerStates.Standing;
                onLadderBool = false;
            }
            if (touchingLadder == false && isGrounded == false)
            {
                myPlayerStates = PlayerStates.Empty;
            }
        }
        else if (myPlayerStates == PlayerStates.Empty)
        {
            LRMovement();
            DoubleJump();
            //print("empty state");
            //jumpWindowTimer = 0;
            //doubleWindowTimer = 0;

            //transitioning from empty to standing, on ladder
            if (touchingLadder == true && onLadderBool == true)
            {
                myPlayerStates = PlayerStates.onLadder;
            }
            else if (isGrounded == true)
            {
                myPlayerStates = PlayerStates.Standing;
            }
        }
        else if (myPlayerStates == PlayerStates.Gliding)
        {
            LRMovement();
        }
    }

    void Update()
    {
        PlayerInput();
        Gravitiy();
    }

    void PlayerInput()
    {
        //LR Movement Input (includes monkey bars)
        horizInput = Input.GetAxis("Horizontal");
        VertInput = Input.GetAxis("Vertical");

        //climbcheck
        if (VertInput > 0.1f || VertInput < -0.1f)
        {
            tryingtoClimb = true;
            print("I want to climb pleease");
        }
        else
        {
            tryingtoClimb = false;
        }

        //Jump input
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded == true)
        {
            jumpWindowTimer = jumpWindow;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && isJumping == true)
        {
            print("space again");
            doubleJumpWindowTimer = doubleJumpWindow;
        }

       /* if (Input.GetKey(KeyCode.UpArrow) && touchingGlider == true)
        {
            isGliding = true;
            transform.position += new Vector3(0, 1);
        }*/

    }

    void Gravitiy()
    {
        if (myPlayerStates == PlayerStates.Standing || myPlayerStates == PlayerStates.Empty)
        {
            rigBod.gravityScale = 6;
        }
        if (myPlayerStates == PlayerStates.onLadder || myPlayerStates == PlayerStates.Gliding)
        {
            rigBod.gravityScale = 0;
        }
    }

    void LRMovement()
    {
        //Horiz Movement
        rigBod.velocity = new Vector2((horizInput * speed), rigBod.velocity.y);

        //flip sprite 
        if (rigBod.velocity.x > 0)
        {
            transform.localScale = new Vector3(0.7f, transform.localScale.y, transform.localScale.z);
        }
        if (rigBod.velocity.x < 0)
        {
            transform.localScale = new Vector3(-0.7f, transform.localScale.y, transform.localScale.z);
        }
    }

    void UDMovement()
    {
        if (tryingtoClimb == true)
        {
            rigBod.velocity = new Vector2(rigBod.velocity.x, VertInput * speed);
        }
        else
        {
            rigBod.velocity = new Vector2(rigBod.velocity.x, 0);
        }
    }
    void Jump()
    {
        //input in PlayerInput void
        //if (isGrounded == true && jumpWindowTimer > 0 && isJumping == false)
        //{
        //    rigBod.AddForce(jump * jumpForce, ForceMode2D.Impulse);
        //    isJumping = true;
        //    jumpWindowTimer = 0;
        //    //isGrounded = false;
        //}
        if (isGrounded == true && jumpWindowTimer > 0 && isJumping == false)
        {
            rigBod.AddForce(jump * jumpForce, ForceMode2D.Impulse);
            isJumping = true;
            jumpWindowTimer = 0;
            //isGrounded = false;
        }
    }

    void DoubleJump()
    {
        if (isGrounded == false && isJumping == true && doubleJumpWindowTimer > 0 && isDoubleJumping == false)
        {
            rigBod.AddForce(jump * jumpForce, ForceMode2D.Impulse);
            isDoubleJumping = true;
            doubleJumpWindowTimer = 0;
        }
    }

    void Gliding()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            isGrounded = true;
            isJumping = false;
            isDoubleJumping = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            isGrounded = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ladder"))
        {
            touchingLadder = true;
            onLadderBool = true;
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("Glider"))
        {
            touchingGlider = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ladder"))
        {
            //print("am off ladder");
            touchingLadder = false;
            onLadderBool = false;
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("Glider"))
        {
            touchingGlider = false;
        }
    }
}

