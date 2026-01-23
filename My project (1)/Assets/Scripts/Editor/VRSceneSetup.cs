using UnityEngine;
using UnityEditor;

/// <summary>
/// Herramienta de editor para configurar automáticamente la escena VR
/// </summary>
public class VRSceneSetup : EditorWindow
{
    [MenuItem("VR Attention/Setup Scene")]
    public static void ShowWindow()
    {
        GetWindow<VRSceneSetup>("VR Scene Setup");
    }
    
    private void OnGUI()
    {
        GUILayout.Label("VR Attention Training - Scene Setup", EditorStyles.boldLabel);
        GUILayout.Space(10);
        
        if (GUILayout.Button("1. Create Materials", GUILayout.Height(30)))
        {
            CreateMaterials();
        }
        
        GUILayout.Space(5);
        
        if (GUILayout.Button("2. Create Stimulus Prefabs", GUILayout.Height(30)))
        {
            CreateStimulusPrefabs();
        }
        
        GUILayout.Space(5);
        
        if (GUILayout.Button("3. Setup VR Scene", GUILayout.Height(30)))
        {
            SetupVRScene();
        }
        
        GUILayout.Space(5);
        
        if (GUILayout.Button("4. Create UI Canvas", GUILayout.Height(30)))
        {
            CreateUICanvas();
        }
        
        GUILayout.Space(20);
        
        if (GUILayout.Button("⚡ Setup Everything", GUILayout.Height(40)))
        {
            CreateMaterials();
            CreateStimulusPrefabs();
            SetupVRScene();
            CreateUICanvas();
            Debug.Log("[VRSceneSetup] ✅ Setup completo!");
        }
    }
    
    private static void CreateMaterials()
    {
        // Crear carpeta Materials si no existe
        if (!AssetDatabase.IsValidFolder("Assets/Materials"))
        {
            AssetDatabase.CreateFolder("Assets", "Materials");
        }
        
        // Material Blanco
        Material matBlanco = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        matBlanco.color = Color.white;
        AssetDatabase.CreateAsset(matBlanco, "Assets/Materials/Blanco.mat");
        
        // Material Negro
        Material matNegro = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        matNegro.color = Color.black;
        AssetDatabase.CreateAsset(matNegro, "Assets/Materials/Negro.mat");
        
        AssetDatabase.SaveAssets();
        Debug.Log("[VRSceneSetup] ✅ Materiales creados");
    }
    
    private static void CreateStimulusPrefabs()
    {
        // Crear carpeta Prefabs si no existe
        if (!AssetDatabase.IsValidFolder("Assets/Prefabs"))
        {
            AssetDatabase.CreateFolder("Assets", "Prefabs");
        }
        
        // Cargar materiales
        Material matBlanco = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/Blanco.mat");
        Material matNegro = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/Negro.mat");
        
        if (matBlanco == null || matNegro == null)
        {
            Debug.LogError("[VRSceneSetup] Materiales no encontrados. Ejecuta 'Create Materials' primero.");
            return;
        }
        
        // Crear EstimuloBlanco
        GameObject estimuloBlanco = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        estimuloBlanco.name = "EstimuloBlanco";
        estimuloBlanco.transform.localScale = Vector3.one * 0.3f;
        estimuloBlanco.GetComponent<Renderer>().material = matBlanco;
        estimuloBlanco.AddComponent<Estimulo>();
        
        PrefabUtility.SaveAsPrefabAsset(estimuloBlanco, "Assets/Prefabs/EstimuloBlanco.prefab");
        DestroyImmediate(estimuloBlanco);
        
        // Crear EstimuloNegro
        GameObject estimuloNegro = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        estimuloNegro.name = "EstimuloNegro";
        estimuloNegro.transform.localScale = Vector3.one * 0.3f;
        estimuloNegro.GetComponent<Renderer>().material = matNegro;
        estimuloNegro.AddComponent<Estimulo>();
        
        PrefabUtility.SaveAsPrefabAsset(estimuloNegro, "Assets/Prefabs/EstimuloNegro.prefab");
        DestroyImmediate(estimuloNegro);
        
        AssetDatabase.SaveAssets();
        Debug.Log("[VRSceneSetup] ✅ Prefabs de estímulos creados");
    }
    
    private static void SetupVRScene()
    {
        // Crear GameManager
        GameObject gameManager = new GameObject("GameManager");
        
        // Agregar SesionVR
        SesionVR sesionVR = gameManager.AddComponent<SesionVR>();
        
        // Agregar componentes necesarios
        ArbolDecision arbolDecision = gameManager.AddComponent<ArbolDecision>();
        GestorDificultad gestorDificultad = gameManager.AddComponent<GestorDificultad>();
        
        // Crear EstimuloManager
        GameObject estimuloManagerObj = new GameObject("EstimuloManager");
        EstimuloManager estimuloManager = estimuloManagerObj.AddComponent<EstimuloManager>();
        estimuloManagerObj.transform.SetParent(gameManager.transform);
        
        // Crear SpawnArea
        GameObject spawnArea = new GameObject("SpawnArea");
        spawnArea.transform.position = new Vector3(0, 1.5f, 3f);
        spawnArea.transform.SetParent(estimuloManagerObj.transform);
        
        Debug.Log("[VRSceneSetup] ✅ Escena VR configurada. Asigna las referencias en el Inspector.");
    }
    
    private static void CreateUICanvas()
    {
        // Crear Canvas
        GameObject canvasObj = new GameObject("FeedbackCanvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.transform.position = new Vector3(0, 1.5f, 2f);
        canvas.transform.localScale = Vector3.one * 0.01f;
        
        canvasObj.AddComponent<UnityEngine.UI.CanvasScaler>();
        canvasObj.AddComponent<UnityEngine.UI.GraphicRaycaster>();
        
        // Crear InterfazRetroalimentacion
        InterfazRetroalimentacion interfaz = canvasObj.AddComponent<InterfazRetroalimentacion>();
        
        // Crear panel de métricas
        GameObject metricsPanel = new GameObject("MetricsPanel");
        metricsPanel.transform.SetParent(canvasObj.transform, false);
        UnityEngine.UI.VerticalLayoutGroup layout = metricsPanel.AddComponent<UnityEngine.UI.VerticalLayoutGroup>();
        layout.spacing = 20;
        layout.padding = new RectOffset(20, 20, 20, 20);
        
        RectTransform rectTransform = metricsPanel.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.sizeDelta = new Vector2(400, 300);
        
        Debug.Log("[VRSceneSetup] ✅ UI Canvas creado");
    }
}
