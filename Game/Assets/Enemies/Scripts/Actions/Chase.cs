using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "EnemyAI/Actions/Chase")]
public class Chase : Action 
{
    public override void Act(StateController controller)
    {
        DoChase(controller);    
    }

    private void DoChase(StateController controller)
    {
        controller.lastSeen.Set(controller.chaseTarget.position.x, controller.chaseTarget.position.y, 0);
        // // Debug.Log("Set Last Seen:");
        // // Debug.Log(controller.lastSeen);

        controller.navMeshAgent.destination = controller.chaseTarget.position;
        // controller.navMeshAgent.isStopped = false;
    }
}
