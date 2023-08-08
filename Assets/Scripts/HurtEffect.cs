using System.Collections;
using UnityEngine;

public class HurtEffect : MonoBehaviour
{
    private SkinnedMeshRenderer skinnedMeshRenderer;
    private Material[] originalMaterials;
    public Color targetColor = Color.red;
    public float changeDuration = 1.0f;

    private void Start()
    {
        skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
        if (skinnedMeshRenderer != null)
        {
            originalMaterials = skinnedMeshRenderer.sharedMaterials;
        }
        else
        {
            Debug.LogError("SkinnedMeshRenderer component not found on the GameObject!");
        }
    }

    public void InitiateHurtEffect()
    {
        StartCoroutine(ChangeMaterialsCoroutine(targetColor, changeDuration));
    }

    private IEnumerator ChangeMaterialsCoroutine(Color targetColor, float duration)
    {
        Material[] currentMaterials = skinnedMeshRenderer.materials;
        Material[] newMaterials = new Material[currentMaterials.Length];

        for (int i = 0; i < currentMaterials.Length; i++)
        {
            newMaterials[i] = new Material(currentMaterials[i]);
            newMaterials[i].color = targetColor;
        }

        skinnedMeshRenderer.materials = newMaterials;
        yield return new WaitForSeconds(duration);

        skinnedMeshRenderer.materials = originalMaterials;
    }
}
