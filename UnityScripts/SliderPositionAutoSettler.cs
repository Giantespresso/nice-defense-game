using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderPositionAutoSettler : MonoBehaviour {
    [SerializeField]
    private Vector3 distance = Vector3.down * 20.0f;
    private Transform targetTransform;
    private RectTransform rectTransform; //Inhereted from transform but also has anchoring property

    public void Setup(Transform target) {
        //Setting the target for the slider UI to follow
        targetTransform = target;
        rectTransform = GetComponent<RectTransform>();
    }

    private void LateUpdate() {
        //If the enemy dies, delete the slider UI
        if (targetTransform == null) {
            Destroy(gameObject);
            return;
        }

        //Get the enemy object's position through object's world position
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(targetTransform.position);
        //enemy object's position + distance = slider UI's position
        rectTransform.position = screenPosition + distance;
    }
}
