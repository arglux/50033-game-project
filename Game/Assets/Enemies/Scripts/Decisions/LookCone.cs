using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "EnemyAI/Decisions/LookCone")]
public class LookCone : Decision {
    public override bool Decide(StateController controller)
    {
        bool targetVisible = DoLook(controller) || controller.aggro > 0;
        return targetVisible;
    }

    private bool DoLook(StateController controller)
    {
        float angle = controller.constants.viewConeAngle * (controller.constants.viewRayCount - 1) / (controller.constants.viewRayCount) / 2; // offset
        float angleIncrement = controller.constants.viewConeAngle / controller.constants.viewRayCount;

        // N arrays forming a cone
        for (int i=0; i<controller.constants.viewRayCount; i++) {
            // Vector3 vertex = controller.eyes.position + GetVectorFromAngle(angle) * controller.constants.viewDistance;
            // RaycastHit2D hit = Physics2D.Linecast(controller.eyes.position, vertex, controller.constants.viewLayerMask);

            Vector3 direction = controller.navMeshAgent.velocity.normalized;
            float dirAngle = - GetAngle360(controller.navMeshAgent.velocity.normalized);

            // // Debug.Log(dirAngle);
            // // Debug.Log(angle);
            // // Debug.Log(dirAngle + angle);
            // // Debug.Log(GetVectorFromAngle(dirAngle));

            Vector3 offset = GetVectorFromAngle(dirAngle + angle);
            
            Debug.DrawRay(controller.eyes.position, offset.normalized * controller.constants.viewDistance, Color.green);
            RaycastHit2D hit = Physics2D.Raycast(controller.eyes.position, offset.normalized, controller.constants.viewDistance, controller.constants.viewLayerMask);

            if (hit.collider != null) {
                if (hit.collider.gameObject.CompareTag("Player")) {
                    // // Debug.Log("Saw Player!");
                    if (!(controller.taunted>0)) {
                        controller.chaseTarget = hit.transform;
                    }
                    return true;
                }
            }
            angle -= angleIncrement;
        }
        return false;

    }

    // TODO: spin these out into utils static class
    private Vector3 GetVectorFromAngle(float angle) {
        float angleRad = angle * (Mathf.PI / 180f);
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }

    private float GetAngle360(Vector3 vect) {
        if (vect.y >= 0) {
            return (Vector3.Angle(Vector3.right, vect) - 360) * -1;
        } else {
            return Vector3.Angle(Vector3.right, vect);
        }
    }
}

