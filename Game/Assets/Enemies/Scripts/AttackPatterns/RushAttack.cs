using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "EnemyAI/AttackPatterns/RushAttack")]
public class RushAttack : AttackPattern
{
    public float delay = 2f;
    private float time;
    public override void Fire(StateController controller)
    {
        Debug.Log(controller.constants.slowSpeed);
        controller.navMeshAgent.speed = controller.constants.slowSpeed;
        DoRushAttack(controller);
    }
    // Start is called before the first frame update
    void DoRushAttack(StateController controller)
    {
        Debug.Log("Charging!");
        time = 0;
        
        while (time < delay) { // charging
            time += Time.deltaTime;
        }
        Debug.Log("Rushing!");
        time = 0;
        controller.navMeshAgent.destination = controller.lastSeen;
        controller.navMeshAgent.isStopped = false;
        controller.navMeshAgent.speed = controller.constants.fastSpeed;
        
    }
}
