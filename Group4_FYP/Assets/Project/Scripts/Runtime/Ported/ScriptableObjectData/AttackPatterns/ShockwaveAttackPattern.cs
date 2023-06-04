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
    [Tooltip("How much Shockwaves when this Attack Pattern invokes.\nX = Min inclusive, Y = Max inclusive.")]
    public Vector2Int repeatCount;
    [Tooltip("Unit: seconds")]
    public float repeatInterval;

    [Header("Boss")]
    public float jumpHeight;
    public float jumpPower;

    public async override Task Invoke(Transform origin)
    {
        for (int i = 0; i < Random.Range(repeatCount.x, repeatCount.y + 1); i++)
        {
            // boss jumping
            await origin.DOLocalJump(origin.position, jumpPower, 1, 0.5f).AsyncWaitForCompletion();

            // spawn shockwave
            GameObject shockwaveClone = GameObject.Instantiate(shockwavePrefab, origin.position, Quaternion.identity);
            if (shockwaveClone.TryGetComponent<AreaEffector2D>(out AreaEffector2D ae2D))
            {
                Vector2 forceDir = GameObject.FindGameObjectWithTag("Player").transform.position - origin.position;
                ae2D.forceAngle = Mathf.Atan2(forceDir.y, forceDir.x) * Mathf.Rad2Deg;
            }

            // scale shockwave to expandFactor
            shockwaveClone.transform.DOScale(shockwaveClone.transform.localScale * expandFactor, expandDuration).SetEase(Ease.InSine);

            // destroy shockwave
            Destroy(shockwaveClone, expandDuration);
            
            await Task.Delay((int)(repeatInterval * 1000));
        }
    }
}
