using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Experimental.Rendering.Universal;


public class TorchLightController : MonoBehaviour {
    [Header ("Params")]
    public PlayerController playerController;
    public CharacterClass characterClass;
    private float lastContactTime = -1f;
    private float HealthToRegen = 0;
    private bool healing = false;

    [Header("Settings")]
    public bool tracking = false;
    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;
    public LayerMask targetMask;
    public LayerMask obstacleMask;
    public List<Transform> visibleTargets = new List<Transform>();
    private Coroutine torchCoroutine;

    private void OnEnable() {
        torchCoroutine = StartCoroutine("FindTargetWithDelay", 0.3f);
    }

    public void Setup(float vr, float va, CharacterClass cc) {
        viewRadius = vr;
        viewAngle = va;
        characterClass = cc;
    }

    // public void StartTorchSkill() {
    //     healing = true;
    //     lastContactTime=Time.time;
    // }
    
    // public void StopTorchSkill() {
    //     lastContactTime = -1f;
    //     visibleTargets.Clear();
    //     HealthToRegen = 0;
    //     healing = false;
    // }

    IEnumerator FindTargetWithDelay(float delay) {
        while (true) {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }
    
    void FindVisibleTargets() {
        visibleTargets.Clear();
        HealthToRegen += characterClass.skillPoints * characterClass.healPerSec * (Time.time - lastContactTime);
        int FlooredHealthToRegen = Mathf.FloorToInt( HealthToRegen );
        HealthToRegen -= FlooredHealthToRegen;
        lastContactTime = Time.time;
        Collider2D[] targetsInViewRadius = Physics2D.OverlapCircleAll(transform.position, viewRadius, targetMask);
        for (int i = 0; i < targetsInViewRadius.Length; i++) {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = new Vector3(target.position.x - transform.position.x, target.position.y - transform.position.y, 0f).normalized;
            if (Vector3.Angle(transform.up, dirToTarget) < viewAngle / 2) {
                float dstToTarget = Vector3.Distance(transform.position, target.position);
                if (!Physics2D.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask)) {
                    visibleTargets.Add(target);     
                    if (target.tag=="Enemy") {
                        var sc = target.GetComponentInParent<StateController>();
                        if (sc==null) {continue;}
                        sc.Aggro(gameObject.transform.parent.transform);
                        if (playerController.skillActive && characterClass.classType==ClassType.Tank) {
                            sc.InflictTaunted(gameObject.transform.parent.transform);
                        } else if (playerController.skillActive && characterClass.classType==ClassType.DPS) {
                            sc.InflictEnraged();
                        }
                    } else if (target.tag=="Player" && playerController.skillActive && characterClass.classType==ClassType.Support) {
                        target.GetComponent<PlayerController>().Heal(FlooredHealthToRegen);
                    }
                }
            }
        }
    }

    // for visualization
    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal) {
        if (!angleIsGlobal) {
            angleInDegrees -= transform.eulerAngles.z;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), 0);
    }

    
}