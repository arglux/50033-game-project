using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "EnemyAI/Decisions/Scan")]
public class Scan : Decision 
{
    public override bool Decide(StateController controller)
    {
        bool noEnemyInSight = DoScan(controller);
        return noEnemyInSight;
    }

    private bool DoScan(StateController controller)
    {
        controller.navMeshAgent.isStopped = true;
        // controller.transform.Rotate (0, controller.enemyStats.searchingTurnSpeed * Time.deltaTime, 0);
        return controller.CheckIfCountDownElapsed(5);
    }
}
