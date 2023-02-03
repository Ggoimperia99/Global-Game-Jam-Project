using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NewPlayerClass : MonoBehaviour
{
    public GameManClass refGameMan;
    //state machine
    public enum PlayerStates { Standing, onLadder, Gliding, Empty };
    public PlayerStates myPlayerStates;

    [Header("Physics")]
    public Rigidbody2D rigBod;
    public bool isGrounded;
    SpriteRenderer myRenderer;
    [SerializeField] AudioSource walkAudio;
    [SerializeField] AudioSource jumpAudio;
  

    [Header("LR Move")]
    public float horizInput, speed;

    //jump
    public float jumpForce;
    public Transform groundCheck;
    public float checkRedius;
    public LayerMask whatisGround;
    private int extraJumps;
    public int extraJumpsValue;

    //gliding
    public float glideForce;
    public int SpaceCounter;

    //Animation
    public Animator Anim;
    public bool AnimRunning, AnimIdle;

    //For Audio
    bool isRunning;

    void Start()
    {
        extraJumps = extraJumpsValue;
        myRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        //jump
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRedius, whatisGround);

        if (myPlayerStates == PlayerStates.Standing)
        {
            LRMovement();
        }
        if (myPlayerStates == PlayerStates.Gliding)
        {

            LRMovement();
            //yeets player
            //rigBod.AddForce(Vector2.up * glideForce);
            //why updraft

            if (isGrounded == true)
            {
                myPlayerStates = PlayerStates.Standing;
            }
        }
        if (myPlayerStates == PlayerStates.Empty)
        {

        }
    }

    void Update()
    {
        //Animations();

        FlipSprite();
        PlayAudio();

        if (refGameMan.dialogueinAction == false)
        {
            PlayerInput();
            Gravity();
            if (isGrounded == true)
            {
                extraJumps = extraJumpsValue;
            }
        }
        else
        {

        }

        // set gliding animation
        if(myPlayerStates == PlayerStates.Gliding)
        {
            Anim.SetBool("IsGliding", true);
        }
        else if(myPlayerStates != PlayerStates.Gliding)
        {
            Anim.SetBool("IsGliding", false);
        }

    }

    void Gravity()
    {
        if (myPlayerStates == PlayerStates.Standing)
        {
            GetComponent<Rigidbody2D>().mass = 4;
            GetComponent<Rigidbody2D>().gravityScale = 6;
        }
        else if (myPlayerStates == PlayerStates.Gliding)
        {
            GetComponent<Rigidbody2D>().mass = 1;
            GetComponent<Rigidbody2D>().gravityScale = 2;
        }
    }

    void PlayerInput()
    {
        //LR Movement Input
        horizInput = Input.GetAxis("Horizontal");

        //jumping
        if (Input.GetKeyDown(KeyCode.Space) && extraJumps > 0)
        {
            //rigBod.velocity = Vector2.up * jumpForce;
            extraJumps -= 1;
        }
        if (Input.GetKeyDown(KeyCode.Space) && extraJumps == 0 && isGrounded == true)
        {
            rigBod.velocity = Vector2.up * jumpForce;
        }

        //gliding
        if (Input.GetKey(KeyCode.Space))
        {
            if (isGrounded == false)
            {
                if (SpaceCounter > 2)
                {
                    myPlayerStates = PlayerStates.Gliding;
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpaceCounter += 1;
        }
    }

    void LRMovement()
    {
        //Horiz Movement
        rigBod.velocity = new Vector2((horizInput * speed), rigBod.velocity.y);
        AnimRunning = true;

        /*//flip sprite 
        if (rigBod.velocity.x > 0)
        {
            transform.localScale = new Vector3(1.5f, transform.localScale.y, transform.localScale.z);
        }
        if (rigBod.velocity.x < 0)
        {
            transform.localScale = new Vector3(-1.5f, transform.localScale.y, transform.localScale.z);
        }*/

        // Setting running animation
        if(Mathf.Abs(horizInput) > Mathf.Epsilon && isGrounded)
        {
            Anim.SetBool("IsRunning", true);

            isRunning = true;
        }
        else if(Mathf.Abs(horizInput) <= Mathf.Epsilon)
        {
            Anim.SetBool("IsRunning", false);

            isRunning = false;
        }

        // Setting jumping animation
        if(rigBod.velocity.y > Mathf.Epsilon && !isGrounded)
        {
            Anim.SetBool("IsJumping", true);
            Anim.SetBool("IsFalling", false);

            isRunning = false;
        }
        else if (rigBod.velocity.y <= Mathf.Epsilon || isGrounded)
        {
            Anim.SetBool("IsJumping", false);
        }

        // Setting falling animation
        if(rigBod.velocity.y < Mathf.Epsilon && !isGrounded)
        {
            Anim.SetBool("IsFalling", true);
        }
        else if (isGrounded)
        {
            Anim.SetBool("IsFalling", false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            SpaceCounter = 0;
            isGrounded = true;
        }
        print(collision.gameObject.name);
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
        if (collision.gameObject.tag == "DialogueStart")
        {
            refGameMan.dialogueinAction = true;
            horizInput = 0;
        }
    }

    private void FlipSprite()
    {
        if(rigBod.velocity.x > 0.01f)
        {
            transform.localScale = new Vector2(1.5f, transform.localScale.y);
        }
        if(rigBod.velocity.x < -0.01f)
        {
            //myRenderer.flipX = true;
            transform.localScale = new Vector2(-1.5f, transform.localScale.y);
        }
    }

    private void PlayAudio()
    {
        if (isRunning)
        {
            walkAudio.enabled = true;
        }
        else if (!isRunning)
        {
            walkAudio.enabled = false;
        }
    }

    public void playJumpAudio()
    {
        jumpAudio.Play();
    }

    /*void Animations()
    {
  
        if (AnimRunning == true)
        {
            Anim.SetBool("IsRunning", true);
        }
    }*/
}
