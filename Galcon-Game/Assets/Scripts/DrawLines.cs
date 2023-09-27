using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLines : Singleton<DrawLines>
{
    [SerializeField] private LineRenderer _lineRendererPrefab;
    public List<LineRenderer> _activeLines = new List<LineRenderer>();
    
    public void DrawLine(Transform origin, Transform targets)
    {
        LineRenderer line = Instantiate(_lineRendererPrefab, origin.position, Quaternion.identity);
        line.SetPosition(0, origin.position);
        line.SetPosition(1, targets.position);
        _activeLines.Add(line);
    }

    public void ClearLines()
    {
        _lineRendererPrefab.SetPosition(0, Vector3.zero);
        _lineRendererPrefab.SetPosition(1, Vector3.zero);

        foreach (LineRenderer line in _activeLines)
        {
            // bug: this is not working
            Destroy(line.gameObject);
        }
        _activeLines.Clear();
    }
}
