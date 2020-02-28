using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class FPSGameManager : MonoBehaviour{
	
	public static FPSGameManager Instance { get { return _instance; } }
	private static FPSGameManager _instance;

    public GameObject gameOverPanel;
    public GameObject hudPanel;
    public float reloadDelay = 5.0f;
    public TextMeshProUGUI scoreValue;
    public TextMeshProUGUI yourScoreValue;
    public int score;
	
	private Animator anim;

    void Awake() {
        //DontDestroyOnLoad(gameObject);
		anim = scoreValue.GetComponent<Animator>();
		
		if (_instance != null && _instance != this){
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }

    void OnEnable() {
        ManagePanels(false,true);
        score = 0;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Q)) {
            Application.Quit();
        }
    }

    public void ManagePanels(bool isGameOverPanel, bool isHUDPanel) {
        gameOverPanel.SetActive(isGameOverPanel);
        hudPanel.SetActive(isHUDPanel);
    }

    public void SetGameTime(float timeScaleCur) {
        Time.timeScale = timeScaleCur;
    }
    
    public void ReloadGame() {
        StartCoroutine(ReloadGameCor());
    }

    IEnumerator ReloadGameCor() {
        yield return new WaitForSeconds(reloadDelay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SetScore() {
        score += 10;
        scoreValue.text = score.ToString();
        
        if (anim) {
            anim.SetTrigger("IsAnimate");
        }
        yourScoreValue.text = score.ToString();
    }
}
