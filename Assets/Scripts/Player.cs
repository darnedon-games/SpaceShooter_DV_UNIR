using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

public class Player : MonoBehaviour
{
    [SerializeField] private float velocidad;
    private float velocidadInicial;
    [SerializeField] private Disparo disparoPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float ratioDisparo;
    private float temporizador = 0.5f;
    private float vidas = 100;

    [SerializeField] private TextMeshProUGUI textoLives;

    [SerializeField] private TextMeshProUGUI textoScore;
    private float score = 0;

    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private AudioClip livesSound;
    [SerializeField] private AudioClip bombSound;
    [SerializeField] private AudioClip speedSound;
    private AudioSource sound;
    private AudioSource music;

    [SerializeField] private GameObject gameOverCanvas;
    [SerializeField] private GameObject pauseCanvas;

    private ObjectPool<Disparo> disparoPool;

    private void Awake()
    {
        disparoPool = new ObjectPool<Disparo>(CrearDisparo, CogerDisparo, DejarDisparo);
    }

    // Este método se llamará cuando se necesite una bala nueva
    private Disparo CrearDisparo()
    {
        Disparo copiaDisparo = Instantiate(disparoPrefab);
        copiaDisparo.MiPool = disparoPool; // Al disparo que ha nacido le comunico quién es su piscina para después liberarse
        return copiaDisparo;
    }

    // Este método se llamará cuando se necesite reciclar una bala ya existente
    private void CogerDisparo(Disparo disparo)
    {
        disparo.gameObject.SetActive(true);
    }

    // Este método se llamará de forma automática cuando una bala tenga que ser devuelta a la piscina
    private void DejarDisparo(Disparo disparo)
    {
        disparo.gameObject.SetActive(false);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        velocidadInicial = velocidad;
        sound = this.GetComponent<AudioSource>();
        music = GameObject.Find("Music").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Movimiento();
        DelimitarMovimiento();

        Disparar();

        if (Input.GetKey(KeyCode.Escape)){
            pauseCanvas.SetActive(true);
            Time.timeScale = 0f; // Se pausa el juego
            music.Pause();// Se pausa la música
        }
    }

    void Movimiento()
    {
        float inputH = Input.GetAxisRaw("Horizontal");
        float inputV = Input.GetAxisRaw("Vertical");
        transform.Translate(new Vector2(inputH, inputV).normalized * velocidad * Time.deltaTime);
    }

    void DelimitarMovimiento()
    {
        float xClamped = Mathf.Clamp(transform.position.x, -8.4f, 8.4f);
        float yClamped = Mathf.Clamp(transform.position.y, -4.5f, 4.5f);
        transform.position = new Vector3(xClamped, yClamped, 0);
    }

    void Disparar()
    {
        temporizador += 1 * Time.deltaTime;

        if (Input.GetKey(KeyCode.Space) && temporizador > ratioDisparo)
        {
            sound.PlayOneShot(shootSound,0.8f);
            
            Disparo copia = disparoPool.Get(); // Dame una bala
            copia.transform.position = spawnPoint.position;

            temporizador = 0;
        }
    }

    public void AddScore(float puntos)
    {
        score += puntos;
        textoScore.text = "Score: " + score;
    }

    IEnumerator SpeedBooster()
    {
        velocidad *= 2f;
        yield return new WaitForSeconds(5f);
        velocidad = velocidadInicial;
    }

    private void OnTriggerEnter2D(Collider2D elOtro)
    {
        if (elOtro.gameObject.CompareTag("EnemyShoot") || (elOtro.gameObject.CompareTag("Enemy")))
        {
            vidas -= 20;
            if (elOtro.gameObject.CompareTag("Enemy"))
            {
                Vector3 deathEnemyPoint = elOtro.gameObject.transform.position;
                Instantiate(explosionPrefab, deathEnemyPoint, Quaternion.identity);
            }
            Destroy(elOtro.gameObject);

            if (vidas <= 0)
            {
                textoLives.text = "Lives: 0";
                Vector3 deathPlayerPoint = this.transform.position;
                Destroy(this.gameObject);
                Instantiate(explosionPrefab, deathPlayerPoint, Quaternion.identity); // Instanciamos la explosión en el último punto donde estuvo el jugador antes de morir

                // Se activa Canvas con la pantalla de Game Over
                gameOverCanvas.SetActive(true);
                //Time.timeScale = 0f; // Se pausa el juego
            }
            else
            {
                textoLives.text = "Lives: " + vidas;
            }
        }
        else if (elOtro.gameObject.CompareTag("LivesBooster")) {
            vidas = 100;
            textoLives.text = "Lives: " + vidas;
            sound.PlayOneShot(livesSound, 1f);
            Destroy(elOtro.gameObject);
            AddScore(5); // Puntos extra por recoger el objeto
        }
        else if (elOtro.gameObject.CompareTag("SpeedBooster"))
        {
            StartCoroutine(SpeedBooster());
            sound.PlayOneShot(speedSound, 1f);
            Destroy(elOtro.gameObject);
            AddScore(5); // Puntos extra por recoger el objeto
        }
        else if (elOtro.gameObject.CompareTag("BombBooster"))
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

            foreach (GameObject obj in enemies)
            {
                Vector3 deathEnemyPoint = obj.transform.position;
                Destroy(obj);
                Instantiate(explosionPrefab, deathEnemyPoint, Quaternion.identity); // Instanciamos la explosión en el último punto donde estuvo cada enemigo antes de morir
            }
            sound.PlayOneShot(bombSound, 1f);
            Destroy(elOtro.gameObject);
            AddScore(50); // Puntos extra por recoger el objeto
        }
    }
}
