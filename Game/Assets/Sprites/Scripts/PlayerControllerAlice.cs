using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerAlice : MonoBehaviour
{
    public float speed;
    private Rigidbody2D playerBody;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 30;
        playerBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        Vector2 movement = new Vector2(moveHorizontal,0);
        playerBody.AddForce(movement * speed);

        float moveVertical = Input.GetAxis("Vertical");
        Vector2 movementV = new Vector2(0, moveVertical);
        playerBody.AddForce(movementV * speed);
    }
}
