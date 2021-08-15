using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "EnemyAI/Actions/Attack")]
public class Attack : Action 
{

    public override void Act(StateController controller)
    {
        DoAttack(controller);
    }

    private void DoAttack(StateController controller)
    {
        float angle = controller.constants.attackConeAngle * (controller.constants.attackRayCount - 1) / (controller.constants.attackRayCount) / 2; // offset
        float angleIncrement = controller.constants.attackConeAngle / controller.constants.attackRayCount;

        for (int i=0; i<controller.constants.attackRayCount; i++) {
            Vector3 direction = controller.navMeshAgent.velocity.normalized;
            float dirAngle = - GetAngle360(controller.navMeshAgent.velocity.normalized);

            Vector3 offset = GetVectorFromAngle(dirAngle + angle);

            Debug.DrawRay(controller.eyes.position, offset.normalized * controller.constants.attackRange, Color.red);
            RaycastHit2D hit = Physics2D.Raycast(controller.eyes.position, offset.normalized, controller.constants.attackRange, controller.constants.attackLayerMask);
            if (hit.collider != null) 
            {
                if (hit.collider.gameObject.CompareTag("Player") && 
                    (Vector3.Distance(controller.navMeshAgent.transform.position, controller.chaseTarget.position) < controller.constants.stoppingDistance)) 
                {
                    controller.navMeshAgent.speed = controller.constants.slowSpeed; // slowed down when attacking
                    // // Debug.Log((Vector3.Distance(controller.navMeshAgent.transform.position, controller.chaseTarget.position) < controller.constants.stoppingDistance));
                    // Debug.Log("BRUHHHHH FIREEEE");

                    // time += Time.deltaTime;
                    // if (time > controller.constants.cooldown)
                    // {
                    //     controller.constants.attackPattern.Fire(controller);
                    //     time = 0;
                    // }
                    if (controller.CheckIfCountDownElapsed(controller.constants.cooldown))
                    {
                        controller.constants.attackPattern.Fire(controller);
                    }

                } else
                {
                    controller.navMeshAgent.speed = controller.constants.normalSpeed;
                }
            }
            angle -= angleIncrement;
        }
    }

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
