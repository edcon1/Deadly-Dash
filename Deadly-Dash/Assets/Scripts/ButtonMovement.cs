
using UnityEngine;

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
    [Tooltip("Starting speed")]
    public float startSpeed = 10;
    [Tooltip("Max speed for the player")]
    public float maxSpeed = 30;

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

    public float TapDistance = 10;
    private Vector2 startTouchPosition = new Vector2();

	// Use this for initialization
	void Start ()
    {
        currentNode = Middle;
        previousNode = currentNode;
        transform.position = Middle.transform.position;
        groundPos = transform.position.y;

        GlobalScript.WorldSpeed = startSpeed;
	}
	
	// Update is called once per frame
	void Update ()
    {
        //for(int i = 0;i < Input.touchCount;++i)
        //{
        //
        //}

        if (Input.touchCount > 0)
        {
            Touch t = Input.touches[0];
            if (t.phase == TouchPhase.Began)
            {
                Debug.Log("hit");
                startTouchPosition = t.position;
            }
            else if (t.phase == TouchPhase.Canceled || t.phase == TouchPhase.Ended)
            {
                Vector2 releasePos = t.position;
                Vector2 swipeOffset = releasePos - startTouchPosition;
                if (swipeOffset.magnitude < TapDistance)
                {
                    if (startTouchPosition.x < Screen.width * 0.5f)
                    {
                        MoveLeft();
                    }
                    else
                    {
                        MoveRight();
                    }
                }
                else
                {
                    float fUp = Vector2.Dot(swipeOffset, Vector2.up);
                    float fDown = Vector2.Dot(swipeOffset, Vector2.down);

                    float fLeft = Vector2.Dot(swipeOffset, Vector2.left);
                    float fRight = Vector2.Dot(swipeOffset, Vector2.right);

                    if (fLeft > fRight && fLeft > fDown && fLeft > fUp)
                    {
                        MoveLeft();
                    }
                    else if (fRight > fLeft && fRight > fDown && fRight > fUp)
                    {
                        MoveRight();
                    }
                    else if (fUp > fLeft && fUp > fRight && fUp > fDown)
                    {
                        DoJump();
                    }
                    else if (fDown > fLeft && fDown > fRight && fDown > fUp)
                    {
                        if (transform.position.y > groundPos)
                        {
                            jTimer = 0;
                        }
                    }
                }
            }

        }

        GlobalScript.WorldSpeed = GlobalScript.WorldSpeed + acceleration * Time.deltaTime;
        GlobalScript.WorldSpeed = Mathf.Min(GlobalScript.WorldSpeed, maxSpeed);

        currentMoveTimer += Time.deltaTime;
        currentMoveTimer = Mathf.Clamp(currentMoveTimer, 0, MoveTime);

        float normalizedTime = currentMoveTimer / MoveTime;

        float normalizedMoveDistance = MoveCurve.Evaluate(normalizedTime);
        Vector3 targetPos = Vector3.Lerp(previousNode.transform.position, currentNode.transform.position, normalizedMoveDistance);


        float posY = transform.position.y;

        if (jTimer > jumpApexTime)
        {
            posY += jumpForce * Time.deltaTime * (jTimer / jumpTime);
            jTimer -= Time.deltaTime;
        }
        else if (jTimer > 0)
        {
            jTimer -= Time.deltaTime;
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

    private void DoJump()
    {
        if (transform.position.y <= groundPos)
        {
            jTimer = jumpTime + jumpApexTime;
            fallVelocity = 0;
        }
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
