using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vaccin : MonoBehaviour
{
    private Vector2 direction;
    private Rigidbody2D rBody;
    [SerializeField] int damage;
    [SerializeField] float speed;
    private AudioSource audioSource;
    private SpriteRenderer sRenderer;
    private BoxCollider2D col;

    [SerializeField] Diseases.DiseaseType disease;

    /// <summary>
    /// Sets its speed
    /// </summary>
    void Start()
    {
        sRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        col = GetComponent<BoxCollider2D>();
        rBody = GetComponent<Rigidbody2D>();
        direction = ((Vector2)(this.transform.rotation * Vector2.up * Mathf.Sin(Time.deltaTime))).normalized;
        rBody.velocity = direction * speed;
    }

    /// <summary>
    /// Deals damage on viruses
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Virus"))
        {
            other.GetComponent<Virus>().TakeDamage(disease, damage);
            audioSource.Play();
            Disable();
            Destroy(this.gameObject, audioSource.clip.length);
        }

    }

    void Disable()
    {
        col.enabled = false;
        sRenderer.enabled = false;
    }    

    /// <summary>
    /// Destroys object when it is out of screen
    /// </summary>
    void OnBecameInvisible()
    {
        if (!audioSource.isPlaying)
            Destroy(this.gameObject);
    }
}