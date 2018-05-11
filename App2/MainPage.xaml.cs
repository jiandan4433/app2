using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;
using System.Diagnostics;
using Windows.Networking.Sockets;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Windows.UI.Core;
using Newtonsoft.Json;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace App2
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>

    public sealed partial class MainPage : Page
    {
        static string PortNumber = "1337";
        public MainPage()
        {

            this.InitializeComponent();
            img.SetValue(Canvas.LeftProperty, 10);
            img.SetValue(Canvas.TopProperty, 10);

            this.StartServer();

        }


        private async void StartServer()
        {
            try
            {
                var streamSocketListener = new Windows.Networking.Sockets.StreamSocketListener();

                // The ConnectionReceived event is raised when connections are received.
                streamSocketListener.ConnectionReceived += this.StreamSocketListener_ConnectionReceived;

                // Start listening for incoming TCP connections on the specified port. You can specify any port that's not currently in use.
                await streamSocketListener.BindServiceNameAsync(MainPage.PortNumber);

                //this.serverListBox.Items.Add("server is listening...");
            }
            catch (Exception ex)
            {
                Windows.Networking.Sockets.SocketErrorStatus webErrorStatus = Windows.Networking.Sockets.SocketError.GetStatus(ex.GetBaseException().HResult);
               // this.serverListBox.Items.Add(webErrorStatus.ToString() != "Unknown" ? webErrorStatus.ToString() : ex.Message);
            }
        }

        private async void StreamSocketListener_ConnectionReceived(Windows.Networking.Sockets.StreamSocketListener sender, Windows.Networking.Sockets.StreamSocketListenerConnectionReceivedEventArgs args)
        {
            double x = 0;
            double y = 0;
            using (var streamReader = new StreamReader(args.Socket.InputStream.AsStreamForRead()))
            {
                //DataContractJsonSerializer ser = new DataContractJsonSerializer(deserializedUser.GetType());  
                //deserializedUser = ser.ReadObject(ms) as Coord; 

                ////var coords = new (streamReader).Deserialize<Dictionary<string, Double>>;
                // x = coords["x"];
                // y = coords["y"];
                var ser = new JsonSerializer();
                Console.WriteLine(streamReader);
                using (var jsonTextReader = new JsonTextReader(streamReader))
                {
                    dynamic c = ser.Deserialize(jsonTextReader);
                    x = c.x * 50 ;
                    y = c.y * 50 ;
                   // updateCoordinates(x, y);
                    Debug.WriteLine(x);
                    Debug.WriteLine(y);


                }

            }

            await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => this.updateCoordinates(x, y));

            // Echo the request back as the response.
            //sender.Dispose();

           // await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => this.serverListBox.Items.Add("server closed its socket"));
        }


        private void updateCoordinates(Double x, Double y)
        {
            this.img.SetValue(Canvas.LeftProperty, x);

            this.img.SetValue(Canvas.TopProperty, y);
        }


    }
}
