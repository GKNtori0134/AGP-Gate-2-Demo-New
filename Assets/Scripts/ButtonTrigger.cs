using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTrigger : MonoBehaviour
{
    [Header("Targets")]
    public TargetObject targetObject;          // 单个目标（可选）
    public TargetObject[] targetObjectsArray;  // 多个目标（可选）

    [Header("Visuals")]
    public GameObject anotherButtonComp;
    public Material[] materials;
    public int bounceTimes = 2;

    [Header("Hit VFX")]
    public GameObject hitVFX;           // VFX prefab to spawn when hit
    public bool destroyAfterHit = false; // 是否在触发后销毁按钮本体

    [Header("Hit VFX Settings")]
    public Vector3 vfxScale = Vector3.one; // default scale = 1


    private bool triggered = false;

    // Arrow will pass in its current bounceTimes
    public void triggerTarget()
    {
        if (triggered) return;
        triggered = true;

        // ---- Activate Targets ----
        if (targetObject != null)
        {
            targetObject.activate();
        }
        else if (targetObjectsArray != null && targetObjectsArray.Length > 0)
        {
            foreach (TargetObject t in targetObjectsArray)
            {
                if (t != null)
                    t.activate();
            }
        }
        else
        {
            Debug.LogWarning($"{name}: No target assigned to ButtonTrigger!");
        }

        // ---- Change Button Visual ----
        if (materials.Length != 0 &&
            bounceTimes >= 0 &&
            bounceTimes < materials.Length)
        {
            GetComponent<MeshRenderer>().material = materials[bounceTimes];

            if (anotherButtonComp != null)
                anotherButtonComp.GetComponent<MeshRenderer>().material = materials[bounceTimes];
        }
    }

    // Called by ArrowController after triggerTarget()
    public void PlayHitVFX(Vector3 hitPos)
    {
        if (hitVFX != null)
        {
            GameObject vfx = Instantiate(hitVFX, hitPos, Quaternion.identity);

            // Apply scale from Inspector
            vfx.transform.localScale = vfxScale;

            Destroy(vfx, 2f);
        }

        if (destroyAfterHit)
            Destroy(gameObject);
    }

}
