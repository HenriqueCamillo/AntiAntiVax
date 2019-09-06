using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private float radius;
    [SerializeField] float spawnRate;
    [SerializeField] Transform screenBorder;
    [SerializeField] GameObject[] viruses;



    // Start is called before the first frame update
    void Start()
    {
        radius = (Vector2.zero - (Vector2)screenBorder.position).magnitude;
        // InvokeRepeating("Spawn", 0f, spawnRate);
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        Vector2 positionOnCircunference = Random.insideUnitCircle.normalized;
        Vector3 spawnPosition = positionOnCircunference * radius;
        Quaternion rotation = Quaternion.Euler(0f, 0f, Vector2.SignedAngle(Vector2.up, -positionOnCircunference));
        GameObject virus = viruses[Random.Range(0, viruses.Length)];
        Instantiate(virus, spawnPosition, rotation);
        yield return new WaitForSeconds(spawnRate);
        spawnRate -= 0.01f * spawnRate;
        StartCoroutine(Spawn());
    }
}
