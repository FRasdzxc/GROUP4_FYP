using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(fileName = "New Shockwave Attack Pattern", menuName = "Game/Attack Patterns/Shockwave")]
public class ShockwaveAttackPattern : AttackPattern
{
    [Header("Shockwave")]
    public GameObject shockwavePrefab;
    [Tooltip("End scale of the Shockwave GameObject.")]
    public float expandFactor;
    [Tooltip("Unit: seconds")]
    public float expandDuration;
    [Tooltip("How much Shockwaves when this Attack Pattern invokes.")]
    public int repeatCount;
    [Tooltip("Unit: seconds")]
    public float repeatInterval;

    [Header("Boss")]
    public float jumpHeight;
    public float jumpPower;

    public async override Task Invoke(Transform origin)
    {
        for (int i = 0; i < repeatCount; i++)
        {
            // boss jumping
            await origin.DOLocalJump(origin.position, jumpPower, 1, 0.5f).AsyncWaitForCompletion();

            // spawn shockwave
            GameObject shockwaveClone = GameObject.Instantiate(shockwavePrefab, origin.position, Quaternion.identity);

            // scale shockwave to expandFactor
            shockwaveClone.transform.DOScale(shockwaveClone.transform.localScale * expandFactor, expandDuration).SetEase(Ease.InSine);

            // destroy shockwave
            Destroy(shockwaveClone, expandDuration);
            
            await Task.Delay((int)(repeatInterval * 1000));
        }
    }
}
