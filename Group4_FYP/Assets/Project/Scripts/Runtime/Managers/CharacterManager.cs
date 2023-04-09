using System.Collections.Generic;
using UnityEngine;
using PathOfHero.Characters;
using PathOfHero.Utilities;

namespace PathOfHero.Managers
{
    // Naming this manager instead of controller because UnityEngine.CharacterController exists
    public class CharacterManager : Singleton<CharacterManager>
    {
        private List<Character> m_Characters;

        public Character Player { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            m_Characters = new List<Character>();
        }

        public void Register(Character character)
        {
            if (m_Characters.Contains(character))
                return;

            if (character.Profile.IsHero())
            {
                Debug.Assert(Player == null, "[Character Manager] More than one Hero exists.");
                Player = character;
            }

            m_Characters.Add(character);
        }

        public void Unregister(Character character)
        {
            if (!m_Characters.Contains(character))
                return;

            if (character == Player)
                Player = null;

            m_Characters.Remove(character);
        }
    }
}
