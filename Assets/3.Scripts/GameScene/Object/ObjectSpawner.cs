using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] foodObject;

    [SerializeField] Vector3[] spawnPos = new Vector3[3];

    //랜덤 스폰 범위
    [SerializeField] float xMin;
    [SerializeField] float xMax;
    [SerializeField] float zMin;
    [SerializeField] float zMax;

    private void Start()
    {
        Shuffle();

        StartCoroutine(Pop_co());
    }

    private void Shuffle()
    {
        GameObject temp;
        int dest = 0;
        int sour = 0;

        for (int i = 0; i < 50; i++)
        {
            dest = Random.Range(0, 9);
            sour = Random.Range(0, 9);

            temp = foodObject[dest];
            foodObject[dest] = foodObject[sour];
            foodObject[sour] = temp;
        }
    }

    private void RandomSpawnPos()
    {
        for(int i = 0; i < spawnPos.Length; i++)
        {
            spawnPos[i].x = Random.Range(xMin, xMax);
            spawnPos[i].z = Random.Range(zMin, zMax);
        }
    }

    IEnumerator Pop_co()
    {
        RandomSpawnPos();
        
        for (int i = 0; i < 3; i++)
        {
            foodObject[i].transform.position = spawnPos[i];

            yield return new WaitForSeconds(0.5f);
        }
    }
}
