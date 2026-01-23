using System;

/// <summary>
/// Representa una métrica individual de interacción con un estímulo
/// </summary>
[Serializable]
public class Metrica
{
    public float TiempoReaccion { get; set; }
    public bool FueCorrecta { get; set; }
    public DateTime Timestamp { get; set; }
    
    public Metrica(float tiempoReaccion, bool fueCorrecta)
    {
        TiempoReaccion = tiempoReaccion;
        FueCorrecta = fueCorrecta;
        Timestamp = DateTime.Now;
    }
}
