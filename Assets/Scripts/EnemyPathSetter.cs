using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyPathSetter : PathSetter
{
    public static event Action<int, Transform> OnTargetSpawnedForEnemy;

    private void OnEnable()
    {
        EnemyMoveState.OnEnemySpawned += HandleEnemySpawned;
        EnemyMoveState.OnEnemyTargetReached += HandleEnemyReachedTarget;
    }

    private void OnDisable()
    {
        EnemyMoveState.OnEnemySpawned -= HandleEnemySpawned;
        EnemyMoveState.OnEnemyTargetReached -= HandleEnemyReachedTarget;
    }

    private void HandleEnemySpawned(int enemyId)
    {
        Transform target = Instantiate(_wayPointPrefab, GetNextPosition(), Quaternion.identity).transform;
        OnTargetSpawnedForEnemy?.Invoke(enemyId, target);
    }

    private void HandleEnemyReachedTarget(Transform target)
    {
        target.position = GetNextPosition();
    }
}
