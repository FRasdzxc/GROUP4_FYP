using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using PathOfHero.Characters.Data;
using PathOfHero.Managers;
using PathOfHero.Items;

namespace PathOfHero.Characters
{
    [RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(Animator))]
    public class Character : MonoBehaviour, IDamageable, IMovable
    {
        [SerializeField]
        private CharacterProfile m_CharacterProfile;

        private Rigidbody2D m_Rigidbody2D;
        private SpriteRenderer m_SpriteRenderer;
        private Animator m_Animator;

        private CharacterProfileRuntime m_Profile;

        private int m_Health;
        private int m_MaxHealth;
        private bool m_IsDead;

        private float m_LastDamageTime;
        private float m_NextStepTime;

        public int Health => m_Health;
        public int MaxHealth => m_MaxHealth;
        public CharacterProfileRuntime Profile => m_Profile;

        public UnityAction OnDeath;

        private void Awake()
        {
            m_Rigidbody2D = GetComponent<Rigidbody2D>();
            m_SpriteRenderer = GetComponent<SpriteRenderer>();
            m_Animator = GetComponent<Animator>();
        }

        private void Start()
        {
            if (m_CharacterProfile != null)
            {
                m_Profile = m_CharacterProfile.Build();
                if (m_Profile.MovementType == MovementType.None)
                    m_Rigidbody2D.bodyType = RigidbodyType2D.Static;
            }
            m_MaxHealth = m_Profile.BaseHealth;
            m_Health = m_Profile.BaseHealth;

            CharacterManager.Instance?.Register(this);
        }

        private void Update()
        {
            if (m_SpriteRenderer.color != Color.white && (Time.time - m_LastDamageTime) > m_Profile.DamageEffectTime)
                m_SpriteRenderer.color = Color.white;
        }

        private void OnDestroy()
            => CharacterManager.Instance?.Unregister(this);

        #region IDamageable
        public void TakeDamage(int amount)
        {
            Debug.Assert(amount > 0, "[IDamagable] Negative amount passed to TakeDamage()");
            m_Health = Mathf.Clamp(m_Health - amount, 0, m_MaxHealth);
            if (m_Health == 0)
            {
                Death();
                return;
            }

            m_SpriteRenderer.color = Color.red;
            m_LastDamageTime = Time.time;
        }

        public void Heal(int amount)
        {
            Debug.Assert(amount > 0, "[IDamagable] Negative amount passed to Heal()");
            m_Health = Mathf.Clamp(m_Health + amount, 0, m_MaxHealth);
        }

        public void Death()
        {
            m_Animator.SetTrigger("Death");
        }
        #endregion

        #region IMovable
        public void Move(Vector2 direction, bool sprint = false)
        {
            if (m_Health <= 0)
                return;

            switch (m_Profile.MovementType)
            {
                case MovementType.None:
                    break;
                case MovementType.Linear:
                    var moving = direction.magnitude > 0f;
                    if (moving)
                    {
                        float moveFactor = m_Profile.MoveSpeed;
                        if (sprint)
                            moveFactor *= m_Profile.SprintFactor;

                        m_Rigidbody2D.MovePosition(m_Rigidbody2D.position + Time.fixedDeltaTime * moveFactor * direction);
                        m_Animator.SetFloat("X", direction.x);
                        m_Animator.SetFloat("Y", direction.y);
                    }
                    m_Animator.SetBool("Moving", moving);
                    break;
                case MovementType.Step:
                    if (Time.time < m_NextStepTime)
                        break;

                    var newPos = m_Rigidbody2D.position + m_Profile.StepDistance * direction;
                    m_Animator.SetFloat("X", direction.x);
                    m_Animator.SetFloat("Y", direction.y);
                    m_Animator.SetTrigger("Step");
                    transform.DOMove(newPos, m_Profile.StepTime).SetDelay(m_Profile.StepStartDelay).SetEase(Ease.InOutSine);
                    m_NextStepTime = Time.time + m_Profile.StepStartDelay + m_Profile.StepTime;
                    break;
            }
        }
        #endregion

        public void PickUpItem(GameObject gameObject)
        {
            if (!m_Profile.CanPickupItems)
                return;

            if (gameObject.TryGetComponent<PointPickup>(out var point))
            {
                Debug.Log($"[Character] Collected {point.Type} x {point.Amount}");
                point.GetComponent<Rigidbody2D>().Sleep();
                point.DelayedDestroy();
            }
            else
                Debug.LogWarning($"[Character] Unknown item type for game object '{gameObject.name}'", gameObject);
        }
    }
}
