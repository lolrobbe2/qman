using qman.controller.src;
using src.Commands;
using System;
using System.Collections.Generic;
using System.Text;

Console.WriteLine("hello world");
BroadCastHandlers.Initialize();
var deviceManager = new XPORT();

// Assign your function
deviceManager.OnData = (data, remoteEndpoint) =>
{
    // 'data' is your byte array (Datagram)
    // 'remoteEndpoint' tells you which IP sent it
    Console.WriteLine($"Received {data.Length} bytes from {remoteEndpoint}");
};
deviceManager.start();