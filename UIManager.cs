using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    // handle to text
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Text _powerupText;
    [SerializeField]
    private Sprite[] _livesprites;
    [SerializeField]
    private Image _livesImage;
    [SerializeField]
    private GameObject _gameOver;
    [SerializeField]
    private float _flickerDuration = 10f;
    [SerializeField]
    private float _flickerInterval = 0.5f;
    private float _elapsedTime = 0;
    private GameManager _gameManager;
    [SerializeField]
    private Button _restartButton;
    [SerializeField]
    private Button _mainMenuButton;
    [SerializeField]
    private Text _countDownText1;
    [SerializeField]
    private Text _countDownText2;
    // Start is called before the first frame update
    void Start()
    {
       _gameOver.SetActive(false);
       _restartButton.gameObject.SetActive(false);
       _mainMenuButton.gameObject.SetActive(false);
       _countDownText1.gameObject.SetActive(false); 
       _countDownText2.gameObject.SetActive(false);
       _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();

        if (_gameManager == null)
        {
            Debug.LogError("Game_Manager is NULL");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateScore(int score)
    {
        _scoreText.text = "Score: " + score.ToString();
    }

    public void updateLIfe(int life)
    {
        _livesImage.sprite = _livesprites[life];
    }

    public void updatePowerup(string powerup) 
    {
        _powerupText.text = "Powerup: " + powerup;
    }

    public void UpdateGameOver()
    {
        StartCoroutine(GameOver());
        _restartButton.gameObject.SetActive(true);
        _mainMenuButton.gameObject .SetActive(true);
        _gameManager.GameOver();
    }

    public void StartCountdownTime(int countDownTime)
    {
        _countDownText1.gameObject .SetActive(true);
        _countDownText1.text = countDownTime.ToString();
    }

    public void StartCountdownText(string countDownText)
    {
        _countDownText2.gameObject.SetActive(true);
        _countDownText2.text += countDownText;
    }

    public void EndCountdown()
    {
        _countDownText1.gameObject.SetActive(false);
        _countDownText2.gameObject.SetActive(false);
    }


    IEnumerator GameOver()
    {
       while (_elapsedTime <= _flickerDuration)
        {
            _gameOver.SetActive(!_gameOver.activeSelf);
            yield return new WaitForSeconds(_flickerInterval);
            _elapsedTime += _flickerInterval;
        }

       _gameOver.SetActive(true);
    }
}
