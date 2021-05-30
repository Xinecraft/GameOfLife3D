using UnityEngine;

namespace Assets.Scripts
{
    public class Node : MonoBehaviour
    {
        public int x;
        public int y;
        public Material GrayMaterial;
        public Material GreenMaterial;

        private GameManager _gameManager;
        // Start is called before the first frame update
        void Start()
        {
            _gameManager = FindObjectOfType<GameManager>();
        }

        public void OnMouseUp()
        {
            // Make Game Manager of this node to 1
            _gameManager.ToggleState(x, y);
        }

        public void SetState(bool newState)
        {
            if (newState == true)
            {
                gameObject.SetActive(true);
                gameObject.GetComponent<MeshRenderer>().material = GreenMaterial;
            }
            else
            {
                gameObject.SetActive(false);
                gameObject.GetComponent<MeshRenderer>().material = GrayMaterial;
            }
        }
    }
}
