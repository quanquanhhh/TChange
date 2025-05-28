using UnityEngine;
using UnityEditor;

public class PeakDesignerWindow : EditorWindow {
    PeakTemplate currentTemplate;
    Vector2 scroll;
    int gridSize = 40;

    // 当前选中的格子坐标，-1表示无选中
    int selectedX = -1;
    int selectedY = -1;

    [MenuItem("Tools/Peak Designer")]
    public static void OpenWindow() {
        GetWindow<PeakDesignerWindow>("Peak Designer");
    }

    private void OnGUI() {
        EditorGUILayout.Space();
        currentTemplate = (PeakTemplate)EditorGUILayout.ObjectField("当前模板", currentTemplate, typeof(PeakTemplate), false);

        if (currentTemplate == null) return;

        EditorGUILayout.Space();

        currentTemplate.width = EditorGUILayout.IntField("宽度", currentTemplate.width);
        currentTemplate.height = EditorGUILayout.IntField("高度", currentTemplate.height);
        if (GUILayout.Button("重置大小")) {
            currentTemplate.Resize(currentTemplate.width, currentTemplate.height);
            EditorUtility.SetDirty(currentTemplate);
        }

        EditorGUILayout.Space();

        scroll = EditorGUILayout.BeginScrollView(scroll);
        for (int y = currentTemplate.height - 1; y >= 0; y--) {
            EditorGUILayout.BeginHorizontal();
            for (int x = 0; x < currentTemplate.width; x++) {
                Color originalColor = GUI.backgroundColor;

                bool hasSlot = currentTemplate.layout != null && currentTemplate.layout[x, y];

                // 选中格子高亮
                if (x == selectedX && y == selectedY) {
                    GUI.backgroundColor = Color.yellow;
                } else {
                    GUI.backgroundColor = hasSlot ? Color.green : Color.gray;
                }

                Rect buttonRect = GUILayoutUtility.GetRect(gridSize, gridSize);

                Event e = Event.current;

                if (e.type == EventType.MouseDown && buttonRect.Contains(e.mousePosition)) {
                    if (e.button == 0) { // 左键 点击切换选中
                        if (hasSlot) {
                            selectedX = x;
                            selectedY = y;
                        } else {
                            currentTemplate.ToggleSlot(x, y);
                            selectedX = x;
                            selectedY = y;
                        }
                        EditorUtility.SetDirty(currentTemplate);
                        e.Use();
                    } else if (e.button == 1) { // 右键 删除选中
                        if (hasSlot) {
                            currentTemplate.ToggleSlot(x, y); // 关闭该slot
                            if (x == selectedX && y == selectedY) {
                                selectedX = -1;
                                selectedY = -1;
                            }
                            EditorUtility.SetDirty(currentTemplate);
                            e.Use();
                        }
                    }
                }

                // 按钮绘制（隐藏文本）
                GUI.Button(buttonRect, "");

                GUI.backgroundColor = originalColor;
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndScrollView();

        // 显示offset输入区，仅当当前格子有slot且选中时
        if (selectedX != -1 && selectedY != -1 && currentTemplate.layout != null && currentTemplate.layout[selectedX, selectedY]) {
            var slot = currentTemplate.slots.Find(s => s.x == selectedX && s.y == selectedY);
            if (slot != null) {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField($"调整偏移: ({selectedX}, {selectedY})");

                Vector2 newOffset = EditorGUILayout.Vector2Field("Offset Pos", slot.offsetPos);
                if (newOffset != slot.offsetPos) {
                    Undo.RecordObject(currentTemplate, "修改偏移");
                    slot.offsetPos = newOffset;
                    EditorUtility.SetDirty(currentTemplate);
                }
            }
        }
    }
}
