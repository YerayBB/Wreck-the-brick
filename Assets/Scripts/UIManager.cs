using UnityEngine;
using TMPro;

namespace WreckTheBrick
{
    [RequireComponent(typeof(Animator))]
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance {  get; private set; }

        private Animator _animator;
        private TMP_Text _notificationText;
        private TMP_Text _livesText;

        #region MonoBehaviourCalls

        private void Awake()
        { 
            if(Instance == null)
            {
                Instance = this;
                _animator = GetComponent<Animator>();
                _notificationText = transform.GetChild(1).GetComponent<TMP_Text>();
                _livesText = transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        #endregion

        public void GameOverUI()
        {
            _animator.SetTrigger("GameOver");
        }

        public void ClearLevelUI()
        {
            _animator.SetTrigger("ClearLevel");
        }

        public void Notification(string message)
        {
            _notificationText.text = message;
            _animator.SetTrigger("NotificationOn");
        }

        public void ContinueUI()
        {
            _animator.SetTrigger("Continue");
        }

        public void ShowLives(int lives)
        {
            _livesText.text = lives.ToString();
        }
    }
}