# 🎮 Menú Principal VR - Configuración Final

## ✅ **Lo Que Se Ha Creado:**

### **Scripts:**
1. ✅ `MenuPrincipalVR.cs` - Controlador del menú
2. ✅ `BotonVRInteractivo.cs` - Efectos visuales para botones

### **GameObjects en la Escena:**
1. ✅ `MenuCanvas` - Canvas WorldSpace del menú
   - Posición: `(0, 1.5, 3)` - Frente al jugador
   - Tamaño: `2m x 1.5m`
   - Escala: `0.001` (para convertir pixels a metros)

2. ✅ `Titulo` - Texto "ENTRENAMIENTO DE ATENCIÓN VR"
3. ✅ `BotonIniciar` - Botón verde para iniciar sesión
4. ✅ `BotonSalir` - Botón rojo para salir

### **Componentes Agregados:**
1. ✅ `MenuPrincipalVR` en `GameManager`
2. ✅ `BotonVRInteractivo` en ambos botones
3. ✅ `SesionVR.iniciarAutomaticamente = false`

---

## ⚙️ **Configuración Manual Requerida:**

### **PASO 1: Configurar Input Actions para Grips**

En Unity, selecciona `GameManager` y en el componente `Menu Principal VR`:

1. **Grip Izquierdo:**
   - Click en el dropdown del campo `Grip Izquierdo`
   - Selecciona: `XRI LeftHand Interaction/Grip`
   - O busca la acción de grip del controlador izquierdo

2. **Grip Derecho:**
   - Click en el dropdown del campo `Grip Derecho`
   - Selecciona: `XRI RightHand Interaction/Grip`
   - O busca la acción de grip del controlador derecho

**Ruta de las acciones:** 
```
Assets/Samples/XR Interaction Toolkit/3.3.1/Starter Assets/XRI Default Input Actions.inputactions
```

---

### **PASO 2: Personalizar Colores de los Botones**

#### **Botón Iniciar (Verde):**
1. Selecciona `MenuCanvas/BotonIniciar`
2. En el componente `Boton VR Interactivo`:
   - **Color Normal**: `RGB(0, 200, 0)` - Verde oscuro
   - **Color Hover**: `RGB(0, 255, 0)` - Verde brillante
   - **Color Presionado**: `RGB(0, 150, 0)` - Verde más oscuro

#### **Botón Salir (Rojo):**
1. Selecciona `MenuCanvas/BotonSalir`
2. En el componente `Boton VR Interactivo`:
   - **Color Normal**: `RGB(200, 0, 0)` - Rojo oscuro
   - **Color Hover**: `RGB(255, 50, 50)` - Rojo brillante
   - **Color Presionado**: `RGB(150, 0, 0)` - Rojo más oscuro

---

### **PASO 3: Configurar Texto de los Botones**

#### **Botón Iniciar:**
1. Selecciona `MenuCanvas/BotonIniciar/Text`
2. En el componente `Text`:
   - **Text**: "INICIAR SESIÓN"
   - **Font Size**: `36`
   - **Alignment**: Center
   - **Color**: Blanco `RGB(255, 255, 255)`

#### **Botón Salir:**
1. Selecciona `MenuCanvas/BotonSalir/Text`
2. En el componente `Text`:
   - **Text**: "SALIR"
   - **Font Size**: `36`
   - **Alignment**: Center
   - **Color**: Blanco `RGB(255, 255, 255)`

---

## 🎯 **Cómo Funciona:**

### **Al Iniciar el Juego:**
```
1. Se muestra el MenuCanvas
2. El FeedbackCanvas (métricas) está OCULTO
3. La sesión NO inicia automáticamente
```

### **Cuando Presionas "INICIAR SESIÓN":**
```
1. MenuCanvas se oculta
2. FeedbackCanvas (métricas) se muestra
3. La sesión VR inicia
4. Empiezan a aparecer estímulos
```

