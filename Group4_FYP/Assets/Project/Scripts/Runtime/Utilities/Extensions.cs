using UnityEngine;

namespace PathOfHero.Utilities
{
    public static class Extensions
    {
        public static bool HasParameter(this Animator animator, string name)
        {
            foreach(var parameter in animator.parameters)
            {
                if (parameter.name == name)
                    return true;
            }

            return false;
        }
    }
}
