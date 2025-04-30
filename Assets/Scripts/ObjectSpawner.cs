using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private GameObject livesPrefab;
    [SerializeField] private GameObject bombPrefab;
    [SerializeField] private GameObject boostPrefab;
    private List<GameObject> objetos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        objetos = new List<GameObject>
        {
            livesPrefab,
            bombPrefab,
            boostPrefab
        };
        StartCoroutine(SpawnearObjetos());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Corutina para lanzar un delay antes de spawnear objetos
    IEnumerator SpawnearObjetos()
    {
        while (true)
        {
            // Esperamos un delay aleatorio antes de la generación de cualquier objeto
            yield return new WaitForSeconds(Random.Range(5f, 20f));

            // Generamos un objeto aleatorio sacado de la lista "objetos"
            GameObject objetoAleatorio = objetos[UnityEngine.Random.Range(0, objetos.Count)];

            // Generamos el objeto en un punto aleatorio dentro del mapa
            Vector3 puntoAleatorio = new Vector3(Random.Range(-8.4f, 8.4f), Random.Range(-4.5f, 4.5f), 0);
            Instantiate(objetoAleatorio, puntoAleatorio, Quaternion.identity);
        }
    }
}
