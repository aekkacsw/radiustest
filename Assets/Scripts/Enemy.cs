using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private static readonly int Idle = Animator.StringToHash("Idle");
    private static readonly int Walk = Animator.StringToHash("Walk");
    private static readonly int Attack = Animator.StringToHash("Attack");
    private static readonly int Die = Animator.StringToHash("Die");

    public Status status;
    public Transform startPoint;
    public bool isAttack;

    // Component
    private Animator m_Anim;
    private NavMeshAgent m_Agent;

    // Enemy
    public Transform m_Target;
    public GameObject UI;

    void Start()
    {
        m_Anim = GetComponent<Animator>();
        m_Agent = GetComponent<NavMeshAgent>();
        status.HP = status.MaxHP;
        // m_Agent.speed = status.Speed;
    }

    private void Update()
    {
        if (Vector3.Distance(startPoint.position, transform.position) >= 10)
        {
            if (isAttack)
            {
                CancelAttacking();
            }
            m_Anim.Play(Walk);
            m_Agent.SetDestination(startPoint.position);
        }
        else if (Vector3.Distance(m_Target.position, transform.position) > status.AtkRange)
        {
            if (!isAttack)
            {
                m_Anim.Play(Idle);
            }
        }

        Attacking();   
    }

    private void CancelAttacking()
    {
        isAttack = false;
    }

    private void Attacking()
    {
        if (Vector3.Distance(m_Target.position, transform.position) <= status.AtkRange)
        {
            if (!isAttack)
            {
                isAttack = true;
            }

            m_Anim.Play(Attack);
        }
        else
        {
            if (isAttack)
            {
                // CancelAttacking();
                m_Anim.Play(Walk);
                m_Agent.SetDestination(m_Target.position);
            }
        }
    }

    private void AttackTarget()
    {
        if (m_Target.TryGetComponent<Player>(out Player player) && Vector3.Distance(m_Target.position, transform.position) <= status.AtkRange)
        {   
            if (player.status.HP > 0)
            {
                player.status.HP -= status.Atk;

                Debug.Log("player " + player.status.HP);
            }
            else
            {
                player.SetDie();
                UI.SetActive(true);
                Debug.Log("Game End");
            }
        }
    }

    public void SetDie()
    {
        m_Anim.Play(Die);
    }
}