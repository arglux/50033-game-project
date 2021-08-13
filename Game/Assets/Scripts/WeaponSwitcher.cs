using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeaponSwitcher : MonoBehaviour
{
    public GameObject weapon;
    public int index;
    public AudioSource switchAudio;
    public TextMeshPro tooltip;

    public float hoverFrequency = 1.0f;
    public float hoverMagnitude = 1.0f;

    private GameObject weaponSprite;

    void Start()
    {
        weaponSprite = Instantiate(weapon.transform.Find("sprite").gameObject.GetComponent<SpriteRenderer>()).gameObject;
        weaponSprite.transform.parent = this.transform;
        weaponSprite.transform.localPosition = new Vector3(0, 0, 0);
        tooltip.SetText("");
    }

    void LateUpdate()
    {
        float displacement = Mathf.Sin(hoverFrequency * Time.time) * hoverMagnitude;
        weaponSprite.transform.localPosition = new Vector3(0, displacement, 0);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            ShootingController shootingController = col.gameObject.transform.Find("Weapon").GetComponent<ShootingController>();
            shootingController.stagedWeaponSwitch = this;
            tooltip.SetText(weapon.name);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            ShootingController shootingController = col.gameObject.transform.Find("Weapon").GetComponent<ShootingController>();
            if (GameObject.ReferenceEquals(this, shootingController.stagedWeaponSwitch)) shootingController.stagedWeaponSwitch = null;
            tooltip.SetText("");
        }
    }

    public void PlaySound()
    {
        AudioManager.instance.Play(switchAudio.clip);
    }
}
