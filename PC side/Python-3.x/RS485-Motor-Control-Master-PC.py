#------------------------------------------------------------------------------------------------------------#
# Python 3.x.x                                                                                               #
#============================================================================================================#
#    Master Control Program to Switch ON and OFF the Motors Connected to the Robot Shield (acts as slave)    #   
#------------------------------------------------------------------------------------------------------------#
#The Program opens up a connection to the USB2SERIAL (USB to RS485) converter board.Put the USB2SERIAL board #
#in Transmit mode and sends control characters (ASCII "A","Q","D","E") which are interpreted by the-         #
# -Robot shield to control the two motors  connected to it.                                                  #
#------------------------------------------------------------------------------------------------------------#
#  |-----------------|                                                                                       #
#  | RS485 Motor-    |                  USB2SERIAL                                                           #
#  | -Ctrl-Master.py |                  +==============================================+                     #
#  |                 |                  |   +-----------+         +---------+          |                     #
#  |-----------------|                  |   |        TXD|-------->|DI       |          |                     #
#   \ [][][][][][][]  \    USB       +--|   |        RXD|---------|RO      A|---+      |                     #
#    \ [] [] [] [] []  \=============|	    |USB        |         |         |   |      |                     #
#     \-----------------\            +--|   |       ~RTS|-------->|~RE     B|---|--+   |                     #
#                                       |   |	    ~DTR|-------->|DE       |   |  |   |                     #
#                                       |   +-----------+         +---------+   |  |   |                     #
#                                       |      FT232RL              MAX485      |  |   |                     #
#                                       +======================================[A]=[B]=+                     #
#                                                                                \/                          #
#                                                                                /\ Twisted Pair             #
#                                                                                \/ RS485                    #
#                                              Robot Shield On MSP430 Launchpad  /\                          #
#                                                       +======================[A]=[B]===+                   #
#                                                       |                       |   |    |                   #
#                                (Motor Right)----------|   +-------+        +--+---+-+  |                   #
#                                                       |   | L293D |        | MAX485 |  |                   #
#                                (Motor  Left)----------|   +-------+        +--------+  |                   #
#                                                       |                                |                   #
#                                                       +================================+                   #
#                                                                                                            #
#============================================================================================================#
# Interpretor/IDE  :	Python3.4.1/IDLE                                                                     #
# Libraries        :    PySerial Required                                                                    #
# OS               :	Windows(Windows 7)/Linux (Python3.x.x required)                                      #                                  
# License          :    BSD (2 clause)                                                                       # 
# Programmer       :	Rahul.S                                                                              #
# Date	           :	20-May-2015                                                                          #
#============================================================================================================#  

import serial # import Pyserial

#Routines to print information about usage of program and CLI
def PrintBanner():
    print('\n\t +-----------------------------------------------+')
    print('\t | RS485 Motor Control using MSP430 Robot Shield | ' )
    print('\t |                Python 3.x                     | ' )
    print('\t +-----------------------------------------------+ ' )
    print('\t |            (c) www.xanthium.in                | ' )
    print('\t +-----------------------------------------------+ ' )

def Usage():
	print('\n\t +-------------------------------------------+')
	print('\t | Windows -> COMxx     eg COM32             |')
	print('\t | Linux   ->/dev/ttyS* eg /dev/ttyUSB0      |')
	print('\t +-------------------------------------------+')

def PrintInstructions():
    print('\n\t  Enter A to Start Right Motor ')
    print('\t  Enter Q to Stop  Right Motor '  )
    print('\t  Enter D to Start Left  Motor '  )
    print('\t  Enter E to Stop  Left  Motor '  )
    print('\t  Enter X to Quit  Program     '  )
    
def banner_bottom():
    print('\t+-------------------------------------------+')
    print('\t|          Press Any Key to Exit            |')
    print('\t+-------------------------------------------+')

PrintBanner() # print the banner
Usage()
PrintInstructions()

COM_PortName = input('\n\t Enter the COM Port Name ->') #Enter the name of your COM port

COM_PortName = COM_PortName.strip()    #Strip extra characters

COM_Port = serial.Serial(COM_PortName) #open the port
print('\n\t   ',COM_PortName,'Opened')

COM_Port.baudrate = 9600               # set Baud rate 
COM_Port.bytesize = 8                  # Number of data bits = 8
COM_Port.parity   = 'N'                # No parity
COM_Port.stopbits = 1                  # Number of Stop bits = 1


#Controlling DTR and RTS pins to put USB2SERIAL in transmit mode

COM_Port.setRTS(0) #RTS=0,~RTS=1 so ~RE=1,
COM_Port.setDTR(0) #DTR=0,~DTR=1 so  DE=1,Transmit mode enabled for MAX485
                   # (In FT232 RTS and DTR pins are inverted)
                   #~RE and DE LED's on USB2SERIAL board will light up
Sentry = True

#while loop for decision making 
while (Sentry == True):
    MenuChoice = input('\n\t EnterChoice-> ') # enter the choice 
    MenuChoice = MenuChoice.strip()           #strip any white spaces
    MenuChoice = MenuChoice.upper()           # convert the character to upper case
    
    if (MenuChoice == 'A'):                   #if choice == A, 
        NoOfBytes  = COM_Port.write(b'A')     #Send 'A' to RobotShield through RS485 port of USB2SERIAL
        print('\n\t\t\tRight Motor ON')
    elif(MenuChoice == 'Q'):
        NoOfBytes  = COM_Port.write(b'Q')
        print('\n\t\t\tRight Motor OFF')
    elif(MenuChoice == 'D'):
        NoOfBytes  = COM_Port.write(b'D')
        print('\n\t\t\tLeft Motor ON')
    elif(MenuChoice == 'E'):
        NoOfBytes  = COM_Port.write(b'E')
        print('\n\t\t\tLeft Motor OFF')
    elif(MenuChoice == 'X'):                 #if choice == 'X' ,exit from the while loop
        Sentry = False                       #by making the Sentry = False 
        print('\n\t\tExiting the program')   #so loop condition will fail

COM_Port.close() # Close the Serial port
banner_bottom()  # Display the bottom banner
dummy = input()  # press any key to close 
