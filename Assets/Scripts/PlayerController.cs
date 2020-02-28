using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Image bar;
    public Transform bulletSpawnPoint;
    public ParticleSystem fireEffect;
    public GameObject healthBar;
    public float maxHealth = 100.0f;
    
    private float currentHealth = 0.0f;
    private AudioSource audioSource;
    private bool isDead = false;

    private const float DIVIDER = 100.0f;

    void Awake() {
        audioSource = GetComponent<AudioSource>();
    }
    void Start(){
        currentHealth = maxHealth;
        bar.fillAmount = currentHealth / DIVIDER;
    }
    
    void Update(){
        if (!isDead) {
            if (Input.GetMouseButtonDown(0)) {
                PlayGunFireEffect(bulletSpawnPoint.transform.position, bulletSpawnPoint.transform.rotation);
                PlayGunSoundEffect();
                ShootRaycast();

                //IF YOU USE BULLETS
                //GameObject bullet = ObjectPooler.Instance.SpawnFromPool("Bullet", bulletSpawnPoint.transform.position, bulletSpawnPoint.transform.rotation);
                //if (bullet) {
                //    bullet.GetComponent<BulletController>().IsActivated = true;
                //}

            }
        }
    }

    private void PlayGunFireEffect(Vector3 pos, Quaternion rot) {
        ParticleSystem ps = Instantiate(fireEffect, pos, rot) as ParticleSystem;
        ps.Play();
    }

    private void PlayGunSoundEffect() {
        audioSource.Play();
    }

    void ShootRaycast() {
        RaycastHit hit;
        if (Physics.Raycast(bulletSpawnPoint.transform.position, bulletSpawnPoint.transform.forward, out hit, 1000.0f)) {
           if(hit.transform.gameObject.CompareTag("Enemy")) {
                hit.transform.gameObject.GetComponent<EnemyController>().InflictDamage(10.0f, hit.point, Quaternion.identity);
                FPSGameManager.Instance.SetScore();
            }
        }
    }

    public void InflictDamage(float damage) {
        if (currentHealth > 0) {
            currentHealth -= damage;
            Animator anim = healthBar.GetComponent<Animator>();
            if (anim) {
                anim.SetTrigger("IsAnimated");
            }
            bar.fillAmount = currentHealth / DIVIDER;
            if (currentHealth < 0) {
                currentHealth = 0;
            }
            if (currentHealth == 0) {
                OnDeath();
            }
        }
    }

    private void OnDeath() {
        isDead = true;
        FPSGameManager.Instance.ManagePanels(true, false);
        //FPSGameManager.Instance.SetGameTime(0.0f);
        FPSGameManager.Instance.ReloadGame();
    }
}
