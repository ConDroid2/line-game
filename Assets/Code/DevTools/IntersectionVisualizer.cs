using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntersectionVisualizer : MonoBehaviour
{
    [SerializeField] GameObject _visualizerPrefab;
    private List<GameObject> _activeVisualizers = new List<GameObject>();

    private Queue<GameObject> _inactiveVisualizers = new Queue<GameObject>();

    public bool VisualizersActive = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TurnOffVisualizers();
        if (VisualizersActive)
        {
            List<IntersectionData> intersections = LevelManager.Instance.GetAllIntersectionData();

            foreach(IntersectionData intersection in intersections)
            {
                GameObject visualizer = _inactiveVisualizers.Count > 0 ? _inactiveVisualizers.Dequeue(): Instantiate(_visualizerPrefab, transform);


                visualizer.transform.position = intersection.IntersectionWorldSpace;
                visualizer.SetActive(true);
                _activeVisualizers.Add(visualizer);
            }
        }
    }

    public void ToggleVisualizer(bool active)
    {
        VisualizersActive = active;

        if(VisualizersActive == false)
        {
            TurnOffVisualizers();
        }
    }

    private void TurnOffVisualizers()
    {
        foreach (GameObject visualizer in _activeVisualizers)
        {
            visualizer.SetActive(false);
            _inactiveVisualizers.Enqueue(visualizer);
        }

        _activeVisualizers.Clear();
    }
}
