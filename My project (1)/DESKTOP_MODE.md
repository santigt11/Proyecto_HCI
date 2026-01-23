# Guía: Configuración para Pruebas en Desktop (Sin VR)

## ✅ Cambios Realizados

He actualizado `VRInteractionHandler.cs` para:
- ✅ Usar el nuevo **Input System** (sin errores)
- ✅ Soportar **modo Desktop** para pruebas sin gafas VR
- ✅ Control de cámara con ratón
- ✅ Interacción con clic izquierdo

## 🎮 Controles en Modo Desktop

| Acción                       | Control        |
| ---------------------------- | -------------- |
| **Mirar alrededor**          | Mover el ratón |
| **Interactuar con estímulo** | Clic izquierdo |
| **Liberar cursor**           | ESC            |
| **Bloquear cursor**          | Clic izquierdo |

## 🔧 Configuración Recomendada

### 1. Verificar Main Camera

En Unity, selecciona **Main Camera** y verifica:
- ✅ Tiene componente `VRInteractionHandler`
- ✅ "Usar Modo Desktop" está marcado (✓)
- ✅ Sensibilidad Raton: 2

### 2. Verificar GameManager

Selecciona **GameManager** en la jerarquía y verifica que todas las referencias estén asignadas:
- Estimulo Manager
- Arbol Decision
- Gestor Dificultad
- Interfaz Retroalimentacion

### 3. Probar el Sistema

1. Presiona **Play** en Unity
2. Deberías ver:
   - Un estímulo (esfera blanca o negra) aparecer frente a ti
   - El UI Canvas con las métricas
3. Mueve el ratón para mirar alrededor
4. Apunta al estímulo y haz **clic izquierdo**
5. Verifica que:
   - El estímulo desaparece
   - Aparece feedback (✓ CORRECTO o ✗ INCORRECTO)
   - Las métricas se actualizan
   - Aparece un nuevo estímulo

## 🐛 Solución de Problemas

### Error: "Input System package"
✅ **SOLUCIONADO** - El código ahora usa `UnityEngine.InputSystem`

### No veo estímulos
1. Verifica que `EstimuloManager` tenga los prefabs asignados
2. Verifica la posición de `SpawnArea` (debe estar frente a la cámara)
3. Ajusta `SpawnArea` position a (0, 1.5, 3)

### No puedo interactuar con estímulos
1. Verifica que la cámara tenga `VRInteractionHandler`
2. Verifica que "Usar Modo Desktop" esté activado
3. Asegúrate de hacer clic izquierdo mientras apuntas al estímulo

### El cursor no se bloquea
- Haz clic izquierdo en la ventana de Game
- Presiona ESC para liberar el cursor si es necesario

## 📊 Advertencias de OpenXR (Puedes Ignorarlas)

Las advertencias que ves son normales cuando no tienes las gafas conectadas:

1. **"At least one interaction profile must be added"**
   - Solo necesario cuando uses las Meta Quest 2
   - Puedes ignorarlo en modo desktop

2. **"Switch to use InputSystem.XR.PoseControl"**
   - Opcional, solo para optimización futura

3. **"Run In Background must be enabled"**
   - Solo importante para VR real
   - En desktop no afecta

## 🥽 Cuando Tengas las Meta Quest 2

Cuando tengas acceso a las gafas:

1. **Instalar Oculus/Meta XR Plugin:**
   - Window → Package Manager
   - Buscar "Oculus XR Plugin" o "Meta XR Plugin"
   - Instalar

2. **Configurar XR:**
   - Edit → Project Settings → XR Plug-in Management
   - Activar "Oculus" o "Meta Quest"

3. **Desactivar Modo Desktop:**
   - En Main Camera → VRInteractionHandler
   - Desmarcar "Usar Modo Desktop"

4. **Conectar Quest 2:**
   - Usar Oculus Link o Air Link
   - O hacer build para Android y cargar en las gafas

## ✨ Próximos Pasos

Una vez que el sistema funcione en desktop:

1. Ajustar parámetros de dificultad en `GestorDificultad`
2. Ajustar umbrales del árbol de decisión en `ArbolDecision`
3. Personalizar colores y textos de la UI
4. Agregar más tipos de estímulos o variaciones

## 🎯 Objetivo de Prueba

Deberías poder:
- ✅ Ver estímulos aparecer
- ✅ Interactuar con clic
- ✅ Ver feedback inmediato
- ✅ Ver métricas actualizarse
- ✅ Observar cambios de dificultad

¡Prueba el sistema ahora presionando Play!
