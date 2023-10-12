using UnityEngine;

public class ConsoleToScreen : MonoBehaviour
{
    const int maxLines = 50;
    const int maxLineLength = 120;

    private string _logStr = "";
    private Vector2 _scrollPosition;
    public int fontSize = 15;

    private void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    private void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    private void HandleLog(string logString, string stackTrace, LogType type)
    {
        var lines = logString.Split('\n');
        foreach (var line in lines)
        {
            if (line.Length <= maxLineLength)
            {
                _logStr += line + "\n";
            }
            else
            {
                var startIndex = 0;
                while (startIndex < line.Length)
                {
                    var length = Mathf.Min(maxLineLength, line.Length - startIndex);
                    _logStr += line.Substring(startIndex, length) + "\n";
                    startIndex += maxLineLength;
                }
            }
        }

        if (_logStr.Split('\n').Length > maxLines)
        {
            var linesToRemove = _logStr.Split('\n').Length - maxLines;
            var firstNewLineIndex = _logStr.IndexOf('\n');
            _logStr = _logStr.Remove(0, firstNewLineIndex + 1);
        }
    }

    private void Update()
    {
        // 自动滚动到底部
        _scrollPosition.y = Mathf.Infinity;
    }

    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10f, 10f, Screen.width - 20f, Screen.height - 20f));
        _scrollPosition = GUILayout.BeginScrollView(_scrollPosition, GUIStyle.none, GUIStyle.none);

        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.fontSize = fontSize;
        GUILayout.Label(_logStr, style);

        GUILayout.EndScrollView();
        GUILayout.EndArea();
    }
}
