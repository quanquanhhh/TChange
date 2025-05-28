// PeakTemplate.cs
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "PeakTemplate", menuName = "Peak Editor/Peak Template")]
public class PeakTemplate : ScriptableObject {
    public int width = 5;
    public int height = 5;
    public bool[,] layout;
    public List<PeakSlot> slots = new List<PeakSlot>();

    public void Resize(int newWidth, int newHeight) {
        bool[,] newLayout = new bool[newWidth, newHeight];
        for (int x = 0; x < Mathf.Min(width, newWidth); x++) {
            for (int y = 0; y < Mathf.Min(height, newHeight); y++) {
                newLayout[x, y] = layout != null ? layout[x, y] : false;
            }
        }
        layout = newLayout;
        width = newWidth;
        height = newHeight;
    }

    public void ToggleSlot(int x, int y) {
        if (layout == null || layout.GetLength(0) != width || layout.GetLength(1) != height) {
            layout = new bool[width, height];
        }

        layout[x, y] = !layout[x, y];
        if (layout[x, y]) {
            int layer = height - y - 1; 
            slots.Add(new PeakSlot(x, y, layer));
        } else {
            slots.RemoveAll(slot => slot.x == x && slot.y == y);
        }
    }

}