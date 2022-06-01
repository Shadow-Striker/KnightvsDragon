using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonBody : MonoBehaviour
{
    [SerializeField] private int bodyLength;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Vector3[] segmentPositions;
    [SerializeField] private Transform targetDir;
    [SerializeField] private float targetDistance;
    private Vector3[] segmentVelocity;
    [SerializeField] private float smoothSpeed;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer.positionCount = bodyLength;
        segmentPositions = new Vector3[bodyLength];
        segmentVelocity = new Vector3[bodyLength];
    }

    // Update is called once per frame
    void Update()
    {
        segmentPositions[0] = Vector3.zero;
      //  print(targetDir.position);

        for (int i = 1; i < segmentPositions.Length; i++)
        {
            segmentPositions[i] = Vector3.SmoothDamp(segmentPositions[i], segmentPositions[i - 1] + targetDir.right * targetDistance, ref segmentVelocity[i], smoothSpeed);
        }
        lineRenderer.SetPositions(segmentPositions);
    }
}
