using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerZone : MonoBehaviour
{
    public BoxCollider2D Trigger;

    private MeshFilter _meshFilter;

    private void Awake()
    {
        if(Trigger == null)
        {
            Trigger = GetComponent<BoxCollider2D>();
        }

        SetUpSprite();

        Debug.Log("Scale: " + transform.localScale);
    }

    private void SetUpSprite()
    {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        sprite.enabled = true;

        sprite.size = Trigger.size;
    }

    private void SetUpQuad()
    {
        _meshFilter = GetComponent<MeshFilter>();
        // Get corners of trigger
        Vector3 bounds = Trigger.bounds.extents;
        Vector3 center = Trigger.bounds.center;

        Vector3 topLeft = new Vector3(-bounds.x, bounds.y, 0f);
        Vector3 topRight = new Vector3(bounds.x, bounds.y, 0f);
        Vector3 bottomRight = new Vector3(bounds.x, -bounds.y, 0f);
        Vector3 bottomLeft = new Vector3(-bounds.x, -bounds.y, 0f);

        Debug.Log(topLeft);
        Debug.Log(topRight);
        Debug.Log(bottomRight);
        Debug.Log(bottomLeft);

        // Set up quad with those corners
        Vector3[] vertices = new Vector3[4];
        Vector2[] uv  = new Vector2[4];
        int[] triangles = new int[6];

        vertices[0] = bottomLeft;
        vertices[1] = topLeft;
        vertices[2] = topRight;
        vertices[3] = bottomRight;

        uv[0] = new Vector2(0, 0);
        uv[1] = new Vector2(0, 1);
        uv[2] = new Vector2(1, 1);
        uv[3] = new Vector2(1, 0);

        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;
        triangles[3] = 0;
        triangles[4] = 2;
        triangles[5] = 3;

        // Create new mesh and set to mesh filter
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

        _meshFilter.mesh = mesh;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.gameObject.GetComponent<Player>().GetKilled();
    }
}
