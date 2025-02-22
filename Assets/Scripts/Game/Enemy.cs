using System.Collections;
using System.Diagnostics.Tracing;
using UnityEngine;

namespace Scripts.Game
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    public class Enemy : MonoBehaviour
    {
        private enum DirectionType
        {
            Down, Left, Right
        }

        private enum EnemyState
        {
            Stopped, Left, Right, Down
        }

        [SerializeField] private EnemyState m_State;
        private EnemyState State
        {
            get => m_State;
            set
            {
                if (value != m_State)
                {
                    m_State = value;
                    UpdateVelocity();
                }
            }
        }
        private EnemyState m_NextState;
        private EnemyPool m_Pool;
        public EnemyPool Pool { get => m_Pool; set => m_Pool = value; }

        private Rigidbody2D m_Rigidbody2D;
        private Collider2D m_Collider;
        private float m_SizeCheckStart;
        private float m_OffsetFactor = 1.2f;
        private float m_RayLength = 0.5f;
        private float m_Speed = 5f;

        private float m_DumbTime = 2f;
        private WaitForSeconds m_DumbWaitForSeconds;

        private void Awake()
        {
            m_Rigidbody2D = GetComponent<Rigidbody2D>();
            m_Collider = GetComponent<Collider2D>();

            m_SizeCheckStart = m_OffsetFactor * m_Collider.bounds.extents.y;
            m_DumbWaitForSeconds = new WaitForSeconds(m_DumbTime);
        }

        public void FixedUpdate()
        {
            CheckDirectioAndUpdateState();
        }

        private void CheckDirectioAndUpdateState()
        {
            switch (State)
            {
                case EnemyState.Stopped:
                    break;
                case EnemyState.Left:
                    if (CheckDirection(DirectionType.Down))
                    {
                        State = EnemyState.Down;
                    }
                    else if (!CheckDirection(DirectionType.Left))
                    {
                        m_NextState = EnemyState.Right;
                        State = EnemyState.Stopped;
                    }
                    break;
                case EnemyState.Right:
                    if (CheckDirection(DirectionType.Down))
                    {
                        State = EnemyState.Down;
                    }
                    else if (!CheckDirection(DirectionType.Right))
                    {
                        m_NextState = EnemyState.Left;
                        State = EnemyState.Stopped;
                    }
                    break;
                case EnemyState.Down:
                    if (!CheckDirection(DirectionType.Down))
                    {
                        State = EnemyState.Stopped;
                    }
                    break;
            }
        }

        private bool CheckDirection(DirectionType directionType)
        {
            Vector3 directionToCheck = Vector3.zero;
            Vector3 offset = Vector3.zero;

            switch (directionType)
            {
                case DirectionType.Down:
                    directionToCheck = Vector3.down;
                    offset = Vector3.left;
                    break;
                case DirectionType.Left:
                    directionToCheck = Vector3.left;
                    offset = Vector3.up;
                    break;
                case DirectionType.Right:
                    directionToCheck = Vector3.right;
                    offset = Vector3.up;
                    break;
            }

            bool checkDownPoint1 = CheckFreePoint(transform.position + m_SizeCheckStart * directionToCheck + m_SizeCheckStart * offset, directionToCheck);
            bool checkDownPoint2 = CheckFreePoint(transform.position + m_SizeCheckStart * directionToCheck, directionToCheck);
            bool checkDownPoint3 = CheckFreePoint(transform.position + m_SizeCheckStart * directionToCheck + m_SizeCheckStart * -1f * offset, directionToCheck);

            return (checkDownPoint1 && checkDownPoint2 && checkDownPoint3);
        }

        private bool CheckFreePoint(Vector2 position, Vector2 direction)
        {
            #if UNITY_EDITOR
            Debug.DrawRay(position, m_RayLength * direction, Color.red);
            #endif
            
            var hit = Physics2D.Raycast(position, direction, m_RayLength);

            if (null == hit.transform)
            {
                return true;
            }
            else
            {
                return !((hit.transform.tag == Constants.TAG_BRICK) || (hit.transform.tag == Constants.TAG_LIMIT));
            }
        }

        private void UpdateVelocity()
        {
            switch (State)
            {
                case EnemyState.Stopped:
                    m_Rigidbody2D.velocity = Vector2.zero;
                    StartCoroutine(DumbTimeAndGoSide());
                    break;
                case EnemyState.Left:
                    m_Rigidbody2D.velocity = -1f * m_Speed * Vector2.right;
                    break;
                case EnemyState.Right:
                    m_Rigidbody2D.velocity = m_Speed * Vector2.right;
                    break;
                case EnemyState.Down:
                    m_Rigidbody2D.velocity = m_Speed * Vector2.down;
                    break;
            }
        }

        IEnumerator DumbTimeAndGoSide()
        {
            yield return m_DumbWaitForSeconds;
            if (m_NextState != EnemyState.Stopped)
            {
                State = m_NextState;
                m_NextState = EnemyState.Stopped;
            }
            else
            {
                int sideRandom = Random.Range(0, 2);
                State = (sideRandom == 0 ? EnemyState.Left : EnemyState.Right);
            }
        }

        public void Activate()
        {
            State = EnemyState.Down;
            m_NextState = EnemyState.Stopped;
        }

        public void Release()
        {
            State = EnemyState.Stopped;
            Pool.ReturnToPool(this);
        }
    }
}
