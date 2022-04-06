using UnityEngine;
using UnityEngine.SceneManagement;

namespace WreckTheBrick
{
    public class MainMenuManager : MonoBehaviour
    {
        public void Play()
        {
            SceneManager.LoadScene(1);
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}
