using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "EnemyAI/Decisions/LookStraight")]
public class LookStraight : Decision {

    public float viewDistance = 10f;
    public LayerMask layerMask;

    public override bool Decide(StateController controller)
    {
        bool targetVisible = DoLook(controller);
        return targetVisible;
    }

    private bool DoLook(StateController controller)
    {
        Vector2 endPos = controller.eyes.position + Vector3.right * viewDistance;
        RaycastHit2D hit = Physics2D.Raycast((Vector2) controller.eyes.position, Vector2.right, viewDistance, layerMask);

        Debug.DrawRay(controller.eyes.position, Vector3.right * viewDistance, Color.green);

        // Physics.SphereCast (controller.eyes.position, controller.enemyStats.lookSphereCastRadius, controller.eyes.forward, out hit, controller.enemyStats.lookRange);

        if (hit.collider != null) {
            if (hit.collider.gameObject.CompareTag("Player")) {
                // // Debug.Log("Saw Player!");
                controller.chaseTarget = hit.transform;
                return true;
            }
            // // Debug.Log("Hit something");
            return false;
        } else 
        {
            // // Debug.Log("Hit Nothing");
            return false;
        }
        
    }
}
