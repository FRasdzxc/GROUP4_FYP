using System.Threading.Tasks;
using UnityEngine;

public abstract class AttackPattern : ScriptableObject
{
    public abstract Task Invoke(Transform origin);
}
