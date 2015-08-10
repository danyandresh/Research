﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
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
            var readStringContentTask = bufferingTask.ContinueWith(
                t => t.Result.ReadStringContent()
                    .Result);
            readStringContentTask.Wait();

            Console.WriteLine(readStringContentTask.Result);
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

            ReadToDestinationStream(buffer, sourceStream, localStream, result);

            return result.Task;
        }

        public static Task<string> ReadStringContent(this Stream sourceStream, int chunkSize = 512, Encoding encoding = null)
        {
            var byteArrayResult = new TaskCompletionSource<IEnumerable<byte>>();

            var localBuffer = new byte[chunkSize];
            var buffer = new List<byte>();
            encoding = encoding ?? Encoding.UTF8;


            ReadToDestinationBuffer(localBuffer, sourceStream, buffer, byteArrayResult);

            var result = new TaskCompletionSource<string>();
            byteArrayResult.Task.ContinueWith(
                (t) =>
                {
                    var stringContent = encoding.GetString(t.Result.ToArray());

                    result.TrySetResult(stringContent);
                });

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
                ReadToDestinationStream(buffer, sourceStream, destinationStream, taskCompletionSource);
            }
            else
            {
                destinationStream.Position = 0;
                taskCompletionSource.TrySetResult(destinationStream);
                sourceStream.Dispose();
            }
        }

        private static void ReadToDestinationStream(byte[] buffer, Stream sourceStream, Stream destinationStream, TaskCompletionSource<Stream> taskCompletionSource)
        {
            var t = sourceStream.ReadAsync(buffer, 0, buffer.Length);
            t.ContinueWith(
                readTask => WriteToLocalStream(readTask, buffer, sourceStream, destinationStream, taskCompletionSource));
        }

        private static void WriteToBuffer(Task<int> readTask, byte[] buffer, Stream sourceStream, List<byte> buffers, TaskCompletionSource<IEnumerable<byte>> taskCompletionSource)
        {
            if (!readTask.IsCompleted)
            {
                return;
            }

            var bytesRead = readTask.Result;
            buffers.AddRange(buffer.Take(bytesRead));
            if (bytesRead > 0)
            {
                ReadToDestinationBuffer(buffer, sourceStream, buffers, taskCompletionSource);
            }
            else
            {

                taskCompletionSource.TrySetResult(buffers);
                sourceStream.Dispose();
            }
        }

        private static void ReadToDestinationBuffer(byte[] buffer, Stream sourceStream, List<byte> buffers, TaskCompletionSource<IEnumerable<byte>> taskCompletionSource)
        {
            var t = Task<int>.Factory.FromAsync(
                sourceStream.BeginRead,
                sourceStream.EndRead,
                buffer,
                0,
                buffer.Length,
                null);

            t.ContinueWith(
                readTask => WriteToBuffer(readTask, buffer, sourceStream, buffers, taskCompletionSource));
        }
    }
}