using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyNPC : MonoBehaviour
{
    [SerializeField] protected float _moveSpeed;
    [SerializeField] protected float _rotSpeed;
    [SerializeField] protected LayerMask _interactionLayers;
    public State state { get; protected set; }

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

    void Start()
    {
        _rotSpeed = Random.Range(1,3);
        _moveSpeed = Random.Range(2, 8);
        SetState(gameObject.AddComponent<EnemyMoveState>());
    }

    void Update()
    {
        state.Action();
    }

    public float GetMovementSpeed()
    {
        return _moveSpeed;
    }

    public float GetRotationSpeed()
    {
        return _rotSpeed;
    }

    public bool AllowedToInteract(GameObject other, out InterarctionType it)
    {
        it = InterarctionType.Null;
        if ((_interactionLayers.value & (1 << other.layer)) != 0)
        {
            if (other.layer == 11)
                it = InterarctionType.EnemyWayPoint;
            return true;
        }
        return false;
    }
}
