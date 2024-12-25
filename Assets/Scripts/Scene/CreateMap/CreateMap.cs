using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Scene.CreateMap
{
    public sealed class CreateMap : MonoBehaviour
    {
        public Button BackButton;
        public Button SaveButton;

        public DragMapParts[] DragMapPartsDatas = new DragMapParts[DragMapParts.TileCount];
        public GameObject[] RedTiles = new GameObject[DragMapParts.TileCount];
        public readonly PartsConnectable[] MapPartsConnectionLimit = new PartsConnectable[15]
        {
            new PartsConnectable() { isLeft = false, isRight = true,  isUp = false, isDown = true  },
            new PartsConnectable() { isLeft = true,  isRight = true,  isUp = false, isDown = true  },
            new PartsConnectable() { isLeft = true,  isRight = false, isUp = false, isDown = true  },
            new PartsConnectable() { isLeft = false, isRight = true,  isUp = true,  isDown = true  },
            new PartsConnectable() { isLeft = true,  isRight = true,  isUp = true,  isDown = true  },
            new PartsConnectable() { isLeft = true,  isRight = false, isUp = true,  isDown = true  },
            new PartsConnectable() { isLeft = false, isRight = true,  isUp = true,  isDown = false },
            new PartsConnectable() { isLeft = true,  isRight = true,  isUp = true,  isDown = false },
            new PartsConnectable() { isLeft = true,  isRight = false, isUp = true,  isDown = false },
            new PartsConnectable() { isLeft = true,  isRight = true,  isUp = false, isDown = false },
            new PartsConnectable() { isLeft = false, isRight = false, isUp = true,  isDown = true  },
            new PartsConnectable() { isLeft = true,  isRight = false, isUp = false, isDown = false },
            new PartsConnectable() { isLeft = false, isRight = false, isUp = true,  isDown = false },
            new PartsConnectable() { isLeft = false, isRight = false, isUp = false, isDown = true  },
            new PartsConnectable() { isLeft = false, isRight = true,  isUp = false, isDown = false },
        };


        /// <summary>
        /// Start
        /// </summary>
        void Start()
        {
            BackButton.onClick.AddListener(() => SceneManager.LoadScene("Home"));
            SaveButton.onClick.AddListener(() => Debug.Log("SaveButton"));
        }
    }
}
