using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class EnemyPath : MonoBehaviour
{
    [SerializeField]
    private List<Transform> m_waypoints = new List<Transform>();

    [SerializeField]
    private bool m_alwaysDrawPath;
    [SerializeField]
    private bool m_drawAsLoop;
    [SerializeField]
    private bool m_drawNumbers;
    [SerializeField]
    private Color debugColour = Color.white;


#if UNITY_EDITOR
    public void DrawPath()
    {
        for (int i = 0; i < m_waypoints.Count; i++)
        {
            GUIStyle labelStyle = new GUIStyle();
            labelStyle.fontSize = 30;
            labelStyle.normal.textColor = debugColour;
            if (m_drawNumbers)
                Handles.Label(m_waypoints[i].position, i.ToString(), labelStyle);
            //Draw Lines Between Points.
            if (i >= 1)
            {
                Gizmos.color = debugColour;
                Gizmos.DrawLine(m_waypoints[i - 1].position, m_waypoints[i].position);

                if (m_drawAsLoop)
                {
                    Gizmos.DrawLine(m_waypoints[m_waypoints.Count - 1].position, m_waypoints[0].position);
                }
                   

            }
        }
    }
    public void OnDrawGizmos()
    {
        if (m_alwaysDrawPath)
        {
            DrawPath();
        }
    }

#endif
    public List<Transform> GetWaypoints()
    {
        return m_waypoints;
    }
}
