using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    
    public float bulletSpeed = 100.0f;
    public bool IsActivated { get; set; }
    public float damage = 10.0f;
    public float lifeTime = 2.0f;
	
	private Rigidbody rb;
	private EnemyController enemyController;

    void Start() {
		rb = GetComponent<Rigidbody>();
        StartCoroutine(SetLifeTimeCor());
    }

    IEnumerator SetLifeTimeCor() {
        yield return new WaitForSeconds(lifeTime);
        DestroyBullet();
    }

    void Update() {
        if (IsActivated) {
            if (rb) {
                rb.velocity = transform.forward * bulletSpeed;
            }
        }
    }

    void OnCollisionEnter(Collision col) {
        if (col.gameObject.CompareTag("Enemy")) {
            enemyController = col.gameObject.GetComponent<EnemyController>();
            if (enemyController) {
                ContactPoint contact = col.contacts[0];
                Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
                Vector3 pos = contact.point;
                enemyController.InflictDamage(damage, pos, rot);
                FPSGameManager.Instance.SetScore();
                DestroyBullet();
            }

        }
    }

    void DestroyBullet() {
        this.gameObject.SetActive(false);
        ObjectPooler.Instance.ReturnToPool("Bullet", this.gameObject);
    }
}
