using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    private static readonly int Idle = Animator.StringToHash("Idle");
    private static readonly int Walk = Animator.StringToHash("Walk");
    private static readonly int Attack = Animator.StringToHash("Attack");
    private static readonly int Die = Animator.StringToHash("Die");

    public Status status;

    [SerializeField] private LayerMask m_Enemy;
    [SerializeField] private LayerMask m_Ground;

    // Component
    private Animator m_Anim;
    private NavMeshAgent m_Agent;

    // Enemy
    private Transform m_Target;
    public GameObject UI;

    void Start()
    {
        m_Anim = GetComponent<Animator>();
        m_Agent = GetComponent<NavMeshAgent>();
        status.HP = status.MaxHP;
        // m_Agent.speed = status.Speed;
    }

    // Update is called once per frame
    private void Update()
    {
        // Left Click on Enemy
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit, Mathf.Infinity, m_Enemy))
            {
                m_Target = hit.collider.transform;
                m_Anim.Play(Walk);

                m_Agent.SetDestination(m_Target.position);
            }
        }

        // Right Click on Ground
        if(Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, m_Ground))
            {
                m_Target = null;
                m_Anim.Play(Walk);

                m_Agent.SetDestination(hit.point);
            }
        }

        if (m_Target != null)
        {
            Attacking();
        }
    }

    private void Attacking()
    {
        if (Vector3.Distance(m_Target.position, transform.position) <= status.AtkRange)
        {
            m_Anim.Play(Attack);
        }
    }

    private void AttackTarget()
    {
        if (m_Target.TryGetComponent<Enemy>(out Enemy enemy) && Vector3.Distance(m_Target.position, transform.position) <= status.AtkRange)
        {
            if (enemy.status.HP > 0)
            {
                enemy.status.HP -= status.Atk;

                Debug.Log("enemy " + enemy.status.HP);
            }
            else
            {
                enemy.SetDie();
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