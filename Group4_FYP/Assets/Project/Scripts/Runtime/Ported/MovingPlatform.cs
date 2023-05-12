using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;

public class MovingPlatform : MonoBehaviour
{
    [Serializable]
    public class posEntry
    {
        public Vector2 position;
        [Tooltip("Unit: seconds")]
        public float moveDuration;
        [Tooltip("Unit: seconds")]
        public float stayDuration;
        public Ease easeMode = Ease.Linear;
    }

    [SerializeField]
    private posEntry[] posEntries;

    private int currentPos = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (posEntries.Length <= 0)
        {
            Debug.LogError($"[MovingPlatform] ({gameObject}): posEntries is empty");
            return;
        }

        transform.position = posEntries[0].position;
        StartCoroutine(Move());
    }

    void OnDestroy()
        => StopCoroutine(Move());

    IEnumerator Move()
    {
        while (true)
        {
            foreach (posEntry entry in posEntries)
            {
                currentPos = currentPos + 1 >= posEntries.Length ? 0 : ++currentPos;
                yield return transform.DOMove(posEntries[currentPos].position, posEntries[currentPos].moveDuration).SetEase(posEntries[currentPos].easeMode).WaitForCompletion();
                yield return new WaitForSeconds(posEntries[currentPos].stayDuration);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision2D)
        => collision2D.transform.SetParent(transform);

    void OnCollisionExit2D(Collision2D collision2D)
        => collision2D.transform.SetParent(null);
}
