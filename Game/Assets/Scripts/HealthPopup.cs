using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum DamageTypes {Heal, Damage};

public class HealthPopup : MonoBehaviour {   
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
    private float upSpeed = 2f;
    private float fadeSpeed = 5f;


    private void Awake() {
        textMesh = transform.GetComponent<TextMeshPro>();
        fadeTime = maxFadeTime;
    }

    public void setup(int amount, DamageTypes dtype) {
        textMesh.SetText(amount.ToString());
        if (dtype==DamageTypes.Damage) textMesh.color= new Color(1, 0, 0, 1);
        else if (dtype==DamageTypes.Heal) textMesh.color= new Color(0, 1, 0, 1);
        textColor = textMesh.color;
    }

    private void Update() {
        transform.position += new Vector3(0, upSpeed) * Time.deltaTime;
        
        if (fadeTime > maxFadeTime * 0.5f) {
            transform.localScale += Vector3.one * Time.deltaTime;
        } else {
            transform.localScale -= Vector3.one * Time.deltaTime;
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
