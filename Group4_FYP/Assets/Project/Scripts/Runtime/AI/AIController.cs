using UnityEngine;
using PathOfHero.Characters;
using PathOfHero.Managers;

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

            var manager = CharacterManager.Instance;
            if (manager == null)
            {
                Debug.LogError("[AI Controller] Character Manager is missing.");
                enabled = false;
                return;
            }
            Transform playerTransform = manager.Player?.transform;
            if (playerTransform != null)
            {
                var playerDistance = Vector2.Distance(transform.position, playerTransform.position);
                if (playerDistance < m_Character.Profile.AttackDistance)
                {
                    // TODO: Attack the player
                }
                else if (playerDistance < m_Character.Profile.DetectionRadius)
                {
                    // Chase the player
                    var direction = (playerTransform.position - transform.position).normalized;
                    m_Character.Move(direction);
                }
            }
        }
    }
}
