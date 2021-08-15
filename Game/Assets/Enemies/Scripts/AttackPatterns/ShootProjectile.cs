using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "EnemyAI/AttackPatterns/ShootProjectile")]
public class ShootProjectile : AttackPattern
{
    public ObjectType bulletType;
    // public float projectileForce = 80f;
    public float delay = 1f;
    
    public override void Fire(StateController controller)
    {
        if (!(controller.blinded>0)) {
            FireProjectile(controller);
        }
    }
    // Start is called before the first frame update
    void FireProjectile(StateController controller)
    {
        // // Debug.Log("Firing Projectiles!");
        GameObject item = ObjectPooler.SharedInstance.GetPooledObject(bulletType);
        if (item != null)
        {
            BulletController bullet = item.GetComponent<BulletController>();
            bullet.position = controller.hands.position;
            bullet.direction = GetProjectileDirection(controller);
            bullet.bulletType = BulletType.fromEnemy;
            controller.animator.SetTrigger("isAttacking");
            item.SetActive(true);
            // time = 0;
        }
    }

    Vector3 GetProjectileDirection(StateController controller) 
    {
        Vector3 direction = controller.lastSeen - controller.navMeshAgent.transform.position;
        return direction.normalized;
    }

}
