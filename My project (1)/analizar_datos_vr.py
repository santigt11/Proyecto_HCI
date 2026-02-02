"""
Análisis de Datos de Sesiones VR
Ayuda a optimizar los umbrales del árbol de decisión
"""

import pandas as pd
import numpy as np
import matplotlib.pyplot as plt
import seaborn as sns
from pathlib import Path


def cargar_datos(ruta_csv):
    """Carga los datos del CSV exportado por Unity"""
    df = pd.read_csv(ruta_csv)
    print(f"✅ Datos cargados: {len(df)} sesiones")
    print(f"\nColumnas disponibles: {list(df.columns)}")
    return df


def analisis_descriptivo(df):
    """Muestra estadísticas descriptivas de los datos"""
    print("\n" + "=" * 60)
    print("ANÁLISIS DESCRIPTIVO")
    print("=" * 60)

    print("\n📊 Estadísticas Generales:")
    print(df[["Precision", "TiempoPromedioReaccion", "TotalErrores"]].describe())

    print("\n📈 Distribución por Nivel de Atención:")
    print(df["NivelAtencionFinal"].value_counts())

    print("\n🎯 Promedios por Nivel:")
    print(
        df.groupby("NivelAtencionFinal")[
            ["Precision", "TiempoPromedioReaccion", "TotalErrores"]
        ].mean()
    )


def sugerir_umbrales(df):
    """Sugiere umbrales óptimos basándose en los datos"""
    print("\n" + "=" * 60)
    print("SUGERENCIA DE UMBRALES ÓPTIMOS")
    print("=" * 60)

    # Separar por nivel de atención
    alto = df[df["NivelAtencionFinal"] == "Alto"]
    medio = df[df["NivelAtencionFinal"] == "Medio"]
    bajo = df[df["NivelAtencionFinal"] == "Bajo"]

    if len(alto) > 0 and len(medio) > 0:
        # Umbral de precisión alta (entre Alto y Medio)
        umbral_precision_alta = (alto["Precision"].min() + medio["Precision"].max()) / 2
        print(f"\n🎯 Umbral Precisión Alta sugerido: {umbral_precision_alta:.2f}")
        print(f"   Actual en Unity: 0.80")
        print(
            f"   Rango Alto: {alto['Precision'].min():.2f} - {alto['Precision'].max():.2f}"
        )
        print(
            f"   Rango Medio: {medio['Precision'].min():.2f} - {medio['Precision'].max():.2f}"
        )

        # Umbral de tiempo rápido
        umbral_tiempo = (
            alto["TiempoPromedioReaccion"].max() + medio["TiempoPromedioReaccion"].min()
        ) / 2
        print(f"\n⏱️  Umbral Tiempo Rápido sugerido: {umbral_tiempo:.2f}s")
        print(f"   Actual en Unity: 1.40s")
        print(
            f"   Rango Alto: {alto['TiempoPromedioReaccion'].min():.2f}s - {alto['TiempoPromedioReaccion'].max():.2f}s"
        )
        print(
            f"   Rango Medio: {medio['TiempoPromedioReaccion'].min():.2f}s - {medio['TiempoPromedioReaccion'].max():.2f}s"
        )

    if len(medio) > 0 and len(bajo) > 0:
        # Umbral de precisión media
        umbral_precision_media = (
            medio["Precision"].min() + bajo["Precision"].max()
        ) / 2
        print(f"\n🎯 Umbral Precisión Media sugerido: {umbral_precision_media:.2f}")
        print(f"   Actual en Unity: 0.50")

        # Umbral de errores tolerables
        umbral_errores = int(
            (medio["TotalErrores"].max() + bajo["TotalErrores"].min()) / 2
        )
        print(f"\n❌ Umbral Errores Tolerables sugerido: {umbral_errores}")
        print(f"   Actual en Unity: 3")


