using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Menu : MonoBehaviour
{
   public void PlayGame()
   {
      SceneManager.LoadSceneAsync(0);
   }
   public void QuitGame()
   {
      Application.Quit();
   }
   public void BackToMenu()
   {
      SceneManager.LoadSceneAsync(1);
   }
}
