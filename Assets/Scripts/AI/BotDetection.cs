using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector.Editor.Validation;
using UnityEngine;

public class BotDetection : MonoBehaviour
{
    public float viewRadius;
    [Range(0,360)]
    public float viewAngle;

    public LayerMask targetMask;

    public LayerMask obstacleMask;
    public Transform validTarget;
    public float height;
    public Collider[] targetsInViewRadius = new Collider[0];
    private AIBehaviour myBehaviour;

    private void Start()
    {
        myBehaviour = GetComponent<AIBehaviour>();
    }

    public Transform FindVisibleTargets()
    {
        targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);
        if (targetsInViewRadius.Length > 0)
        {
            for (int i = 0; i < targetsInViewRadius.Length; i++)
            {
                if(targetsInViewRadius[i].transform.root != transform)
                {
                    Transform target = targetsInViewRadius[i].transform;
                    Vector3 dirToTarget = (target.position - transform.position).normalized;
                    if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
                    {
                        if (myBehaviour.myTeam != null)
                        {
                            if (myBehaviour.myTeam != target.GetComponent<AIBehaviour>().myTeam)
                            {
                                float dstToTarget = Vector3.Distance(transform.position, target.position);
                                if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
                                {
                                    validTarget = target;
                                }
                            }
                        }
                        else
                        {
                            float dstToTarget = Vector3.Distance(transform.position, target.position);
                            if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
                            {
                                validTarget = target;
                            }
                        }
                    }
                
                    

                }
            }
        }
        else
        {
            validTarget = null;
        }

        return validTarget;
    }

    // Start is called before the first frame update
    public Vector3 DirFromAngle(float  angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0,Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
