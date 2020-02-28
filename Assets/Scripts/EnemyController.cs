using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    public GameObject player;
    public ParticleSystem bloodEffect;
    public float speed = 2.0f;
    public float distance = 2.0f;
    public float damage = 10.0f;
    private PlayerController playerController;
    public int hunger;
    public float nextTime;
	public float maxHealth = 100.0f;
	
    private float currentHealth = 0.0f;
	private Renderer ren;

    void Start() {
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player");
		ren = GetComponent<Renderer>();
    }

    public void Red() {
       ren.material.color = Color.red;
    }

    public void Blue() {
        ren.material.color = Color.blue;
    }

    public void Black() {
        ren.material.color = Color.black;
    }

    void FixedUpdate() {
        // Calculate direction vector
        Vector3 dir = player.transform.position - gameObject.transform.position;
        // Normalize resultant vector to unit Vector
        dir = dir.normalized;
        gameObject.transform.LookAt(player.transform);
        // Move in the direction of the direction vector every frame 
        if (gameObject.transform.position.magnitude > distance) {
            gameObject.transform.position += dir * Time.deltaTime * speed;
        }
    }

    void Update() {
        if (playerController) {
            if (Time.time > nextTime) {
                
                playerController.InflictDamage(damage);

                nextTime = Time.time + 1;
            }
        }
    }

    void OnTriggerEnter(Collider col) {
        if (col.CompareTag("Player")) {
            playerController = col.gameObject.GetComponent<PlayerController>();
        }
    }

    public void InflictDamage(float damage, Vector3 pos, Quaternion rot) {
        if (currentHealth > 0) {
            PlayBloodEffect(pos, rot);
            currentHealth -= damage;
            
        }
        if (currentHealth < 0) {
            currentHealth = 0;
        }
        if (currentHealth == 0) {
            StartCoroutine(OnDeathCor());
        }
    }

    private void PlayBloodEffect(Vector3 pos, Quaternion rot) {
        ParticleSystem blood = Instantiate(bloodEffect, pos, rot) as ParticleSystem;
        blood.GetComponent<ParticleSystem>().Play();
    }

    IEnumerator OnDeathCor() {
        yield return new WaitForSeconds(1.0f);
        this.gameObject.SetActive(false);
        ObjectPooler.Instance.ReturnToPool("Enemy", this.gameObject);
    }

}