def visualizar_datos(df, guardar=True):
    """Crea visualizaciones de los datos"""
    print("\n📊 Generando visualizaciones...")

    fig, axes = plt.subplots(2, 2, figsize=(15, 12))
    fig.suptitle("Análisis de Sesiones VR - Entrenamiento de Atención", fontsize=16)

    # 1. Distribución de Precisión por Nivel
    sns.boxplot(data=df, x="NivelAtencionFinal", y="Precision", ax=axes[0, 0])
    axes[0, 0].set_title("Distribución de Precisión por Nivel de Atención")
    axes[0, 0].set_ylabel("Precisión")
    axes[0, 0].axhline(y=0.8, color="r", linestyle="--", label="Umbral Alto (0.8)")
    axes[0, 0].axhline(
        y=0.5, color="orange", linestyle="--", label="Umbral Medio (0.5)"
    )
    axes[0, 0].legend()

    # 2. Distribución de Tiempo de Reacción por Nivel
    sns.boxplot(
        data=df, x="NivelAtencionFinal", y="TiempoPromedioReaccion", ax=axes[0, 1]
    )
    axes[0, 1].set_title("Tiempo de Reacción Promedio por Nivel")
    axes[0, 1].set_ylabel("Tiempo (segundos)")
    axes[0, 1].axhline(y=1.4, color="r", linestyle="--", label="Umbral Rápido (1.4s)")
    axes[0, 1].legend()

    # 3. Scatter: Precisión vs Tiempo
    for nivel in df["NivelAtencionFinal"].unique():
        data = df[df["NivelAtencionFinal"] == nivel]
        axes[1, 0].scatter(
            data["Precision"],
            data["TiempoPromedioReaccion"],
            label=nivel,
            alpha=0.6,
            s=100,
        )
    axes[1, 0].set_xlabel("Precisión")
    axes[1, 0].set_ylabel("Tiempo Promedio (s)")
    axes[1, 0].set_title("Relación Precisión vs Tiempo de Reacción")
    axes[1, 0].axvline(x=0.8, color="r", linestyle="--", alpha=0.5)
    axes[1, 0].axhline(y=1.4, color="r", linestyle="--", alpha=0.5)
    axes[1, 0].legend()
    axes[1, 0].grid(True, alpha=0.3)

    # 4. Distribución de Errores
    sns.boxplot(data=df, x="NivelAtencionFinal", y="TotalErrores", ax=axes[1, 1])
    axes[1, 1].set_title("Distribución de Errores por Nivel")
    axes[1, 1].set_ylabel("Número de Errores")
    axes[1, 1].axhline(y=3, color="r", linestyle="--", label="Umbral Tolerables (3)")
    axes[1, 1].legend()

    plt.tight_layout()

    if guardar:
        plt.savefig("analisis_sesiones_vr.png", dpi=300, bbox_inches="tight")
        print("✅ Gráficos guardados en: analisis_sesiones_vr.png")

    plt.show()


def main():
    """Función principal"""
    print("=" * 60)
    print("ANÁLISIS DE DATOS - ENTRENAMIENTO DE ATENCIÓN VR")
    print("=" * 60)

    # Ruta por defecto en la carpeta DatosExportados del proyecto
    ruta_default = "DatosExportados/datos_sesiones_vr.csv"

    # Cargar datos
    ruta_csv = input(
        f"\n📁 Ingresa la ruta del archivo CSV (o presiona Enter para usar '{ruta_default}'): "
    ).strip()
    if not ruta_csv:
        ruta_csv = ruta_default

    try:
        df = cargar_datos(ruta_csv)

        # Análisis
        analisis_descriptivo(df)
        sugerir_umbrales(df)

        # Visualizaciones
        respuesta = input("\n¿Deseas generar visualizaciones? (s/n): ").strip().lower()
        if respuesta == "s":
            visualizar_datos(df)

        print("\n✅ Análisis completado!")

    except FileNotFoundError:
        print(f"\n❌ Error: No se encontró el archivo '{ruta_csv}'")
        print("   Asegúrate de que el archivo existe y la ruta es correcta.")
    except Exception as e:
        print(f"\n❌ Error: {e}")


if __name__ == "__main__":
    main()
