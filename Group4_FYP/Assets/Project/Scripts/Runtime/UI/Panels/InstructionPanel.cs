using UnityEngine;
using PathOfHero.Controllers;

namespace PathOfHero.UI
{
    public class InstructionPanel : Panel
    {
        [SerializeField]
        private string m_GameplaySceneToLoad;

        public void OnReady()
        {
            var sceneController = SceneController.Instance;
            if (sceneController == null)
            {
                Debug.LogError("[Instruction Panel] Scene controller missing! Unable to start.");
                return;
            }

            sceneController.ChangeScene(m_GameplaySceneToLoad, true);
        }
    }
}
