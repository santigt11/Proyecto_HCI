# Árbol de Decisión - Clasificación de Nivel de Atención

## Estructura del Árbol

```
                    [Raíz: Precisión >= 80%?]
                    /                        \
                  SÍ                          NO
                  /                            \
    [Tiempo <= 1.4s?]              [Precisión >= 50%?]
        /        \                      /            \
      SÍ         NO                   SÍ             NO
      /           \                   /               \
  [ALTO]      [MEDIO]      [Errores <= 3?]        [BAJO]
                              /        \
                            SÍ         NO
                            /           \
                        [MEDIO]      [BAJO]
```

## Componentes del Árbol

### 1. **Clase Base: `NodoArbol`**
- Clase abstracta que define la interfaz para todos los nodos
- Método: `Evaluar(MetricasClasificacion)` → `NivelAtencion`

### 2. **Nodos de Decisión: `NodoDecision`**
- Nodos internos del árbol
- Contienen una condición (lambda function)
- Tienen dos hijos: `hijoVerdadero` y `hijoFalso`
- Evalúan la condición y delegan al hijo correspondiente

### 3. **Nodos Hoja: `NodoHoja`**
- Nodos terminales del árbol
- Retornan un resultado final (`NivelAtencion`)
- No tienen hijos

### 4. **Estructura de Datos: `MetricasClasificacion`**
- Encapsula las métricas que se pasan entre nodos:
  - `precision` (float)
  - `tiempoReaccionPromedio` (float)
  - `numeroErrores` (int)

## Construcción del Árbol

El árbol se construye en `Awake()` de abajo hacia arriba:

1. **Nivel 3 (Hojas)**: Se crean 5 nodos hoja con resultados finales
2. **Nivel 2**: Se crean nodos de decisión que apuntan a las hojas
3. **Nivel 1 (Raíz)**: Se crea el nodo raíz que apunta a los nodos de nivel 2

## Ventajas de esta Implementación

**Estructura Real de Árbol**: Usa nodos y referencias
**Escalable**: Fácil agregar nuevos nodos o modificar la estructura
**Mantenible**: Cada nodo es independiente y reutilizable
**Testeable**: Se pueden probar nodos individuales
**Extensible**: Se pueden agregar nuevos tipos de nodos (ej: nodos probabilísticos)
**Patrón de Diseño**: Implementa el patrón Composite

## Ejemplo de Recorrido

**Entrada**: `precision=0.85, tiempo=1.2s, errores=2`

1. **Raíz**: ¿Precisión >= 0.8? → **SÍ** → Ir a nodo de tiempo
2. **Nodo Tiempo**: ¿Tiempo <= 1.4s? → **SÍ** → Ir a hoja ALTO
3. **Hoja**: Retornar `NivelAtencion.Alto`

**Resultado**: `ALTO`

## Configuración

Los umbrales son configurables en el Inspector:
- `umbralPrecisionAlta`: 0.8 (80%)
- `umbralPrecisionMedia`: 0.5 (50%)
- `umbralTiempoRapido`: 1.4 segundos
- `umbralErroresTolerables`: 3 errores
