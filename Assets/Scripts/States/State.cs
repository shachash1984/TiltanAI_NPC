using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : MonoBehaviour
{
    protected NPC _npc;
    public abstract void Action();
    public virtual void OnEnterState(NPC npc)
    {
        _npc = npc;
    }

    public virtual void OnEnterState(EnemyNPC eNpc)
    {

    }

    public virtual void OnExitState()
    {
        _npc = null;
    }
}
