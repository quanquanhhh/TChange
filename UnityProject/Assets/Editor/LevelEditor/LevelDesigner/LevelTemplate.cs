using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "LevelTemplate", menuName = "Peak Editor/Level Template")]
public class LevelTemplate : ScriptableObject {
    [System.Serializable]
    public class PeakInstance {
        public PeakTemplate template;
        public Vector2Int position;
    }

    public List<PeakInstance> peaks = new List<PeakInstance>();
}