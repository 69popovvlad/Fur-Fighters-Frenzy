using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Core.Ui
{
    public static class ExceptionShowerHolder
    {
        private static Action<Exception> _exceptionShowerAction;
        
        public static void SetExceptionShower(Action<Exception> exceptionShowerAction)
        {
            _exceptionShowerAction = exceptionShowerAction;
        }

        public static void ShowException(this Exception exception)
        {
            if (_exceptionShowerAction == null)
            {
                throw new Exception($"{nameof(_exceptionShowerAction)} should be initialized first." +
                                    $"\nUse {nameof(SetExceptionShower)} in {nameof(ExceptionShowerHolder)}");
            }
            
            _exceptionShowerAction.Invoke(exception);
        }
    }
    
    public abstract class ExceptionWindowBase: PopUp
    {
        [SerializeField] protected Button ReloadButton;

        protected virtual void OnReloadButtonClicked()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private void Awake()
        {
            ReloadButton.clicked += OnReloadButtonClicked;
        }

        private void OnDestroy()
        {
            ReloadButton.clicked -= OnReloadButtonClicked;
        }
    }
}