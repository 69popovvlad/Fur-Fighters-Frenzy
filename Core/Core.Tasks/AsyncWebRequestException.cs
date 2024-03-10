using System;
using UnityEngine.Networking;

namespace Core.Tasks
{
    internal class AsyncWebRequestException: Exception
    {
        public string RequestUrl
        {
            get;
        }

        public string Error
        {
            get;
        }

        public long ErrorCode
        {
            get;
        }

        internal AsyncWebRequestException(UnityWebRequest request) :
            base($"url: \"{request.url}\" error: \"{request.error}\" responseCode: \"{request.responseCode}\"")
        {
            RequestUrl = request.url;
            Error = request.error;
            ErrorCode = request.responseCode;
        }
    }
}