### **Durante la Sesión:**
```
Presiona y mantén ambos GRIPS por 2 segundos
   ↓
1. La sesión se detiene
2. Se exporta el CSV automáticamente
3. FeedbackCanvas se oculta
4. MenuCanvas se muestra
```

### **Cuando Presionas "SALIR":**
```
1. Si hay sesión activa, se detiene y exporta
2. La aplicación se cierra
```

---

## 🔧 **Ajustes Opcionales:**

### **Cambiar Tiempo de Presión de Grips:**
En `GameManager → Menu Principal VR`:
- **Tiempo Presion Requerido**: `2.0` (segundos)
- Puedes cambiarlo a `1.5` o `3.0` según prefieras

### **Cambiar Posición del Menú:**
Si el menú está muy cerca o muy lejos:
1. Selecciona `MenuCanvas`
2. Modifica la posición Z:
   - Más cerca: `Z = 2`
   - Más lejos: `Z = 4`

### **Cambiar Tamaño del Menú:**
1. Selecciona `MenuCanvas`
2. Modifica la escala:
   - Más grande: `0.0015`
   - Más pequeño: `0.0008`

---

## ✅ **Checklist de Verificación:**

Antes de probar, verifica:

- [ ] `MenuPrincipalVR` tiene asignados:
  - [ ] Menu Canvas → `MenuCanvas`
  - [ ] Boton Iniciar → `MenuCanvas/BotonIniciar`
  - [ ] Boton Salir → `MenuCanvas/BotonSalir`
  - [ ] Canvas Metricas → `XR Origin (VR)/Camera Offset/Main Camera/FeedbackCanvas`
  - [ ] Grip Izquierdo → `XRI LeftHand/Grip`
  - [ ] Grip Derecho → `XRI RightHand/Grip`

- [ ] `SesionVR.iniciarAutomaticamente` = `false`

- [ ] Los botones tienen:
  - [ ] Componente `Button`
  - [ ] Componente `BotonVRInteractivo`
  - [ ] Texto configurado

- [ ] El `MenuCanvas` tiene:
  - [ ] `Canvas` (Render Mode = WorldSpace)
  - [ ] `GraphicRaycaster`
  - [ ] Posición `(0, 1.5, 3)`

---

## 🎮 **Probar el Sistema:**

1. **Presiona Play** en Unity
2. **Verifica** que aparece el menú frente a ti
3. **Apunta** con el rayo VR a "INICIAR SESIÓN"
4. **Click** para iniciar
5. **Juega** la sesión
6. **Presiona ambos grips** por 2 segundos
7. **Verifica** que vuelves al menú y se exportó el CSV

---

## 🐛 **Solución de Problemas:**

### **"Los botones no responden al rayo VR"**
✅ Verifica que `MenuCanvas` tenga `GraphicRaycaster`
✅ Verifica que los rayos VR estén activos

### **"No detecta los grips"**
✅ Asigna las acciones de input en `MenuPrincipalVR`
✅ Verifica que las acciones estén habilitadas

### **"El menú no se oculta al iniciar"**
✅ Verifica que `menuCanvas` esté asignado en `MenuPrincipalVR`

### **"Las métricas no aparecen durante la sesión"**
✅ Verifica que `canvasMetricas` esté asignado correctamente

---

## 📊 **Flujo Completo:**

```
INICIO
  ↓
[MENÚ VISIBLE]
[MÉTRICAS OCULTAS]
  ↓
Click "INICIAR SESIÓN"
  ↓
[MENÚ OCULTO]
[MÉTRICAS VISIBLES]
[SESIÓN ACTIVA]
  ↓
Presionar Grips (2s)
  ↓
[SESIÓN DETENIDA]
[CSV EXPORTADO]
[MENÚ VISIBLE]
[MÉTRICAS OCULTAS]
```

---

¡Todo listo! Solo falta configurar los Input Actions para los grips y personalizar los colores de los botones.
