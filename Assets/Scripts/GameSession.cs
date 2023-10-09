using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameSession : MonoBehaviour
{
    [SerializeField] int playerLives = 3;
    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] float levelLoadDelay = 1f;

    [SerializeField] AudioClip deathSFX;
    [SerializeField] AudioClip gameOverSFX;


    public int playerScore = 0;
    void Awake() {
        int numGameSessions = FindObjectsOfType<GameSession>().Length;
        if(numGameSessions>1){
            Destroy(gameObject);
        }
        else{
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start() {
        livesText.text = playerLives.ToString();
        scoreText.text = playerScore.ToString();
    }

    public void ProcessPlayerDeath(){
        if(playerLives>1){
            GetComponent<AudioSource>().PlayOneShot(deathSFX);
            StartCoroutine(TakeLife());
        }
        else{
            GetComponent<AudioSource>().PlayOneShot(gameOverSFX);
            StartCoroutine(ResetGameSession());
        }
    }

    public void ProcessPoints(int score){
        playerScore+=score;
        scoreText.text = playerScore.ToString();
    }

    IEnumerator TakeLife()
    {
        yield return new WaitForSecondsRealtime(levelLoadDelay);
        playerLives--;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
        livesText.text = playerLives.ToString();
    }

    IEnumerator ResetGameSession()
    {
        yield return new WaitForSecondsRealtime(levelLoadDelay);
        FindObjectOfType<ScenePersist>().SceneDestroy();
        Destroy(gameObject);
        SceneManager.LoadScene(0);

    }
}
