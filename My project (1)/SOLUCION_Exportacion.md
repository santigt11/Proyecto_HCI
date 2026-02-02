# 🔧 Solución: Exportación de CSV

## ✅ **Cambios Realizados**

He modificado el sistema para que los CSV se exporten en una ubicación más accesible:

### **Nueva Ubicación:**
```
[TuProyecto]/DatosExportados/datos_sesiones_vr.csv
```

En tu caso:
```
d:\Trabajos_Universidad\Septimo_Ciclo\HCI\Proyecto-Final\My project\Proyecto_HCI\My project (1)\DatosExportados\
```

## 🎮 **Cómo Probar la Exportación**

### **Opción 1: Usar Atajos de Teclado (Recomendado)**

1. **Agregar el script de prueba:**
   - Selecciona `GameManager` en Unity
   - Add Component → `PruebaExportacionCSV`

2. **Jugar en Unity:**
   - Presiona Play
   - Juega normalmente
   - **Presiona F9** para detener la sesión y exportar
   - **Presiona F10** para iniciar una nueva sesión

3. **Verificar el CSV:**
   - Ve a la carpeta del proyecto
   - Abre `DatosExportados/`
   - Deberías ver `datos_sesiones_vr.csv`

### **Opción 2: Detener Manualmente**

Si no quieres usar el script de prueba:

1. En Unity, mientras juegas, abre la Console
2. Escribe en un script o usa el Inspector para llamar:
   ```csharp
   SesionVR.Instance.DetenerSesion();
   ```

## 📋 **Checklist de Configuración**

Verifica que todo esté configurado:

- [ ] `ExportadorDatosCSV` agregado al `GameManager`
- [ ] Campo `Exportador CSV` en `SesionVR` apunta a `GameManager`
- [ ] `PruebaExportacionCSV` agregado al `GameManager` (opcional, para testing)
- [ ] Jugaste una sesión y presionaste F9 (o llamaste a `DetenerSesion()`)
- [ ] Verificaste que existe la carpeta `DatosExportados/`

## 🔍 **Verificar en Unity**

### **Ver la Ruta de Exportación:**
1. Selecciona `GameManager`
2. En `ExportadorDatosCSV`
3. Click derecho → **"Mostrar Ruta del Archivo"**
4. La ruta aparecerá en la Console

### **Abrir la Carpeta:**
1. Selecciona `GameManager`
2. En `ExportadorDatosCSV`
3. Click derecho → **"Abrir Carpeta de Datos"**
4. Se abrirá la carpeta `DatosExportados/`

## 📊 **Analizar los Datos**

Una vez tengas el CSV:

```bash
python analizar_datos_vr.py
```

El script ahora busca automáticamente en `DatosExportados/datos_sesiones_vr.csv`

## ⚠️ **Problemas Comunes**

### **"No se exporta nada"**
- ✅ Verifica que llamaste a `DetenerSesion()` (presiona F9)
- ✅ Verifica que el campo `exportadorCSV` esté asignado en `SesionVR`
- ✅ Revisa la Console de Unity para ver mensajes de exportación

### **"No encuentro la carpeta DatosExportados"**
- ✅ La carpeta se crea automáticamente la primera vez que exportas
- ✅ Debe estar al mismo nivel que la carpeta `Assets/`
- ✅ Usa el menú contextual "Abrir Carpeta de Datos" para encontrarla

### **"El script Python no encuentra el CSV"**
- ✅ Asegúrate de ejecutar el script desde la carpeta del proyecto
- ✅ O proporciona la ruta completa cuando te lo pida

## 🎯 **Flujo Completo**

```
1. Play Mode en Unity
        ↓
2. Jugar sesión VR
        ↓
3. Presionar F9 (o llamar DetenerSesion())
        ↓
4. CSV se guarda en DatosExportados/
        ↓
5. Ejecutar: python analizar_datos_vr.py
        ↓
6. Revisar análisis y sugerencias
        ↓
7. Ajustar umbrales en Unity
```

## 📁 **Estructura de Archivos**

```
My project (1)/
├── Assets/
├── DatosExportados/          ← Nueva carpeta (se crea automáticamente)
│   ├── datos_sesiones_vr.csv
│   └── metricas_detalladas_Usuario01_20260201_170000.csv
├── analizar_datos_vr.py
└── README_Exportacion_Datos.md
```
