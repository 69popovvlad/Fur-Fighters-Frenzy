using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Core.Tasks
{
    public class WebRequestHandler
    {
        private readonly UnityWebRequest _webRequest;
        private readonly CancellationToken _token;

        private readonly UnityWebRequestAsyncOperation _operation;
        private readonly TaskCompletionSource<string> _completion;

        public Task<string> Task => _completion.Task;
        
        public WebRequestHandler(UnityWebRequest webRequest, CancellationToken token)
        {
            _webRequest = webRequest;
            _token = token;

            _completion = new TaskCompletionSource<string>();
            
            _operation = webRequest.SendWebRequest();
            _operation.completed += OnCompleted;
        }

        private void OnCompleted(AsyncOperation operation)
        {
            if (_token.IsCancellationRequested)
            {
                return;
            }

            var request = ((UnityWebRequestAsyncOperation) operation).webRequest;
            if (request.result is UnityWebRequest.Result.ProtocolError or UnityWebRequest.Result.ConnectionError)
            {
                _completion.TrySetException(new AsyncWebRequestException(request));
            }
            else
            {
                _completion.TrySetResult(request.downloadHandler.text);
            }
        }
    }
}