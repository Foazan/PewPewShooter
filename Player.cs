using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private float _speed = 5f;
	[SerializeField]
	private GameObject _laserPrefab;
	[SerializeField]
	private float _laserOffset = 0.8f;
	[SerializeField]
	private float _canFire = -1f;
	[SerializeField]
	private float _fireRate = 0.2f;
	[SerializeField]
	private int _life = 3;
	private SpawnManager _spawnManager;
	[SerializeField]
	private GameObject _tripleShotPrefab;
	[SerializeField]
	private bool _canMove = true;
	[SerializeField]
	private bool _isTripleShotActive = false;
	[SerializeField]
	private bool _isSpeedBoostActive = false;
	[SerializeField]
	private bool _isShieldActive = false;
	private GameObject _Shield;
	private GameObject _Player_Hurt;
	[SerializeField]
	private GameObject _ExplosionPrefab;
	private GameObject _Explosion;
	private float DestructionDelay = 2f;
	[SerializeField]
	private int _score;
	private UIManager _UI_Manager;
	[SerializeField]
	private string _poweruptext;
	private SoundManager _SoundManager;
    void Start()
    {
		//take the current position = new psoition (0, 0, 0)
		transform.position = new Vector3(0, 0, 0);

		_spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
		_UI_Manager = GameObject.Find("Canvas").GetComponent<UIManager>();
		_SoundManager = GameObject.Find("Sound_Manager").GetComponent<SoundManager>();
		_Shield = GameObject.Find("Shields");
		_Player_Hurt = GameObject.Find("Fire");
		_Shield.SetActive(false);
		_Player_Hurt.SetActive(false);
        _score = 0;
        UpdateUI();

        if (_spawnManager == null)
		{
			Debug.LogError("The Spawn Manager is NULL.");
		}

        if (_UI_Manager == null)
        {
			Debug.LogError("The UI Manager is NULL");
        }
    }

    // Update is called once per frame
    void Update()
    {
		CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
		{
			shootLaser();
		}

		PowerupSwitching();
            
        
    }

    void CalculateMovement()
    {
		float horizontalInput = Input.GetAxis("Horizontal");
		float verticalInput = Input.GetAxis("Vertical");

        if (_canMove == true)
        {
            transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * _speed * Time.deltaTime);
        }
        
        if (transform.position.y >= 7)
		{
			transform.position = new Vector3(transform.position.x, 7, 0);
		}
		else if (transform.position.y <= -7)
		{
			transform.position = new Vector3(transform.position.x, -7, 0);
		}

		if (transform.position.x >= 12)
		{
			transform.position = new Vector3(-12, transform.position.y, 0);
		}
		else if (transform.position.x <= -12)
		{
			transform.position = new Vector3(12, transform.position.y, 0);
		}
	}

	void shootLaser()
	{
		_canFire = Time.time + _fireRate;

		if (_isTripleShotActive == true)
		{
			Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
		}
		else
		{
			Instantiate(_laserPrefab, transform.position + new Vector3(0, _laserOffset, 0), Quaternion.identity);
		}

		_SoundManager.LaserSoundActive();
	}

	public void Damage()
	{
		if (_isShieldActive == true)
		{
			_isShieldActive = false;
			_Shield.SetActive(false);
			return;
		}

		else
		{
			_life--;
			UpdateUI();
		}

        if (_life == 1)
        {
			_Player_Hurt.SetActive(true);
        }

        if (_life <= 0)
		{
			DisableMovement();
			_spawnManager.onPlayerDeath();
			ExplosionActivate();
			StartCoroutine(PlayerDeathRoutine());
            this.gameObject.SetActive(false);
            _UI_Manager.UpdateGameOver();
        }
	}

	public void TripleShotActive()
	{ 
		_isTripleShotActive = true;
		StartCoroutine(TripleShotPowerDownRoutine());
	}

	IEnumerator TripleShotPowerDownRoutine()
	{
		yield return new WaitForSeconds(5);
        _isTripleShotActive = false;
    }

	public void SpeedBoostActive()
	{
		_isSpeedBoostActive = true;
		_speed = 10f;
		StartCoroutine(SpeedBoostDownRoutine());
	}

	IEnumerator SpeedBoostDownRoutine()
	{
		yield return new WaitForSeconds(5);
		_isSpeedBoostActive = false;
		_speed = 5f;
	}

	public void ShieldActive()
	{
		_isShieldActive = true;
		_Shield.SetActive(true);
		StartCoroutine(ShieldPowerDownRoutine());
	}

    IEnumerator ShieldPowerDownRoutine()
	{
		yield return new WaitForSeconds(5);
		_isShieldActive = false;
		_Shield.SetActive(false);
	}

	IEnumerator PlayerDeathRoutine()
	{
		yield return new WaitForSeconds(DestructionDelay);
		Destroy(this.gameObject);
    }

	private void ExplosionActivate()
	{
        _Explosion = Instantiate(_ExplosionPrefab, transform.position, Quaternion.identity);
		_SoundManager.ExplosionSoundActive();
		Destroy(_Explosion, DestructionDelay);
    }


	private void DisableMovement()
	{
		_canMove = false;
	}

	private void EnableMovement()
	{
		_canMove = true;
	}

	public void Scorring(int points)
	{
		_score += points;
		UpdateUI();
	}

	private void PowerupSwitching()
	{
        if (_isTripleShotActive == true)
        {
			_poweruptext = "Triple Shot";
			UpdateUI();
        }
		else if (_isSpeedBoostActive == true)
        {
			_poweruptext = "Speed Boost";
			UpdateUI();
        }
		else if (_isShieldActive ==  true)
        {
			_poweruptext = "Shield";
			UpdateUI();
        }
        else
        {
			_poweruptext = "None";
			UpdateUI();
        }
    }

	public void UpdateUI()
	{
		_UI_Manager.updateScore(_score);
		_UI_Manager.updateLIfe(_life);
		_UI_Manager.updatePowerup(_poweruptext);
	}

}