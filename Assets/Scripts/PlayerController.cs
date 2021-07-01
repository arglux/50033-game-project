using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public List<GameObject> arsenal;
    public Text ammoText;
    
    private Vector2 movementVector;
    private Vector2 aimVector = Vector2.up;
    private Weapon weapon;
    private int weaponIndex = 0;
    private bool firing;

    void Start()
    {
        for (int i = 0; i < this.arsenal.Count; i++)
        {
            this.arsenal[i].SetActive(false);
            GameObject weap = Instantiate(this.arsenal[i], this.gameObject.transform);
            weap.transform.parent = this.gameObject.transform;
            weap.GetComponent<Weapon>().player = this;
            this.arsenal[i] = weap;
        }
        this.switchWeapon();
    }

    void Update()
    {
        this.moveHandler();
        if (this.firing) this.weapon.fireHandler(this.aimVector);
    }

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

    void moveHandler()
    {
        this.gameObject.transform.Translate(this.moveSpeed * (Vector3)this.movementVector * Time.deltaTime, Space.World);
        this.gameObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, (Vector3)this.aimVector);
        this.weapon.setFlipped(this.aimVector.x < 0);
    }

    void OnMove(InputValue input)
    {
        this.movementVector = input.Get<Vector2>();
    }

    void OnAim(InputValue input)
    {
        Vector2 inputVector = input.Get<Vector2>();
        if (inputVector.magnitude > 0.5)
        {
            this.aimVector = inputVector.normalized;
        }
    }

    void OnFire(InputValue input)
    {
        this.firing = input.Get<float>() > 0.5;
    }

    void OnNextWeapon(InputValue input)
    {
        if (input.isPressed)
        {
            this.weaponIndex++;
            if (this.weaponIndex >= this.arsenal.Count) this.weaponIndex = 0;
            this.switchWeapon();
        }
    }

    void OnPrevWeapon(InputValue input)
    {
        if (input.isPressed)
        {
            this.weaponIndex--;
            if (this.weaponIndex < 0) this.weaponIndex = this.arsenal.Count - 1;
            this.switchWeapon();
        }
    }

    void OnSpawn(InputValue input)
    {
        if (input.isPressed)
        {
            EnemyManager.SharedInstance.SpawnEnemy();
        }
    }

    void OnReload(InputValue input)
    {
        if (input.isPressed)
        {
            this.weapon.Reload();
        }
    }
}
