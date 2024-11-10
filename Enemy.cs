using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed;
    [SerializeField]
    private GameObject _ExplosionPrefab;
    private GameObject _Explosion;
    [SerializeField]
    private float DestructionDelay = 2f;
    private Player _player;
    private SoundManager _soundManager;
    
    // Start is called before the first frame update
    void Start()
    {
        _soundManager = GameObject.Find("Sound_Manager").GetComponent<SoundManager>();
        _speed = Random.Range(3, 6);
        _player = GameObject.Find("Player").transform.GetComponent<Player>();
    }   

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y <= -7)
        {
            transform.position = new Vector3(Random.Range(-10f, 10f), 7, 0);    
        }
    }

	private void OnTriggerEnter2D(Collider2D other)
	{
        if (other.CompareTag("Player"))
        {
            Player player = other.transform.GetComponent<Player>();
            
            if (player != null)
            {
                player.Damage();
            }
            
            HandleEnemyDeath();
        }

        else if (other.CompareTag("Laser"))
        {
            Destroy(other.gameObject);
            
            if (_player != null)
            {
                _player.Scorring(10);
            }
            
            HandleEnemyDeath();
        }
    }

    public void ExplosionActivate()
    {
        _Explosion = Instantiate(_ExplosionPrefab, transform.position, Quaternion.identity);
        _soundManager.ExplosionSoundActive();
        Destroy(_Explosion, DestructionDelay);
    }

    IEnumerator EnemyDeathRoutine()
    {
        yield return new WaitForSeconds(DestructionDelay);
        Destroy(this.gameObject);
    }

    private void HandleEnemyDeath()
    {
        StoppingEnemy();
        ExplosionActivate();
        StartCoroutine(EnemyDeathRoutine());
        this.gameObject.SetActive(false);
    }

    private void StoppingEnemy()
    {
        _speed = 0;
    }
}
