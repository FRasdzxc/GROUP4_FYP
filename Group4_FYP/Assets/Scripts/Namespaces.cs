using UnityEngine;

namespace FYP
{
    public static class Common
    {
        public static Transform RecursiveFindChild(Transform parent, string childName)
        {
            foreach (Transform child in parent)
            {
                if (child.name == childName)
                {
                    return child;
                }
                else
                {
                    Transform child2 = RecursiveFindChild(child, childName);
                    if (child2 != null)
                    {
                        return child2;
                    }
                }
            }

            return null;
        }

        public static Transform RecursiveFindTag(Transform parent, string tagName)
        {
            foreach (Transform child in parent)
            {
                if (child.CompareTag(tagName))
                {
                    return child;
                }
                else
                {
                    Transform child2 = RecursiveFindChild(child, tagName);
                    if (child2 != null)
                    {
                        return child2;
                    }
                }
            }

            return null;
        }
    }
}
