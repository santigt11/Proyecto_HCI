# 🎯 Guía Paso a Paso: Configuración del Sistema VR

## ⚠️ IMPORTANTE: Configuración Inicial

Si ves el error "No existe el componente SesionVR", sigue estos pasos:

---

## 📋 **PASO 1: Verificar/Crear GameManager**

### En Unity:
1. Mira la jerarquía (panel izquierdo)
2. ¿Existe un GameObject llamado `GameManager`?
   - ✅ **SÍ**: Selecciónalo y continúa al Paso 2
   - ❌ **NO**: Créalo:
     - Click derecho en la jerarquía
     - `Create Empty`
     - Nómbralo `GameManager`

---

## 📋 **PASO 2: Agregar Componentes Necesarios**

Con `GameManager` seleccionado en la jerarquía:

### 2.1 Agregar SesionVR
1. En el Inspector (panel derecho), click en **Add Component**
2. Escribe: `SesionVR`
3. Click en el resultado para agregarlo

### 2.2 Agregar ExportadorDatosCSV
1. Click en **Add Component**
2. Escribe: `ExportadorDatosCSV`
3. Click para agregar

### 2.3 Agregar DiagnosticoSistema (para verificar)
1. Click en **Add Component**
2. Escribe: `DiagnosticoSistema`
3. Click para agregar

### 2.4 Agregar PruebaExportacionCSV (opcional)
1. Click en **Add Component**
2. Escribe: `PruebaExportacionCSV`
3. Click para agregar

---

## 📋 **PASO 3: Ejecutar Diagnóstico**

1. Con `GameManager` seleccionado
2. En el componente `DiagnosticoSistema`
3. Click derecho → **"Verificar Configuración"**
4. **Revisa la Console** (Window → General → Console)

La Console te dirá:
- ✅ Qué está bien configurado
- ⚠️ Qué falta configurar
- ❌ Qué componentes faltan

---

## 📋 **PASO 4: Asignar Referencias**

Según lo que diga el diagnóstico, necesitarás asignar referencias en `SesionVR`:

### 4.1 Buscar GameObjects en la Escena

En la jerarquía, busca estos GameObjects:
- `EstimuloManager` (o el que tenga ese componente)
- `GestorDificultad` (o el que tenga ese componente)
- `InterfazRetroalimentacion` (o el que tenga ese componente)
- `ArbolDecision` (probablemente el mismo GameManager)

### 4.2 Asignar en SesionVR

Con `GameManager` seleccionado, en el componente `Sesion VR`:

1. **Estimulo Manager**: 
   - Click en el círculo → Selecciona el GameObject que tiene `EstimuloManager`

2. **Arbol Decision**: 
   - Click en el círculo → Selecciona el GameObject que tiene `ArbolDecision`

3. **Gestor Dificultad**: 
   - Click en el círculo → Selecciona el GameObject que tiene `GestorDificultad`

4. **Interfaz Retroalimentacion**: 
   - Click en el círculo → Selecciona el GameObject que tiene `InterfazRetroalimentacion`

5. **Exportador CSV**: 
   - Arrastra el mismo `GameManager` aquí

---

## 📋 **PASO 5: Verificar de Nuevo**

1. Click derecho en `DiagnosticoSistema` → **"Verificar Configuración"**
2. Revisa la Console
3. Deberías ver solo ✅ (checkmarks verdes)

---

## 📋 **PASO 6: Probar la Exportación**

1. Presiona **Play** en Unity
2. Juega una sesión VR
3. Presiona **F9** para detener y exportar
4. Revisa la carpeta `DatosExportados/`

---

## 🔍 **Si Algo Falta**

### "No encuentro EstimuloManager en la escena"
Probablemente necesitas crear el GameObject:
1. Click derecho en jerarquía → Create Empty
2. Nómbralo `EstimuloManager`
3. Add Component → `EstimuloManager`

### "No encuentro GestorDificultad"
1. Click derecho en jerarquía → Create Empty
2. Nómbralo `GestorDificultad`
3. Add Component → `GestorDificultad`

### "No encuentro InterfazRetroalimentacion"
Busca en la jerarquía un Canvas o GameObject de UI que tenga este componente.

---

## ✅ **Configuración Completa**

Cuando todo esté bien, deberías tener:

```
GameManager
├── SesionVR ✅
│   ├── Estimulo Manager: [EstimuloManager GameObject]
│   ├── Arbol Decision: [GameManager]
│   ├── Gestor Dificultad: [GestorDificultad GameObject]
│   ├── Interfaz Retroalimentacion: [Canvas/UI GameObject]
│   └── Exportador CSV: [GameManager]
├── ExportadorDatosCSV ✅
├── ArbolDecision ✅
├── DiagnosticoSistema ✅
└── PruebaExportacionCSV ✅
```

---

## 🎮 **Atajos de Teclado**

Una vez configurado:
- **F9**: Detener sesión y exportar CSV
- **F10**: Iniciar nueva sesión

---

## 📞 **¿Necesitas Ayuda?**

Si el diagnóstico muestra errores:
1. Copia el mensaje de la Console
2. Compártelo para ayudarte a resolver el problema específico
