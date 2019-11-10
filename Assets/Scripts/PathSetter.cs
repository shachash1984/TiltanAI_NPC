using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class PathSetter : MonoBehaviour
{
    [SerializeField] protected Bounds _pathBounds;
    [SerializeField] protected GameObject _wayPointPrefab;
    private GameObject _currentWayPoint;
    public static event Action<Transform> OnWayPointSet;

    private void OnEnable()
    {
        MoveState.OnTargetReached += HandleTargetReached;
        MoveState.QueryTarget += HandleQueryTarget;
    }

    private void OnDisable()
    {
        MoveState.OnTargetReached -= HandleTargetReached;
        MoveState.QueryTarget -= HandleQueryTarget;
    }

    private void HandleTargetReached()
    {
        if (_currentWayPoint)
            Destroy(_currentWayPoint.gameObject);
        SetNewWayPoint();
    }

    private void HandleQueryTarget(Transform t)
    {
        if (_currentWayPoint)
        {
            t = _currentWayPoint.transform;
            OnWayPointSet?.Invoke(_currentWayPoint.transform);
        }
            
        else
            SetNewWayPoint();
    }

    public Vector3 GetNextPosition()
    {
        return new Vector3(
            Random.Range(_pathBounds.min.x, _pathBounds.max.x),
            Random.Range(_pathBounds.min.y, _pathBounds.max.y),
            Random.Range(_pathBounds.min.z, _pathBounds.max.z));
    }

    public void SetNewWayPoint()
    {
        _currentWayPoint = Instantiate(_wayPointPrefab, GetNextPosition(), Quaternion.identity);
        OnWayPointSet?.Invoke(_currentWayPoint.transform);
    }
}
