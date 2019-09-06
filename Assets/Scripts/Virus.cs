using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Virus : MonoBehaviour
{
    private Vector2 direction;
    private float fTime = 0;
    [SerializeField] Diseases.DiseaseType disease;

    [SerializeField] float moveSpeed;
    [SerializeField] float curveSpeed;
    [SerializeField] float amplitude;

    [Space(5)]
    [SerializeField] float minCurveSpeed;
    [SerializeField] float maxCurveSpeed;
    [SerializeField] float minAmplitude;
    [SerializeField] float maxAmplitude;

    [Space(5)]
    [SerializeField] int life;

    private delegate void MoveFuncPointer();
    private MoveFuncPointer moveFunction = null;


    private int Life
    {
        get { return life; }
        set
        {
            if (value <= 0)
                Destroy(this.gameObject);
            else
                life = value;
        }
    }

    void Start()
    {
        // Calculates the direciton vector
        direction = ((Vector2)(this.transform.rotation * Vector2.up * Mathf.Sin(Time.deltaTime))).normalized;

        // Chooses one of the movement functions
        switch (Random.Range(0, 2))
        {
            case 0:
                moveFunction = StraightMovement;
                break;
            
            case 1:
                moveFunction = SinusoidalMovement;
                RandomCurveParameters();
                break;

            default:
                moveFunction = StraightMovement;
                break;
        }
    }

    void RandomCurveParameters()
    {
        curveSpeed = Random.Range(minCurveSpeed, maxCurveSpeed);
        amplitude = Random.Range(minAmplitude, maxAmplitude);
    }

    /// <summary>
    /// Moves the virus
    /// </summary>
    void FixedUpdate()
    {
        moveFunction();
    }

    /// <summary>
    /// Moves the virus straight to the destiny
    /// </summary>
    void StraightMovement()
    {
        this.transform.position += (Vector3)direction * moveSpeed * Time.fixedDeltaTime;
    }

    /// <summary>
    /// Moves the virus in a sinusoidal way
    /// </summary>
    void SinusoidalMovement()
    {
        Vector3 vLastPos = this.transform.position;
        fTime += Time.deltaTime * curveSpeed;
        Vector2 perpendicular = Vector2.Perpendicular(direction).normalized;

        Vector3 hMovement = Mathf.Cos(fTime) * amplitude * perpendicular;
        Vector3 vMovement = direction * moveSpeed;

        this.transform.position += (hMovement + vMovement) * Time.fixedDeltaTime;
        Debug.DrawLine(vLastPos, transform.position, Color.green, 100);
    }

    /// <summary>
    /// Takes damage only if the vaccin and the virus disease matches
    /// </summary>
    /// <param name="disease"></param>
    /// <param name="damage"></param>
    public void TakeDamage(Diseases.DiseaseType disease, int damage)
    {
        if (this.disease == disease)
            Life -= damage;
    }
}
