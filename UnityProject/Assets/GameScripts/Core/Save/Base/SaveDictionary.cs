using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameScripts.Core
{
    [Serializable]
    public class SaveDictionary<TKey, TValue> : Dictionary<TKey, TValue>
    {
        
        public SaveDictionary(bool syncForce = false, bool syncForceRemote = false)
        {
            SyncForce = syncForce;
            SyncForceRemote = syncForceRemote;
        }
        bool SyncForce { get; set; }
        bool SyncForceRemote { get; set; }

        void OnStorageChanged()
        {
            SaveManager.Instance.LocalVersion++;
            if (SyncForce)
            {
                SaveManager.Instance.SyncForce = true;
            }
            if (SyncForceRemote)
            {
                SaveManager.Instance.SyncForceRemote = true;
            }
        }

        public new void Add(TKey key, TValue value)
        {
            base.Add(key, value);
            OnStorageChanged();
        }

        public new void Clear()
        {
            Debug.LogWarning("You are deleting some of the contents of the storage, please double check if you really want to do this");
            base.Clear();
            OnStorageChanged();
        }

        public new bool Remove(TKey key)
        {
            Debug.LogWarning("You are deleting some of the contents of the storage, please double check if you really want to do this");
            OnStorageChanged();
            return base.Remove(key);
        }

        public new TValue this[TKey key]
        {
            get
            {
                return base[key];
            }
            set
            {
                Debug.Assert(value != null);
                if (!base.ContainsKey(key) || !value.Equals(base[key]))
                {
                    base[key] = value;
                    OnStorageChanged();
                }
            }
        }
    }
}