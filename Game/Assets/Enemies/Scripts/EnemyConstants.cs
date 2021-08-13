using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "EnemyAI/EnemyConstants")]
public class EnemyConstants : ScriptableObject
{
    [Header ("Enemy Health Stats")]
    public float maxHealth = 100;
    public float maxShield = 100;
    public float maxArmour = 100;

    public float healthMultiplier = 1.0f;

    [Header ("Enemy Movement Behaviour")]

    public float fastSpeed = 10f;
    public float normalSpeed = 5f;
    public float slowSpeed = 0.1f;


    [Header ("Enemy Attack Behaviour")]
    public AttackPattern attackPattern;
    public float stoppingDistance = 5f;
    public float cooldown = 0.3f;
    public float attackRange = 5f;
    public int attackRayCount = 9;
    public float attackConeAngle = 120f;
    public LayerMask attackLayerMask;

    [Header ("Enemy Vision Behaviour")]

    public float viewDistance = 7f;
    public int viewRayCount = 9;
    public float viewConeAngle = 120f;
    public LayerMask viewLayerMask;

}
