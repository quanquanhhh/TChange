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
            var totalGridHeight = 12; // Choose an appropriate height that fits your level design
            foreach (var peak in currentLevel.peaks)
            {
                if (peak.peakTemplate == null) continue;

                // Calculate draw position
                Vector2 pos = new Vector2(
                    previewRect.x + peak.position.x * cellSize,
                    previewRect.y + (totalGridHeight - peak.position.y - peak.peakTemplate.GetShowHeight()) * cellSize
                );

                float showHeight = peak.peakTemplate.GetShowHeight();
                Vector2 size = new Vector2(peak.peakTemplate.width * cellSize, showHeight * cellSize);
                Rect peakRect = new Rect(pos, size);

                if (draggingPeak == peak)
                {
                    // Calculate drag position
                    Vector2 newPos = Event.current.mousePosition + dragOffset;
                    Vector2 localPos = newPos - new Vector2(previewRect.x, previewRect.y);

                    // Calculate new grid position (rounding to nearest cell)
                    Vector2Int newGridPos = new Vector2Int(
                        Mathf.RoundToInt(localPos.x / cellSize),
                        totalGridHeight - Mathf.RoundToInt((localPos.y + showHeight * cellSize) / cellSize)
                    );

                    // Restrict to non-negative
                    newGridPos.x = Mathf.Max(0, newGridPos.x);
                    newGridPos.y = Mathf.Max(0, newGridPos.y);

                    // Check for overlap (excluding itself)
                    bool overlap = false;
                    foreach (var other in currentLevel.peaks)
                    {
                        if (other == draggingPeak) continue;
                        var showheight = other.peakTemplate.GetShowHeight();
                        Rect otherRect = new Rect(
                            previewRect.x + other.position.x * cellSize,
                            previewRect.y + (totalGridHeight - other.position.y - showheight) * cellSize,
                            other.peakTemplate.width * cellSize,
                            showheight * cellSize
                        );

                        Rect newRect = new Rect(
                            previewRect.x + newGridPos.x * cellSize,
                            previewRect.y + (totalGridHeight - newGridPos.y - showHeight) * cellSize,
                            size.x, size.y
                        );

                        if (newRect.Overlaps(otherRect))
                        {
                            overlap = true;
                            break;
                        }
                    }

                    // Allow movement if no overlap
                    if (!overlap)
                    {
                        peak.position = newGridPos;
                        peakRect.position = newPos;
                    }

                    Repaint();
                }

                EditorGUI.DrawRect(peakRect, new Color(0, 1, 1, 0.5f)); // Semi-transparent cyan
                // Draw white outline
                Color outlineColor = Color.white;
                float outlineThickness = 2f;
                
                Handles.DrawSolidRectangleWithOutline(peakRect, Color.clear, outlineColor);
                
                EditorGUI.LabelField(peakRect, peak.peakTemplate.name, EditorStyles.boldLabel);

                // Begin drag on mouse down
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
