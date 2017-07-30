using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class LaunchArcMesh : MonoBehaviour
{
    private Mesh mesh;
    public float meshWidth = .8f;
    public MeshRenderer arcRenderer;

    public float Angle;
    public int Resolution = 100;
    public float TargetDistance;
    public float DistanceMultiplier = 1f;

    private float g;
    private float radianAngle;
    private Vector3[] arcPosArray;
    private Vector3[] vertices;
    private int[] triangles;


    void Awake()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        g = Mathf.Abs(Physics2D.gravity.y);
    }

    void OnEnable()
    {
        arcRenderer.enabled = true;
    }

    void OnDisable()
    {
        mesh.Clear();
        arcRenderer.enabled = false;
    }

    void Start()
    {
        arcPosArray = new Vector3[(int)(DistanceMultiplier * Resolution + 1)];
        vertices = new Vector3[(int)(2 * (DistanceMultiplier * Resolution + 1))];
        triangles = new int[(int)(2 * Resolution * 6 * DistanceMultiplier)];
    }

    void Update()
    {
        RenderArcMesh(CalculateArcArray());
    }

    void RenderArcMesh(Vector3[] arcVerts)
    {
        mesh.Clear();
        for (int i = 0; i < Resolution* DistanceMultiplier + 1; ++i)
        {
            vertices[i*2] = new Vector3(meshWidth*0.5f, arcVerts[i].y, arcVerts[i].x);
            // The below coords can be switched.
            vertices[i*2+1] = new Vector3(meshWidth*-0.5f, arcVerts[i].y, arcVerts[i].x);
            if (i != (int)(Resolution*DistanceMultiplier))
            {
                triangles[i*12] = i * 2;
                triangles[i*12 + 1] = triangles[i*12 + 4] = i * 2 + 1;
                triangles[i*12 + 2] = triangles[i*12 + 3] = (i + 1) * 2;
                triangles[i*12 + 5] = (i+1)*2 + 1;

                triangles[i * 12 + 6] = i * 2;
                triangles[i * 12 + 7] = triangles[i * 12 + 10] = (i + 1) * 2;
                triangles[i * 12 + 8] = triangles[i * 12 + 9] = i * 2 + 1;
                triangles[i * 12 + 11] = (i + 1) * 2 + 1;
            }
            
        }
        mesh.vertices = vertices;
        mesh.triangles = triangles;

    }

    Vector3[] CalculateArcArray()
    {
        radianAngle = Mathf.Deg2Rad*Angle;
        float horizontalDistance = TargetDistance;
        float velocitySquared = horizontalDistance*g/Mathf.Sin(2*radianAngle);
        for (int i = 0; i < DistanceMultiplier * Resolution + 1; ++i)
        {
            float t = (float) i/(float) Resolution;
            arcPosArray[i] = CalculateArcPoint(t, velocitySquared, horizontalDistance);
        }
        return arcPosArray;
    }

    private Vector3 CalculateArcPoint(float t, float velocitySquared, float horizontalDistance)
    {
        float x = t*horizontalDistance;
        float y = x * Mathf.Tan(radianAngle) - (g * x * x) / (2 * velocitySquared * Mathf.Cos(radianAngle) * Mathf.Cos(radianAngle));
        return new Vector3(x, y);
    }
}
