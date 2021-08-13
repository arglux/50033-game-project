using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "EnemyAI/Actions/Patrol")]
public class Patrol : Action
{
    public override void Act(StateController controller)
    {
        DoPatrol(controller);
    }

    private void DoPatrol(StateController controller)
    {
        controller.navMeshAgent.destination = controller.wayPointList[controller.nextWayPoint].position;
        controller.navMeshAgent.isStopped = false;
        controller.navMeshAgent.speed = controller.constants.normalSpeed;

        // Debug.Log(controller.navMeshAgent.remainingDistance);
        // Debug.Log(controller.navMeshAgent.stoppingDistance);
        // Debug.Log(controller.navMeshAgent.pathPending);
        if (controller.navMeshAgent.remainingDistance <= controller.navMeshAgent.stoppingDistance && !controller.navMeshAgent.pathPending) 
        {
            controller.nextWayPoint = (controller.nextWayPoint + 1) % controller.wayPointList.Count;
        }
    }
}