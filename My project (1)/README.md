# Sistema de Entrenamiento de Atención en VR

## 📋 Descripción del Proyecto

Sistema de realidad virtual desarrollado en Unity que mantiene y mejora la atención del usuario mediante:
- Retroalimentación visual inmediata
- Visualización de métricas en tiempo real
- Adaptación dinámica de dificultad
- Clasificación del nivel de atención usando IA explicable (Árbol de Decisión)

**Nota:** Este es un prototipo académico funcional, NO es clínico ni diagnóstico.

## 🎯 Principio HCI Aplicado

**Retroalimentación:** El sistema asegura que el usuario:
- Comprenda qué hizo bien o mal
- Perciba su desempeño
- Mantenga la atención durante la sesión

## 🏗️ Arquitectura del Sistema

### Clases Principales

- **`SesionVR`**: Controlador principal que coordina todo el flujo
- **`Usuario`**: Representa al usuario y almacena métricas
- **`Estimulo`**: Círculos blancos (interactuar) o negros (no interactuar)
- **`Metrica`**: Almacena tiempo de reacción y resultado de cada interacción
- **`ArbolDecision`**: IA que clasifica nivel de atención (Bajo/Medio/Alto)
- **`GestorDificultad`**: Ajusta parámetros según nivel de atención
- **`EstimuloManager`**: Genera estímulos en posiciones aleatorias
- **`InterfazRetroalimentacion`**: Muestra feedback visual y métricas
- **`VRInteractionHandler`**: Maneja interacción por raycast (mirada + clic)

## 🔄 Flujo del Sistema

```
1. Genera estímulo (blanco o negro)
2. Usuario interactúa (o no)
3. Sistema registra acierto/error
4. Actualiza métricas
5. Muestra retroalimentación inmediata
6. IA clasifica nivel de atención
7. Muestra métricas acumuladas
8. Ajusta dificultad
9. Vuelve al paso 1
```

## 📊 Métricas Registradas

**Por interacción:**
- Tiempo de reacción
- Resultado (correcto/incorrecto)

**Por sesión:**
- Precisión (aciertos/total)
- Número de errores
- Promedio de tiempo de reacción
- Evolución del nivel de atención

## 🎮 Interacción VR

- **Método:** Raycast con mirada + clic
- **Estímulos:**
  - ⚪ Círculos blancos → Usuario DEBE interactuar
  - ⚫ Círculos negros → Usuario NO debe interactuar
- **Sin locomoción**
- **Sin eye tracking real**

## 🤖 Inteligencia Artificial

**Tipo:** Árbol de Decisión supervisado (implementado en C#)

**Entradas:**
- Tiempo de reacción promedio
- Precisión
- Número de errores

**Salida:**
- Nivel de atención: Bajo, Medio, Alto

**Función:** Solo clasifica, NO controla UI ni lógica VR

## 📈 Adaptación de Dificultad

| Nivel | Velocidad | Intervalo | Estímulos |
| ----- | --------- | --------- | --------- |
| Bajo  | 0.5x      | 3s        | 1         |
| Medio | 1.0x      | 2s        | 1         |
| Alto  | 1.5x      | 1.5s      | 2         |

La adaptación es gradual y natural.

## 🛠️ Tecnologías Utilizadas

- **Unity 2022+**
- **OpenXR Plugin** (v1.16.1)
- **XR Interaction Toolkit** (v3.3.1)
- **Input System** (v1.17.0)
- **C#**

## 📁 Estructura del Proyecto

```
Assets/
├── Scenes/
│   └── MainVRScene.unity
├── Scripts/
│   ├── Core/
│   │   ├── Usuario.cs
│   │   ├── SesionVR.cs
│   │   ├── Estimulo.cs
│   │   └── Metrica.cs
│   ├── AI/
│   │   ├── ArbolDecision.cs
│   │   └── NivelAtencion.cs
│   ├── Managers/
│   │   ├── GestorDificultad.cs
│   │   └── EstimuloManager.cs
│   ├── UI/
│   │   └── InterfazRetroalimentacion.cs
│   └── VR/
│       └── VRInteractionHandler.cs
├── Prefabs/
│   ├── EstimuloBlanco.prefab
│   └── EstimuloNegro.prefab
├── Materials/
│   ├── Blanco.mat
│   └── Negro.mat
└── UI/
    └── FeedbackCanvas.prefab
```

## 🚀 Próximos Pasos de Implementación

- [ ] Crear escena VR con cámara XR
- [ ] Crear prefabs de estímulos (círculos blanco/negro)
- [ ] Configurar UI Canvas para retroalimentación
- [ ] Asignar referencias en SesionVR
- [ ] Probar flujo completo
- [ ] Ajustar parámetros de dificultad

## 💾 Respaldo del Proyecto

Este proyecto usa **Git LFS** para manejar archivos grandes de Unity.

### Inicializar repositorio:

```bash
git lfs install
git init
git add .
git commit -m "Initial commit: VR Attention Training System"
git remote add origin <tu-repo-url>
git push -u origin main
```

## 👥 Autor

Proyecto académico - HCI (Human-Computer Interaction)
Universidad - Séptimo Ciclo

## 📄 Licencia

Proyecto académico - Uso educativo
