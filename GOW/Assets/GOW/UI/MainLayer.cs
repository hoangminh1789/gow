using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GOW
{
    public class MainLayer : MonoBehaviour
    {
        void Start()
        {

        }

        public void OnClick_OOP()
        {
            SceneManager.LoadScene("GOW/_Scenes/Battle_OOP");
        }

        public void OnClick_ECS()
        {
            SceneManager.LoadScene("GOW/_Scenes/Battle_ECS");
        }
    }
}
