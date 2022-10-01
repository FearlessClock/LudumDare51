using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToPointer : MonoBehaviour
{
    [SerializeField] private CatMovement catMovement = null;
    private new Camera camera = null;

    private void Awake()
    {
        camera = FindObjectOfType<Camera>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 pos = camera.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0;
            catMovement.MoveToPointAndWaitForTime(pos, 3);
        }
    }
}
