using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIBehaviour : MonoBehaviour
{
    public enum State{
        Roaming,
        Chase,
        Attack,
        BackToStart,
        Flee,
        Wait
    }
    

    public State state;

    private NavMeshAgent _agent;
    private bool finishedTravel;
    private Vector3 destination;
   
    [SerializeField] private AiProfil profile;

    private Vector3 chasePositionStart;
    
    private void Start() {
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update() {
        UpdateState();
        profile.target = GetComponent<BotDetection>().FindVisibleTargets();
        if(Vector3.Distance(transform.position, _agent.destination) < 2 && state == State.Roaming)
        {
            finishedTravel = true;
            ChangeState(State.Wait);
        }

        if(profile.target)
        {
            print("Je vois le joueur");
        }
    }

    public void ChangeState(State newState)
    {
        state = newState;
        OnChangeState();
    }


    private void OnChangeState()
    {
        switch (state)
        {
            case State.Roaming:
                destination = Random.insideUnitSphere * 25f;
                _agent.SetDestination(destination);
                finishedTravel = false;
            break;
            case State.Chase:
                _agent.SetDestination(transform.position);
                chasePositionStart = transform.position;
                _agent.SetDestination(profile.target.position);
                _agent.isStopped = false;
            break;
            case State.Attack:
                _agent.isStopped = true;
            break;
            case State.BackToStart:
                _agent.SetDestination(chasePositionStart);
            break;
            case State.Flee:
                _agent.SetDestination(transform.position);
                Vector3 fleeDir = ((transform.position - profile.target.transform.position)).normalized;
                Vector3 fleeMotion = fleeDir * profile.fleeDistance;
                destination = transform.position + fleeMotion;
                _agent.SetDestination(destination);
                finishedTravel = false;
            break;
            case State.Wait:
            break;
            default:
            break;
        }
    }

    private void UpdateState()
    {
        switch (state)
        {
            case State.Roaming:
                if(finishedTravel)
                {
                    destination = Random.insideUnitSphere * 25f;
                    _agent.SetDestination(destination);
                    finishedTravel = false;
                }
                else if(profile.target)
                {
                    if(profile.type == AiProfil.AItype.AnimalNeutral ||profile.type == AiProfil.AItype.HumanNeutral)
                        ChangeState(State.Flee);
                    else
                        ChangeState(State.Chase);
                }
            break;
            case State.Chase:
                if(!profile.target)
                {
                    ChangeState(State.BackToStart);
                }
                else
                {
                    if(Vector3.Distance(transform.position, profile.target.position) <= profile.attackDistance)
                                        ChangeState(State.Attack);
                }
            break;
            case State.Attack:
                if(Vector3.Distance(transform.position, profile.target.position) > profile.attackDistance)
                    ChangeState(State.Chase);
                else
                    Attack();
            break;
            case State.BackToStart:
                if(Vector3.Distance(transform.position, _agent.destination) <= 2)
                    ChangeState(State.Roaming);
            break;
            case State.Flee:
                if(!profile.target)
                {
                    profile.target = null;
                    _agent.SetDestination(transform.position);
                    finishedTravel = true;
                    ChangeState(State.Roaming);
                }
                else
                {
                    Vector3 fleeDir = ((transform.position - profile.target.transform.position)).normalized;
                    Vector3 fleeMotion = fleeDir * profile.fleeDistance;
                    destination = transform.position + fleeMotion;
                    _agent.SetDestination(destination);
                    finishedTravel = false;
                }
            break;
            case State.Wait:
                WaitSequence();
                if(profile.target)
                {
                    if(profile.type == AiProfil.AItype.AnimalNeutral ||profile.type == AiProfil.AItype.HumanNeutral)
                        ChangeState(State.Flee);
                    else
                        ChangeState(State.Attack);
                }
            break;
            default:
            break;
        }
    }

    private void WaitSequence()
    {
        profile.actualWaitTime -= 1 * Time.deltaTime;
        if(profile.actualWaitTime <= 0)
        {
            ChangeState(State.Roaming);
            profile.actualWaitTime = profile.waitTime;
        }
            
    }

    private void Attack()
    {
        if(profile.actualAttackInterval <= 0)
        {
            //Attack
            print("Attack");
            profile.actualAttackInterval = profile.attackInterval;
        }
        else
        {
            profile.actualAttackInterval -= 1 * Time.deltaTime;
        }
    }
}
