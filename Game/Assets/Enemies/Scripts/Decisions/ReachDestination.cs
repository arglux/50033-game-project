using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "EnemyAI/Decisions/ReachDestination")]
public class ReachDestination : Decision {

    public float timeout = 3f;
    public override bool Decide(StateController controller)
    {
        if (controller.navMeshAgent.remainingDistance <= controller.navMeshAgent.stoppingDistance && !controller.navMeshAgent.pathPending) {
            controller.stuckTime = 0f; // reset
            return true;
        }
        if (Vector3.Distance(controller.lastPosition, controller.navMeshAgent.transform.position) < 0.1f) {
            controller.stuckTime += Time.deltaTime;
        }
        if (controller.stuckTime >= timeout) {
            controller.stuckTime = 0f; // reset
            return true; // stuck, try to go back to patrolling
        }

        controller.lastPosition = controller.navMeshAgent.transform.position;
        
        return false;
    }
}
