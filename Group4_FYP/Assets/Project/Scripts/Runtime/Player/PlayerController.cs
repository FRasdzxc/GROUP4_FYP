using System.Collections;
using UnityEngine;
using PathOfHero.Controllers;
using PathOfHero.Characters;

namespace PathOfHero.Player
{
	public class PlayerController : MonoBehaviour
    {
		[SerializeField]
		private PlayerInputController m_InputController;

        [SerializeField]
        private Character m_ControllingCharacter;

        private Vector2 m_MoveDirection;
        private bool m_IsSprinting;

        private Coroutine m_DeathCoroutine;

        private void Start()
        {
            m_InputController.EnableInput(PlayerInputController.ActionType.Gameplay);
        }

        private void OnEnable()
        {
            m_InputController.MoveEvent += OnMove;
            m_InputController.SprintEvent += OnSprint;
            m_InputController.SprintCanceledEvent += OnSprintCanceled;
        }

        private void OnDisable()
        {
            m_InputController.MoveEvent -= OnMove;
            m_InputController.SprintEvent -= OnSprint;
            m_InputController.SprintCanceledEvent -= OnSprintCanceled;
        }

        private void FixedUpdate()
        {
            m_ControllingCharacter?.Move(m_MoveDirection, m_IsSprinting);
        }

        private void Update()
        {
            // Death
            if (m_ControllingCharacter.Health <= 0)
            {
                OnCharacterDeath();
                return;
            }
        }

        private void OnMove(Vector2 direction)
            => m_MoveDirection = direction;

        private void OnSprint()
            => m_IsSprinting = true;

        private void OnSprintCanceled()
            => m_IsSprinting = false;

        private void OnCharacterDeath()
        {
            if (m_DeathCoroutine != null)
                return;

            m_DeathCoroutine = StartCoroutine(DeathAnimation());
        }

        private IEnumerator DeathAnimation()
        {
            yield break;
        }
    }
}
