using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Enemigo enemigoPrefab;
    [SerializeField] private TextMeshProUGUI textoOleadas;
    [SerializeField] private GameObject winnerCanvas;

    private ObjectPool<Enemigo> enemigoPool;

    private void Awake()
    {
        enemigoPool = new ObjectPool<Enemigo>(CrearEnemigo, CogerEnemigo, DejarEnemigo);
    }

    private Enemigo CrearEnemigo()
    {
        Enemigo copiaEnemigo = Instantiate(enemigoPrefab);
        copiaEnemigo.MiPoolEnemigo = enemigoPool;
        return copiaEnemigo;
    }

    private void CogerEnemigo(Enemigo enemigo)
    {
        enemigo.gameObject.SetActive(true);
    }

    private void DejarEnemigo(Enemigo enemigo)
    {
        enemigo.gameObject.SetActive(false);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(SpawnearEnemigos());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnearEnemigos()
    {
        for (int i = 0; i < 5; i++) // Niveles
        {          
            for (int j = 0; j < 3; j++) // Oleadas
            {
                textoOleadas.text = "Nivel " + (i+1) + " -" + " Oleada " + (j+1);
                yield return new WaitForSeconds(1.5f);
                textoOleadas.text = "";
                for (int k = 0; k < 10; k++) // Enemigos
                {
                    Vector3 puntoAleatorio = new Vector3(transform.position.x, Random.Range(-4.5f, 4.5f), 0);
                    Enemigo copia = enemigoPool.Get();
                    copia.transform.position = puntoAleatorio;
                    yield return new WaitForSeconds(0.5f);
                }
                yield return new WaitForSeconds(2f);
            }
            yield return new WaitForSeconds(3f);
        }
        // Se activa Canvas con la pantalla de partida finalizada
        winnerCanvas.SetActive(true);
    }
}
