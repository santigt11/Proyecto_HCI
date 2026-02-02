using UnityEngine;
using UnityEngine.UI;

public class MenuButtonLogger : MonoBehaviour
{
    [SerializeField] private Button botonIniciar;
    [SerializeField] private Button botonSalir;

    private void Start()
    {
        if (botonIniciar != null)
            botonIniciar.onClick.AddListener(() => Debug.Log("Botón Iniciar presionado"));
        else
            Debug.LogWarning("Botón Iniciar no asignado en MenuButtonLogger");

        if (botonSalir != null)
            botonSalir.onClick.AddListener(() => Debug.Log("Botón Salir presionado"));
        else
            Debug.LogWarning("Botón Salir no asignado en MenuButtonLogger");
    }
}