using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    DeathMenu deathMenu;

    // Use this for initialization
    void Start ()
    {
        deathMenu = GameObject.Find("DeathMenu").GetComponent<DeathMenu>();
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Damage")
        {
            deathMenu.Death();
        }
    }
}
