using qman.controller.src;
using src;
using src.Commands;
using System;
using System.Collections.Generic;
using System.Text;

Console.WriteLine("hello world");
BroadCastHandlers.Initialize();
var controller = new Controller();

// Assign your function

controller.InitializeXport();
while (true) ;