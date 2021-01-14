using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public float explosionRadius;
    public LayerMask damageLayerGranade;
    public GameObject explosionPrefab;
    public float grenadeDamage;
    public float throwForce;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        rb.velocity = -transform.up * throwForce;
        StartCoroutine(Explosion());
    }


    private IEnumerator Explosion()
    {
        yield return new WaitForSeconds(1f);
        Explode();
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (!collision.gameObject.CompareTag("Player"))
        {
            Explode();
            Destroy(gameObject);
        }
    }


    private void Explode()
    {
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius, damageLayerGranade);
        foreach (Collider2D collider in colliders)
        {
            collider.gameObject.SendMessage("LoseHp", grenadeDamage);
        }

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
