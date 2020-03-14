using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoShootInRange : MonoBehaviour
{
    WaitUntilInRange _waitUntilEnemyInRange;

    void Awake()
    {
        Transform tower = transform;
        Transform enemy = GameObject.Find("Enemy").transform;
        _waitUntilEnemyInRange = new WaitUntilInRange(tower, enemy);
    }

    void Start()
    {
        StartCoroutine(CShootIfEnemyInRange());
    }

    #region Coroutines

    private IEnumerator CShootIfEnemyInRange()
    {
        yield return _waitUntilEnemyInRange;
        GameObject.Find("Enemy").GetComponent<MeshRenderer>().material.color = Color.red;
    }

    #endregion

}
