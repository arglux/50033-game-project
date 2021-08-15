using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.Experimental.Rendering.Universal;

public class PlayerController : MonoBehaviour {
    [Header ("State Trackers")]
    public bool isDead = false;
    public bool isDodging = false;
    public bool isMoving = false;
    public bool isHurting = false;

    [Header ("References")]
    public Camera playerCam;
    public GameObject deathPanel;
    public Text respawnText;
    private CharacterClass characterClass;
    
    [Header ("Player Body")]
    public SpriteRenderer sprite;
	private Rigidbody2D body;
    private CapsuleCollider2D bcollider;
    private Animator animator;

    [Header ("Movement Params")]
	public float RunSpeed = 5f; // Maximum Horizontal Run Speeds
    private Vector2 movementInput;

    [Header ("Dodge System")]
    public BarSlider staminaBar;
    public int MaxStamina = 100;
    private float StaminaRegenRate = 8f;
    private int DodgeStaminaCost = 50;
    private float LastDodgeTime = 0;
    private float dodgeTime = 0.2f;
    private int CurrentStamina;
    private float StaminaToRegen;

    [Header ("Health System")]
    public BarSlider healthBar;
    public int MaxHealth = 100;
    private float HealthRegenRate = 5f;
    private float HealthRegenDelay = 5f;
    private float LastDamagedTime = -1f;
    private float damageMitigation = 0;
    private int CurrentHealth;
    private float HealthToRegen;

    [Header ("Skill System")]
    public Image skillOverlay;
    public Image skillIcon;
    public Text skillText;
    public Image passiveIcon;
    public Text passiveText;
    public Sprite tankSKL;
    public Sprite dpsSKL;
    public Sprite supportSKL;
    public Sprite tankPSV;
    public Sprite dpsPSV;
    public Sprite supportPSV;
    public ParticleSystem SkillParticles;
    public Text SkillPointsText;
    public bool skillActive = false;
    private bool skillReady = true;
    private float currentCooldown = 0f;

    [Header ("Light System")]
    public BarSlider batteryBar;
    public Light2D torchlight;
    public TorchLightController torchLightController;
    public GameObject torch60;
    public GameObject torch50;
    public GameObject torch22;
    public GameObject vision;
    private int MaxBattery = 100;
    private float BatteryUseRate = 1f;
    private int CurrentBattery;
    private float BatteryToUse;
    private Coroutine torchCoroutine;

    [Header ("Feedback System")]
    public AudioClip[] deathSFX;
    public AudioClip[] hurtSFX;
    public AudioClip dodgeSFX;
    public Material hitMaterial;
    private Material normalMaterial;


    void Awake() {
        body=GetComponent<Rigidbody2D>();
        bcollider=GetComponent<CapsuleCollider2D>();
        animator=GetComponent<Animator>();
        normalMaterial = sprite.material;
    }

    public void SetupClass(CharacterClass cc) 
    {
        characterClass = cc;
        switch(cc.classType) {
            case ClassType.Tank:
                skillIcon.sprite = tankSKL;
                skillOverlay.sprite = tankSKL;
                passiveIcon.sprite = tankPSV;
                setTorchlight(3f, 60f, torch60);
                break;
            case ClassType.DPS:
                skillIcon.sprite = dpsSKL;
                skillOverlay.sprite = dpsSKL;
                passiveIcon.sprite = dpsPSV;
                setTorchlight(12f, 25f, torch22);
                break;
            case ClassType.Support:
                skillIcon.sprite = supportSKL;
                skillOverlay.sprite = supportSKL;
                passiveIcon.sprite = supportPSV;
                setTorchlight(10f, 50f, torch50);
                break;
        }
        UpdateClass();
        Respawn();
    }

    public void UpdateClass() {
        skillText.text = "SKL. " + characterClass.skillPoints;
        passiveText.text = "PSV. " + characterClass.passivePoints;
        if (characterClass.unassignedPoints > 0) {
            SkillPointsText.text = characterClass.unassignedPoints + " Points Available";
            skillIcon.color = new Color32(155,155,0,255);
            skillOverlay.color= Color.yellow;
            passiveIcon.color= Color.yellow;
            skillText.color= Color.yellow;
            passiveText.color= Color.yellow;
        } else {
            SkillPointsText.text="";
            skillIcon.color = new Color32(100,100,100,255);
            passiveIcon.color = new Color32(200,200,200,255);
            skillOverlay.color = new Color32(200,200,200,255);
            skillText.color= Color.white;
            passiveText.color= Color.white;
        }

        switch(characterClass.classType) {
            case ClassType.Tank:
                float healthMult = 1f + characterClass.passivePoints * characterClass.healthBonusPercent;
                int OldMaxHealth = MaxHealth;
                MaxHealth = (int) (100 * healthMult);
                healthBar.SetMaxValue(MaxHealth);
                Heal(MaxHealth - OldMaxHealth);
                HealthRegenRate = MaxHealth / 20f;
                break;
            case ClassType.DPS:
                float agiMult = 1f + characterClass.passivePoints * characterClass.agilityBonusPercent;
                RunSpeed = 5f * agiMult;
                StaminaRegenRate = 8f * agiMult * 1.5f;
                break;
            case ClassType.Support:
                float visMult = 1f + characterClass.passivePoints * characterClass.visionMultiplier;
                vision.transform.localScale = new Vector3(visMult, visMult , 1f);
                Light2D visionLight = vision.GetComponent<Light2D>();
                visionLight.pointLightInnerRadius = 0.75f * visMult;
                visionLight.pointLightOuterRadius = 1.5f * visMult;
                break;
        }
    }
 
