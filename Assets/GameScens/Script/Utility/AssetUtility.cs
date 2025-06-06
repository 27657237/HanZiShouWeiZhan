//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using UnityEngine.U2D.IK;


public static class AssetUtility
    {
        public static string GetConfigAsset(string assetName)
        {
            return Utility.Text.Format("Assets/GameScens/Configs/{0}", assetName);
        }

        public static string GetDataTableAsset(string assetName)
        {
            return Utility.Text.Format("Assets/GameScens/DataTable/{0}", assetName);
        }
 

    
        public static string GetFontAsset(string assetName)
        {
            return Utility.Text.Format("Assets/GameScens/Fonts/{0}.ttf", assetName);
        }

        public static string GetSceneAsset(string assetName)
        {
            return Utility.Text.Format("Assets/GameScens/Scenes/{0}.unity", assetName);
        }

        public static string GetMusicAsset(string assetName)
        {
            return Utility.Text.Format("Assets/GameScens/Music/{0}.mp3", assetName);
        }

        public static string GetSoundAsset(string assetName)
        {
            return Utility.Text.Format("Assets/GameScens/Sounds/{0}.wav", assetName);
        }

        public static string GetEntityAsset(string assetName)
        {
            return Utility.Text.Format("Assets/GameScens/Entities/{0}.prefab", assetName);
        }

        public static string GetUIFormAsset(string assetName)
        {
            return Utility.Text.Format("Assets/GameScens/UI/UIForms/{0}.prefab", assetName);
        }

        public static string GetUISoundAsset(string assetName)
        {
            return Utility.Text.Format("Assets/GameScens/UI/UISounds/{0}.wav", assetName);
        }
    }

