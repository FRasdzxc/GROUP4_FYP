using System.Collections.Generic;
using UnityEngine;
using PathOfHero.Characters;

namespace PathOfHero.Items
{
    [RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(TrailRenderer))]
    public class PointPickup : MonoBehaviour
    {
        public static readonly Dictionary<PointType, Color> k_TypeColors = new()
        {
            { PointType.Health, new Color(0.89f, 0.1f, 0.13f) },
            { PointType.Mana, new Color(0.3f, 0.76f, 0.97f) },
            { PointType.Experience, Color.green },
            { PointType.Coins, new Color(0.98f, 0.75f, 0.17f) },
        };

        public enum PointType
        {
            Health,
            Mana,
            Experience,
            Coins
        }

        [SerializeField]
        private float m_MoveSpeed;
        public float MoveSpeed
        {
            get => m_MoveSpeed;
            set => m_MoveSpeed = value;
        }

        [SerializeField]
        [Range(0.1f, 2.0f)]
        private float m_PickupRange;
        public float PickupRange
        {
            get => m_PickupRange;
            set => m_PickupRange = value;
        }

        [SerializeField]
        private PointType m_Type;
        public PointType Type
        {
            get => m_Type;
            set
            {
                m_Type = value;

                var gradient = new Gradient()
                {
                    alphaKeys = new GradientAlphaKey[] { new(1f, 0f), new(0f, 1f) },
                    colorKeys = new GradientColorKey[] { new(k_TypeColors[m_Type], 0.7f), new(Color.black, 1f) },
                    mode = GradientMode.Blend
                };
                m_TrailRenderer.colorGradient = gradient;
                m_SpriteRenderer.color = k_TypeColors[m_Type];
            }
        }

        [SerializeField]
        private float m_Amount;
        public float Amount
        {
            get => m_Amount;
            set => m_Amount = value;
        }

        private Rigidbody2D m_Rigidbody2D;
        private SpriteRenderer m_SpriteRenderer;
        private TrailRenderer m_TrailRenderer;
        private Transform m_MoveTarget;

        private void Awake()
        {
            m_Rigidbody2D = GetComponent<Rigidbody2D>();
            m_SpriteRenderer = GetComponent<SpriteRenderer>();
            m_TrailRenderer = GetComponent<TrailRenderer>();
        }

        private void FixedUpdate()
        {
            if (m_MoveTarget == null)
                return;

            float distance = Vector2.Distance(transform.position, m_MoveTarget.position);
            if (distance <= m_PickupRange)
            {
                if (m_MoveTarget.TryGetComponent<Character>(out var character))
                    character.PickUpItem(gameObject);
            }
            else
                m_Rigidbody2D.AddForce((m_MoveTarget.position - transform.position).normalized * m_MoveSpeed);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (m_MoveTarget == null && collision.CompareTag("Player"))
                m_MoveTarget = collision.transform;
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (m_MoveTarget == collision.transform)
                m_MoveTarget = null;
        }

        public void DelayedDestroy()
            => Destroy(gameObject, m_TrailRenderer.time);
    }
}
