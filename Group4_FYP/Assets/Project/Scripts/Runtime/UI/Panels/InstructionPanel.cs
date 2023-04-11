using UnityEngine;
using PathOfHero.Controllers;

namespace PathOfHero.UI
{
    public class InstructionPanel : Panel
    {
        [SerializeField]
        private float m_DemoTimeLimit;

        public void OnReady()
        {
            var demoController = DemoController.Instance;
            if (demoController == null)
            {
                Debug.LogError("[Instruction Panel] Demo controller missing! Unable to start.");
                return;
            }
            demoController.StartDemo(m_DemoTimeLimit);
        }
    }
}
