using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BuildHelper
{
    private static readonly Dictionary<BuildTargetGroup, List<BuildTarget>> k_BuildTargets = new()
    {
        {
            BuildTargetGroup.Standalone,
            new()
            {
#if UNITY_EDITOR_WIN
                BuildTarget.StandaloneWindows64
#elif UNITY_EDITOR_OSX
                BuildTarget.StandaloneOSX
#endif
            }
        }
    };


    private static readonly BuildPlayerOptions k_ProjectOptions = new BuildPlayerOptions()
    {
        scenes = new[]
        {
            "Assets/Project/Scenes/Bootstrap.unity",
            "Assets/Project/Scenes/InGameScene.unity",
            "Assets/Project/Scenes/Shared/Controllers.unity",
            "Assets/Scenes/StartScene.unity"
        }
    };

    public static void BuildPlayers()
    {
        foreach (var groups in k_BuildTargets)
        {
            foreach (var target in groups.Value)
            {
                var options = new BuildPlayerOptions()
                {
                    scenes = k_ProjectOptions.scenes,
                    targetGroup = groups.Key,
                    target = target,
                    locationPathName = $"Builds/{target}/{Application.productName}"
                };
                BuildPipeline.BuildPlayer(options);
            }
        }
    }
}
