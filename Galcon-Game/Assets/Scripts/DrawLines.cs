using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLines : MonoBehaviour
{
    public static DrawLines _instance;
    private LineRenderer _lineRenderer;
    public List<LineRenderer> _activeLines = new List<LineRenderer>();
    void Start()
    {
        _instance = this;
        _lineRenderer = GetComponent<LineRenderer>();
    }

    public void DrawLine(Transform origin, Transform target)
    {
        _lineRenderer.SetPosition(0, origin.position);
        _lineRenderer.SetPosition(1, target.position);
    }

    public void DrawFewLines(Transform origin, Transform targets)
    {
        LineRenderer line = Instantiate(_lineRenderer, origin.position, Quaternion.identity);
        line.SetPosition(0, origin.position);
        line.SetPosition(1, targets.position);
        _activeLines.Add(line);
    }

    public void ClearLines()
    {
        _lineRenderer.SetPosition(0, Vector3.zero);
        _lineRenderer.SetPosition(1, Vector3.zero);

        foreach (LineRenderer line in _activeLines)
        {
            // bug: this is not working
            Destroy(line.gameObject);
        }
        _activeLines.Clear();
    }
}
