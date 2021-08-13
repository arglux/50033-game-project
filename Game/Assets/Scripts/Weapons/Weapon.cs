using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

abstract public class Weapon : MonoBehaviour
{
    public float rpm;
    public float bulletSpeed;
    public float baseDamage;
    public float bulletSpread = 0.0f;
    public float reloadTime;
    public int pierceCount = 1;
    public int magazineCapacity;
    public float spawnDistance = 1.0f;

    public Sprite bulletSprite;
    // public PlayerController player;

    private float damageMultiplier = 1.0f;
    public float healthMultiplier = 1.0f;
    public float armourMultiplier = 1.0f;
    public float shieldMultiplier = 1.0f;

    public DamageTypes damageType;

    public AudioSource fireSource;
    public AudioSource reloadSource;
    public AudioSource casingSource;
    public AudioSource emptySource;

    private float lastShot;
    private float fireCooldown;
    private int inMagazine;
    private bool isReloading = false;
    private bool isFlipped = false;
    private SpriteRenderer sprite;
    private Vector3 weaponScale;

    [Header ("References")]
    public Text ammoText;

    void Awake()
    {
        this.fireCooldown = 1 / (this.rpm / 60);
        this.lastShot = Time.time - this.fireCooldown;
        this.inMagazine = this.magazineCapacity;
        this.sprite = this.gameObject.transform.Find("sprite").gameObject.GetComponent<SpriteRenderer>();
        weaponScale = sprite.transform.localScale;
        damageType = GetDamageType();
    }

    void OnEnable()
    {
        ammoText.text = $"{this.inMagazine} / {this.magazineCapacity}";
    }

    void OnDisable()
    {
        if (this.isReloading)
        {
            CancelInvoke("CompleteReload");
            this.isReloading = false;
            this.reloadSource.Stop();
        }
    }

    DamageTypes GetDamageType()
    {
        float[] multipliers = new float[] { healthMultiplier, armourMultiplier, shieldMultiplier };
        DamageTypes[] dtypes = new DamageTypes[] { DamageTypes.DHealth, DamageTypes.DArmour, DamageTypes.DShield };
        int index = -1;
        float max = -1.0f;
        for (int i = 0; i < multipliers.Length; i++)
        {
            float mult = multipliers[i];
            if (mult > max)
            {
                if (!Mathf.Approximately(max, mult))
                {
                    index = i;
                    max = mult;
                }
            }
            else if (Mathf.Approximately(max, mult))
            {
                index = -1;
            }
        }
        return (index == -1) ? DamageTypes.DNeutral : dtypes[index];
    }

    public void setFlipped(bool flip)
    {
        if (flip == this.isFlipped) return;
        this.isFlipped = flip;
        this.sprite.flipY = this.isFlipped;
        Vector3 position = this.sprite.transform.localPosition;
        this.sprite.transform.localPosition = new Vector3(-position.x, position.y, position.z);
        // sprite.transform.localScale = new Vector3( weaponScale.x, isFlipped ? -weaponScale.y : weaponScale.y, weaponScale.z);
    }

    public void fireHandler(Vector2 aimVector, float damageMultiplier, bool isFirstShot)
    {
        this.damageMultiplier = damageMultiplier;
        if (this.isReloading) return;
        float time = Time.time;
        if (time - this.fireCooldown > this.lastShot)
        {
            this.lastShot = time;
            if (this.inMagazine <= 0)
            {
                if (isFirstShot) AudioManager.instance.Play(emptySource.clip);
            }
            else {
                this.fire(aimVector);
                AudioManager.instance.Play(fireSource.clip);
                this.inMagazine--;
                Invoke("playCasing", 0.6f);
                ammoText.text = $"{this.inMagazine} / {this.magazineCapacity}";
                if (this.inMagazine == 0) this.Reload();
            }
        }
    }

    void playCasing()
    {
        AudioManager.instance.Play(casingSource.clip);
    }

    public void Reload()
    {
        if (this.isReloading) return;
        if (this.inMagazine == this.magazineCapacity) return;
        this.isReloading = true;
        reloadSource.Play();
        ammoText.text = $"-- / {this.magazineCapacity}";
        Invoke("CompleteReload", this.reloadTime);
    }

    void CompleteReload()
    {
        if (!this.isReloading) return;
        this.isReloading = false;
        reloadSource.Stop();
        this.inMagazine = this.magazineCapacity;
        ammoText.text = $"{this.inMagazine} / {this.magazineCapacity}";
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
            bullet.baseDamage = this.baseDamage * this.damageMultiplier;
            bullet.pierceCount = this.pierceCount;
            bullet.healthMultiplier = this.healthMultiplier;
            bullet.shieldMultiplier = this.shieldMultiplier;
            bullet.armourMultiplier = this.armourMultiplier;
            bullet.damageType = this.damageType;
            bullet.bulletType = BulletType.fromPlayer;
            if (bulletSprite)
            {
                SpriteRenderer bulletSR = bullet.transform.Find("sprite").gameObject.GetComponent<SpriteRenderer>();
                bulletSR.sprite = bulletSprite;
            }
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

    protected virtual void fire(Vector2 aimVector)
    {
        this.spawnBullet(spawnDistance * aimVector, aimVector);
    }
}
