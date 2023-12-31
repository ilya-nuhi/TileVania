using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    [SerializeField] float movementSpeed = 1f;
    Rigidbody2D myRigidBody;
    BoxCollider2D myBoxCollider;
    CircleCollider2D myCircleCollider;
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myBoxCollider = GetComponent<BoxCollider2D>();
        myCircleCollider = GetComponent<CircleCollider2D>();
    }

    void Update()
    {
        myRigidBody.velocity = new Vector2 (Mathf.Sign(myRigidBody.transform.localScale.x)*movementSpeed,0f);
        FlipEnemyFacingHelper();
    }

    void FlipEnemyFacingHelper()
    {
        if(myBoxCollider.IsTouchingLayers(LayerMask.GetMask("Hazard","Ground"))){
            FlipEnemyFacing();
        }
        if(!myCircleCollider.IsTouchingLayers(LayerMask.GetMask("Ground","Climbing"))){
            FlipEnemyFacing();
        }
    }

    void FlipEnemyFacing()
    {
        transform.localScale = new Vector2 (-Mathf.Sign(myRigidBody.velocity.x),1f);
    }
}
