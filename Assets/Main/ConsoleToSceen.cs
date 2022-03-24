using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsoleToSceen : MonoBehaviour
{
    const int maxLines = 50;
    private string _logStr = "";

    private readonly List<string> _lines = new List<string>();
    void OnEnable() { Application.logMessageReceived += Log; }
    void OnDisable() { Application.logMessageReceived -= Log; }
    void Update() { }

    public void Log(string logString, string stackTrace, LogType type)
    {
        _lines.AddRange(logString.Split('\n'));
        if (_lines.Count > maxLines)
        {
            _lines.RemoveRange(0, _lines.Count - maxLines);
        }
        _logStr = string.Join("\n", _lines);
    }

    void OnGUI()
    {
        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity,
           new Vector3(Screen.width / 1200.0f, Screen.height / 800.0f, 1.0f));
        GUI.Label(new Rect(10, 10, 800, 370), _logStr, new GUIStyle());
    }
}
