using UnityEngine;

namespace Assets.Scripts
{
    public class MenuManager : MonoBehaviour
    {
        private GameManager _gameManager;

        void Start()
        {
            _gameManager = FindObjectOfType<GameManager>();
        }

        public void SetBoolCustomSeed(bool value)
        {
            _gameManager.UseRandomSeed = !value;
        }

        public void SetIsRunning(bool value)
        {
            _gameManager.HasStarted = value;
        }

        public void Regenerate()
        {
            _gameManager.GenerateBoard();
        }

        public void SetRandomFillPercent(float fillPercent)
        {
            _gameManager.RandomFillPercent = fillPercent;
        }

        public void SetSpeed(float speed)
        {
            _gameManager.Speed = speed;
        }

        public void SetCustomSeedValue(string seed)
        {
            _gameManager.Seed = seed;
        }

        public void SetAlgorithm(int value)
        {
            _gameManager.Algorithm = (GameManager.Types) value;
            this.Regenerate();
        }
    }
}
