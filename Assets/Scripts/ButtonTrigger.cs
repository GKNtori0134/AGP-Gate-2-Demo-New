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

    private bool triggered = false;

    public void triggerTarget()
    {
        if (triggered) return; // 防止重复触发
        triggered = true;

        // ✅ 激活逻辑
        if (targetObject != null)
        {
            // 情况 1：使用单个 targetObject
            targetObject.activate();
        }
        else if (targetObjectsArray != null && targetObjectsArray.Length > 0)
        {
            // 情况 2：单个为空时，使用数组
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

        // ✅ 改变按钮材质视觉
        if (materials.Length != 0)
        {
            if (bounceTimes >= 0 && bounceTimes < materials.Length)
            {
                GetComponent<MeshRenderer>().material = materials[bounceTimes];
                if (anotherButtonComp != null)
                    anotherButtonComp.GetComponent<MeshRenderer>().material = materials[bounceTimes];
            }
        }
    }
}
