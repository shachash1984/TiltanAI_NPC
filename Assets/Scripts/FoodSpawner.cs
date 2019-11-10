using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class FoodSpawner : MonoBehaviour
{
    [SerializeField] private Bounds _spawnBounds;
    [SerializeField] private Vector2 _spawnTimeRange;
    [SerializeField] private GameObject _foodPrefab;
    private GameObject currentFood;
    private float _currentSpawnTime;
    private float _timer;
    public static event Action<Transform> OnFoodSpawned;


    private void Start()
    {
        ResetSpawnTime();
    }

    private void OnEnable()
    {
        EatState.OnFoodEaten += HandleFoodEaten;
    }

    private void OnDisable()
    {
        EatState.OnFoodEaten -= HandleFoodEaten;
    }

    private void Update()
    {
        if(Time.time - _currentSpawnTime >= _timer)
        {
            if (!currentFood)
                SpawnFood();
            ResetSpawnTime();
        }
    }

    private void HandleFoodEaten()
    {
        Destroy(currentFood);
    }

    private void ResetSpawnTime()
    {
        _timer = Time.time;
        _currentSpawnTime = Random.Range(_spawnTimeRange.x, _spawnTimeRange.y);
    }

    private void SpawnFood()
    {
        currentFood = Instantiate(_foodPrefab, GetNextPosition(), Quaternion.identity);
        OnFoodSpawned?.Invoke(currentFood.transform);
    }

    private Vector3 GetNextPosition()
    {
        return new Vector3(
            Random.Range(_spawnBounds.min.x, _spawnBounds.max.x),
            Random.Range(_spawnBounds.min.y, _spawnBounds.max.y),
            Random.Range(_spawnBounds.min.z, _spawnBounds.max.z));
    }
}
