using System.Collections;
using UnityEngine;

public class ScavengerNPC : MonoBehaviour
{
    [SerializeField] Animator animator;

    [Header("Vomit Settings")]
    [SerializeField] GameObject vomitSegmentPrefab;
    [SerializeField] Transform vomitSpawnPoint;
    [SerializeField] float vomitInterval = 0.5f;
    [SerializeField] float vomitDuration = 10f;
    [SerializeField] int totalTickLimit = 6; // Max ticks across all vomit segments, no massive stacking

    [Header("Flight Settings")]
    [SerializeField] float enterDuration = 1.5f;
    [SerializeField] float flyHeight = 3f;
    [SerializeField] float spawnZ = 50f;
    [SerializeField] float targetZ = 35f;

    [Header("Audio")]
    [SerializeField] AK.Wwise.Event vomitImpactSoundEvent;
    [SerializeField] AK.Wwise.Event scavengerLaughEvent;

    private Vector3 targetPosition;
    private bool hasLaughed = false;
    private int ticksUsed = 0;

    void Start()
    {
        Vector3 pos = transform.position;
        pos.y = flyHeight;
        pos.z = spawnZ;
        transform.position = pos;

        targetPosition = new Vector3(pos.x, flyHeight, targetZ);

        Debug.Log(">> Scavenger spawned at " + transform.position);
        StartCoroutine(HandleBehavior());
    }

    IEnumerator HandleBehavior()
    {
        yield return FlyToTarget();
        yield return SpewVomit();
        yield return FlyAway();
    }

    IEnumerator FlyToTarget()
    {
        Vector3 start = transform.position;
        float t = 0f;
        while (t < enterDuration)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(start, targetPosition, t / enterDuration);
            yield return null;
        }
        transform.position = targetPosition;
        Debug.Log(">> Scavenger in position");
    }

    IEnumerator SpewVomit()
    {
        Debug.Log(">> Scavenger vommiting...");
        float elapsed = 0f;
        while (elapsed < vomitDuration)
        {
            Vector3 spawnPos = vomitSpawnPoint != null ? vomitSpawnPoint.position : transform.position;

            GameObject segment = Instantiate(vomitSegmentPrefab, spawnPos, Quaternion.identity);

            VomitSegment vs = segment.GetComponent<VomitSegment>();
            if (vs != null)
            {
                vs.SetImpactSound(vomitImpactSoundEvent);
                vs.SetScavengerReference(this);
            }

            yield return new WaitForSeconds(vomitInterval);
            elapsed += vomitInterval;
        }
    }

    IEnumerator FlyAway()
    {
        float t = 0f;
        float duration = 1.5f;
        Vector3 start = transform.position;
        Vector3 end = start + new Vector3(0f, 6f, 0f);
        while (t < duration)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(start, end, t / duration);
            yield return null;
        }
        Debug.Log(">> Scavenger is flying away");
        PotionTracker.Instance?.SetScavengerActive(false);
        Destroy(gameObject);
    }

    public void PlayLaugh()
    {
        if (!hasLaughed)
        {
            scavengerLaughEvent?.Post(gameObject);
            hasLaughed = true;
            Debug.Log(">> Scavenger laughed at you!");
        }
    }

    public bool CanApplyTick()
    {
        if (ticksUsed < totalTickLimit)
        {
            ticksUsed++;
            return true;
        }
        return false;
    }
}
