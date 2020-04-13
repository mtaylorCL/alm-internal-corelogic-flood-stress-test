using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace CoreLogic_Flood
{
    class Program
    {
        static void Main(string[] args)
        {
            List <LatLon> latlonlist = LatLonFactory.Create();

            var tasks = latlonlist
                .Select(data => Task.Factory.StartNew(arg => CallService(data.Latitude, data.Longitude), TaskContinuationOptions.LongRunning | TaskContinuationOptions.PreferFairness))
                .ToArray();

            var timeout = TimeSpan.FromMinutes(10);
            Task.WaitAll(tasks, timeout);
        }

        static long CallService(double latitude, double longitude)
        {
            try
            {
                //ManualResetEvent queryCompletedEvent = new ManualResetEvent(false);

                //ThreadPool.QueueUserWorkItem(o =>
                //{

                    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("https://betasoap.floodcert.com/wspost/v2/autohit?schemaId=mismo24");

                    string payload = BuildPayload(latitude, longitude);
                    byte[] bytes = Encoding.ASCII.GetBytes(payload);

                    request.ContentType = "text/xml";
                    request.ContentLength = bytes.Length;
                    request.Method = "POST";
                    request.Proxy = WebRequest.GetSystemWebProxy();
                    request.Proxy.Credentials = CredentialCache.DefaultCredentials;

                    Stream stream = request.GetRequestStream();
                    stream.Write(bytes, 0, bytes.Length);
                    stream.Close();

                //    queryCompletedEvent.Set();
                //});

                //while (!queryCompletedEvent.WaitOne(10))
                //    Application.DoEvents();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                //_completedCount++;
            }

            return 0;
        }

        static string BuildPayload(double latitude, double longitude)
        {
            string payload = $@"<REQUEST_GROUP MISMOVersionID=""2.4""><SUBMITTING_PARTY LoginAccountIdentifier = ""ALAM-00001"" LoginAccountPassword = ""iE7S3wfL"" /><REQUEST><REQUEST_DATA><FLOOD_REQUEST MISMOVersionID = ""2.4"" _ActionType = ""Original""><_PRODUCT _CategoryDescription = ""Flood""><_NAME _Identifier = ""FCK"" /></_PRODUCT><PROPERTY><_IDENTIFICATION LatitudeNumber = ""{latitude}"" LongitudeNumber = ""{longitude}"" /></PROPERTY></FLOOD_REQUEST></REQUEST_DATA></REQUEST></REQUEST_GROUP>";

            return payload;
        }
    }
}
