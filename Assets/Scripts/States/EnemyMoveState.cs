using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody))]
public class EnemyMoveState : State
{
    public Transform target { get; protected set; }
    EnemyNPC _eNpc;
    public static event Action<int> OnEnemySpawned;
    public static event Action<Transform> OnEnemyTargetReached;


    public override void OnEnterState(EnemyNPC eNpc)
    {
        _eNpc = eNpc;
        EnemyPathSetter.OnTargetSpawnedForEnemy += HandleTargetSpawnedForEnemy;
        OnEnemySpawned?.Invoke(GetInstanceID());
        
    }

    public override void OnExitState()
    {
        EnemyPathSetter.OnTargetSpawnedForEnemy -= HandleTargetSpawnedForEnemy;
    }

    private void Update()
    {
        Action();
    }

    private void HandleTargetSpawnedForEnemy(int id, Transform t)
    {
        if(id == GetInstanceID())
        {
            target = t;
        }
    }

    protected void OnTriggerEnter(Collider other)
    {
        InterarctionType it;
        if (_eNpc && _eNpc.AllowedToInteract(other.gameObject, out it))
        {
            if (it == InterarctionType.EnemyWayPoint)
                OnEnemyTargetReached?.Invoke(other.transform);
        }
    }

    public override void Action()
    {
        MoveToPosition();
    }

    protected void MoveToPosition()
    {
        if (target)
        {
            Vector3 dir = (target.position - transform.position);
            dir.Normalize();

            float speed = _eNpc.GetMovementSpeed() * Time.deltaTime;
            var rot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, _eNpc.GetRotationSpeed() * Time.deltaTime);
            Vector3 wantedPos = transform.forward * speed;
            transform.position += wantedPos;

            //Quaternion wantedRot = Quaternion.LookRotation(target.position);
            //transform.rotation = Quaternion.Slerp(transform.rotation, wantedRot, Time.deltaTime * _eNpc.GetRotationSpeed());
            //Vector3 nextPos = (target.position - transform.position).normalized;
            //nextPos = Vector3.Lerp(transform.position, transform.position + nextPos, Time.deltaTime * _eNpc.GetMovementSpeed());
            //transform.position = (nextPos);
        }
    }



}
