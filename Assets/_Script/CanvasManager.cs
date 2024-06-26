using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    public Canvas UIEditor, UIPreview;
    public Button editor, preview; // Referencia al botón que quieres modificar


    // Método para activar el Canvas
    public void CambiarModo()
    {
        if (UIEditor.gameObject.activeSelf)
        {
            UIEditor.gameObject.SetActive(false);
            UIPreview.gameObject.SetActive(true);

            // Cambiar el texto del botón
            Text buttonText = editor.GetComponentInChildren<Text>();
            if (buttonText != null)
            {
                buttonText.text = "Modo Preview";
            }
            else
            {
                Debug.LogError("No Text component found on the button.");
            }
        }
        else
        {
            UIEditor.gameObject.SetActive(true);
            UIPreview.gameObject.SetActive(false);

             // Cambiar el texto del botón
            Text buttonText = preview.GetComponentInChildren<Text>();
            if (buttonText != null)
            {
                buttonText.text = "Modo Edición";
            }
            else
            {
                Debug.LogError("No Text component found on the button.");
            }
        }
    }
}