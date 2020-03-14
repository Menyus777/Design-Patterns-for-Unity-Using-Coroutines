using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerGenerator : MonoBehaviour
{
    [SerializeField]
    GameObject OriginalWorker;
    [SerializeField]
    bool GenerateWorkers = false;


    private const string _WorkerNamingConvention = "Worker - ";
    
    void Start()
    {
        if (GenerateWorkers)
        {
            StartCoroutine(CGenerateWorkers());
        }
    }

    #region Coroutines

    IEnumerator CGenerateWorkers()
    {
        Vector3 startPos = OriginalWorker.transform.position;
        var parent = GameObject.Find("4th Example - Why you should cache yield instructions").transform;
        int id = 0;
        float instantioationBreak = 0.025f;
        bool finish = false;
        for (int i = 0; i < 100; i++)
        {
            for(int j = 0; j < 100; j++)
            {
                id++;
                var gameObject = Instantiate(OriginalWorker, new Vector3(startPos.x - 0.5f - i * 2, startPos.y, startPos.z + 0.5f + j * 2), Quaternion.identity, parent);
                gameObject.name = _WorkerNamingConvention + id;
                if(!finish || i < 10)
                    yield return new WaitForSeconds(instantioationBreak);
            }
            if (!finish)
            {
                instantioationBreak /= 2.0f;
            }
            if (instantioationBreak <= 0.0025f)
            {
                finish = true;
            }
        }
    }

    #endregion
}
