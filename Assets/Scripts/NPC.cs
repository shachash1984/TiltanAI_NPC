using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InterarctionType {Null, WayPoint, Food, EnemyWayPoint}

public class NPC : MonoBehaviour
{
    public State state {get; protected set;}
    [SerializeField] protected float _moveSpeed;
    [SerializeField] protected float _rotSpeed;
    [SerializeField] protected LayerMask _interactionLayers;
    [SerializeField] private LayerMask _obstacleMask;
    public void SetState(State newState)
    {
        if (state != null)
        {
            state.OnExitState();
            Destroy(state);
        }
            
        state = newState;
        newState.OnEnterState(this);
    }

    private void HandleFoodSpawned(Transform food)
    {
        SetState(gameObject.AddComponent<EatState>());
    }

    private void HandleFoodConsumed()
    {
        SetState(gameObject.AddComponent<MoveState>());
    }

    private void OnEnable()
    {
        FoodSpawner.OnFoodSpawned += HandleFoodSpawned;
        EatState.OnFoodEaten += HandleFoodConsumed;
    }

    private void OnDisable()
    {
        FoodSpawner.OnFoodSpawned -= HandleFoodSpawned;
        EatState.OnFoodEaten -= HandleFoodConsumed;
    }

    private void Start()
    {
        SetState(gameObject.AddComponent<MoveState>());
    }

    private void Update()
    {
        state.Action();
    }

    public bool AllowedToInteract(GameObject other, out InterarctionType it)
    {
        it = InterarctionType.Null;
        if( (_interactionLayers.value & (1 << other.layer)) != 0)
        {
            if (other.layer == 8)
                it = InterarctionType.WayPoint;
            else if(other.layer == 9)
                it = InterarctionType.Food;
            return true;
        }
        return false;
    }

    public float GetMovementSpeed()
    {
        return _moveSpeed;
    }

    public float GetRotationSpeed()
    {
        return _rotSpeed;
    }

    public LayerMask GetObstacleMask()
    {
        return _obstacleMask;
    }
}
