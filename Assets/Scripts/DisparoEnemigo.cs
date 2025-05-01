using UnityEngine;
using UnityEngine.Pool;

public class DisparoEnemigo : MonoBehaviour
{
    [SerializeField] private float velocidad;
    [SerializeField] private Vector3 direccion;

    public ObjectPool<DisparoEnemigo> PoolDisparoEnemigo { get; set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(direccion * velocidad * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D elOtro)
    {
        if (elOtro.gameObject.CompareTag("Wall"))
        {
            PoolDisparoEnemigo.Release(this);
        }
    }
}
