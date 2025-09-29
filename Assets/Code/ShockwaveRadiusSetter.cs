using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes;

public class ShockwaveRadiusSetter : MonoBehaviour
{
    public Disc disc;
    public Disc ring;
    public ParticleSystem particles;

    //private bool isShockwaveFired = false;

    public float startRadius;
    public float finalRadius;

    // Note, the final radius is supposed to be larger than the start radius. Start radius is supposed to be 0 originally.

    //Start is called before the first frame update
    void Start()
    {
        // Make sure these are turned off, if they are on already.
        disc.gameObject.SetActive(false);
        ring.gameObject.SetActive(false);
        particles.gameObject.SetActive(false);
        //myShapeModule = gameObject.GetComponent<ParticleSystem>().shape;

        // Add the necessary event to this object.
        //timer.OnPercentDoneChange.AddListener((float fractionComplete) => UpdateRadiiFromFractionComplete(fractionComplete));
    }

    //// Update is called once per frame
    //void Update()
    //{

    //}
    public void UpdateRadiiFromFractionComplete(float fractionComplete)
    {
        float convertedRadius = ConvertFractionDoneToRadius(fractionComplete);
        UpdateRadii(convertedRadius);
    }

    private float ConvertFractionDoneToRadius(float fractionComplete)
    {
        float convertedRadius = (finalRadius - startRadius) * fractionComplete + startRadius;
        return convertedRadius;
    }

    private void UpdateRadii(float radius)
    {
        //if (!this.isShockwaveFired)
        //{
        //    this.StartShockwave();
        //}
        Debug.Log($"Radius = {radius}");
        ring.Radius = radius;
        disc.Radius = Mathf.Max(radius-.25f,0);
        ParticleSystem.ShapeModule myShape = particles.shape;
        myShape.radius = radius;
    }
    public void StartShockwave()
    {
        Debug.Log("StartShockwave");
        // Turn the objects back on
        disc.gameObject.SetActive(true);
        ring.gameObject.SetActive(true);
        particles.gameObject.SetActive(true);
        //this.isShockwaveFired = true;
    }

}
