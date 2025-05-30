using UnityEngine;
using UnityEditor;

public class DrawPileCardPlacerWindow : EditorWindow
{
    LevelLayoutAsset currentLevel;
    Vector2 scroll;
    const int cellSize = 50;

    [MenuItem("Tools/DrawPile / Card Placer")]
    public static void OpenWindow()
    {
        GetWindow<DrawPileCardPlacerWindow>("DrawPile / Card Placer");
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
        GUI.Box(previewRect, "DrawPile / Card 区域");

        // DrawPile 范围
        if (currentLevel.drawPileArea.width > 0 && currentLevel.drawPileArea.height > 0)
        {
            Rect pileRect = new Rect(
                previewRect.x + currentLevel.drawPileArea.x * cellSize,
                previewRect.y + currentLevel.drawPileArea.y * cellSize,
                currentLevel.drawPileArea.width * cellSize,
                currentLevel.drawPileArea.height * cellSize
            );
            EditorGUI.DrawRect(pileRect, new Color(0.9f, 0.9f, 0.3f, 0.2f));
            Handles.DrawSolidRectangleWithOutline(pileRect, Color.clear, Color.yellow);
        }

        // 遍历每个峰
        if (currentLevel.peaks != null)
        {
            
            foreach (var peak in currentLevel.peaks)
            {
                if (peak.peakTemplate == null || peak.peakTemplate.slots == null) continue;

                for (int i = 0; i < peak.peakTemplate.slots.Count; i++)
                {
                    var slot = peak.peakTemplate.slots[i];
                    
                    Vector2 offset = new Vector2(slot.x,slot.y) + slot.offsetPos;

                    // 精确位置 = 峰起始位置 + 偏移量（保持浮点精度）
                    Vector2 drawPos = new Vector2(
                        previewRect.x + (peak.position.x + offset.x) * cellSize,
                        previewRect.y + (peak.position.y + offset.y) * cellSize
                    );

                    Rect cardRect = new Rect(drawPos.x, drawPos.y, cellSize, cellSize);

                    // 绘制卡片背景
                    EditorGUI.DrawRect(cardRect, new Color(0.3f, 0.6f, 1f, 0.8f));
                    Handles.DrawSolidRectangleWithOutline(cardRect, Color.clear, Color.white);

                    // 显示卡牌编号（slot index）
                    GUIStyle style = new GUIStyle(EditorStyles.boldLabel);
                    style.normal.textColor = Color.white;
                    style.alignment = TextAnchor.MiddleCenter;
                    EditorGUI.LabelField(cardRect, i.ToString(), style);
                }
            }
        }

        EditorGUILayout.EndScrollView();
    }
}
