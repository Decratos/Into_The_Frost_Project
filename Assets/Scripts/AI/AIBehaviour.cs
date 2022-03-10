using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using FMODUnity;

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
    private BanditEquipmentLevel equipment;
   
    [SerializeField] private AiProfil profile;

    private Vector3 chasePositionStart;

    public bool isFromACamp = false;
    public Vector3 CampPosition;
    public Transform target;
    public AiTeam myTeam;
    
    private void Start() {
        _agent = GetComponent<NavMeshAgent>();
        ChangeState(State.Roaming);
        GetComponent<Animator>().SetBool("IsWalking", true);
        if(profile.type == AiProfil.AItype.HumanHostile)
        {
            equipment = GetComponent<BanditEquipmentLevel>();
        }
    }

    private void Update() {
        UpdateState();
        target = GetComponent<BotDetection>().FindVisibleTargets();
        if(Vector3.Distance(transform.position, _agent.destination) < 2 && state == State.Roaming)
        {
            finishedTravel = true;
            ChangeState(State.Wait);
        }
        if (target)
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
                GetComponent<Animator>().SetBool("IsWalking", true);
                finishedTravel = false;
            break;
            case State.Chase:
                _agent.SetDestination(transform.position);
                chasePositionStart = transform.position;
                _agent.SetDestination(target.position);
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
                Vector3 fleeDir = ((transform.position - target.transform.position)).normalized;
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
                    if (!isFromACamp)
                    {
                        destination = Random.insideUnitSphere * 25f;
                    }
                    else
                    {
                        destination = Random.insideUnitSphere * 25f + CampPosition;
                    }
                    _agent.SetDestination(destination);
                    finishedTravel = false;
                }
                else if(target)
                {
                    if(profile.type == AiProfil.AItype.AnimalNeutral ||profile.type == AiProfil.AItype.HumanNeutral)
                        ChangeState(State.Flee);
                    else
                        ChangeState(State.Chase);
                }
            break;
            case State.Chase:
                if(!target)
                {
                    print("Je reviens à mon point de départ");
                    ChangeState(State.BackToStart);
                }
                else
                {
                    if(Vector3.Distance(transform.position, target.position) <= equipment.attackDistance)
                                        ChangeState(State.Attack);
                    else
                        _agent.SetDestination(target.position);
                }
            break;
            case State.Attack:
                print("J'attaque");
                if(target.GetComponent<AIstats>().GetHealth() > 0 && Vector3.Distance(transform.position, target.position) > equipment.attackDistance)
                    ChangeState(State.Chase);
                else
                    Attack();
                if(target == null || target.GetComponent<AIstats>().GetHealth() <= 0)
                {
                    ChangeState(State.Roaming);
                }
            break;
            case State.BackToStart:
                if(Vector3.Distance(transform.position, _agent.destination) <= 2)
                    ChangeState(State.Roaming);
            break;
            case State.Flee:
                if(!target)
                {
                    target = null;
                    _agent.SetDestination(transform.position);
                    finishedTravel = true;
                    ChangeState(State.Roaming);
                }
                else
                {
                    Vector3 fleeDir = ((transform.position - target.transform.position)).normalized;
                    Vector3 fleeMotion = fleeDir * profile.fleeDistance;
                    destination = transform.position + fleeMotion;
                    _agent.SetDestination(destination);
                    finishedTravel = false;
                }
            break;
            case State.Wait:               
                GetComponent<Animator>().SetBool("IsWalking", false);
                if (target)
                {
                    if(profile.type == AiProfil.AItype.AnimalNeutral ||profile.type == AiProfil.AItype.HumanNeutral)
                        ChangeState(State.Flee);
                    else
                    {
                        ChangeState(State.Attack);
                    }
                        
                }
                WaitSequence();
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
            switch(equipment.equipmentLevel)
            {
                case BanditEquipmentLevel.Levels.Light:
                    
                    break;
                case BanditEquipmentLevel.Levels.Moderate:
                    RuntimeManager.PlayOneShot("event:/Weapons/BerettaShoot", transform.position);
                    break;
                case BanditEquipmentLevel.Levels.Heavy:
                    RuntimeManager.PlayOneShot("event:/Weapons/SKSShoot", transform.position);
                    break;
            }
            //Attack
            print("Attack");
            GetComponent<Animator>().Play("SlashOneHand");
            profile.actualAttackInterval = profile.attackInterval;
            GetComponent<BanditAttack>().Attack(target.position, equipment.attackDistance, equipment.damage);
        }
        else
        {
            profile.actualAttackInterval -= 1 * Time.deltaTime;
        }
    }
}
