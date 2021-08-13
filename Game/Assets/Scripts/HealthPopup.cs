using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum DamageTypes {Heal, DNeutral, DHealth, DShield, DArmour, DPlayer};

public class HealthPopup : MonoBehaviour {   
    public static HealthPopup Create(Vector3 position, float amount, DamageTypes dtype) {
        return Create(position, Mathf.RoundToInt(amount), dtype);
    }
    
    public static HealthPopup Create(Vector3 position, int amount, DamageTypes dtype) {
        Transform healthInstance = Instantiate(GameAssets.i.healthPrefab, position, Quaternion.identity);
        HealthPopup healthPopup = healthInstance.GetComponent<HealthPopup>();
        healthPopup.setup(amount, dtype);
        return healthPopup;
    }

    private const float maxFadeTime = 0.5f;
    private TextMeshPro textMesh;
    private float fadeTime;
    private Color textColor;
    private float speed = 2f;
    private float fadeSpeed = 5f;
    private float randDx;


    private void Awake() {
        textMesh = transform.GetComponent<TextMeshPro>();
        fadeTime = maxFadeTime;
        randDx = Random.Range(-speed,speed);
    }

    public void setup(int amount, DamageTypes dtype) {
        textMesh.SetText(amount.ToString());
        switch (dtype) {
            case DamageTypes.DHealth:
                textMesh.color = Color.red;
                break;
            case DamageTypes.DShield:
                textMesh.color = Color.blue;
                break;
            case DamageTypes.DArmour:
                textMesh.color = Color.yellow;
                break;
            case DamageTypes.DNeutral:
                textMesh.color = Color.white;
                break;
            case DamageTypes.DPlayer:
                textMesh.color = Color.red;
                break;
            case DamageTypes.Heal:
                textMesh.color = Color.green;
                break;
        }
        // if (dtype==DamageTypes.Damage) textMesh.color= new Color(1, 0, 0, 1);
        // else if (dtype==DamageTypes.Heal) textMesh.color= new Color(0, 1, 0, 1);
        textColor = textMesh.color;
    }

    private void Update() {
        if (fadeTime > maxFadeTime * 0.5f) {
            transform.localScale += Vector3.one * Time.deltaTime;
            transform.position += new Vector3(randDx, speed) * Time.deltaTime;
        } else {
            transform.localScale -= Vector3.one * Time.deltaTime;
            transform.position += new Vector3(randDx,-speed) * Time.deltaTime;
        }

        fadeTime -= Time.deltaTime;
        if (fadeTime <0) {
            textColor.a -= fadeSpeed * Time.deltaTime;
            textMesh.color = textColor;
            if (textColor.a < 0) {
                Destroy(gameObject);
            }
        }
    }
}
