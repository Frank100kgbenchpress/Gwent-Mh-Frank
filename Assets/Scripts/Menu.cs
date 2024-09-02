using UnityEngine;
using UnityEngine.SceneManagement;
//aqui manejo los cambios de escena//
public  class Menu : MonoBehaviour
{
   public void  PlayGame() =>   SceneManager.LoadSceneAsync(0); 
   public static void WinnerScreen() =>   SceneManager.LoadScene(1);
   public static void WinnerScreen2() =>   SceneManager.LoadScene(3);
   public void QuitGame() =>   Application.Quit();
   public void BackToMenu() =>   SceneManager.LoadSceneAsync(2);
}
