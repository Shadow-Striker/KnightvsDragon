using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    private KnightController knightController;
    private bool isColliding;

    public bool IsColliding
    {
        get
        {
            return isColliding;
        }
        private set
        {
            isColliding = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        knightController = FindObjectOfType<KnightController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Dragon")
        {
            isColliding = true;
        }
    }
}
