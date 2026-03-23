using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private Material laserMaterial;

    [Header("Laser Settings")]
    public float laserHeight = 50f;
    public float scrollSpeed = 5f;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();

        if (lineRenderer != null)
        {
            laserMaterial = lineRenderer.material;
        }
    }

    private void OnEnable()
    {
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, transform.position + Vector3.up * laserHeight);
    }

    private void Update()
    {
        if (laserMaterial == null) return;

        Vector2 currentOffset = laserMaterial.mainTextureOffset;

        currentOffset.y -= Time.deltaTime * scrollSpeed;

        laserMaterial.mainTextureOffset = currentOffset;
    }
}