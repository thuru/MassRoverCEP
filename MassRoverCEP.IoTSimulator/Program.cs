﻿using Microsoft.Azure.EventHubs;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;

namespace MassRoverCEP.IoTSimulator
{
    class Program
    {
        private static EventHubClient eventHubClient;
        private const string EhConnectionString = "";
        private const string EhEntityPath = "";

        static void Main(string[] args)
        {
            var connectionStringBuilder = new EventHubsConnectionStringBuilder(EhConnectionString)
            {
                EntityPath = EhEntityPath
            };

            

            eventHubClient = EventHubClient.CreateFromConnectionString(connectionStringBuilder.ToString());

            while (true)
            {
                var message = JsonConvert.SerializeObject(IoTLocalSimulator.GetMessage());

                eventHubClient.SendAsync(new EventData(Encoding.UTF8.GetBytes(message))).GetAwaiter().GetResult();

                Console.WriteLine(message);
            }
        }
    }

    class IoTMessage
    {
        public string DeviceId { get; set; }
        public string PipeCode { get; set; }
        public int Temperature { get; set; }
        public DateTime DateTime { get; set; }
    }


    static class IoTLocalSimulator
    {
        internal static IoTMessage GetMessage()
        {
            var deviceId = Guid.NewGuid().ToString().ToLower();
            var pipecode = deviceId.ToCharArray()[0].ToString();
            var temperature = new Random().Next(45, 100);

            return new IoTMessage
            {
                DeviceId = deviceId,
                PipeCode = pipecode,
                Temperature = temperature,
                DateTime = DateTime.UtcNow
            };
        }
    }
}
