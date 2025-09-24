using UnityEngine;
using UnityEngine.AI;

public class MutantEnemyController : MonoBehaviour
{
    public Transform Target;
    public Animator Anim;
    public NavMeshAgent Agent;

    public float DetectionRadius = 5;
    public float AttackRadius = 1.5f;
    public float AttackDamage = 10;

    private bool isDead;
    private bool pathCalculated = true;
    private Vector3 startPosition;
    private NavMeshPath navMeshPath;

    void Start()
    {
        isDead = false;
        startPosition = transform.position;
        navMeshPath = new NavMeshPath();
    }

    void Update()
    {
        if (isDead)
            return;

        if (IsTargetInAttackRadius())
        {
            Attack();
        }
        else if (!Anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            if ((!Agent.hasPath || !CanReachPath()) && pathCalculated)
            {
                MoveToStartPosition();
            }
            else if (IsTargetInDetectionRadius())
            {
                MoveToTarget();
            }
            else
            {
                MoveToStartPosition();
            }
        }

        //Animation control
        Anim.SetBool("IsAttack", IsTargetInAttackRadius());
        Anim.SetBool("IsWalking", IsWalking());
    }

    private void Attack()
    {
        StopMoving();
    }

    private void MoveToTarget()
    {
        //TODO: Set agent to target destination
        Agent.SetDestination(Target.position);


        Agent.isStopped = false;
        pathCalculated = true;
    }

    private void MoveToStartPosition()
    {
        //TODO: Set agent to start destination
        Agent.SetDestination(startPosition);

        Agent.isStopped = false;
        pathCalculated = false;
    }

    private void TryDealDamageToTarget()
    {
        //TODO: Deal damage if target in attack radius
        if(IsTargetInAttackRadius() && Target.TryGetComponent(out Health targetHealth))
        {
            targetHealth.TakeDamage(AttackDamage);   
        }
    }

    private bool CanReachPath()
    {
        Agent.CalculatePath(Target.position, navMeshPath);
        if (navMeshPath.status != NavMeshPathStatus.PathComplete)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void TryDealDamageToTarget_AnimationTrigger()
    {
        TryDealDamageToTarget();
    }

    public bool IsTargetInDetectionRadius()
    {
        return Vector3.Distance(transform.position, Target.position) <= DetectionRadius;
    }

    public bool IsTargetInAttackRadius()
    {
        return Vector3.Distance(transform.position, Target.position) <= AttackRadius;
    }

    public void Dead()
    {
        Anim.SetTrigger("Dead");
        isDead = true;
        StopMoving();
    }

    public bool IsWalking()
    {
        return Agent.velocity.sqrMagnitude > 0f && !Agent.isStopped;
    }

    public bool IsReachDestination()
    {
        if (Agent.remainingDistance <= Agent.stoppingDistance)
        {
            if (!Agent.hasPath || Agent.velocity.sqrMagnitude <= 0f)
            {
                return true;
            }
        }
        return false;
    }

    public void StopMoving()
    {
        Agent.velocity = Vector3.zero;
        Agent.isStopped = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, DetectionRadius);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, AttackRadius);
    }
}
