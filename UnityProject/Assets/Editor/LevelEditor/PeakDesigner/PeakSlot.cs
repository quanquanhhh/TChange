using UnityEngine;

[System.Serializable]
public class PeakSlot {
    public int x;
    public int y;
    public int layer;
    public Vector2 offsetPos;  // 新增偏移字段

    public PeakSlot(int x, int y, int layer = 0) {
        this.x = x;
        this.y = y;
        this.layer = layer;
        this.offsetPos = Vector2.zero; // 默认无偏移
    }
}