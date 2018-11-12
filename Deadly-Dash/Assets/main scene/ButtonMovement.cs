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



    private GameObject currentNode;
    private TargetPosition currentPosition = TargetPosition.Middle;
	// Use this for initialization
	void Start ()
    {
        currentNode = Middle;
        transform.position = Middle.transform.position;
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void MoveLeft()
    {
        if(currentPosition == TargetPosition.Middle)
        {
            SetTarget(TargetPosition.Left);
            Debug.Log("working");
        }
        if(currentPosition == TargetPosition.Right)
        {
            SetTarget(TargetPosition.Middle);
            Debug.Log("working");
        }
        
    }
    public void MoveRight()
    {
      if(currentPosition == TargetPosition.Middle)
        {
            SetTarget(TargetPosition.Right);
            Debug.Log("working");
        }
      if(currentPosition == TargetPosition.Left)
        {
            SetTarget(TargetPosition.Middle);
            currentNode = Middle;
            Debug.Log("working");
        }
        
    }


    private void SetTarget(TargetPosition targetPosition)
    {
        if(targetPosition == TargetPosition.Left)
        {
            currentNode = Left;
            currentPosition = TargetPosition.Left;
        }
        if(targetPosition == TargetPosition.Middle)
        {
            currentNode = Middle;
            currentPosition = TargetPosition.Middle;
        }
        if(targetPosition == TargetPosition.Right)
        {
            currentNode = Right;
            currentPosition = TargetPosition.Right;
        }
        transform.position = currentNode.transform.position;
    }




}
