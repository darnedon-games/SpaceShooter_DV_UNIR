using System.Collections;
using TMPro;
using UnityEngine;

public class Enemigo : MonoBehaviour
{
    [SerializeField] private float velocidad;
    [SerializeField] private GameObject disparoPrefab;
    [SerializeField] private GameObject spawnPoint;
    [SerializeField] private GameObject explosionPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(Disparar());
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(-1, 0, 0).normalized * velocidad * Time.deltaTime);
    }

    IEnumerator Disparar()
    {
        while(true)
        {
            Instantiate(disparoPrefab, spawnPoint.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(1f);
        }
    }
    private void OnTriggerEnter2D(Collider2D elOtro)
    {
        if (elOtro.gameObject.CompareTag("PlayerShoot"))
        {
            Destroy(elOtro.gameObject);
            Vector3 deathEnemyPoint = this.transform.position;
            Destroy(this.gameObject);
            Instantiate(explosionPrefab, deathEnemyPoint, Quaternion.identity); // Instanciamos la explosión en el último punto donde estuvo el enemigo antes de morir

            // Sumamos puntuación desde un método de la clase Player
            GameObject playerFind = GameObject.Find("Player"); // Primero buscamos al game object Player
            if (playerFind != null)
            {
                Player player = playerFind.GetComponent<Player>(); // Si el objeto no es nulo, entonces guardamos en un objeto de tipo Player para acceder a su método
                if (player != null)
                {
                    player.AddScore(10);
                }
            }
        }
        else if (elOtro.gameObject.CompareTag("Wall"))
        {
            Destroy(this.gameObject);
        }
    }
}
