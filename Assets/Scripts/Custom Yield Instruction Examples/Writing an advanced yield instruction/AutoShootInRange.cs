using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// If the enemy comes close to our tower the tower will mark it red.
/// Move the enemy cube towards the tower and it will be painted red.
/// </summary>
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
        StartCoroutine(ShootIfEnemyInRangeCoroutine());
    }

    #region Coroutines

    private IEnumerator ShootIfEnemyInRangeCoroutine()
    {
        yield return _waitUntilEnemyInRange;
        GameObject.Find("Enemy").GetComponent<MeshRenderer>().material.color = Color.red;
    }

    #endregion

}
