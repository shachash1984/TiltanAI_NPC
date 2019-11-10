using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[RequireComponent(typeof(Rigidbody))]
public class MoveState : State
{
    public Transform target { get; protected set; }
    float _currentSpeed;
    [SerializeField] private float _force = 20f;
    [SerializeField] private float _minimumDistToAvoid = 10.0f;
    
    public static event Action OnTargetReached;
    public static event Action<Transform> QueryTarget;

    public override void OnEnterState(NPC npc)
    {
        base.OnEnterState(npc);
        PathSetter.OnWayPointSet += HandleWayPointSet;
        
    }

    public override void Action()
    {
        if (!target)
            QueryTarget?.Invoke(target);
        MoveToPosition();
    }

    public override void OnExitState()
    {
        base.OnExitState();
        PathSetter.OnWayPointSet -= HandleWayPointSet;
    }

    private void HandleWayPointSet(Transform wp)
    {
        target = wp;
    }

    protected void MoveToPosition()
    {
        if(target)
        {
            Vector3 dir = (target.position - transform.position);
            dir.Normalize();
            AvoidObstacles(ref dir);

            _currentSpeed = _npc.GetMovementSpeed() * Time.deltaTime;
            var rot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, _npc.GetRotationSpeed() * Time.deltaTime);
            Vector3 wantedPos = transform.forward * _currentSpeed;
            transform.position += wantedPos;

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        InterarctionType it;
        if (_npc && _npc.AllowedToInteract(other.gameObject, out it))
        {
            if (it == InterarctionType.WayPoint)
                OnTargetReached?.Invoke();
        }
    }

    public void AvoidObstacles(ref Vector3 dir)
    {
        RaycastHit hit;
        //if (Physics.Raycast(transform.position, transform.forward, out hit, _minimumDistToAvoid, _npc.GetObstacleMask()))
        //{
        //    Vector3 hitNormal = hit.normal;
        //    hitNormal.y = 0.0f;
        //    dir = transform.forward + hitNormal * _force;
        //}
        if(Physics.SphereCast(transform.position, 10f, transform.forward, out hit, 10f, _npc.GetObstacleMask()))
        {
            Vector3 hitNormal = hit.normal;
            dir = transform.forward + hitNormal * _force;
        }
    }

}
