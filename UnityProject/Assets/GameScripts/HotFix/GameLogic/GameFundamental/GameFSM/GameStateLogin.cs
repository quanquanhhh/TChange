using System;
using System.Linq;
using System.Threading.Tasks;
using GameScripts;
using GameScripts.Net;
using GameScripts.UI.Login;
using Passport;
using Unity.Passport.Runtime;
using Unity.Passport.Runtime.UI;
using UnityEngine;

namespace GameLogic.GameFSM
{
    public class GameStateLogin : GameFsmStateBase
    {
        // sdk 配置（Config 是 SDK 初始化时的配置）
        private readonly PassportUIConfig _config = new()
        {
            AutoRotation = true, // 是否开启自动旋转，默认值为 false。
            InvokeLoginManually = false, // 是否通过自行调用 Login 函数启动登录面板，默认值为 false。
            Theme = PassportUITheme.Dark, // 风格主题配置。
            UnityContainerId = "unity-container" // WebGL 场景下 Unity 实例容器 Id。
        };

        public override async void OnEnter(params object[] inEnterParams)
        {
            base.OnEnter(inEnterParams);
            try
            {
                await PassportUI.Init(_config, _callback);
                GameModule.UI.ShowUI<UILogin>();
                // await NetworkManager.Login(username, password);
              
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                Debug.Log(e.Message);
                // ShowLoading(false);
                // MessageUI.Show(e.Message);
            }
        }
        
        // sdk 回调
        private async void _callback(PassportEvent e)
        {
            // event: 不同情况下的回调事件，详情可以参考下面的回调类型。
            switch (e)
            {
                case PassportEvent.RejectedTos:
                    Debug.Log("用户拒绝了协议");
                    break;
                case PassportEvent.LoggedIn:
                    Debug.Log("完成登录");
                    break;
                case PassportEvent.Completed:
                    Debug.Log("完成所有流程");
                    await SelectPersona();
                    break;
                case PassportEvent.LoggedOut:
                    Debug.Log("用户登出");
                    break;
            }
        }
        

        // 选择角色
        private async Task SelectPersona()
        {
            // 选择域
            var realms = await PassportSDK.Identity.GetRealms(); // 获取域列表
            var realmID = realms[0].RealmID; // 根据需要自行选择域
            // var realmID = "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"; // 也可以填写固定的 RealmID 而不是动态获取

            // 获取（或创建）与选择角色
            Persona persona = null;
            var personas = await PassportSDK.Identity.GetPersonas(); // 获取角色列表
            if (!personas.Any())
            {
                // 若没有角色，则新建角色
                persona = await PassportSDK.Identity.CreatePersona("YourDisplayName", realmID);
            }
            else
            {
                // 若有角色，则选择第一个角色
                persona = personas[0];
            }
            // 选择角色
            await PassportSDK.Identity.SelectPersona(persona.PersonaID);
        }
        
    }
}