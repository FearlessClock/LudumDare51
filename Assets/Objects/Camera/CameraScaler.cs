using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScaler : MonoBehaviour
{
    private new Camera camera = null;
    private Transform[] players = new Transform[0];
    [SerializeField] private float moveStepTime = 0.1f;

    private float width = 0;
    private float height = 0;
    [SerializeField] private float edgeOffset = 2f;
    [SerializeField] private float minimumSize = 5f;
    private bool hasLoaded = false;

    private void Awake()
    {
        camera = GetComponent<Camera>();
        width = (camera.ViewportToWorldPoint(new Vector2(0,1)) - camera.ViewportToWorldPoint(Vector2.one)).magnitude;
        height = (camera.ViewportToWorldPoint(new Vector2(0,0)) - camera.ViewportToWorldPoint(new Vector2(0,1))).magnitude;
    }

    private IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        hasLoaded = true;
        PlayerHandler[] playerHandlers = FindObjectsOfType<PlayerHandler>();
        players = new Transform[playerHandlers.Length];
        for (int i = 0; i < playerHandlers.Length; i++)
        {
            players[i] = playerHandlers[i].transform;
        }
    }

    private void Update()
    {
        if (!hasLoaded) return;
        Vector3 centerOfGravity = Vector3.zero;

        float minX = 0;
        float maxX = 0;

        float minY = 0;
        float maxY = 0;

        for (int i = 0; i < players.Length; i++)
        {
            minX = Mathf.Min(minX, players[i].position.x);
            maxX = Mathf.Max(maxX, players[i].position.x);
            minY = Mathf.Min(minY, players[i].position.y);
            maxY = Mathf.Max(maxY, players[i].position.y);

            centerOfGravity += players[i].position;
        }
        if(centerOfGravity != Vector3.zero)
        {
            centerOfGravity /= players.Length;
        }
        centerOfGravity.z = -10;
        camera.transform.DOMove(centerOfGravity, moveStepTime);
        float newHeight = maxY - minY;
        float newWidth = maxX - minX;
        float orthoSize = camera.orthographicSize;
        if (newHeight > newWidth)
        {
            if(newHeight != 0)
            {
                orthoSize = Mathf.Max(CalculateSizeHeight(newHeight + edgeOffset), minimumSize);
            }
        }
        else
        {
            if(newWidth != 0)
            {
                orthoSize = Mathf.Max(CalculateSizeWidth(newWidth + edgeOffset * camera.aspect), minimumSize);
            }
        }

        if (orthoSize != camera.orthographicSize)
        {
            camera.DOKill();
            camera.DOOrthoSize(orthoSize, 1);
        }
        width = (camera.ViewportToWorldPoint(new Vector2(0, 1)) - camera.ViewportToWorldPoint(Vector2.one)).magnitude;
        height = (camera.ViewportToWorldPoint(new Vector2(0, 0)) - camera.ViewportToWorldPoint(new Vector2(0, 1))).magnitude;

    }

    private float CalculateSizeWidth(float newWidth)
    {
        return camera.orthographicSize * newWidth / width;
    }

    private float CalculateSizeHeight(float newHeight)
    {
        return camera.orthographicSize * newHeight / height;
    }

    private void OnDrawGizmos()
    {
        if(camera == null)
        {
            camera=GetComponent<Camera>();
        }
        Vector3 centerOfGravity = Vector3.zero;
        Gizmos.color = Color.green;
        float minX = 0;
        float maxX = 0;

        float minY = 0;
        float maxY = 0;

        for (int i = 0; i < players.Length; i++)
        {
            minX = Mathf.Min(minX, players[i].position.x);
            maxX = Mathf.Max(maxX, players[i].position.x);
            minY = Mathf.Min(minY, players[i].position.y);
            maxY = Mathf.Max(maxY, players[i].position.y);

            Gizmos.DrawLine(players[i].position, Vector3.zero);
            centerOfGravity += players[i].position;
        }

        Debug.DrawLine(new Vector2(minX, minY), new Vector2(maxX, maxY));
        if(players.Length > 0)
        {
            centerOfGravity /= players.Length;
        }
        Gizmos.color = Color.red;
        Gizmos.DrawLine(centerOfGravity, Vector3.zero);
    }
}