    #region Updates
    void Update()
    {
        playerCam.transform.position = new Vector3 (transform.position.x, transform.position.y, playerCam.transform.position.z);
        animator.SetBool("isMoving", isMoving);
        animator.SetBool("isDodging", isDodging);
        animator.SetBool("isDead", isDead);
    }

    void FixedUpdate()
    {
        if (!isDead) {
            if (!isHurting || isDodging) {
                body.velocity= new Vector2(movementInput.x * RunSpeed, movementInput.y * RunSpeed);
            } else {
                body.velocity= Vector2.zero;
            }
            
            if ( LastDamagedTime >= 0 && Time.time - LastDamagedTime >= HealthRegenDelay ) {
                RegenerateHealth(HealthRegenRate);
            }
            if (CurrentStamina<MaxStamina) {
                RegenerateStamina();
            }
            // if (CurrentBattery>0) {
            //     UseTorchlight();
            // }
        }
    }
    #endregion

    #region Movement
    public void Move(InputAction.CallbackContext context) {
        movementInput = context.ReadValue<Vector2>();
        isMoving = !(movementInput==Vector2.zero);
    }

    public void Dodge(InputAction.CallbackContext context) {
        if (context.started  && CurrentStamina>=DodgeStaminaCost && !isDodging) {
            AudioManager.instance.Play(dodgeSFX);
            LastDodgeTime=Time.time;
            CurrentStamina-=DodgeStaminaCost;
            StartCoroutine(RecoverFromDodge());
        }
    }

    private IEnumerator RecoverFromDodge() {
        isDodging=true;
        float prevSpeed = RunSpeed;
        RunSpeed += 10f;
        yield return new WaitForSeconds(dodgeTime);
        RunSpeed -= 10f;
        isDodging=false;
    }

