using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillController : MonoBehaviour {
    [Header ("Setup")]
    public string skillName;
    public Image skillCooldownOverlay;
    public Sprite skillImage;

    [Header ("Cooldown Time")]
    public float cooldownTime = 15f;
    private bool ready = true;
    private float currentTime = 0f;

    IEnumerator SkillCooldown() {
        while (currentTime<cooldownTime) {
            currentTime += Time.deltaTime;
            skillCooldownOverlay.fillAmount = currentTime/cooldownTime;
            yield return null;
        }
        skillCooldownOverlay.fillAmount=1; 
        ready=true;
        currentTime=0;
    }

    public void UseSkill() {
        if (ready) {
            ready=false;
            currentTime=0;
            StartCoroutine("SkillCooldown");
        }
    }
}
