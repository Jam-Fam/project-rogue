using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectRogue.UI
{
    [ExecuteInEditMode]
    public class LinearBar : MonoBehaviour
    {
        [SerializeField] private RectTransform fill;
        [Range(0, 1)]
        public float Progress = 1;

        private void Update()
        {
            fill.localScale = new Vector3(1f, Progress, 1f);
        }
    }
}
