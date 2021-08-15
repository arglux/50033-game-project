using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum HealthType
{
    health,
    shield,
    armour
}

class HealthBar
{
    public HealthType healthType
    {
        get
        {
            return _healthType;
        }
        set
        {
            _healthType = value;
            barSlider.SetColourByType(value);
        }
    }
    public float currentValue
    {
        get
        {
            return _currentValue;
        }
        set
        {
            _currentValue = value;
            barSlider.SetFraction(Mathf.Clamp(_currentValue / maxValue, 0.0f, 1.0f));
        }
    }
    public bool isFirst
    {
        get
        {
            return _isFirst;
        }
        set
        {
            if (value)
            {
                barSlider.setHeightScale(1.0f);
            }
            else
            {
                barSlider.setHeightScale(0.4f);
            }
            _isFirst = value;
        }
    }
    public float maxValue;

    private float _currentValue;
    private bool _isFirst;
    private HealthType _healthType;

    private EnemyBarSlider barSlider;

    public HealthBar(GameObject sliderBar)
    {
        barSlider = sliderBar.GetComponent<EnemyBarSlider>();
    }

    public void setTransform(Vector3 trf)
    {
        barSlider.gameObject.transform.localPosition = trf;
    }

    public void setActive(bool active)
    {
        barSlider.gameObject.SetActive(active);
    }
}

public class EnemyHealthController : MonoBehaviour
{
    public EnemyConstants constants;
    public bool isEntrance;
    public float difficultyMultiplier = 1.35f;

    public GameObject barPrefab;
    public List<HealthType> healthBars;
    public UnityEvent onEntranceDeath;
    public bool isDead = false;

    public float healthDisplayHeight;

    private List<HealthBar> _healthBars;
    private Animator takeDamageAnimator;

    void Start()
    {
        takeDamageAnimator = GetComponentInParent<Animator>();
        ResetHealth();
    }

    void Update()
    {
        if (_healthBars.Count <= 0)
        {
            if (isEntrance && !isDead)
            {
                // Debug.Log("Spawner Dead");
                onEntranceDeath.Invoke();
                isDead = true;
                gameObject.GetComponent<BoxCollider2D>().enabled = false;

            } else if (isEntrance & isDead) {
               return; 
            } else
            {
                // Debug.Log("Enemy Dead");
                GetRootObject().SetActive(false);

                // reset healthbar
                ResetHealth();
            }

        } // else 
        // {
        //     // testing
        //     // Debug.Log("Spawner Not Dead");
        //     HealthBar bar = _healthBars[0];
        //     bar.currentValue -= 100;
        //     if (bar.currentValue <= 0)
        //     {
        //         _healthBars.RemoveAt(0);
        //     }
        // }
    }

    IEnumerator Blink(GameObject child)
    {
        Debug.Log(child);
        Color tmp = child.GetComponent<SpriteRenderer>().color;
        // Debug.Log(tmp.a);

        tmp.a = 255;
        // Debug.Log(tmp.a);
        yield return new WaitForSeconds (0.5f);
        tmp.a = 0;
        // Debug.Log(tmp.a);
        yield return new WaitForSeconds (0.5f);
    }

    public void TakeDamage(BulletController bullet)
    {
        if (_healthBars.Count <= 0) return;

        float effectiveDamage = RecursiveDamage(bullet, bullet.baseDamage);
        if (!isEntrance)
        {
            takeDamageAnimator.SetTrigger("isHit");
            Vector3 knockback = bullet.direction * effectiveDamage / 100;
            transform.parent.parent.Translate(new Vector3(Mathf.Min(1f,knockback.x), 0 , Mathf.Min(1f,knockback.y)));
        }
        HealthPopup.Create(transform.position, effectiveDamage, bullet.damageType);
        
        // foreach(Transform child in transform)
        // {
        //     StartCoroutine(Blink(child.gameObject));
        // }
    }

    float RecursiveDamage(BulletController bullet, float damage)
    {
        if (damage <= 0 || Mathf.Approximately(damage, 0.0f) || _healthBars.Count <= 0) return 0.0f;
        HealthBar bar = _healthBars[0];
        bar.isFirst = true;
        float multiplier = bullet.GetMultiplierFor(bar.healthType);
        float effectiveDamage = Mathf.Min(damage * multiplier, bar.currentValue);
        bar.currentValue -= effectiveDamage;
        damage -= effectiveDamage / multiplier;
        if (Mathf.Approximately(bar.currentValue, 0.0f) || bar.currentValue <= 0)
        {
            _healthBars.RemoveAt(0);
            bar.setActive(false);
            renderBarPositions();
            return effectiveDamage + RecursiveDamage(bullet, damage);
        }
        else
        {
            return effectiveDamage;
        }

    }

    private void ResetHealth()
    {
        _healthBars = new List<HealthBar>();
        bool firstAdded = false;
        for (int i = 0; i < healthBars.Count; i++)
        {
            HealthType type = healthBars[i];
            float maxHealth = 0.0f;
            switch (type)
            {
                case HealthType.health:
                    maxHealth = constants.maxHealth;
                    break;
                case HealthType.shield:
                    maxHealth = constants.maxShield;
                    break;
                case HealthType.armour:
                    maxHealth = constants.maxArmour;
                    break;
            }
            if (Mathf.Approximately(0.0f, maxHealth))
            {
                continue;
            }

            GameObject sliderBar = Instantiate(barPrefab, transform.position, transform.rotation);
            sliderBar.transform.SetParent(isEntrance ? this.transform : this.transform.parent.parent);

            HealthBar bar = new HealthBar(sliderBar);
            bar.maxValue = maxHealth;
            bar.healthType = type;
            int numPlayers = PlayerConfigurationManager.Instance.GetPlayerConfigs().Count;
            bar.maxValue *= Mathf.Pow(difficultyMultiplier, numPlayers);
            bar.currentValue = bar.maxValue;
            _healthBars.Add(bar);
            if (!firstAdded)
            {
                firstAdded = true;
                bar.isFirst = true;
            }
            else
            {
                bar.isFirst = false;
            }
        }

        renderBarPositions();
    }

    void renderBarPositions()
    {
        float minibarSeparation = 0.02f; // lmao this is such horrible code
        float mainbarSeparation = 0.03f;
        
        for (int i = 0; i < _healthBars.Count; i++)
        {
            HealthBar bar = _healthBars[i];
            float displayHeight;
            if (i == 0)
            {
                displayHeight = healthDisplayHeight;
            }
            else
            {
                displayHeight = healthDisplayHeight + (i - 1) * minibarSeparation + mainbarSeparation;
            }
            if (isEntrance)
            {
                bar.setTransform(new Vector3(0, displayHeight, 0));
            }
            else
            {
                bar.setTransform(new Vector3(0, 0, displayHeight));
            }

        }
    }

    private GameObject GetRootObject()
    {
        return gameObject.transform.parent.gameObject.transform.parent.gameObject;
    }

}
