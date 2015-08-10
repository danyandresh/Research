using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace CodeSandbox
{
    internal class Tpl
    {
        public static void Main(string[] args)
        {
            var httpClient = new HttpClient();
            var streamTask = httpClient.GetStreamAsync(@"https://raw.githubusercontent.com/danyandresh/Research/high_performance_net_apps/TPL.md");
            streamTask.Wait();
            var bufferingTask = streamTask.Result.BufferStreamToLocalStream();
            bufferingTask.Wait();

            var streamReader = new StreamReader(bufferingTask.Result);
            Console.WriteLine(streamReader.ReadToEnd());
        }
    }

    internal static class AsyncStreamReader
    {
        public static Task<Stream> BufferStreamToLocalStream(this Stream sourceStream, int chunkSize = 1024)
        {
            var result = new TaskCompletionSource<Stream>();

            var buffer = new byte[chunkSize];
            var localStream = new MemoryStream
            {
                Capacity = chunkSize
            };

            ReadToBuffer(buffer, sourceStream, localStream, result);

            return result.Task;
        }

        private static void WriteToLocalStream(Task<int> readTask, byte[] buffer, Stream sourceStream, Stream destinationStream, TaskCompletionSource<Stream> taskCompletionSource)
        {
            if (!readTask.IsCompleted)
            {
                return;
            }

            var bytesRead = readTask.Result;
            destinationStream.Write(buffer, 0, bytesRead);
            if (bytesRead > 0)
            {
                ReadToBuffer(buffer, sourceStream, destinationStream, taskCompletionSource);
            }
            else
            {
                destinationStream.Position = 0;
                taskCompletionSource.TrySetResult(destinationStream);
                sourceStream.Dispose();
            }
        }

        private static void ReadToBuffer(byte[] buffer, Stream sourceStream, Stream destinationStream, TaskCompletionSource<Stream> taskCompletionSource)
        {
            var t = sourceStream.ReadAsync(buffer, 0, buffer.Length);
            t.ContinueWith(
                readTask => WriteToLocalStream(readTask, buffer, sourceStream, destinationStream, taskCompletionSource));
        }
    }
}