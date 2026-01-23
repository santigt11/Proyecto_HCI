# Guía de Configuración de la Escena VR

## ✅ Progreso Actual

**Completado:**
- ✅ Estructura de carpetas creada
- ✅ Scripts C# implementados (9 archivos)
- ✅ Materiales creados (Blanco.mat, Negro.mat)
- ✅ Prefabs creados (EstimuloBlanco, EstimuloNegro)
- ✅ Herramienta de configuración automática lista

## 🎯 Siguiente Paso: Configurar la Escena

### Opción 1: Configuración Automática (Recomendado)

1. En Unity, ve al menú: **VR Attention → Configure Scene**
2. Haz clic en **"⚡ Configurar Escena Completa"**
3. Espera el mensaje de confirmación

### Opción 2: Configuración Manual

Si la herramienta automática no funciona, sigue estos pasos:

#### 1. Crear GameManager
1. Clic derecho en Hierarchy → Create Empty
2. Nombrar: "GameManager"
3. Agregar componentes:
   - SesionVR
   - ArbolDecision
   - GestorDificultad

#### 2. Crear EstimuloManager
1. Clic derecho en GameManager → Create Empty
2. Nombrar: "EstimuloManager"
3. Agregar componente: EstimuloManager
4. En el Inspector, asignar:
   - Estimulo Blanco Prefab → Assets/Prefabs/EstimuloBlanco
   - Estimulo Negro Prefab → Assets/Prefabs/EstimuloNegro

#### 3. Crear SpawnArea
1. Clic derecho en EstimuloManager → Create Empty
2. Nombrar: "SpawnArea"
3. Position: (0, 1.5, 3)
4. Asignar en EstimuloManager → Spawn Area

#### 4. Crear UI Canvas
1. Clic derecho en Hierarchy → UI → Canvas
2. Nombrar: "FeedbackCanvas"
3. Canvas → Render Mode: World Space
4. Transform:
   - Position: (0, 2, 4)
   - Scale: (0.005, 0.005, 0.005)
5. Agregar componente: InterfazRetroalimentacion

#### 5. Crear Textos de Métricas
Dentro del Canvas, crear 4 TextMeshPro - Text:
- TextoPrecision
- TextoErrores
- TextoTiempoPromedio
- TextoNivelAtencion

#### 6. Crear Feedback Visual
Dentro del Canvas, crear 2 TextMeshPro - Text:
- FeedbackCorrecto (texto: "✓ CORRECTO", color verde)
- FeedbackIncorrecto (texto: "✗ INCORRECTO", color rojo)

#### 7. Asignar Referencias en SesionVR
En GameManager → SesionVR, asignar:
- Estimulo Manager
- Arbol Decision
- Gestor Dificultad
- Interfaz Retroalimentacion

#### 8. Configurar Cámara
En Main Camera, agregar componente: VRInteractionHandler

## 🔍 Verificación

Una vez configurado, verifica:
- [ ] GameManager existe con todos los componentes
- [ ] EstimuloManager tiene los prefabs asignados
- [ ] UI Canvas está en World Space
- [ ] Todas las referencias en SesionVR están asignadas
- [ ] Main Camera tiene VRInteractionHandler

## ▶️ Probar el Sistema

1. Presiona Play en Unity
2. Deberías ver un estímulo aparecer frente a ti
3. Haz clic para interactuar
4. Verifica que las métricas se actualicen en el Canvas

## 🐛 Solución de Problemas

**No aparecen estímulos:**
- Verifica que los prefabs estén asignados en EstimuloManager
- Verifica la posición de SpawnArea

**No se registran interacciones:**
- Verifica que Main Camera tenga VRInteractionHandler
- Verifica que los estímulos tengan el componente Estimulo

**UI no se ve:**
- Verifica que el Canvas esté en World Space
- Ajusta la posición y escala del Canvas
