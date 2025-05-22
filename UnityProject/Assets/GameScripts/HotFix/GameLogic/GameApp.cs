using System;
using System.Collections.Generic;
using System.Reflection;
using GameLogic;
using GameLogic.GameFSM;
using TEngine;
using Unity.UOS.CloudSave;

#pragma warning disable CS0436


/// <summary>
/// 游戏App。
/// </summary>
public partial class GameApp : Singleton<GameApp>
{
    private static List<Assembly> _hotfixAssembly;
    private Dictionary<Type, IModule> _modules;
    private Dictionary<Type, LogicBase> _logics;
    /// <summary>
    /// 热更域App主入口。
    /// </summary>
    /// <param name="objects"></param>
    public static void Entrance(object[] objects)
    {
        GameEventHelper.Init();
        _hotfixAssembly = (List<Assembly>)objects[0];
        
        Log.Warning("======= 看到此条日志代表你成功运行了热更新代码 =======");
        Log.Warning("======= Entrance GameApp =======");
        Utility.Unity.AddDestroyListener(Release);
        Instance.Init();
        Instance.Start();
        Instance.GameLogic();
        
        Log.Warning("======= Entrance GameApp - End =======");
        // StartGameLogic();
    }

    private async void GameLogic()
    {
        foreach (var logicSys in _logics)
        {
            logicSys.Value.OnLogicStart();
        }
            
        foreach (var module in _modules)
        {
            module.Value.OnStartGameLogic();
        }

        await CloudSaveSDK.InitializeAsync();
        Get<GameFsm>().StartState();
    }

    private void Start()
    {
        foreach (var logicSys in _logics)
        {
            logicSys.Value.OnStart();
        }
    }

    private void Init()
    {
        RegisterModule();
        RegisterLogicSys();
    }
    
    private async void RegisterModule()
    {
        // InstallSDKExtra();
            
        _modules = new Dictionary<Type, IModule>();
        var types = Utility.Assembly.GetTypes();
            
        foreach (var type in types)
        {
            if (typeof(IModule).IsAssignableFrom(type))
            {
                if (type.BaseType != null)
                {
                    var property = type.BaseType.GetProperty("Instance");
                    if (property != null)
                    {
                        var module = (IModule) property.GetValue(null);
                        module.InitializeModule();

                        _modules.Add(type, (IModule) property.GetValue(null));
                    }
                }
            }
        }
    }
    
    private void RegisterLogicSys()
    {
        _logics = new Dictionary<Type, LogicBase>();
            
        var types = Utility.Assembly.GetTypes();
            
        foreach (var type in types)
        {
            if (typeof(LogicBase).IsAssignableFrom(type))
            {
                _logics[type] = (LogicBase) Activator.CreateInstance(type);
                    
                _logics[type].InitializeOnCreate();
            }
        }
    }


    // private static void StartGameLogic()
    // {
    //     GameEvent.Get<ILoginUI>().ShowLoginUI();
    //     GameModule.UI.ShowUIAsync<BattleMainUI>();
    // }
    
    private static void Release()
    {
        SingletonSystem.Release();
        Log.Warning("======= Release GameApp =======");
    }
    public static T Get<T>() where T : LogicBase
    {
        if (Instance._logics.ContainsKey(typeof(T)))
        {
            return (T) Instance._logics[typeof(T)];
        }
        return null;
    }
    
}