using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace PathOfHero.UI
{
    public class CharacterEntry : MonoBehaviour
    {
        public static readonly List<char> k_Characters = new () { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };

        [SerializeField]
        private TMP_Text m_CharacterText;

        private int m_CurrentIndex;
        public int CurrentIndex
        {
            get => m_CurrentIndex;
            set
            {
                m_CharacterText.text = k_Characters[value].ToString();
                m_CurrentIndex = value;
            }
        }

        public char Character
        {
            get => k_Characters[m_CurrentIndex];
            set
            {
                var index = k_Characters.IndexOf(value);
                if (index == -1)
                {
                    Debug.LogWarning($"[Character Entry] Character '{value}' not accepted.");
                    return;
                }

                CurrentIndex = index;
            }
        }

        private void Start()
            => CurrentIndex = 0;

        public void PreviousCharacter()
            => CurrentIndex = (CurrentIndex + k_Characters.Count - 1) % k_Characters.Count;

        public void NextCharacter()
            => CurrentIndex = (CurrentIndex + 1) % k_Characters.Count;
    }
}
