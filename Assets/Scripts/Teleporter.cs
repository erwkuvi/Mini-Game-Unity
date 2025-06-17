using UnityEngine;
using System.Collections.Generic;

public class Teleporter : MonoBehaviour
{
    [Tooltip("Tag that links this teleporter with its destination. E.g., 'Teleporter_Red'")]
    public string teleporterTag;

    [Tooltip("Offset when teleporting (to avoid overlapping collider centers)")]
    public Vector3 teleportOffset = new Vector3(0, 1, 0);

    // Cooldown system
    private static Dictionary<GameObject, float> teleportCooldown = new Dictionary<GameObject, float>();
    private static float cooldownDuration = 0.5f;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // Check cooldown
        if (teleportCooldown.TryGetValue(other.gameObject, out float lastTime))
        {
            if (Time.time - lastTime < cooldownDuration)
                return;
        }

        Teleporter destination = FindDestination();
        if (destination != null && destination != this)
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.position = destination.transform.position + destination.teleportOffset;
                rb.linearVelocity = Vector3.zero;

                // Set cooldown on BOTH teleporters
                teleportCooldown[other.gameObject] = Time.time;
            }
        }
    }

    private Teleporter FindDestination()
    {
        Teleporter[] all = FindObjectsOfType<Teleporter>();
        foreach (Teleporter t in all)
        {
            if (t != this && t.teleporterTag == this.teleporterTag)
                return t;
        }
        return null;
    }
}
