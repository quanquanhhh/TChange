using System;
using System.Collections.Generic;
using System.IO;

namespace GameScripts.Core
{
    
    public class GameCreateOptions
    {
        // 角色Id
        [Obsolete("将在后续版本中删除，PersonaId将通过登录后的AuthToken获取")]
        public string PersonaId;
        // 存档所属命名空间, 不传默认为"Default"
        public string Namespace;
        // 存档描述
        public string Description;
        // 存档模式 不传默认为"Multi"
        public GameProgressType ProgressType;
        // 单进度存档进度值
        [Obsolete("将在后续版本中删除，如需记录进度值，可以在Properties中配置相应属性")]
        public int? ProgressNumber;
        // 存档属性
        public Dictionary<string, string> Properties;
        // 封面文件 (可选)
        public FileOptions Cover;
    }

    public class GameUpdateOptions
    {
        // 存档名称
        public string Name;
        // 存档所属命名空间
        [Obsolete("将在后续版本中删除，客户端将不再支持对Namespace进行修改")]
        public string Namespace;
        // 存档描述
        public string Description;
        // 单进度存档进度值
        [Obsolete("将在后续版本中删除，如需记录进度值，可以在Properties中配置相应属性")]
        public int? ProgressNumber;
        // 存档属性
        public Dictionary<string, string> Properties;
        // 存档文件（可选）
        public FileOptions File;
        // 封面文件 (可选)
        public FileOptions Cover;
    }

    public class SavePFPOptions
    {
        // 角色Id
        public string PersonaId;
    }
    
    public class GameFileOptions
    {
        // 更新文件方式，默认为Unknown， 代表不更新文件
        public GameUpdateFileWay UpdateFileWay;
        // 更新文件方式为 ByFilepath 时，需填写该项（不支持WebGL平台）
        public string Filepath;
        // 更新文件方式为 ByFileStream 时，需填写该项
        public Stream FileStream;
        // 更新文件方式为 ByFileBytes 时，需填写该项
        public byte[] FileBytes;
    }
    
    public class GameListOptions
    {
        // 存档所属命名空间列表
        public List<string> Namespaces; 
        // 角色Id
        public string PersonaId;
        // 存档模式, 不传或传 UNKNOWN 代表 list 所有
        public GameProgressType ProgressType;
        // 存档文件中是否包含封面
        public bool IncludeCover;
    }

    public enum GameProgressType
    {
        // 传此参数作为存档类型时，将自动替换为 MULTI
        Unknown = 0,
        // 单进度存档
        Linear = 1,
        // 多进度存档
        Multi = 2
    }

    public enum GameUpdateFileWay
    {
        Unknown = 0,
        ByFilePath = 1,
        ByFileStream = 2,
        ByFileBytes = 3
    }
    
    public enum ProfilePictureFormat
    {
        Unknown = 0,
        Jpeg = 1,
        Jpg = 2,
        Png = 3,
        Gif = 4,
        Svg = 5
    }
}