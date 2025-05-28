using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "LevelLayoutAsset", menuName = "Peak Editor/Level Layout")]
public class LevelLayoutAsset : ScriptableObject
{
    [System.Serializable]
    public class PeakInstance
    {
        public PeakTemplate peakTemplate;
        public Vector2Int position;  // 关卡中这个峰的位置（格子坐标）
    }

    public List<PeakInstance> peaks = new List<PeakInstance>();

    // 可以加一些辅助方法，比如添加/移除峰
    public void AddPeak(PeakTemplate peak, Vector2Int pos)
    {
        peaks.Add(new PeakInstance() { peakTemplate = peak, position = pos });
    }

    public void RemovePeak(PeakInstance instance)
    {
        peaks.Remove(instance);
    }
}   