using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateScreen : MonoBehaviour
{    
    public void RotateCamera()
    {
        Camera mainCamera = Camera.main;
        mainCamera.transform.localScale = new Vector3(-1, -1, 1);
    }

    // Método para revertir la rotación de la cámara
    public void RotateBack()
    {
        Camera mainCamera = Camera.main;
        mainCamera.transform.localScale = new Vector3(1, 1, 1);
    }
}
