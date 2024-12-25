using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Scene.Home
{
    public class Home : MonoBehaviour
    {
        [SerializeField] private Button _soundTestButton;
        [SerializeField] private Button _createMapButton;

        /// <summary>
        /// Awake
        /// </summary>
        private void Awake()
        {
            _soundTestButton.onClick.AddListener(() => SceneManager.LoadScene("SoundTest"));
            _createMapButton.onClick.AddListener(() => SceneManager.LoadScene("CreateMap"));
        }
    }
}
