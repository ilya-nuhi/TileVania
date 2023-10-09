using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] float runSpeed = 10f;
    [SerializeField] float jumpSpeed = 20f;
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] Vector2 deathKick = new Vector2 (10f,10f);

    [Header("Gun")]
    [SerializeField] GameObject bullet;
    [SerializeField] Transform gun;

    [Header("Audio")]
    [SerializeField] AudioClip fireSFX;
    [SerializeField] AudioClip bounceSFX;
    

    
    Vector2 moveInput;
    Rigidbody2D myRigidBody;
    Animator myAnimator;
    
    float gravityScaleAtStart;

    int jumpCount;

    CapsuleCollider2D myBodyCollider;
    
    BoxCollider2D myFeetCollider;

    SpriteRenderer mySprite;

    

    bool isAlive = true;
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
        gravityScaleAtStart = myRigidBody.gravityScale;
        mySprite = GetComponent<SpriteRenderer>();
    
    }

    void Update()
    {
        if(!isAlive){return;}
        Run();
        FlipSprite();
        ClimbLadder();
        Die();
    }

    void ClimbLadder()
    {
        if(!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Climbing"))){
            myRigidBody.gravityScale = gravityScaleAtStart;
            myAnimator.SetBool("isClimbing",false);    
            return;
        }
        myRigidBody.gravityScale = 0f;
        Vector2 playerVelocity = new Vector2( myRigidBody.velocity.x ,moveInput.y * climbSpeed);
        myRigidBody.velocity = playerVelocity;
        bool playerHasVerticalSpeed = Mathf.Abs(myRigidBody.velocity.y) > Mathf.Epsilon;
        myAnimator.SetBool("isClimbing",playerHasVerticalSpeed);
    }

    void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed){
            transform.localScale = new Vector2 (Mathf.Sign(myRigidBody.velocity.x),1f);
        }
        
    }

    void OnMove(InputValue value){
        if(!isAlive){return;}
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value){
        if(!isAlive){return;}
        if(myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))){
            jumpCount = 1;
        }
        if(value.isPressed && jumpCount>=1){
            jumpCount--;
            myRigidBody.velocity += new Vector2 (0f,jumpSpeed);
        }
    }

    void OnFire(InputValue value){
        if(!isAlive){return;}
        if(value.isPressed){
            GetComponent<AudioSource>().PlayOneShot(fireSFX);
            Instantiate(bullet, gun.position, transform.rotation);
        }
    }

    void Run(){
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, myRigidBody.velocity.y);
        myRigidBody.velocity = playerVelocity;
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("isRunning", playerHasHorizontalSpeed);
    }

    void Die()
    {
        if(myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemies","Hazard"))){
            isAlive = false;
            myAnimator.SetTrigger("Dying");
            myRigidBody.velocity = deathKick;
            //mySprite.color = new Color (1,0.325f,0.325f,1);
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
    }

    void OnCollisionEnter2D(Collision2D other){
        if(other.gameObject.tag=="Bouncing"){
            AudioSource.PlayClipAtPoint(bounceSFX, other.gameObject.transform.position);
        }
    }
}
