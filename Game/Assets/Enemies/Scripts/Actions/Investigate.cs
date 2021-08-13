using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "EnemyAI/Actions/Investigate")]
public class Investigate : Action
{
    public override void Act(StateController controller)
    {
        DoInvestigate(controller);
    }

    private void DoInvestigate(StateController controller)
    {
        controller.navMeshAgent.destination = controller.lastSeen;
        // controller.navMeshAgent.isStopped = false;
        controller.navMeshAgent.speed = controller.constants.fastSpeed;
        // // Debug.Log("Investigating");
        // // Debug.Log(controller.lastSeen);
    }
}
