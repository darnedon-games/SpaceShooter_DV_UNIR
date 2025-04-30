using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private float velocidad;
    [SerializeField] private Vector3 direccion;
    [SerializeField] private float anchoImagen;
    private Vector3 posicionInicial;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        posicionInicial = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //float espacio = velocidad*Time.time;
        // Resto: indica cuánto me queda de recorrido para alcanzar un nuevo ciclo
        float resto = (velocidad*Time.time) % anchoImagen;

        // Mi posición va refrescando desde la inicial sumando tanto como resto me quede en la dirección deseada
        transform.position = posicionInicial + resto * direccion;
    }
}
