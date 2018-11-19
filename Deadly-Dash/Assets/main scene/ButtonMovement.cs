using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonMovement : MonoBehaviour
{

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
	}
	
	// Update is called once per frame
	void Update ()
    {
        currentMoveTimer += Time.deltaTime;
        currentMoveTimer = Mathf.Clamp(currentMoveTimer, 0, MoveTime);

        float normalizedTime = currentMoveTimer / MoveTime;

        float normalizedMoveDistance = MoveCurve.Evaluate(normalizedTime);
        transform.position = Vector3.Lerp(previousNode.transform.position, currentNode.transform.position, normalizedMoveDistance);

        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveLeft();
        }

        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveRight();
        }
        //transform.position = Vector3.MoveTowards(transform.position, currentNode.transform.position, speed * Time.deltaTime);
        //float remainingTravelDistance = Vector3.Distance(transform.position, currentNode.transform.position);


        //float travelPercent = remainingTravelDistance / travelNodeDistance;
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

    private void Jump()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            this.GetComponent<Rigidbody>().AddForce(Vector3.up * JumpPower, ForceMode.Impulse);
        }
    }


}
