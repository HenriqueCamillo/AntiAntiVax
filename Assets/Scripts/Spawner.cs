using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using  System;

public class Spawner : MonoBehaviour
{
    private float radius;
    [SerializeField] Transform screenBorder;
    [SerializeField] float spawnRate;
    [SerializeField] int waveSize;
    float baseSpawnRate;
    int baseWaveSize;

    [Space(5)]
    [SerializeField] GameObject[] tuberculosisViruses;
    [SerializeField] GameObject[] measlesViruses;
    [SerializeField] GameObject[] rubellaViruses;
    [SerializeField] GameObject[] whoopingCoughViruses;

    [Space(5)]
    [SerializeField] string[] diseases;
    [SerializeField] string[] messages;
    [SerializeField] string[] firstAppearenceMessages;
    
    [Space(5)]
    [SerializeField] GameObject messagePanel;
    [SerializeField] TextMeshProUGUI message; 


    int numberOfViruses = 0;
    int tuberculosisLevel = 0;
    int measlesLevel = 0;
    int rubellaLevel = 0;
    int whoopingCoughLevel = 0;
    private GameObject[] viruses = new GameObject[12];
    [SerializeField] int deadEnemies = 0;
    [SerializeField] int spawnedEnemies = 0;
    private bool tryingToEvolve = false;


    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.OnGameStart += Evolve;
        GameManager.instance.OnGameEnd += ResetSpawner;
        GameManager.instance.OnEnemyDeath += IncreaseDeadEnemies;

        baseSpawnRate = spawnRate;
        baseWaveSize = waveSize;

        radius = (Vector2.zero - (Vector2)screenBorder.position).magnitude;
    }

    void Spawn()
    {
        Vector2 positionOnCircunference = UnityEngine.Random.insideUnitCircle.normalized;
        Vector3 spawnPosition = positionOnCircunference * radius;
        Quaternion rotation = Quaternion.Euler(0f, 0f, Vector2.SignedAngle(Vector2.up, -positionOnCircunference));
        GameObject virus = viruses[UnityEngine.Random.Range(0, numberOfViruses)];
        Instantiate(virus, spawnPosition, rotation);

        if (++spawnedEnemies == waveSize)
            StopSpawner();
    }

    bool ValidEvolution(int disease)
    {
        switch (disease)
        {
            case 0:
                if (tuberculosisLevel != 3)
                    return true;
                else
                    return false;
            case 1:
                if (measlesLevel != 3)
                    return true;
                else
                    return false;
            case 2:
                if (rubellaLevel != 3)
                    return true;
                else
                    return false;
            case 3:
                if (whoopingCoughLevel != 3)
                    return true;
                else
                    return false;
            default:
                return false;
        }
    }

    void Evolve()
    {
        GameManager.instance.Evolving();

        bool firstAppearence = false;
        int nextEvolution;
        if (numberOfViruses != viruses.Length)
        {
            do
            {
                nextEvolution = UnityEngine.Random.Range(0, 4);
            }
            while (!ValidEvolution(nextEvolution));

            switch (nextEvolution)
            {
                case 0:
                    viruses[numberOfViruses] = tuberculosisViruses[tuberculosisLevel];
                    tuberculosisLevel++;

                    if (tuberculosisLevel == 1)
                        firstAppearence = true;

                    break;
                case 1:
                    viruses[numberOfViruses] = measlesViruses[measlesLevel];
                    measlesLevel++;

                    if (measlesLevel == 1)
                        firstAppearence = true;

                    break;
                case 2:
                    viruses[numberOfViruses] = rubellaViruses[rubellaLevel];
                    rubellaLevel++;

                    if (rubellaLevel == 1)
                        firstAppearence = true;

                    break;
                case 3:
                    viruses[numberOfViruses] = whoopingCoughViruses[whoopingCoughLevel];
                    whoopingCoughLevel++;

                    if (whoopingCoughLevel == 1)
                        firstAppearence = true;

                    break;
            }
            numberOfViruses++;
        }
        else
        {
            nextEvolution = UnityEngine.Random.Range(0, 4);
        }

        if (firstAppearence)
            message.text = String.Format(firstAppearenceMessages[UnityEngine.Random.Range(0, firstAppearenceMessages.Length)], diseases[nextEvolution]);
        else
            message.text = String.Format(messages[UnityEngine.Random.Range(0, messages.Length)], diseases[nextEvolution]);
        messagePanel.SetActive(true);

        spawnRate -= 0.025f * spawnRate;
        waveSize +=  (int)Mathf.Ceil(0.1f * (float)waveSize);
    }

    public void StartSpawner()
    {
        tryingToEvolve = false;
        spawnedEnemies = 0;
        deadEnemies = 0;
        GameManager.instance.SpawnerStarted();
        messagePanel.SetActive(false);
        InvokeRepeating("Spawn", 0f, spawnRate);

    }

    void StopSpawner()
    {
        CancelInvoke("Spawn");
        tryingToEvolve = true;
    }

    void ResetSpawner()
    {
        CancelInvoke("Spawn");
        numberOfViruses = 0;
        tuberculosisLevel = 0;
        measlesLevel = 0;
        whoopingCoughLevel = 0;
        rubellaLevel = 0;
        spawnRate = baseSpawnRate;
        waveSize = baseWaveSize;
    }


    void IncreaseDeadEnemies()
    {
        deadEnemies++;
        if (spawnedEnemies == deadEnemies && tryingToEvolve)
            Evolve();
    }
}
