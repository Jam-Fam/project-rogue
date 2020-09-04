using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LinearBar : MonoBehaviour
{
    [SerializeField] private RectTransform fill;
    [Range(0, 100)]
    [SerializeField] private float progress = 100f;

    private void Update()
    {
        fill.localScale = new Vector3(1f, progress / 100, 1f);
    }
}
