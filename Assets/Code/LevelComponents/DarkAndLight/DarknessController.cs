using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarknessController : MonoBehaviour
{
    public float Radius = 1;
    public List<Transform> Locations;

    public SpriteRenderer Renderer;
    private Material _darknessMaterial;

    private void Awake()
    {
        _darknessMaterial = Renderer.material;
    }

    // Update is called once per frame
    void Update()
    {
        
        _darknessMaterial.SetFloat("_CircleRadius", Radius);

        List<Vector4> positions = new List<Vector4>();

        for (int i = 0; i <Locations.Count; i++)
        {
            positions.Add(Locations[i].position);
        }

        _darknessMaterial.SetVectorArray("_CirclePositions", positions);
    }
}
