using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "EnemyAI/AttackPatterns/ShootProjectile")]
public class ShootProjectile : AttackPattern
{
    public ObjectType bulletType;
    public float projectileForce = 80f;
    public int numOfProjectile = 3;
    public float delay = 1f;
    // private float time = 0f;
    
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
        for (int i=0; i<numOfProjectile; i++) 
        {
            // while (time < delay) 
            // {
            //     time += Time.deltaTime;
            //     Debug.Log(i);
            //     Debug.Log(time);
            // }

            GameObject item = ObjectPooler.SharedInstance.GetPooledObject(bulletType);
            if (item != null)
            {
                BulletController bullet = item.GetComponent<BulletController>();
                bullet.position = controller.hands.position;
                bullet.direction = GetProjectileDirection(controller);
                // TODO configure damage and speed of bullet
                // bullet.moveSpeed = 5.0f;
                // bullet.baseDamage = 1.0f;
                // bullet.pierceCount = 1;
                bullet.bulletType = BulletType.fromEnemy;
                controller.animator.SetTrigger("isAttacking");
                item.SetActive(true);
                // time = 0;
            }
            
            // GameObject projectile = Instantiate(projectilePrefab, controller.hands.position, Quaternion.identity);
            // Rigidbody2D projectileBody = projectile.GetComponent<Rigidbody2D>();
            // projectileBody.AddForce(GetProjectileDirection(controller) * projectileForce, ForceMode2D.Impulse);
        }
    }

    Vector3 GetProjectileDirection(StateController controller) 
    {
        Vector3 direction = controller.lastSeen - controller.navMeshAgent.transform.position;
        return direction.normalized;
    }

}
