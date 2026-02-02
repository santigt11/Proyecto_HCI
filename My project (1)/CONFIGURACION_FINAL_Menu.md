# ✅ Menú VR - Configuración Final

## 🎉 **Lo Que Se Ha Configurado Automáticamente:**

### **Eventos de Botones:**
✅ **BotonIniciar** → `MenuPrincipalVR.IniciarSesion()`
✅ **BotonSalir** → `MenuPrincipalVR.SalirAplicacion()`

### **Referencias:**
✅ **menuCanvas** → `MenuCanvas`
✅ **canvasMetricas** → `FeedbackCanvas`

### **Script Actualizado:**
✅ `MenuPrincipalVR.cs` - Versión simplificada con detección de grips

---

## ⚙️ **ÚNICA Configuración Manual Requerida:**

### **Configurar Input Actions para Grips**

En Unity:

1. **Selecciona `GameManager`** en la jerarquía

2. **En el Inspector**, busca el componente `Menu Principal VR`

3. **Configura los campos:**

   **Grip Izquierdo:**
   - Click en el dropdown (círculo con flecha)
   - Busca: `XRI LeftHand Interaction` → `Grip`
   - O navega a: `XRI Default Left Controller` → `Grip`
   
   **Grip Derecho:**
   - Click en el dropdown (círculo con flecha)
   - Busca: `XRI RightHand Interaction` → `Grip`
   - O navega a: `XRI Default Right Controller` → `Grip`

4. **Ajusta el tiempo (opcional):**
   - **Tiempo Presion Requerido**: `2.0` segundos
   - Puedes cambiarlo a `1.5` o `3.0` según prefieras

---

## 🎮 **Cómo Funciona el Sistema:**

### **Al Iniciar el Juego:**
```
✅ MenuCanvas visible
✅ FeedbackCanvas (métricas) oculto
✅ Sesión NO inicia automáticamente
```

### **Cuando Clickeas "BotonIniciar":**
```
1. MenuCanvas se oculta
2. FeedbackCanvas se muestra
3. SesionVR.IniciarSesion() se ejecuta
4. Empiezan a aparecer estímulos
```

### **Durante la Sesión:**
```
Presiona y mantén AMBOS GRIPS por 2 segundos
   ↓
1. SesionVR.DetenerSesion() se ejecuta
2. CSV se exporta automáticamente
3. FeedbackCanvas se oculta
4. MenuCanvas se muestra
```

### **Cuando Clickeas "BotonSalir":**
```
1. Si hay sesión activa → Se detiene y exporta
2. La aplicación se cierra
```

---

## 🔍 **Verificación:**

### **Antes de Probar:**

- [ ] `GameManager` tiene `MenuPrincipalVR` con:
  - [ ] `menuCanvas` = `MenuCanvas`
  - [ ] `canvasMetricas` = `FeedbackCanvas`
  - [ ] `gripIzquierdo` = `XRI LeftHand/Grip` ⚠️ **CONFIGURAR**
  - [ ] `gripDerecho` = `XRI RightHand/Grip` ⚠️ **CONFIGURAR**

- [ ] `BotonIniciar` tiene evento:
  - [ ] `ButtonEventSample.onButtonClicked` → `GameManager.MenuPrincipalVR.IniciarSesion()`

- [ ] `BotonSalir` tiene evento:
  - [ ] `Button.onClick` → `GameManager.MenuPrincipalVR.SalirAplicacion()`

- [ ] `SesionVR.iniciarAutomaticamente` = `false`

---

## 🧪 **Probar el Sistema:**

1. **Presiona Play**
2. **Verifica** que aparece el menú
3. **Click en "BotonIniciar"** (con el rayo VR)
4. **Verifica** que:
   - El menú desaparece
   - Las métricas aparecen
   - Empiezan a aparecer estímulos
5. **Presiona ambos grips** por 2 segundos
6. **Verifica** que:
   - Vuelves al menú
   - Las métricas desaparecen
   - Se creó el archivo CSV en `DatosExportados/`

---

## 🐛 **Solución de Problemas:**

### **"Los grips no funcionan"**
✅ Asegúrate de haber configurado los Input Actions
✅ Verifica que las acciones estén habilitadas en el Input System

### **"El botón Iniciar no hace nada"**
✅ Revisa la Console para ver si hay errores
✅ Verifica que el evento esté configurado en `ButtonEventSample`

### **"Las métricas no aparecen"**
✅ Verifica que `canvasMetricas` apunte a `FeedbackCanvas`
✅ Revisa que `FeedbackCanvas` esté en la ruta correcta

### **"El CSV no se exporta"**
✅ Verifica que `ExportadorDatosCSV` esté en el GameManager
✅ Verifica que la referencia esté asignada en `SesionVR`

---

## 📊 **Flujo Completo:**

```
INICIO
  ↓
[MENÚ VISIBLE] ← Aquí empiezas
[MÉTRICAS OCULTAS]
  ↓
Click "BotonIniciar" (con rayo VR)
  ↓
[MENÚ OCULTO]
[MÉTRICAS VISIBLES] ← Aquí juegas
[SESIÓN ACTIVA]
[ESTÍMULOS APARECEN]
  ↓
Presionar AMBOS GRIPS (2s)
  ↓
[SESIÓN DETENIDA]
[CSV EXPORTADO] ← Datos guardados
[MENÚ VISIBLE] ← Vuelves aquí
[MÉTRICAS OCULTAS]
```

---

## ✨ **¡Todo Listo!**

Solo falta:
1. Configurar los Input Actions de los grips
2. Presionar Play y probar

¿Necesitas ayuda con algo más?
