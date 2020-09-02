using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayHealth : MonoBehaviour
{

    //This currently doesn't work beacuse there is no way to get the health value from core.health

    [SerializeField]
    Slider display;
    private RectTransform rt;

    [SerializeField]
    private ProjectRogue.Core.Health health;
    private void Awake()
    {
        rt = GetComponent<RectTransform>();
        display.maxValue = health.currentHealth;
    }
    void Update()
    {
        if (health == null)
        {
            Destroy(gameObject);
        }
        transform.position = (health.transform.position + Vector3.up * 1.5f);
        transform.rotation = Quaternion.identity;
        display.value = health.currentHealth;

    }
}
