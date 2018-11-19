using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SantaJump : MonoBehaviour
{
    [Tooltip("How fast the player will ascend while jumping.")]
    public float jumpForce = 5;
    [Tooltip("How long the player will be ascending for, in seconds.")]
    public float jumpTime = 0.6f;
    [Tooltip("How long the player will stay at the top of their jump for. It's to create a natural floaty feeling before falling. In seconds.")]
    public float jumpApexTime = 0.017f;
    [Tooltip("When falling, accelerates the player towards the ground at this percent of the jumpForce per second.")]
    public float fallSpeedMulti = 0.08f;

    private float groundPos;
    private float jTimer;
    private float fallVelocity;

    void Start()
    {
        groundPos = transform.position.y;
    }

    // Update is called once per frame
    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Space) && transform.position.y == groundPos)
            jTimer = jumpTime + jumpApexTime;

        if (jTimer > jumpApexTime)
        {
            transform.position += new Vector3(0, jumpForce * Time.deltaTime * (jTimer / jumpTime), 0);
            jTimer -= Time.deltaTime;
        }
        else if (jTimer > 0)
        {
            jTimer -= Time.deltaTime;
            fallVelocity = 0;
        }
        else
        {
            transform.position -= new Vector3(0, fallVelocity);
            fallVelocity += jumpForce * Time.deltaTime * fallSpeedMulti;
        }

        float posY = Mathf.Max(transform.position.y, groundPos);
        transform.position = new Vector3(transform.position.x, posY, transform.position.z);
    }
}
