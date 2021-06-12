using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShootingController : MonoBehaviour {
    [Header ("References")]
    public Transform firePoint;
    public GameObject bullet1;

    public void Shoot(InputAction.CallbackContext context) {
        if (context.performed) {
            GameObject firedbullet = Instantiate(bullet1, firePoint.position, firePoint.rotation);
            firedbullet.GetComponent<Rigidbody2D>().AddForce(firePoint.up * 20f, ForceMode2D.Impulse);
        }
    }
}
