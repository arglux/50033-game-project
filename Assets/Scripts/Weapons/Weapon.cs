using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Weapon : MonoBehaviour
{
    public float rpm;
    public float bulletSpeed;
    public float baseDamage;
    public float bulletSpread = 0.0f;
    public float reloadTime;
    public int pierceCount = 1;
    public int magazineCapacity;
    public PlayerController player;

    private float lastShot;
    private float fireCooldown;
    private int inMagazine;
    private bool isReloading = false;
    private bool isFlipped = false;
    private AudioSource reloadSound;
    private SpriteRenderer sprite;

    void Awake()
    {
        this.fireCooldown = 1 / (this.rpm / 60);
        this.lastShot = Time.time - this.fireCooldown;
        this.inMagazine = this.magazineCapacity;
        this.sprite = this.gameObject.transform.Find("sprite").gameObject.GetComponent<SpriteRenderer>();
    }

    void OnEnable()
    {
        this.player.ammoText.text = $"{this.inMagazine} / {this.magazineCapacity}";
    }

    void OnDisable()
    {
        if (this.isReloading)
        {
            CancelInvoke("CompleteReload");
            this.isReloading = false;
            if (this.reloadSound != null) 
            {
                this.reloadSound.Stop();
            }
        }
    }

    public void setFlipped(bool flip)
    {
        if (flip == this.isFlipped) return;
        this.isFlipped = flip;
        this.sprite.flipY = this.isFlipped;
        Vector3 position = this.sprite.transform.localPosition;
        this.sprite.transform.localPosition = new Vector3(-position.x, position.y, position.z);
    }

    public void fireHandler(Vector2 aimVector)
    {
        if (this.isReloading) return;
        float time = Time.time;
        if (time - this.fireCooldown > this.lastShot)
        {
            this.lastShot = time;
            if (this.inMagazine <= 0)
            {
                AudioManager.SharedInstance.Play(AudioManager.SharedInstance.emptySound);
            }
            else {
                this.fire(aimVector);
                this.inMagazine--;
                Invoke("playCasing", 0.6f);
                this.player.ammoText.text = $"{this.inMagazine} / {this.magazineCapacity}";
                if (this.inMagazine == 0) this.Reload();
            }
        }
    }

    void playCasing()
    {
        AudioManager.SharedInstance.Play(AudioManager.SharedInstance.casingSound);
    }

    public void Reload()
    {
        if (this.isReloading) return;
        if (this.inMagazine == this.magazineCapacity) return;
        this.isReloading = true;
        this.reloadSound = this.playReload();
        this.player.ammoText.text = $"-- / {this.magazineCapacity}";
        Invoke("CompleteReload", this.reloadTime);
    }

    void CompleteReload()
    {
        if (!this.isReloading) return;
        this.isReloading = false;
        this.reloadSound = null;
        this.inMagazine = this.magazineCapacity;
        this.player.ammoText.text = $"{this.inMagazine} / {this.magazineCapacity}";
    }

    protected void spawnBullet(Vector2 offset, Vector2 direction)
    {
        GameObject item = ObjectPooler.SharedInstance.GetPooledObject(ObjectType.bullet);
        if (item != null)
        {
            BulletController bullet = item.GetComponent<BulletController>();
            bullet.position = this.transform.parent.position + (Vector3)offset;
            bullet.direction = (Vector3)Weapon.Rotate(direction, Random.Range(-this.bulletSpread, this.bulletSpread));
            bullet.moveSpeed = this.bulletSpeed;
            bullet.baseDamage = this.baseDamage;
            bullet.pierceCount = this.pierceCount;
            item.SetActive(true);
        }
    }

    public static Vector2 Rotate(Vector2 v, float angle)
    {
        angle = angle * Mathf.PI / 180;
        return new Vector2(
            v.x * Mathf.Cos(angle) - v.y * Mathf.Sin(angle),
            v.x * Mathf.Sin(angle) + v.y * Mathf.Cos(angle)
        );
    }

    protected abstract void fire(Vector2 aimVector);
    protected abstract AudioSource playReload();
}
