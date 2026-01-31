using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using TMPro;

/// <summary>
/// Herramienta para configurar la escena VR completa
/// </summary>
public class SceneConfigurator : EditorWindow
{
    [MenuItem("VR Attention/Configure Scene")]
    public static void ShowWindow()
    {
        GetWindow<SceneConfigurator>("Scene Configurator");
    }
    
    private void OnGUI()
    {
        GUILayout.Label("VR Attention Training - Scene Configuration", EditorStyles.boldLabel);
        GUILayout.Space(10);
        
        EditorGUILayout.HelpBox("Este asistente configurará automáticamente la escena VR con todos los componentes necesarios.", MessageType.Info);
        
        GUILayout.Space(10);
        
        if (GUILayout.Button("⚡ Configurar Escena Completa", GUILayout.Height(50)))
        {
            ConfigureCompleteScene();
        }
        
        GUILayout.Space(20);
        
        GUILayout.Label("Pasos individuales:", EditorStyles.boldLabel);
        
        if (GUILayout.Button("1. Crear GameManager", GUILayout.Height(30)))
        {
            CreateGameManager();
        }
        
        if (GUILayout.Button("2. Crear UI Canvas", GUILayout.Height(30)))
        {
            CreateUICanvas();
        }
        
        if (GUILayout.Button("3. Configurar Cámara VR", GUILayout.Height(30)))
        {
            ConfigureVRCamera();
        }
    }
    
    private static void ConfigureCompleteScene()
    {
        Debug.Log("[SceneConfigurator] Iniciando configuración completa de la escena...");
        
        CreateGameManager();
        CreateUICanvas();
        ConfigureVRCamera();
        
        // Guardar escena
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        EditorSceneManager.SaveOpenScenes();
        
        Debug.Log("[SceneConfigurator] ✅ Escena configurada completamente!");
        EditorUtility.DisplayDialog("Configuración Completa", 
            "La escena VR ha sido configurada exitosamente.\n\n" +
            "Siguiente paso:\n" +
            "1. Selecciona 'GameManager' en la jerarquía\n" +
            "2. Asigna las referencias en el Inspector\n" +
            "3. Configura tu dispositivo VR en Project Settings → XR Plug-in Management", 
            "Entendido");
    }
    
    private static void CreateGameManager()
    {
        // Verificar si ya existe
        if (GameObject.Find("GameManager") != null)
        {
            Debug.LogWarning("[SceneConfigurator] GameManager ya existe en la escena");
            return;
        }
        
        // Crear GameManager
        GameObject gameManager = new GameObject("GameManager");
        
        // Agregar SesionVR
        SesionVR sesionVR = gameManager.AddComponent<SesionVR>();
        
        // Agregar ArbolDecision
        ArbolDecision arbolDecision = gameManager.AddComponent<ArbolDecision>();
        
        // Agregar GestorDificultad
        GestorDificultad gestorDificultad = gameManager.AddComponent<GestorDificultad>();
        
        // Crear EstimuloManager como hijo
        GameObject estimuloManagerObj = new GameObject("EstimuloManager");
        estimuloManagerObj.transform.SetParent(gameManager.transform);
        EstimuloManager estimuloManager = estimuloManagerObj.AddComponent<EstimuloManager>();
        
        // Crear SpawnArea
        GameObject spawnArea = new GameObject("SpawnArea");
        spawnArea.transform.SetParent(estimuloManagerObj.transform);
        spawnArea.transform.position = new Vector3(0, 1.5f, 3f);
        
        // Cargar prefabs
        GameObject prefabBlanco = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/EstimuloBlanco.prefab");
        GameObject prefabNegro = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/EstimuloNegro.prefab");
        
        // Asignar referencias usando SerializedObject
        SerializedObject soEstimuloManager = new SerializedObject(estimuloManager);
        soEstimuloManager.FindProperty("estimuloBlancoPrefab").objectReferenceValue = prefabBlanco;
        soEstimuloManager.FindProperty("estimuloNegroPrefab").objectReferenceValue = prefabNegro;
        soEstimuloManager.FindProperty("spawnArea").objectReferenceValue = spawnArea.transform;
        soEstimuloManager.ApplyModifiedProperties();
        
        // Asignar referencias a SesionVR
        SerializedObject soSesionVR = new SerializedObject(sesionVR);
        soSesionVR.FindProperty("estimuloManager").objectReferenceValue = estimuloManager;
        soSesionVR.FindProperty("arbolDecision").objectReferenceValue = arbolDecision;
        soSesionVR.FindProperty("gestorDificultad").objectReferenceValue = gestorDificultad;
        soSesionVR.ApplyModifiedProperties();
        
        Selection.activeGameObject = gameManager;
        
        Debug.Log("[SceneConfigurator] ✅ GameManager creado y configurado");
    }
    
