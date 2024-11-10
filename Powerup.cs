using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3f;
    [SerializeField]
    private int powerupID;
    [SerializeField]
    private GameObject _ExplosionPrefab;
    private GameObject _Explosion;
    private float DestructionDelay = 2f;
    private UIManager _UIManager;
    private string _powerup;
    private SoundManager _SoundManager;

    void Start()
    {
        _SoundManager = GameObject.Find("Sound_Manager").GetComponent<SoundManager>();
    }
    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y <= -8)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.GetComponent<Player>();
        if (other.CompareTag("Player"))
        {
            if (player != null)
            {
                _SoundManager.PowerupSoundActive();
                switch (powerupID)
                {
                    case 0:
                        player.TripleShotActive();
                        Debug.Log("triple shot activated");
                        break;
                    case 1:
                        player.SpeedBoostActive();
                        Debug.Log("speed boosted");
                        break;
                    case 2:
                        player.ShieldActive();
                        Debug.Log("shield activated");
                        break;
                    default:
                        Debug.Log("default value");
                        break;
                }

            }
            Destroy(this.gameObject);
        }
        else if (other.CompareTag("Laser"))
        {
            HandlePowerupDestruction();
            Destroy(other.gameObject);
        }
    }

    private void HandlePowerupDestruction()
    {
        StoppingPowerup();
        ExplosionActivate();
        StartCoroutine(PowerupDestroyRoutine());
        this.gameObject.SetActive(false);
    }

    IEnumerator PowerupDestroyRoutine()
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

    private void StoppingPowerup()
    {
        _speed = 0;
    }
}
