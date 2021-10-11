using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class gameController : MonoBehaviour
{
    //text, buttons, carFX, timer, and that kind of stuff
    public TMP_Text timerText;
    public TMP_Text scoreText;
    public Button eatButton;
    public Button flightButton;
    public carFxController carFX;

    private float startTime;
    private float currentTime;
    private float score;
    
    public int gameMode;
    public bool timerOn = false;
    void Start()
    {
        updateScore(0);
        startTime = Time.time + 25;
        currentTime = 25f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            currentTime += 10f;
            startTime += 10f;
        }
    }

    private void FixedUpdate()
    {
        if (currentTime > -0.1 && timerOn == true)
        {
            currentTime -=.03f;
            score -= .03f;
            timerText.text = currentTime.ToStringTime();
            updateScore(0);
        }
        else if (timerOn == true && currentTime < -0.1)
        {
            timerText.text = "00:00:00";
            carFX.waterboom();
            carFX.showDeathText();
        }

        if(score < 0)
        {
            score = 0;
        }

    }
    public void addTime(float time)
    {
        currentTime = Mathf.RoundToInt(currentTime) + time;
        startTime += currentTime;
    }

    public void eatEverything()
    {
        eatButton.SetActive(false);
        flightButton.SetActive(false);
        gameMode = 1;
        timerOn = true;
        currentTime = 25f;
        startTime = 25f;
    }
    public void fightMode()
    {
        eatButton.SetActive(false);
        flightButton.SetActive(false);
        gameMode = 2;
    }

    public void updateScore(int addScore)
    {
        score += addScore;
        scoreText.text = "SCORE: " + Mathf.RoundToInt(score);
    }
}