    public void Stim(int stamina) {
        if (!isDead){
            CurrentStamina += stamina;
            if (CurrentStamina >= MaxStamina) {
                CurrentStamina = MaxStamina;
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
    public void TakeDamage(BulletController bullet) {
        Damage(bullet.baseDamage);
    }

    public void TakeDamage(float damage) {
        Damage(damage);
    }

    public void Damage(float inDmg) {
        if (!isDead && !isDodging && !isHurting) {
            int damage = Mathf.RoundToInt(inDmg * (1f - damageMitigation));
            if (damage >0) {
                HealthPopup.Create(transform.position, damage, DamageTypes.DPlayer);
                CurrentHealth -= damage;
                if (CurrentHealth<=0) {
                    CurrentHealth=0;
                    Die();
                } else {
                    if (!isHurting) {
                        StartCoroutine(HurtFlash());
                    }
                }
                LastDamagedTime=Time.time;
                 healthBar.SetValue(CurrentHealth);
            }
        }
    }

    public IEnumerator HurtFlash ()
    {
        isHurting=true;
        sprite.material = hitMaterial;
        AudioManager.instance.Play(hurtSFX[Random.Range(0, hurtSFX.Length)]);
        yield return new WaitForSeconds(0.2f);
        sprite.material = normalMaterial;
        yield return new WaitForSeconds(0.1f);
        isHurting=false;
    }

    public void Heal(int heal) {
        if (!isDead && heal>0) {
            CurrentHealth += heal;
            if (CurrentHealth >= MaxHealth) {
                CurrentHealth = MaxHealth;
                LastDamagedTime = -1;
                HealthToRegen = 0;
            }
            healthBar.SetValue(CurrentHealth);
        }
    }

    public void RegenerateHealth(float rate) {
        HealthToRegen += rate * Time.deltaTime;
        int FlooredHealthToRegen = Mathf.FloorToInt( HealthToRegen );
        HealthToRegen -= FlooredHealthToRegen;
        Heal( FlooredHealthToRegen );
    }
    #endregion


    #region Skill System

    private IEnumerator SkillCooldown() {
        // do something
        skillOverlay.fillAmount = 0;
        while (currentCooldown<characterClass.effectTime) {
            currentCooldown += Time.deltaTime;
            skillOverlay.fillAmount = currentCooldown/characterClass.cooldownTime;
            yield return null;
        }
        switch(characterClass.classType) {
            case ClassType.Tank:
                damageMitigation = 0;
                break;
            default:
                break;
        }
        torchlight.color = Color.white;
        // torchLightController.StopTorchSkill();
        SkillParticles.Stop();
        skillActive=false;
        while (currentCooldown<characterClass.cooldownTime) {
            currentCooldown += Time.deltaTime;
            skillOverlay.fillAmount = currentCooldown/characterClass.cooldownTime;
            yield return null;
        }
        skillOverlay.fillAmount=1; 
        skillReady=true;
        currentCooldown=0;
    }

    public void UseSkill(InputAction.CallbackContext context) {
        if (context.started && skillReady && characterClass.skillPoints>0) {
            skillReady = false;
            skillActive = true;
            var main = SkillParticles.main;
            main.startColor = characterClass.skillColor;
            SkillParticles.Play();
            currentCooldown=0;
            torchlight.color = characterClass.skillColor;
            switch(characterClass.classType) {
                case ClassType.Tank:
                    damageMitigation = characterClass.reductionMultiplier * characterClass.skillPoints;
                    break;
                default:
                    // Debug.Log("class does not affect player controller");
                    break;
            }
            StartCoroutine("SkillCooldown");
        }
    }
    #endregion


    #region Light System
    public void setTorchlight(float lightDistance, float lightAngle, GameObject spriteMask) {
        torchlight.pointLightInnerRadius = lightDistance*0.8f;
        torchlight.pointLightOuterRadius = lightDistance;
        torchlight.pointLightInnerAngle = lightAngle;
        torchlight.pointLightOuterAngle = lightAngle;
        torchLightController.Setup(lightDistance, lightAngle, characterClass);
        // float x = Mathf.Sin(lightAngle/360f*Mathf.PI)*lightDistance;
        // float y = Mathf.Cos(lightAngle/360f*Mathf.PI)*lightDistance;
        spriteMask.SetActive(true);
        // spriteMask.transform.localScale= new Vector3(x*3.5f,y/1.15f,1f);
        spriteMask.transform.localScale = new Vector3(lightDistance*0.83f,lightDistance*0.83f,1f);
    }        

    private void UseTorchlight() {
        BatteryToUse += BatteryUseRate * Time.deltaTime;
        int FlooredBatteryToUse = Mathf.FloorToInt( BatteryToUse );
        BatteryToUse -= FlooredBatteryToUse;
        Decharge( FlooredBatteryToUse );
    }

    public void Decharge(int amount) {
        if (!isDead && amount>0) {
            CurrentBattery -= amount;
            if (CurrentBattery <= (int) (MaxBattery * 0.5f)) {
                torchlight.intensity = 0.7f + (0.6f * CurrentBattery / MaxBattery);
            }
            if (CurrentBattery<=0) {
                CurrentBattery=0;
                torchCoroutine = StartCoroutine(TorchDead());
            }
            batteryBar.SetValue(CurrentBattery);
        }
    }

    public void Charge(int amount) {
        if (!isDead && amount>0) {
            CurrentBattery += amount;
            if (torchCoroutine!=null){
                StopCoroutine(torchCoroutine);
            }
            if (CurrentBattery <= (int) (MaxBattery * 0.5f)) {
                torchlight.intensity = 0.7f + (0.6f * CurrentBattery / MaxBattery);
            } else {
                torchlight.intensity= 1f;
            }
            if (CurrentBattery >= MaxBattery) {
                CurrentBattery = MaxBattery;
            }
            batteryBar.SetValue(CurrentBattery);
        }
    }

    IEnumerator TorchDead() {
        for(int i = 5; i > 0; i--) {
            torchlight.intensity = 0.5f;
            yield return new WaitForSeconds(Random.Range(0.01f,0.1f));
            torchlight.intensity = 0;
            yield return new WaitForSeconds(0.05f*i);
        }
        
    }

    #endregion


    #region Respawn System
    public void Die() {
        isDead=true;
        PlayerConfigurationManager.Instance.CountPlayerDeath();
        AudioManager.instance.Play(deathSFX[Random.Range(0, deathSFX.Length)]);
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

    public void Respawn() {
        isDead=false;
        PlayerConfigurationManager.Instance.CountPlayerRespawn();
        healthBar.SetMaxValue(MaxHealth);
        staminaBar.SetMaxValue(MaxStamina);
        batteryBar.SetMaxValue(MaxBattery);
        Heal(MaxHealth);
        Stim(MaxStamina);
        Charge(MaxBattery);
        deathPanel.SetActive(false);

        //activate body components
        bcollider.enabled=true;
    }
    #endregion
}

