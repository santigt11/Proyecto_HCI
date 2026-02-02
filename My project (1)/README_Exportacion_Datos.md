# Sistema de Exportación y Análisis de Datos VR

## 📊 Descripción

Este sistema permite exportar datos de sesiones VR a formato CSV para análisis estadístico y optimización de umbrales del árbol de decisión.

## 🎯 Objetivo

Recolectar datos reales de múltiples sesiones de juego para:
- Analizar patrones de rendimiento
- Optimizar umbrales del árbol de decisión
- Validar la clasificación de niveles de atención
- Mejorar la adaptación de dificultad

## 📁 Archivos Generados

### 1. **datos_sesiones_vr.csv**
Archivo principal con resumen de cada sesión:

| Columna                | Descripción                           |
| ---------------------- | ------------------------------------- |
| Fecha                  | Timestamp de la sesión                |
| Usuario                | Nombre del usuario                    |
| Precision              | Porcentaje de aciertos (0-1)          |
| TiempoPromedioReaccion | Tiempo promedio en segundos           |
| TotalErrores           | Número de errores cometidos           |
| TotalAciertos          | Número de aciertos                    |
| TotalInteracciones     | Total de interacciones                |
| TiempoMinimo           | Tiempo de reacción más rápido         |
| TiempoMaximo           | Tiempo de reacción más lento          |
| DesviacionEstandar     | Variabilidad de tiempos               |
| NivelAtencionFinal     | Clasificación final (Alto/Medio/Bajo) |

### 2. **metricas_detalladas_[usuario]_[fecha].csv**
Archivo con métricas individuales de cada interacción (opcional):

| Columna           | Descripción                      |
| ----------------- | -------------------------------- |
| NumeroInteraccion | Número de la interacción         |
| TiempoReaccion    | Tiempo de reacción en segundos   |
| FueCorrecta       | Si fue correcta (True/False)     |
| Timestamp         | Momento exacto de la interacción |

## 🚀 Configuración en Unity

### Paso 1: Agregar Componente
1. Selecciona el GameObject `GameManager` en la jerarquía
2. Add Component → `ExportadorDatosCSV`

### Paso 2: Configurar Referencias
En el componente `SesionVR`:
- Arrastra el `GameManager` al campo `Exportador CSV`

### Paso 3: Configurar Opciones
En el componente `ExportadorDatosCSV`:
- **Nombre Archivo**: `datos_sesiones_vr.csv` (por defecto)
- **Exportar Automaticamente**: ✅ (recomendado)
- **Incluir Metricas Individuales**: ✅ (opcional, para análisis detallado)

## 📍 Ubicación de Archivos

Los archivos CSV se guardan en:
```
Windows: C:\Users\[TuUsuario]\AppData\LocalLow\[CompanyName]\[ProjectName]\
```

Para abrir la carpeta rápidamente:
1. Selecciona `ExportadorDatosCSV` en el Inspector
2. Click derecho → `Abrir Carpeta de Datos`

## 📈 Análisis de Datos

### Requisitos Python
```bash
pip install pandas numpy matplotlib seaborn
```

### Ejecutar Análisis
1. Copia el archivo `datos_sesiones_vr.csv` a la carpeta del proyecto
2. Ejecuta el script de análisis:
```bash
python analizar_datos_vr.py
```

### Funcionalidades del Script
- ✅ Estadísticas descriptivas
- ✅ Distribución por nivel de atención
- ✅ Sugerencia de umbrales óptimos
- ✅ Visualizaciones gráficas
- ✅ Análisis de correlaciones

## 🎯 Optimización de Umbrales

### Proceso Recomendado

1. **Recolectar Datos** (10-20 sesiones mínimo)
   - Juega varias sesiones
   - Varía tu nivel de concentración intencionalmente
   - Asegúrate de tener datos en los 3 niveles

2. **Analizar Datos**
   ```bash
   python analizar_datos_vr.py
   ```

3. **Revisar Sugerencias**
   - El script sugerirá umbrales basados en tus datos
   - Compara con los valores actuales

4. **Actualizar Umbrales en Unity**
   - Selecciona `GameManager` → `Arbol Decision`
   - Ajusta los valores en el Inspector:
     - `Umbral Precision Alta`
     - `Umbral Precision Media`
     - `Umbral Tiempo Rapido`
     - `Umbral Errores Tolerables`

5. **Validar**
   - Juega más sesiones con los nuevos umbrales
   - Verifica que la clasificación sea más precisa

## 📊 Ejemplo de Análisis

### Datos de Entrada
```csv
Fecha,Usuario,Precision,TiempoPromedioReaccion,TotalErrores,...
2026-02-01 12:00,Usuario01,0.85,1.2,2,...
2026-02-01 12:15,Usuario01,0.65,1.8,5,...
2026-02-01 12:30,Usuario01,0.45,2.1,8,...
```

### Salida del Análisis
```
🎯 Umbral Precisión Alta sugerido: 0.75
   Actual en Unity: 0.80
   Rango Alto: 0.80 - 0.90
   Rango Medio: 0.60 - 0.70

⏱️  Umbral Tiempo Rápido sugerido: 1.5s
   Actual en Unity: 1.40s
   Rango Alto: 1.0s - 1.4s
   Rango Medio: 1.6s - 2.0s
```

## 🛠️ Funciones Útiles

### En Unity (Context Menu)
- **Abrir Carpeta de Datos**: Abre la carpeta donde se guardan los CSV
- **Mostrar Ruta del Archivo**: Muestra la ruta completa en Console
- **Reiniciar Archivo CSV**: Elimina el CSV actual para empezar de nuevo

### En Python
- `cargar_datos()`: Carga el CSV
- `analisis_descriptivo()`: Muestra estadísticas
- `sugerir_umbrales()`: Calcula umbrales óptimos
- `visualizar_datos()`: Genera gráficos

## 📝 Notas Importantes

- ⚠️ Los datos se exportan **solo al finalizar la sesión** (llamar a `DetenerSesion()`)
- ⚠️ Si el archivo CSV no existe, se crea automáticamente
- ⚠️ Los datos se **agregan** al archivo existente (no se sobrescriben)
- ✅ Los archivos están en formato UTF-8
- ✅ Compatible con Excel, Google Sheets, y herramientas de análisis

## 🔍 Troubleshooting

**Problema**: No se genera el archivo CSV
- **Solución**: Verifica que `exportadorCSV` esté asignado en `SesionVR`

**Problema**: No encuentro el archivo CSV
- **Solución**: Usa el menú contextual "Abrir Carpeta de Datos"

**Problema**: El script Python da error
- **Solución**: Instala las dependencias: `pip install pandas numpy matplotlib seaborn`

**Problema**: Los umbrales sugeridos son muy diferentes
- **Solución**: Recolecta más datos (mínimo 10-20 sesiones)
