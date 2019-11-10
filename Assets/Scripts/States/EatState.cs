using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EatState : State
{
    public Transform food { get; private set; }
    public static event Action OnFoodEaten;
    [SerializeField] private float _force = 20f;
    [SerializeField] private float _minimumDistToAvoid = 10.0f;

    public override void Action()
    {
        MoveToPosition();
    }

    public override void OnEnterState(NPC npc)
    {
        base.OnEnterState(npc);
        food = GameObject.FindGameObjectWithTag("Food").transform;
    }

    private void MoveToPosition()
    {
        if (food)
        {
            Vector3 dir = (food.position - transform.position);
            dir.Normalize();
            AvoidObstacles(ref dir);

            float speed = _npc.GetMovementSpeed() * Time.deltaTime;
            var rot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, _npc.GetRotationSpeed() * Time.deltaTime);
            Vector3 wantedPos = transform.forward * speed;
            transform.position += wantedPos;

            //Quaternion wantedRot = Quaternion.LookRotation(food.position);
            //transform.rotation = Quaternion.Slerp(transform.rotation, wantedRot, Time.deltaTime * _npc.GetRotationSpeed());
            //Vector3 nextPos = (food.position - transform.position).normalized;
            //nextPos = Vector3.Lerp(transform.position, transform.position + nextPos, Time.deltaTime * _npc.GetMovementSpeed());
            //transform.position = (nextPos);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        InterarctionType it;
        if (_npc && _npc.AllowedToInteract(other.gameObject, out it))
        {
            if (it == InterarctionType.Food)
                OnFoodEaten?.Invoke();
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
        if (Physics.SphereCast(transform.position, 10f, transform.forward, out hit, 10f, _npc.GetObstacleMask()))
        {
            Debug.Log(hit.collider.name);
            Vector3 hitNormal = hit.normal;
            dir = transform.forward + hitNormal * _force;
        }
    }
}
