using System.Collections.Generic;

/// <summary>
/// Representa al usuario y almacena sus métricas de atención
/// </summary>
public class Usuario
{
    public string Id { get; private set; }
    public NivelAtencion NivelAtencionActual { get; set; }
    public List<Metrica> HistorialMetricas { get; private set; }
    
    public Usuario(string id)
    {
        Id = id;
        NivelAtencionActual = NivelAtencion.Medio;
        HistorialMetricas = new List<Metrica>();
    }
    
    public void AgregarMetrica(Metrica metrica)
    {
        HistorialMetricas.Add(metrica);
    }
}
