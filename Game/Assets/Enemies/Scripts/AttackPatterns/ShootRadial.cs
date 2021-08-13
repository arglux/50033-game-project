using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "EnemyAI/AttackPatterns/ShootRadial")]
public class ShootRadial : AttackPattern
{
    // Start is called before the first frame update
    public ObjectType bulletType;
    public float projectileForce = 50f;
    public int numOfProjectile = 8;
    public override void Fire(StateController controller)
    {
        FireProjectile(controller);
    }
    // Start is called before the first frame update
    void FireProjectile(StateController controller)
    {
        if (controller.aggro>0) {
            float angle = 0;
            float angleIncrement = 360 / numOfProjectile;
            for (int i=0; i<numOfProjectile; i++) {
                GameObject item = ObjectPooler.SharedInstance.GetPooledObject(bulletType);
                Vector3 offset = GetVectorFromAngle(angle);
                // // Debug.Log(angle);
                // // Debug.Log(offset);

                if (item != null)
                {
                    BulletController bullet = item.GetComponent<BulletController>();
                    bullet.position = controller.eyes.position;
                    bullet.direction = offset;

                    // TODO configure damage and speed of bullet
                    // bullet.moveSpeed = 5.0f;
                    // bullet.baseDamage = 1.0f;
                    // bullet.pierceCount = 1;
                    bullet.bulletType = BulletType.fromEnemy;
                    controller.animator.SetTrigger("isAttacking");
                    item.SetActive(true);
                }
                angle -= angleIncrement;
            }
        } else {
            GameObject item = ObjectPooler.SharedInstance.GetPooledObject(bulletType);
            if (item != null) {
                BulletController bullet = item.GetComponent<BulletController>();
                bullet.position = controller.hands.position;
                bullet.direction = GetProjectileDirection(controller);
                bullet.bulletType = BulletType.fromEnemy;
                controller.animator.SetTrigger("isAttacking");
                item.SetActive(true);
            }
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

    Vector3 GetProjectileDirection(StateController controller) 
    {
        Vector3 direction = controller.lastSeen - controller.navMeshAgent.transform.position;
        return direction.normalized;
    }
}
