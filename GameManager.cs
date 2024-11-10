using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private bool _isGameOver;
    [SerializeField]
    private int _countDownTime = 3;
    [SerializeField]
    private GameObject[] gameplayElement;
    private UIManager _UImanager;
    private SoundManager _SoundManager;

    private void Start()
    {
        _UImanager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _SoundManager = GameObject.Find("Sound_Manager").GetComponent<SoundManager>();
        
        foreach (GameObject element in gameplayElement)
        {
            element.SetActive(false);
        }

        StartCoroutine(StartAfterDelay());
        StartCoroutine(BackgroundMusic());
    }

    private void Update()
    {
        
    }

    public void GameOver()
    {
        _isGameOver = true;
    }

    public void Restart()
    {
        if (_isGameOver == true)
        {
            Debug.Log("Reloading scene...");
            SceneManager.LoadScene(1); //current game scene
        }
    }

    public void ExittoMainMenu()
    {
        if (_isGameOver == true)
        {
            Debug.Log("Exit to main menu scene...");
            SceneManager.LoadScene(0); //main menu scene
        }
    }

    IEnumerator StartAfterDelay()
    {
        for (int i = _countDownTime; i > 0; i--)
        {
            _UImanager.StartCountdownTime(i);
            yield return new WaitForSeconds(1);
        }
        _UImanager.EndCountdown();
        _UImanager.StartCountdownText("START");
        yield return new WaitForSeconds(1);
        _UImanager.EndCountdown();

        foreach (GameObject element in gameplayElement)
        {
            element.SetActive(true);
        }
    }

    IEnumerator BackgroundMusic()
    {
        while (_isGameOver == false)
        {
            _SoundManager.BackgroundMusicActive();
            yield return new WaitForSeconds(13);
        }
    }
}
