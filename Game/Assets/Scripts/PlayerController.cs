using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    // Run Speed & Acceleration
    [Header ("Movement Params")]
	public float MaxRunSpeed = 90f; // Maximum Horizontal Run Speed
	public float RunAccel = 1000f; // Horizontal Acceleration Speed
	public float RunReduce = 400f; // Horizontal Acceleration when you're already when your horizontal speed is higher or equal to the maximum

    [Header ("Health System")]
    public HealthBar healthBar;
    public int maxHealth = 100;
    private int currentHealth;

    [Header ("References")]
	public Rigidbody2D rb;
    public Camera playerCam;

    private Vector2 movementInput;
    private float rotateAngle = 0f;

    void Start()
    {
        Application.targetFrameRate = 60;
        healthBar.SetMaxHealth(maxHealth);
        currentHealth=maxHealth;
    }

    
    void Update()
    {
        playerCam.transform.position=new Vector3 (transform.position.x,transform.position.y,playerCam.transform.position.z);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.velocity= new Vector2(movementInput.x * MaxRunSpeed, movementInput.y * MaxRunSpeed);
        
        if (movementInput.x==0 && movementInput.y==1) rotateAngle=0;
        else if (movementInput.x==1 && movementInput.y==0) rotateAngle=-90f;
        else if (movementInput.x==0 && movementInput.y==-1) rotateAngle=-180f;
        else if (movementInput.x==-1 && movementInput.y==0) rotateAngle=-270f;
   
        transform.rotation = Quaternion.Euler(0,0,rotateAngle);

    }

    public void Move(InputAction.CallbackContext context) {
        movementInput = context.ReadValue<Vector2>();
        
        Debug.Log(movementInput);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        TakeDamage(20);
    }

    void TakeDamage(int damage) {
        currentHealth-=damage;
        healthBar.SetHealth(currentHealth);
    }
}

