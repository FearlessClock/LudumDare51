using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToPointerFox : MonoBehaviour
{
    [SerializeField] private FoxMovement foxMovement = null;
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
            foxMovement.MoveToPoint(pos, 3);
        }
    }
}
