using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(AudioSource))]
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
        public AudioClip[] moveSound;
        public AudioClip[] stopSound;
    }

    [SerializeField]
    private posEntry[] posEntries;

    [SerializeField]
    private bool moveAutomatically = true;

    private int currentPos = 0;

    [SerializeField] [Tooltip("for debugging only, should be hidden from the inspector by default")]
    private bool canMove;

    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        if (posEntries.Length <= 0)
        {
            Debug.LogError($"[MovingPlatform] ({gameObject}): posEntries is empty");
            return;
        }

        audioSource = GetComponent<AudioSource>();
        if (!audioSource)
        {
            Debug.LogError($"[MovingPlatform] ({gameObject}): no audio source");
            return;
        }

        transform.position = posEntries[0].position;
        StartCoroutine(Move());
    }

    void Update()
    {
        if (moveAutomatically && !canMove)
            canMove = true;
    }

    void OnDestroy()
        => StopCoroutine(Move());

    IEnumerator Move()
    {
        while (true)
        {
            foreach (posEntry entry in posEntries)
            {
                while (!canMove)
                    yield return null;

                currentPos = currentPos + 1 >= posEntries.Length ? 0 : ++currentPos;

                if (posEntries[currentPos].moveSound.Length > 0)
                    audioSource.PlayOneShot(posEntries[currentPos].moveSound[UnityEngine.Random.Range(0, posEntries[currentPos].moveSound.Length)]);
                yield return transform.DOMove(posEntries[currentPos].position, posEntries[currentPos].moveDuration).SetEase(posEntries[currentPos].easeMode).WaitForCompletion();

                if (posEntries[currentPos].stopSound.Length > 0)
                    audioSource.PlayOneShot(posEntries[currentPos].stopSound[UnityEngine.Random.Range(0, posEntries[currentPos].stopSound.Length)]);
                yield return new WaitForSeconds(posEntries[currentPos].stayDuration);

                canMove = false;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision2D)
        => collision2D.transform.SetParent(transform);

    void OnCollisionExit2D(Collision2D collision2D)
        => collision2D.transform.SetParent(null);

    public void ActivateMove()
        => canMove = true;
}
