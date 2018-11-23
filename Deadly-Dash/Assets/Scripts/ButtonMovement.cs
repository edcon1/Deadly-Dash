using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor.SceneManagement;

public class ButtonMovement : MonoBehaviour
{

    [Tooltip("How fast the player will ascend while jumping.")]
    public float jumpForce = 5;
    [Tooltip("How long the player will be ascending for, in seconds.")]
    public float jumpTime = 0.6f;
    [Tooltip("How long the player will stay at the top of their jump for. It's to create a natural floaty feeling before falling. In seconds.")]
    public float jumpApexTime = 0.017f;
    [Tooltip("When falling, accelerates the player towards the ground at this percent of the jumpForce per second.")]
    public float fallSpeedMulti = 0.08f;
    [Tooltip("How fast the player accelerates per second.")]
    public float acceleration = 0;

    private float groundPos;
    private float jTimer;
    private float fallVelocity;

    enum TargetPosition
    {
        Left,
        Middle,
        Right 
    };

    public GameObject Left;
    public GameObject Middle;
    public GameObject Right;

    [Range(0.05f,1.5f)]
    public float MoveTime = 0.5f;

    [Range(1,30)]
    public float JumpPower = 2;

    private GameObject currentNode;
    private GameObject previousNode;
    
    private TargetPosition currentPosition = TargetPosition.Middle;

    public AnimationCurve MoveCurve;
    

    private float currentMoveTimer = 0;

	// Use this for initialization
	void Start ()
    {
        currentNode = Middle;
        previousNode = currentNode;
        transform.position = Middle.transform.position;
        groundPos = transform.position.y;
	}
	
	// Update is called once per frame
	void Update ()
    {
        GlobalScript.WorldSpeed = GlobalScript.WorldSpeed + acceleration * Time.deltaTime;

        currentMoveTimer += Time.deltaTime;
        currentMoveTimer = Mathf.Clamp(currentMoveTimer, 0, MoveTime);

        float normalizedTime = currentMoveTimer / MoveTime;

        float normalizedMoveDistance = MoveCurve.Evaluate(normalizedTime);
        Vector3 targetPos = Vector3.Lerp(previousNode.transform.position, currentNode.transform.position, normalizedMoveDistance);

        

        if (Input.GetKeyDown(KeyCode.Space) && transform.position.y <= groundPos)
            jTimer = jumpTime + jumpApexTime;
        

            float posY = transform.position.y;
            if (jTimer > jumpApexTime)
            {
                posY += jumpForce * Time.deltaTime * (jTimer / jumpTime);
                jTimer -= Time.deltaTime;
            }
            else if (jTimer > 0)
            {
                jTimer -= Time.deltaTime;
                fallVelocity = 0;
            }
            else
            {
                posY -= fallVelocity;
                fallVelocity += jumpForce * Time.deltaTime * fallSpeedMulti;
            }

        

        posY = Mathf.Max(posY, groundPos);
        targetPos.y = posY;


        transform.position = targetPos;
    }

    public void MoveLeft()
    {
        if (currentMoveTimer < MoveTime - 0.001f) return;
        if(currentPosition == TargetPosition.Middle)
        {
            SetTarget(TargetPosition.Left); 
        }
        else if (currentPosition == TargetPosition.Right)
        {
            SetTarget(TargetPosition.Middle);
        }   
    }
    public void MoveRight()
    {
        if (currentMoveTimer < MoveTime - 0.001f) return;
        if (currentPosition == TargetPosition.Middle)
        {
            
            SetTarget(TargetPosition.Right);
           
        }
      else if(currentPosition == TargetPosition.Left)
        {
            SetTarget(TargetPosition.Middle);
        }
        
    }


    private void SetTarget(TargetPosition targetPosition)
    {
        previousNode = currentNode;
        currentMoveTimer = 0.0f;
        if (targetPosition == TargetPosition.Left)
        {
            
            currentNode = Left;
            currentPosition = TargetPosition.Left;
        }
        else if(targetPosition == TargetPosition.Middle)
        {
            currentNode = Middle;
            currentPosition = TargetPosition.Middle;
        }
        else if(targetPosition == TargetPosition.Right)
        {
            currentNode = Right;
            currentPosition = TargetPosition.Right;
        }
        //transform.position = currentNode.transform.position;
    }
}
