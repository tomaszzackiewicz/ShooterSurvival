using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemyManager : MonoBehaviour{
	
    public List<GameObject> spawnPoints = new List<GameObject>();
    void Start(){
        InvokeRepeating("SpawnEnemy", 0, 5);
    }

    void SpawnEnemy() {
        foreach (GameObject sp in spawnPoints) {
            ObjectPooler.Instance.SpawnFromPool("Enemy", sp.transform.position, sp.transform.rotation);
        }
    }
}
