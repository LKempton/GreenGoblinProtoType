﻿using UnityEngine;
using System.Collections;

public class EnemyDamageScript : MonoBehaviour {

    [SerializeField]
    private float health = 5f;
    [SerializeField]
    private int rndChance = 10;
    [SerializeField]
    private GameObject[] powerups = new GameObject[2];

    private SpriteRenderer m_enemySprite;
    private Color m_defaultColour;

    [SerializeField]
    private Color damagedColour = new Color(0, 0, 0);

    private GameControllerScript gc;
    

    void Start()
    {
        gc = GameObject.FindWithTag("GameController").GetComponent<GameControllerScript>();

        m_enemySprite = GetComponentInChildren<SpriteRenderer>();

       // m_defaultColour = m_enemySprite.color;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Bullet"))
        {
            health -= 1;

            StartCoroutine("DamageColour");
        }
        else if (col.CompareTag("Rocket"))
        {
            health -= 5;
        }
    }

    void Update()
    {
        int rng;
        // Detects if the enemy health has dropped below 0 and destroys them if it has.
        if (health <= 0)
        {
            rng = Random.Range(1, 100);
            Debug.Log(rng);
            if ((rng <= rndChance && gc.activePowerups == 0) || gc.dropNext == true)
            {
                SpawnPowerup();

                gc.PowerupDrop();
            }

            StopCoroutine("DamageColour");

            gc.activeEnemies--;

            Destroy(gameObject);
        }

        if (m_enemySprite == null)
        {
            m_enemySprite = GetComponentInChildren<SpriteRenderer>();
            m_defaultColour = m_enemySprite.color;
        }
    }

    private void SpawnPowerup()
    {
        // Spawn a random powerup on the enemies position.
        Instantiate(powerups[Random.Range(0, powerups.Length)], transform.position, transform.rotation);
    }

    IEnumerator DamageColour()
    {
        m_enemySprite.color = damagedColour;

        yield return new WaitForSeconds(0.2f);

        m_enemySprite.color = m_defaultColour;
     
    }
}