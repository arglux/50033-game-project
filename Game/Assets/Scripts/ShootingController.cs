using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ShootingController : MonoBehaviour {
    [Header ("Player State Trackers")]
    public bool isDead = false;
    public bool isDodging = false;

    [Header ("Shooting System")]
    public List<GameObject> arsenal;
    private Vector2 aimVector;
    private Weapon weapon;
    private int weaponIndex = 0;
    private bool firing;

    [Header ("References")]
    public Text ammoText;

    #region Main
    void Start()
    {
        for (int i = 0; i < this.arsenal.Count; i++)
        {
            this.arsenal[i].SetActive(false);
            GameObject weap = Instantiate(this.arsenal[i], this.gameObject.transform);
            weap.transform.parent = this.gameObject.transform;
            weap.GetComponent<Weapon>().ammoText = ammoText;
            this.arsenal[i] = weap;
        }
        this.switchWeapon();
    }

    void Update()
    {
        if (this.firing) this.weapon.fireHandler(this.aimVector);
    }
    #endregion

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
    }

    public void Aim(InputAction.CallbackContext context) {
        Vector2 inputVector  = context.ReadValue<Vector2>();
        if (inputVector.magnitude > 0.5) {
            this.aimVector = inputVector.normalized;
        }
        this.gameObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, (Vector3)this.aimVector);
        this.weapon.setFlipped(this.aimVector.x < 0);
        
    }

    public void OnFire(InputAction.CallbackContext context) {
        float inputVector  = context.ReadValue<float>();
        this.firing = inputVector > 0.5;
    }

     public void OnNextWeapon(InputAction.CallbackContext context) {
        if (context.started) {
            this.weaponIndex++;
            if (this.weaponIndex >= this.arsenal.Count) this.weaponIndex = 0;
            this.switchWeapon();
        }
    }

    public void OnPrevWeapon(InputAction.CallbackContext context) {
        if (context.started) {
            this.weaponIndex--;
            if (this.weaponIndex < 0) this.weaponIndex = this.arsenal.Count - 1;
            this.switchWeapon();
        }
    }

    public void OnReload(InputAction.CallbackContext context) {
        if (context.started) {
            this.weapon.Reload();
        }
    }
    #endregion
}