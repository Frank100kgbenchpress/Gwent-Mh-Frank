using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LineNumberDisplay : MonoBehaviour
{
    public TMP_InputField inputField;
    public TMP_Text lineNumbersText;
    public Scrollbar scrollbar;
    public Scrollbar otherScrollbar; // Aquí asigna el Scrollbar del otro texto que también se puede desplazar
    void Start()
    {
        // Suscribirse al evento de cambio de texto del campo de entrada
        inputField.onValueChanged.AddListener(UpdateLineNumbers);

        // Suscribirse al evento de desplazamiento del Scrollbar del campo de entrada
        scrollbar.onValueChanged.AddListener(OnScrollbarValueChanged);
        
        // Actualizar los números de línea inicialmente
        UpdateLineNumbers(inputField.text);
    }
    void UpdateLineNumbers(string newText)
    {
        string[] lines = newText.Split('\n');

        // Construir el texto de los números de línea
        string lineNumbers = "";
        for (int i = 1; i <= lines.Length; i++)
        {
            lineNumbers += i.ToString() + "\n";
        }

        // Actualizar el texto que muestra los números de línea
        lineNumbersText.text = lineNumbers;
    }
    void OnScrollbarValueChanged(float value)
    {
        // Si tienes otro scrollbar, ajusta su valor también
        if (otherScrollbar != null)
        {
            otherScrollbar.value = value;
        }

        // Ajustar la posición vertical del texto de los números de línea
        lineNumbersText.rectTransform.anchoredPosition = new Vector2(0, value * (inputField.textComponent.preferredHeight - inputField.textViewport.rect.height));
    }
}