    private static void CreateUICanvas()
    {
        // Verificar si ya existe
        if (GameObject.Find("FeedbackCanvas") != null)
        {
            Debug.LogWarning("[SceneConfigurator] FeedbackCanvas ya existe en la escena");
            return;
        }
        
        // Crear Canvas
        GameObject canvasObj = new GameObject("FeedbackCanvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvasObj.AddComponent<UnityEngine.UI.CanvasScaler>();
        canvasObj.AddComponent<UnityEngine.UI.GraphicRaycaster>();
        
        // Posicionar canvas
        canvasObj.transform.position = new Vector3(0, 2f, 4f);
        canvasObj.transform.localScale = Vector3.one * 0.005f;
        
        RectTransform canvasRect = canvasObj.GetComponent<RectTransform>();
        canvasRect.sizeDelta = new Vector2(800, 600);
        
        // Agregar InterfazRetroalimentacion
        InterfazRetroalimentacion interfaz = canvasObj.AddComponent<InterfazRetroalimentacion>();
        
        // Crear Panel de Fondo
        GameObject bgPanel = new GameObject("BackgroundPanel");
        bgPanel.transform.SetParent(canvasObj.transform, false);
        UnityEngine.UI.Image bgImage = bgPanel.AddComponent<UnityEngine.UI.Image>();
        bgImage.color = new Color(0, 0, 0, 0.8f);
        RectTransform bgRect = bgPanel.GetComponent<RectTransform>();
        bgRect.anchorMin = Vector2.zero;
        bgRect.anchorMax = Vector2.one;
        bgRect.offsetMin = Vector2.zero;
        bgRect.offsetMax = Vector2.zero;
        
        // Crear Panel de Métricas
        GameObject metricsPanel = new GameObject("MetricsPanel");
        metricsPanel.transform.SetParent(canvasObj.transform, false);
        UnityEngine.UI.VerticalLayoutGroup layout = metricsPanel.AddComponent<UnityEngine.UI.VerticalLayoutGroup>();
        layout.spacing = 20;
        layout.padding = new RectOffset(40, 40, 40, 40);
        layout.childAlignment = TextAnchor.MiddleCenter;
        layout.childControlHeight = false;
        layout.childControlWidth = false;
        layout.childForceExpandHeight = false;
        layout.childForceExpandWidth = false;
        
        RectTransform metricsRect = metricsPanel.GetComponent<RectTransform>();
        metricsRect.anchorMin = Vector2.zero;
        metricsRect.anchorMax = Vector2.one;
        metricsRect.offsetMin = Vector2.zero;
        metricsRect.offsetMax = Vector2.zero;
        
        // Crear textos de métricas
        GameObject textoPrecision = CreateTextMeshPro("TextoPrecision", "Precisión: 0%", metricsPanel.transform);
        GameObject textoErrores = CreateTextMeshPro("TextoErrores", "Errores: 0", metricsPanel.transform);
        GameObject textoTiempo = CreateTextMeshPro("TextoTiempoPromedio", "Tiempo Promedio: 0s", metricsPanel.transform);
        GameObject textoNivel = CreateTextMeshPro("TextoNivelAtencion", "Nivel de Atención: Medio", metricsPanel.transform);
        
        // Crear feedback visual
        GameObject feedbackCorrecto = CreateFeedbackObject("FeedbackCorrecto", "CORRECTO", Color.green, canvasObj.transform);
        GameObject feedbackIncorrecto = CreateFeedbackObject("FeedbackIncorrecto", "INCORRECTO", Color.red, canvasObj.transform);
        
        feedbackCorrecto.SetActive(false);
        feedbackIncorrecto.SetActive(false);
        
        // Asignar referencias a InterfazRetroalimentacion
        SerializedObject soInterfaz = new SerializedObject(interfaz);
        soInterfaz.FindProperty("feedbackCorrecto").objectReferenceValue = feedbackCorrecto;
        soInterfaz.FindProperty("feedbackIncorrecto").objectReferenceValue = feedbackIncorrecto;
        soInterfaz.FindProperty("textoPrecision").objectReferenceValue = textoPrecision.GetComponent<TextMeshProUGUI>();
        soInterfaz.FindProperty("textoErrores").objectReferenceValue = textoErrores.GetComponent<TextMeshProUGUI>();
        soInterfaz.FindProperty("textoTiempoPromedio").objectReferenceValue = textoTiempo.GetComponent<TextMeshProUGUI>();
        soInterfaz.FindProperty("textoNivelAtencion").objectReferenceValue = textoNivel.GetComponent<TextMeshProUGUI>();
        soInterfaz.ApplyModifiedProperties();
        
        // Asignar a SesionVR
        SesionVR sesionVR = GameObject.FindFirstObjectByType<SesionVR>();
        if (sesionVR != null)
        {
            SerializedObject soSesionVR = new SerializedObject(sesionVR);
            soSesionVR.FindProperty("interfazRetroalimentacion").objectReferenceValue = interfaz;
            soSesionVR.ApplyModifiedProperties();
        }
        
        Debug.Log("[SceneConfigurator] ✅ UI Canvas creado y configurado");
    }
    
    private static GameObject CreateTextMeshPro(string name, string text, Transform parent)
    {
        GameObject textObj = new GameObject(name);
        textObj.transform.SetParent(parent, false);
        
        TextMeshProUGUI tmp = textObj.AddComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.fontSize = 36;
        tmp.color = Color.white;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.enableAutoSizing = false;
        
        RectTransform rect = textObj.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(700, 60);
        
        return textObj;
    }
    
    private static GameObject CreateFeedbackObject(string name, string text, Color color, Transform parent)
    {
        GameObject feedbackObj = new GameObject(name);
        feedbackObj.transform.SetParent(parent, false);
        
        TextMeshProUGUI tmp = feedbackObj.AddComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.fontSize = 72;
        tmp.color = color;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.fontStyle = FontStyles.Bold;
        
        RectTransform rect = feedbackObj.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.sizeDelta = new Vector2(600, 150);
        
        return feedbackObj;
    }
    
    private static void ConfigureVRCamera()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogWarning("[SceneConfigurator] No se encontró Main Camera");
            return;
        }
        
        GameObject cameraObj = mainCamera.gameObject;
        
        // Agregar VRInteractionHandler si no existe
        if (cameraObj.GetComponent<VRInteractionHandler>() == null)
        {
            cameraObj.AddComponent<VRInteractionHandler>();
            Debug.Log("[SceneConfigurator] ✅ VRInteractionHandler agregado a la cámara");
        }
        
        // Posicionar cámara
        cameraObj.transform.position = new Vector3(0, 1.6f, 0);
        cameraObj.transform.rotation = Quaternion.identity;
        
        Debug.Log("[SceneConfigurator] ✅ Cámara VR configurada");
    }
}
