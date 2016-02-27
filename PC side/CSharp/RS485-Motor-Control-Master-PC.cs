
//============================================================================================================//
//    Master Control Program to Switch ON and OFF the Motors Connected to the Robot Shield (acts as slave)    //   
//------------------------------------------------------------------------------------------------------------//
//The Program opens up a connection to the USB2SERIAL (USB to RS485) converter board.Put the USB2SERIAL board //
//in Transmit mode and sends control characters (ASCII "A","Q","D","E") which are interpreted by the-         //
// -Robot shield to control the two motors  connected to it.                                                  //
//------------------------------------------------------------------------------------------------------------//
//  |-----------------|                                                                                       //
//  | RS485 Motor-    |                  USB2SERIAL                                                           //
//  | -Ctrl-Master.exe|                  +==============================================+                     //
//  |                 |                  |	 +-----------+         +---------+          |                     //
//  |-----------------|                  |   |   	  TXD|-------->|DI       |          |                     //
//   \ [][][][][][][]  \    USB       +--|	 |        RXD|---------|RO      A|---+      |                     //
//    \ [] [] [] [] []  \=============|	     |USB        |         |         |   |      |                     //
//     \-----------------\            +--|   |       ~RTS|-------->|~RE     B|---|--+   |                     //
//                                       |   |	     ~DTR|-------->|DE       |   |  |   |                     //
//                                       |   +-----------+         +---------+   |  |   |                     //
//                                       |		FT232RL              MAX485      |  |   |                     //
//                                       +======================================[A]=[B]=+                     //
//                                                                                \/                          //
//                                                                                /\ Twisted Pair             //
//                                                                                \/ RS485                    //
//                                              Robot Shield On MSP430 Launchpad  /\                          //
//                                                       +======================[A]=[B]===+                   //
//                                                       |                       |   |    |                   //
//                                (Motor Right)----------|   +-------+        +--+---+-+  |                   //
//                                                       |   | L293D |        | MAX485 |  |                   //
//                                (Motor  Left)----------|   +-------+        +--------+  |                   //
//                                                       |                                |                   //
//                                                       +================================+                   //
//                                                                                                            //
//============================================================================================================//
// Compiler/IDE  :	Microsoft Visual Studio Express 2013 for Windows Desktop(Version 12.0)                    //
//                  C# (Dot net Framework)                                                                    //
// OS            :	Windows(Windows 7)                                                                        // 
// License       :  BSD (2 clause)                                                                            // 
// Programmer    :	Rahul.S                                                                                   //
// Date	         :	20-May-2015                                                                               //
//============================================================================================================//  

//============================================================================================================//
// www.xanthium.in                                                                                            //
// (c) Rahul.S 2015                                                                                           //
//============================================================================================================//

using System;
using System.IO.Ports; //required for accessing the SerialPort Class

namespace RS485_Motor_Control
{
    class MasterControlProgram
    {
        static void Main()
        {
            string COMPortName;   // Name of the Serial port eg :- COM32
            bool Sentry = true; 
            string Choice;        // Used for Menu

            HeaderDialog();       // Displays the top banner
            InstructionsDialog(); // Displays the instructions 

            Console.Write("\n\t  Enter The COM port Number ->"); // Enter the COM port number 
            COMPortName = Console.ReadLine();                    // Read the COM port number
            COMPortName = COMPortName.Trim();                    // Trim any unwanted SPACE's
            

            SerialPort COMPort = new SerialPort();//Create a new SerialPort Object named COMPort

            COMPort.PortName = COMPortName;   // Assign the name of your COM port 
            COMPort.BaudRate = 9600;          // Set the baudrate = 9600 ,(8N1 Format)
            COMPort.StopBits = StopBits.One;  // Number of Stop bits = 1,(8N1 Format) 
            COMPort.Parity = Parity.None;     // No Parity, (8N1 Format)
            COMPort.DataBits = 8;             // Databits = 8,(8N1 Format)

            try
            {
                COMPort.Open(); // try to open the COM port based on the name you entered earlier 
            }
            catch               // If opening fails ,print error message and exit program              
            {
                Console.WriteLine("\n\t  Unable to Open Serial Port !");
                Console.WriteLine("\n\t  Press Any Key to Exit");
                Sentry = false; // Make Sentry False So that code will not enter the While () loop below 
            }
            
            while (Sentry == true) 
            {
                Console.Clear();                           // Clear the console Window 
                HeaderDialog();                            // Show the dialogs again
                InstructionsDialog();
                Console.Write("\n\t  Enter your Choice->");// Prompt for choice
                Choice = Console.ReadLine();               // Read choice 
                Choice = Choice.ToUpper();                 // Convert to upper case
                Choice = Choice.Trim();                    // Remove SPACE's

                COMPort.DtrEnable = false ; // ~DTR [FT232] ->  DE[MAX485] in USB2SERIAL,DTR =0 ,so ~DTR = 1,DE=1 so transmit mode 
                COMPort.RtsEnable = false; //  ~RTS [FT232] -> ~RE[MAX485] in USB2SERIAL,RTS =0,so ~RTS =1,~RE =1

                switch(Choice)
                {
                    case "A": COMPort.Write("A"); // Send 'A' to Start the Right motor
                              break;
                    case "Q": COMPort.Write("Q"); // Send 'Q' to Stop  the Right Motor
                              break;
                    case "D": COMPort.Write("D"); // Send 'D' to Start the Left Motor 
                              break;
                    case "E": COMPort.Write("E"); // Send 'E' to Stop  the Left Motor
                              break;
                    case "X": Sentry = false;     // Exit from the loop
                              Console.WriteLine("\n\t  Press Any Key to Exit");
                              break;
                    default: break;
                }
            }
            COMPort.Close(); //Close the ComPort
            Console.Read();
        }//End of Main()

        static void HeaderDialog()
        {
            Console.WriteLine("\n\t +-----------------------------------------------+ ");
            Console.WriteLine("\t | RS485 Motor Control using MSP430 Robot Shield | ");
            Console.WriteLine("\t +-----------------------------------------------+ ");
            Console.WriteLine("\t |            (c) www.xanthium.in                | ");
            Console.WriteLine("\t +-----------------------------------------------+ ");
         }

        static void InstructionsDialog()
        {
            Console.WriteLine("\n\t  Enter A to Start Right Motor ");
            Console.WriteLine("\t  Enter Q to Stop  Right Motor ");
            Console.WriteLine("\t  Enter D to Start Left  Motor ");
            Console.WriteLine("\t  Enter E to Stop  Left  Motor ");
            Console.WriteLine("\t  Enter X to Quit  Program");
        }

    }//End of class MasterControlProgram
}//End of namespace 
