using UnityEngine;

public class WaterClass : TargetObject
{
    [Header("Frozen Replacement")]
    public GameObject frozenWaterPrefab;   // Assign in Inspector

    private bool isFrozen = false;

    public override void activate()
    {
        if (isFrozen) return;
        isFrozen = true;

        if (frozenWaterPrefab == null)
        {
            Debug.LogError($"{name}: Frozen water prefab not assigned!");
            return;
        }

        // Spawn frozen version
        GameObject frozen = Instantiate(
            frozenWaterPrefab,
            transform.position,
            transform.rotation
        );

        // Preserve scale
        frozen.transform.localScale = transform.localScale;

        // Destroy original water
        Destroy(gameObject);
    }
}
