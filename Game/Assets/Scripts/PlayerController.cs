using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    // Run Speed & Acceleration
    [Header ("Movement Params")]
	public float RunSpeed = 5f; // Maximum Horizontal Run Speeds
    private Vector2 movementInput;

    [Header ("Dodge System")]
    public BarSlider staminaBar;
    public int MaxStamina = 100;
    public float StaminaRegenRate = 25;
    public int DodgeStaminaCost = 50;
    private float LastDodgeTime = 0;
    public int CurrentStamina;
    private float StaminaToRegen;

    [Header ("Health System")]
    public BarSlider healthBar;
    public int MaxHealth = 100;
    public float HealthRegenRate = 10;
    public float HealthRegenDelay = 5;
    private float LastDamagedTime = -1;
    private int CurrentHealth;
    private float HealthToRegen;

    [Header ("State Trackers")]
    public bool isDead = false;
    public bool isDodging = false;

    [Header ("References")]
    public Camera playerCam;
    public GameObject deathPanel;
    public Text respawnText;

    [Header ("Player Body")]
	private Rigidbody2D body;
    private BoxCollider2D bcollider;
    private Animator animator;

    void Awake()
    {
        Application.targetFrameRate = 60;
        body=GetComponent<Rigidbody2D>();
        bcollider=GetComponent<BoxCollider2D>();
        animator=GetComponent<Animator>();
        Respawn();
    }

    #region Main
    void Update()
    {
        playerCam.transform.position=new Vector3 (transform.position.x, transform.position.y, playerCam.transform.position.z);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isDead) {
            body.velocity= new Vector2(movementInput.x * RunSpeed, movementInput.y * RunSpeed);
            if ( LastDamagedTime >= 0 && Time.time - LastDamagedTime >= HealthRegenDelay ) {
                RegenerateHealth();
            }
            if (CurrentStamina<=MaxStamina) {
                RegenerateStamina();
            }

        }
    }
    #endregion

    #region Movement
    public void Move(InputAction.CallbackContext context) {
        movementInput = context.ReadValue<Vector2>();
    }

    public void Dodge(InputAction.CallbackContext context) {
        if (context.performed && CurrentStamina>=DodgeStaminaCost) {
            LastDodgeTime=Time.time;
            CurrentStamina-=DodgeStaminaCost;
            StartCoroutine(RecoverFromDodge());
        }
    }

    private IEnumerator RecoverFromDodge() {
        isDodging=true;
        RunSpeed=25f;
        yield return new WaitForSeconds(0.3f);
        RunSpeed=5f;
        isDodging=false;
    }

    void Stim(int stamina) {
        if (!isDead){
            CurrentStamina += stamina;
            if (CurrentStamina >= MaxStamina) {
                CurrentStamina = MaxStamina;
                HealthToRegen = 0;
            }
            staminaBar.SetValue(CurrentStamina);
        }
    }

    public void RegenerateStamina() {
        StaminaToRegen += StaminaRegenRate * Time.deltaTime;
        int FlooredStaminaToRegen = Mathf.FloorToInt( StaminaToRegen );
        StaminaToRegen -= FlooredStaminaToRegen;
        Stim( FlooredStaminaToRegen );
     }

    #endregion

    #region Health System
    void OnCollisionEnter2D(Collision2D col)
    {
        if (!isDodging) Damage(20);
    }

    void Damage(int damage) {
        if (!isDead){
            LastDamagedTime=Time.time;
            CurrentHealth -= damage;
            if (CurrentHealth<=0) {
                CurrentHealth=0;
                Die();
            }
            healthBar.SetValue(CurrentHealth);
        }
    }

    void Heal(int heal) {
        if (!isDead){
            CurrentHealth += heal;
            if (CurrentHealth >= MaxHealth) {
                CurrentHealth = MaxHealth;
                LastDamagedTime = -1;
                HealthToRegen = 0;
            }
            healthBar.SetValue(CurrentHealth);
        }
    }

    public void RegenerateHealth() {
        HealthToRegen += HealthRegenRate * Time.deltaTime;
        int FlooredHealthToRegen = Mathf.FloorToInt( HealthToRegen );
        HealthToRegen -= FlooredHealthToRegen;
        Heal( FlooredHealthToRegen );
     }
    #endregion

    #region Respawn System
    void Die() {
        isDead=true;
        deathPanel.SetActive(true);
        StartCoroutine(StartRespawn());

        //deactivate body components
        body.velocity=Vector2.zero;
        bcollider.enabled=false;
    }

    private IEnumerator StartRespawn() {
        for (int i = 15; i > 0; i--) {
            respawnText.text = string.Format("Respawning in {0}...", i.ToString());
            yield return new WaitForSeconds(1f);
        }
        Respawn();
    }

    void Respawn() {
        isDead=false;
        healthBar.SetMaxValue(MaxHealth);
        staminaBar.SetMaxValue(MaxStamina);
        Heal(MaxHealth);
        Stim(MaxStamina);
        deathPanel.SetActive(false);

        //activate body components
        bcollider.enabled=true;
    }
    #endregion
}

