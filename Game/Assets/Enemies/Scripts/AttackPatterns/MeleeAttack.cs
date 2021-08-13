using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "EnemyAI/AttackPatterns/MeleeAttack")]
public class MeleeAttack : AttackPattern
{
    public float attackRadius;
    public float baseDamage;
    public float animationDelay;
    public LayerMask playerLayer;
    // Start is called before the first frame update
    public override void Fire(StateController controller)
    {
        if (!(controller.blinded>0)) {
            DoMeleeAttack(controller);
        }
    }
    // Start is called before the first frame update
    void DoMeleeAttack(StateController controller)
    {
        controller.animator.SetTrigger("isAttacking");
        Collider2D[] playersHit = Physics2D.OverlapCircleAll(controller.hands.position, attackRadius, playerLayer);
        foreach(Collider2D player in playersHit) {
            if (player.CompareTag("Player")) {
                player.GetComponent<PlayerController>().TakeDamage(baseDamage);
            }
        }
        
    }

    void DrawHitBox(StateController controller)
    {
        if (controller.hands.position == null) return;
        Gizmos.DrawWireSphere(controller.hands.position, attackRadius);
    }
}
