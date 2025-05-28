using System;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class LevelLayoutWindow : EditorWindow
{
    LevelLayoutAsset currentLevel;
    Vector2 scroll;
    Vector2 dragStartPos;
    LevelLayoutAsset.PeakInstance draggingPeak;
    Vector2 dragOffset;

    const int cellSize = 50;

    [MenuItem("Tools/Level Layout Designer")]
    public static void OpenWindow()
    {
        GetWindow<LevelLayoutWindow>("Level Layout Designer");
    }

    private void OnGUI()
{
    EditorGUILayout.Space();
    currentLevel = (LevelLayoutAsset)EditorGUILayout.ObjectField("当前关卡布局", currentLevel, typeof(LevelLayoutAsset), false);
    if (currentLevel == null)
    {
        EditorGUILayout.HelpBox("请先选择一个 LevelLayoutAsset 资源。", MessageType.Info);
        return;
    }

    EditorGUILayout.Space();

    scroll = EditorGUILayout.BeginScrollView(scroll);

    Rect previewRect = GUILayoutUtility.GetRect(800, 600);

    GUI.Box(previewRect, "布局预览区");

    if (Event.current.type == EventType.MouseUp)
    {
        draggingPeak = null;
    }

    if (currentLevel.peaks != null)
    {
        foreach (var peak in currentLevel.peaks)
        {
            if (peak.peakTemplate == null) continue;

            // 计算绘制区域，峰大小按模板宽高绘制
            Vector2 pos = new Vector2(previewRect.x + peak.position.x * cellSize, previewRect.y + peak.position.y * cellSize);

            
            float min = 0, max = 0;
            foreach (var slot in peak.peakTemplate.slots)
            {
                if (slot.offsetPos != Vector2.zero)
                {
                    float y = slot.offsetPos.y;
                    min = Math.Min(min, y);
                    max = Math.Max(max, y);
                }
            }
            var cellzize_H = 1 + (max - min);
            Vector2 size = new Vector2(peak.peakTemplate.width * cellSize, cellzize_H * cellSize);
            Rect peakRect = new Rect(pos, size);

            if (draggingPeak == peak)
            {
                // 计算拖动位置
                Vector2 newPos = Event.current.mousePosition + dragOffset;
                Vector2 localPos = newPos - new Vector2(previewRect.x, previewRect.y);

                // 计算新格子位置（四舍五入）
                Vector2Int newGridPos = new Vector2Int(
                    Mathf.RoundToInt(localPos.x / cellSize),
                    Mathf.RoundToInt(localPos.y / cellSize)
                );

                // 限制非负
                newGridPos.x = Mathf.Max(0, newGridPos.x);
                newGridPos.y = Mathf.Max(0, newGridPos.y);

                // 判断是否重叠（排除自己）
                bool overlap = false;
                foreach (var other in currentLevel.peaks)
                {
                    if (other == draggingPeak) continue;

                    Rect otherRect = new Rect(
                        previewRect.x + other.position.x * cellSize,
                        previewRect.y + other.position.y * cellSize,
                        other.peakTemplate.width * cellSize,
                        other.peakTemplate.height * cellSize
                    );

                    Rect newRect = new Rect(
                        previewRect.x + newGridPos.x * cellSize,
                        previewRect.y + newGridPos.y * cellSize,
                        size.x, size.y
                    );

                    if (newRect.Overlaps(otherRect))
                    {
                        overlap = true;
                        break;
                    }
                }

                // 如果不重叠，允许移动
                if (!overlap)
                {
                    peak.position = newGridPos;
                    peakRect.position = newPos;
                }

                Repaint();
            }

            EditorGUI.DrawRect(peakRect, new Color(0, 1, 1, 0.5f)); // 半透明青色
            // 再画白色描边
            Color outlineColor = Color.white;
            float outlineThickness = 2f;
            
            Handles.DrawSolidRectangleWithOutline(peakRect, Color.clear, outlineColor);
            
            EditorGUI.LabelField(peakRect, peak.peakTemplate.name, EditorStyles.boldLabel);

            // 鼠标按下开始拖动
            if (Event.current.type == EventType.MouseDown && peakRect.Contains(Event.current.mousePosition))
            {
                draggingPeak = peak;
                dragOffset = peakRect.position - Event.current.mousePosition;
                Event.current.Use();
            }
        }
    }

    EditorGUILayout.EndScrollView();

    if (GUI.changed)
    {
        EditorUtility.SetDirty(currentLevel);
    }
}

}
