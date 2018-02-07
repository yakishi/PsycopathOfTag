using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif 

public class MuzzleType : ScriptableObject{


    [SerializeField]
    public List<GameObject> muzzTypeList = new List<GameObject>();

#if UNITY_EDITOR
    [MenuItem("Assets/Create/MuzzleTypeList")]
#endif
    static void CreateMuzzleTypeInstance()
    {
        MuzzleType muzzleType = CreateInstance<MuzzleType>();
#if UNITY_EDITOR
        string path = AssetDatabase.GenerateUniqueAssetPath("Assets/Resources/ScriptableObject/MuzzleTypeList.asset");
        AssetDatabase.CreateAsset(muzzleType, path);
        AssetDatabase.Refresh();
#endif
    }
}

