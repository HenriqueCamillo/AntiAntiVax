using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Syringe: MonoBehaviour
{
    private SpriteRenderer sRenderer;
    private Vector2 direction;
    private bool canShoot = false;
    [SerializeField] float cooldown;
    [SerializeField] Transform needle;

    [Space(5)]
    [Header("Vaccin prefabs")]
    [SerializeField] GameObject tuberculosisVaccin;
    [SerializeField] GameObject measlesVaccin;
    [SerializeField] GameObject rubellaVaccin;
    [SerializeField] GameObject whoopingCoughVaccin;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        sRenderer = GetComponent<SpriteRenderer>();
        this.transform.position = Vector3.zero;

        // GameManager.instance.OnGameStart += Enable;
        GameManager.instance.OnGameEnd += Disable;
        GameManager.instance.OnSpawnerStart += Enable;
        GameManager.instance.OnEvolving += Disable;
    }

    void Disable()
    {
        canShoot = false;
        sRenderer.enabled = false;
        StopAllCoroutines();
    }

    void Enable()
    {
        sRenderer.enabled = true;
        canShoot = true;
    }

    void Update()
    {
        // Gets direction from mouse position
        direction = ((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - Vector2.zero).normalized;

        UpdateRotation();

        // Checks input and shoots the vaccin
        if (canShoot)
        {
            if (Input.GetKeyDown(KeyCode.S))
                StartCoroutine(Shoot(tuberculosisVaccin));
            else if (Input.GetKeyDown(KeyCode.D))
                StartCoroutine(Shoot(measlesVaccin));
            else if (Input.GetKeyDown(KeyCode.A))
                StartCoroutine(Shoot(rubellaVaccin));
            else if (Input.GetKeyDown(KeyCode.F))
                StartCoroutine(Shoot(whoopingCoughVaccin));
        }

        
    }

    IEnumerator Shoot(GameObject prefab)
    {
        animator.Play("Press");
        canShoot = false;
        Instantiate(prefab, needle.position, this.transform.rotation);
        yield return new WaitForSeconds(cooldown);
        animator.Play("Idle");
        canShoot = true;
    }

    /// <summary>
    /// Rotates the syringe according to the direction vector
    /// </summary>
    void UpdateRotation()
    {
        this.transform.rotation = Quaternion.Euler(0f, 0f, Vector2.SignedAngle(Vector2.up, direction)); 
    }
}
