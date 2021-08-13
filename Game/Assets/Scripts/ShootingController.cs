using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ShootingController : MonoBehaviour
{
    [Header("Shooting System")]
    private GameObject[] arsenal;
    private Vector2 aimVector;
    private Vector3 playerScale;
    private bool facingRight;
    private bool isFirstShot = true;
    private Weapon weapon;
    private int weaponIndex = 0;
    private bool firing;
    private float damageMultiplier
    {
        get
        {
            if (playerController.skillActive && characterClass.classType == ClassType.DPS)
            {
                return 1.0f + characterClass.skillPoints * characterClass.damageMultiplier ;
            }
            else 
            {
                return 1.0f;
            }
        }
    }

    [Header("References")]
    public WeaponSwitcher stagedWeaponSwitch;
    public Text ammoText;
    private CharacterClass characterClass; 
    public GameObject playerSprite;
    public PlayerController playerController;

    #region Main
    void Awake()
    {
        arsenal = new GameObject[2];
        playerScale = playerSprite.transform.localScale;
    }

    void Update()
    {
        if (this.firing)
        {
            this.weapon.fireHandler(this.aimVector, this.damageMultiplier, this.isFirstShot);
            if (this.isFirstShot) this.isFirstShot = false;
        }
        else
        {
            this.isFirstShot = true;
        }
    }
    #endregion

    public void SetupClass(CharacterClass cc)
    {
        characterClass = cc;
        SetupWeapon(characterClass.weapon1, 0);
        SetupWeapon(characterClass.weapon2, 1);
    }

    void SetupWeapon(GameObject weap, int i)
    {
        weap.SetActive(false);
        weap = Instantiate(weap, this.gameObject.transform);
        weap.transform.parent = this.gameObject.transform;
        weap.GetComponent<Weapon>().ammoText = ammoText;
        // Debug.Log("SetupWeapon");
        // Debug.Log(this.arsenal);
        // // Debug.Log(weap);
        this.arsenal[i] = weap;
        switchWeapon();
    }

    #region Shooting
    void switchWeapon()
    {
        GameObject target = this.arsenal[this.weaponIndex];
        if (this.weapon != null)
        {
            this.weapon.gameObject.SetActive(false);
        }
        target.SetActive(true);
        this.weapon = target.GetComponent<Weapon>();
        this.weapon.setFlipped(!facingRight);
    }

    public void Aim(InputAction.CallbackContext context)
    {
        Vector2 inputVector = context.ReadValue<Vector2>();
        if (inputVector.magnitude > 0.5)
        {
            this.aimVector = inputVector.normalized;
        }
        this.gameObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, (Vector3)this.aimVector);
        facingRight = this.aimVector.x > 0;
        this.weapon.setFlipped(!facingRight);
        // flips the player sprite
        playerSprite.transform.localScale = new Vector3(facingRight ? playerScale.x : -playerScale.x, playerScale.y, playerScale.z);
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        float inputVector = context.ReadValue<float>();
        this.firing = inputVector > 0.5;
    }

    public void OnNextWeapon(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            this.weaponIndex++;
            if (this.weaponIndex >= this.arsenal.Length) this.weaponIndex = 0;
            this.switchWeapon();
        }
    }

    public void OnPrevWeapon(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            this.weaponIndex--;
            if (this.weaponIndex < 0) this.weaponIndex = this.arsenal.Length - 1;
            this.switchWeapon();
        }
    }

    public void OnReload(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            this.weapon.Reload();
        }
    }

    public void OnReplaceWeapon(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        GameObject weap = stagedWeaponSwitch.weapon;
        if (weap == null) return;

        stagedWeaponSwitch.PlaySound();

        int i = stagedWeaponSwitch.index;
        GameObject oldWeapon = this.arsenal[i];
        weap.SetActive(false);
        weap = Instantiate(weap, this.gameObject.transform);
        weap.transform.parent = this.gameObject.transform;
        weap.GetComponent<Weapon>().ammoText = ammoText;
        this.arsenal[i] = weap;
        Destroy(oldWeapon);
        switchWeapon();
    }

    // weap: Weapon prefab (e.g. Rifle)
    // i: index of arsenal -- 0 is primary, 1 is secondary
    public void OnReplaceWeapon(GameObject weap, int i)
    {
        stagedWeaponSwitch.PlaySound();
        GameObject oldWeapon = this.arsenal[i];
        weap.SetActive(false);
        weap = Instantiate(weap, this.gameObject.transform);
        weap.transform.parent = this.gameObject.transform;
        weap.GetComponent<Weapon>().ammoText = ammoText;
        this.arsenal[i] = weap;
        Destroy(oldWeapon);
        switchWeapon();
    }
    #endregion
}