using UnityEngine;
using UnityEditor;

/// <summary>
/// Script de inicialización automática para configurar la escena VR
/// Se ejecuta automáticamente al cargar Unity
/// </summary>
[InitializeOnLoad]
public static class AutoVRSetup
{
    static AutoVRSetup()
    {
        EditorApplication.delayCall += () =>
        {
            // Solo ejecutar una vez
            if (EditorPrefs.GetBool("VRAttention_AutoSetupDone", false))
                return;
            
            Debug.Log("[AutoVRSetup] Iniciando configuración automática...");
            
            // Crear materiales
            CreateMaterials();
            
            // Crear prefabs
            CreatePrefabs();
            
            // Marcar como completado
            EditorPrefs.SetBool("VRAttention_AutoSetupDone", true);
            
            Debug.Log("[AutoVRSetup] ✅ Configuración automática completada!");
        };
    }
    
    private static void CreateMaterials()
    {
        // Crear carpeta Materials
        if (!AssetDatabase.IsValidFolder("Assets/Materials"))
        {
            AssetDatabase.CreateFolder("Assets", "Materials");
        }
        
        // Material Blanco
        if (!System.IO.File.Exists("Assets/Materials/Blanco.mat"))
        {
            Material matBlanco = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            matBlanco.color = Color.white;
            matBlanco.SetFloat("_Smoothness", 0.5f);
            AssetDatabase.CreateAsset(matBlanco, "Assets/Materials/Blanco.mat");
            Debug.Log("[AutoVRSetup] Material Blanco creado");
        }
        
        // Material Negro
        if (!System.IO.File.Exists("Assets/Materials/Negro.mat"))
        {
            Material matNegro = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            matNegro.color = Color.black;
            matNegro.SetFloat("_Smoothness", 0.5f);
            AssetDatabase.CreateAsset(matNegro, "Assets/Materials/Negro.mat");
            Debug.Log("[AutoVRSetup] Material Negro creado");
        }
        
        AssetDatabase.SaveAssets();
    }
    
    private static void CreatePrefabs()
    {
        // Crear carpeta Prefabs
        if (!AssetDatabase.IsValidFolder("Assets/Prefabs"))
        {
            AssetDatabase.CreateFolder("Assets", "Prefabs");
        }
        
        // Cargar materiales
        Material matBlanco = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/Blanco.mat");
        Material matNegro = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/Negro.mat");
        
        if (matBlanco == null || matNegro == null)
        {
            Debug.LogError("[AutoVRSetup] No se pudieron cargar los materiales");
            return;
        }
        
        // Crear EstimuloBlanco
        if (!System.IO.File.Exists("Assets/Prefabs/EstimuloBlanco.prefab"))
        {
            GameObject estimuloBlanco = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            estimuloBlanco.name = "EstimuloBlanco";
            estimuloBlanco.transform.localScale = Vector3.one * 0.3f;
            estimuloBlanco.GetComponent<Renderer>().material = matBlanco;
            
            // Agregar componente Estimulo
            Estimulo estimuloComp = estimuloBlanco.AddComponent<Estimulo>();
            estimuloComp.Tipo = TipoEstimulo.Blanco;
            
            // Guardar como prefab
            PrefabUtility.SaveAsPrefabAsset(estimuloBlanco, "Assets/Prefabs/EstimuloBlanco.prefab");
            Object.DestroyImmediate(estimuloBlanco);
            Debug.Log("[AutoVRSetup] Prefab EstimuloBlanco creado");
        }
        
        // Crear EstimuloNegro
        if (!System.IO.File.Exists("Assets/Prefabs/EstimuloNegro.prefab"))
        {
            GameObject estimuloNegro = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            estimuloNegro.name = "EstimuloNegro";
            estimuloNegro.transform.localScale = Vector3.one * 0.3f;
            estimuloNegro.GetComponent<Renderer>().material = matNegro;
            
            // Agregar componente Estimulo
            Estimulo estimuloComp = estimuloNegro.AddComponent<Estimulo>();
            estimuloComp.Tipo = TipoEstimulo.Negro;
            
            // Guardar como prefab
            PrefabUtility.SaveAsPrefabAsset(estimuloNegro, "Assets/Prefabs/EstimuloNegro.prefab");
            Object.DestroyImmediate(estimuloNegro);
            Debug.Log("[AutoVRSetup] Prefab EstimuloNegro creado");
        }
        
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
    
    [MenuItem("VR Attention/Reset Auto Setup")]
    private static void ResetAutoSetup()
    {
        EditorPrefs.DeleteKey("VRAttention_AutoSetupDone");
        Debug.Log("[AutoVRSetup] Reset completado. Reinicia Unity para ejecutar el setup nuevamente.");
    }
}
