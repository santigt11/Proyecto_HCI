# 🔧 Solución: Botones No Se Presionan

## ✅ **Lo Que Ya Se Configuró Automáticamente:**

- ✅ `XRRayInteractor` (ambos) → `interactionManager` asignado
- ✅ `XRSimpleInteractable` (ambos botones) → `interactionManager` asignado
- ✅ Eventos configurados en `ButtonEventSample`

---

## ⚠️ **Problema: El rayo detecta pero no selecciona**

Esto sucede porque **faltan las Input Actions de selección** en los Ray Interactors.

---

## 🔧 **Solución Manual:**

### **PASO 1: Configurar Right Ray Interactor**

1. Selecciona: `XR Origin (VR) → Camera Offset → Main Camera → Right_Hand → Right Ray Interactor`

2. En el componente `XR Ray Interactor`, busca la sección **Input Configuration**

3. Configura:
   - **Select Action**: 
     - Click en el dropdown
     - Busca: `XRI RightHand Interaction` → `Select`
     - O: `XRI Default Right Controller` → `Select Value`
   
   - **Activate Action** (opcional):
     - `XRI RightHand Interaction` → `Activate`

---

### **PASO 2: Configurar Left Ray Interactor**

1. Selecciona: `XR Origin (VR) → Camera Offset → Main Camera → Left_Hand → Left Ray Interactor`

2. En el componente `XR Ray Interactor`, busca la sección **Input Configuration**

3. Configura:
   - **Select Action**: 
     - Click en el dropdown
     - Busca: `XRI LeftHand Interaction` → `Select`
     - O: `XRI Default Left Controller` → `Select Value`
   
   - **Activate Action** (opcional):
     - `XRI LeftHand Interaction` → `Activate`

---

## 🎯 **Alternativa: Usar UI Input Module**

Si los Ray Interactors siguen sin funcionar, puedes usar el sistema de UI tradicional:

### **Opción A: Agregar XR UI Input Module**

1. Busca en la jerarquía: `XR Interaction Manager`
2. Add Component → `XR UI Input Module`
3. Configura las acciones de UI

### **Opción B: Verificar Layers**

1. Verifica que `MenuCanvas` esté en un layer que los rayos puedan detectar
2. En los Ray Interactors, verifica el `Raycast Mask`

---

## 🔍 **Verificación:**

### **Después de configurar, verifica:**

1. **En Play Mode:**
   - Apunta con el rayo a un botón
   - El rayo debe cambiar de color (hover)
   - Presiona el trigger/select
   - El botón debe ejecutar la acción

2. **En la Console:**
   - Deberías ver: `[MenuPrincipalVR] Iniciando sesión...`
   - O: `[MenuPrincipalVR] Saliendo de la aplicación...`

---

## 🐛 **Si Aún No Funciona:**

### **Problema 1: "El rayo no cambia de color al apuntar"**
✅ Verifica que `XRInteractorLineVisual` esté configurado
✅ Verifica que `setLineColorGradient` esté en `true`

### **Problema 2: "El rayo cambia de color pero no selecciona"**
✅ **Falta la Select Action** (ver PASO 1 y 2)
✅ Verifica que el Input Action esté habilitado

### **Problema 3: "Dice que no encuentra el método"**
✅ Verifica que `MenuPrincipalVR` esté en el `GameManager`
✅ Verifica que los métodos sean públicos (`public void IniciarSesion()`)

---

## 📋 **Checklist Final:**

- [ ] Right Ray Interactor tiene `Select Action` configurada
- [ ] Left Ray Interactor tiene `Select Action` configurada
- [ ] Ambos Ray Interactors tienen `Interaction Manager` asignado
- [ ] Ambos botones tienen `XRSimpleInteractable` con `Interaction Manager`
- [ ] Los eventos están configurados en `ButtonEventSample`
- [ ] `MenuPrincipalVR` está en el `GameManager`

---

## 🎮 **Ruta de las Input Actions:**

Las acciones de selección están en:
```
Assets/Samples/XR Interaction Toolkit/3.3.1/Starter Assets/XRI Default Input Actions.inputactions
```

Busca:
- `XRI LeftHand Interaction/Select`
- `XRI RightHand Interaction/Select`

---

¡Una vez configuradas las Select Actions, los botones deberían funcionar perfectamente!
