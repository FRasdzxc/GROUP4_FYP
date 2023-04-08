using UnityEngine;
using PathOfHero.Characters;

namespace PathOfHero.AI
{
	[RequireComponent(typeof(Character))]
	public class AIController : MonoBehaviour
	{
        [SerializeField]
        private Collider2D m_Collider;

		private Character m_Character;

        private void Awake()
        {
            m_Character = GetComponent<Character>();
        }

        private void FixedUpdate()
        {
            if (m_Character.Health <= 0)
                return;

            // Detection
            float targetDistance = float.MaxValue;
            Transform target = null;
            foreach (var hit in Physics2D.OverlapCircleAll(transform.position, m_Character.Profile.DetectionRadius))
            {
                // Ignore self
                if (hit.gameObject == gameObject)
                    continue;

                // Ignore non-character objects
                if (!hit.TryGetComponent<Character>(out var character))
                    continue;

                // Ignore non-hero characters
                if (!character.Profile.IsHero())
                    continue;

                var colliderDistance = hit.Distance(m_Collider);
                if (colliderDistance.distance < targetDistance)
                {
                    targetDistance = colliderDistance.distance;
                    target = hit.transform;
                }
            }

            // Chase
            if (target != null && targetDistance > m_Character.Profile.AttackDistance)
            {
                var direction = (target.position - transform.position).normalized;
                m_Character.Move(direction);
            }
        }
    }
}
