using System.Threading;
using System.Threading.Tasks;
using Core.Tasks.ThreadTrampolines;
using UnityEngine.Networking;

namespace Core.Tasks
{
    public static class RemoteFileDownloader
    {
        // It should be equals Environment.ProcessorCount but we have bad data hosting
        private static readonly SemaphoreSlim _webRequestsThrottler = new SemaphoreSlim(1);

        public static async Task<string> DownloadFile(string url, CancellationToken token, bool clearCookie = true)
        {
            await _webRequestsThrottler.WaitAsync(token).ScheduleAwait();
            
            await UnityTask.MainThread;

            token.ThrowIfCancellationRequested();
            
            if (clearCookie)
            {
                UnityWebRequest.ClearCookieCache();
            }

            var request = new UnityWebRequest(url, UnityWebRequest.kHttpVerbGET, new DownloadHandlerBuffer(), default);

#if UNITY_EDITOR
            var certificate = new CertificateMock();
            request.certificateHandler = certificate;
#endif

            var handler = new WebRequestHandler(request, token);
            
            await UnityTask.OffThread;
            
            string result;
            try
            {
                var task = handler.Task;
                await task;
                result = task.Result;
            }
            finally{
#if UNITY_EDITOR
            certificate.Dispose();
#endif
                try
                {
                    _webRequestsThrottler.Release();
                }
                catch
                {
                   /* Nothing to do */
                }
            }
            
            
            return result;
        }
    }
    
#if UNITY_EDITOR
    // Cause Unity bug
    // https://issuetracker.unity3d.com/issues/curl-error-60-cert-verify-failed-error-when-using-unitywebrequest-dot-get-on-hololens-2
    //
    // https://answers.unity.com/questions/1874008/curl-error-60-cert-verify-failed-unitytls-x509veri-1.html
    // https://forum.unity.com/threads/webrequest-fails-with-curl-error-60-cert-verify-failed-unitytls_x509verify_flag_user_error1.1206283/
    // https://forum.unity.com/threads/unitywebrequest-report-an-error-ssl-ca-certificate-error.617521/
    public class CertificateMock : CertificateHandler
    {
        protected override bool ValidateCertificate(byte[] certificateData)
        {
            return true;
        }
    }
#endif
}