using System;
using System.Diagnostics;
using Stateless;

namespace Communication.Actisense
{
    public class ActisenseNGT1 : BaseDevice
    {
        private const byte NMEA2000Data = 0x93; // Indicates message is n2k

        private readonly byte[] localBuffer = new byte[1024];
        
        int messageAssemblyIndex;
        private int sumOfChars;
        private int bytesRemaining;

        private readonly StateMachine<State, Trigger> machine = ActisenseState.GetStateMachine();

        public ActisenseNGT1(ISerialPort serialPort) : base(serialPort)
        {
            // NGT-1 com port settings
            serialPort.BaudRate = 115200;
            serialPort.DtrEnable = true;
            serialPort.RtsEnable = true;
            //Encoding = System.Text.Encoding.UTF8; 
        }

        protected override void OnDeviceDataReceived(byte[] data)
        {
            MessageType messageType = MessageType.Unknown;    
            bool dataLinkEscapeJustRead = false;

            foreach (byte b in data)
            {
                Debug.WriteLine(String.Format("current character {0:x2}", b));

                switch(machine.State)
                {
                    case State.WaitingForStart:
                        if (b == (byte)ControlCode.DLE)
                        {
                            Fire(Trigger.DataLinkEscapeRead);
                        }
                        break;
                    case State.Started:
                        Fire(b == (byte)ControlCode.STX ? Trigger.StartOfTextRead : Trigger.UnexpectedCharacter);
                        break;
                    case State.ReadingMessageType:
                        messageType = (b == NMEA2000Data) ? MessageType.NMEA2000 : MessageType.Unknown;
                        sumOfChars = b;
                        Fire(Trigger.MessageTypeRead);
                        break;
                    case State.ReadingMessageLength:
                        messageAssemblyIndex = 0; // start writing to beginning of buffer
                        sumOfChars += b;
                        bytesRemaining = b;
                        dataLinkEscapeJustRead = false;
                        Fire(Trigger.MessageLengthRead);
                        break;
                    case State.BuildingMessage:
                        if (b == (byte)ControlCode.DLE)
                        {
                            // A Data Link Escape is "escaped" by another Data Link Escape
                            if (!dataLinkEscapeJustRead)
                            {
                                AppendToMessage(b);
                            }
                            dataLinkEscapeJustRead = !dataLinkEscapeJustRead;
                        }
                        else
                        {
                            // if (dataLinkEscapeJustRead) LogError(...);
                            AppendToMessage(b);
                        }

                        if (--bytesRemaining == 0x00)
                        {
                            Fire(Trigger.MessageTextRead);
                        }
                        break;
                    case State.ReadingCheckSum:
                        byte checkSum = b;
                        bool checkSumPasses = ((sumOfChars + checkSum)%256) == 0;

                        if (checkSumPasses)
                        {
                            // send localBuffer, even if DLE and ETX don't follow:
                            byte[] message = new byte[messageAssemblyIndex];
                            Array.Copy(localBuffer, 0, message, 0, messageAssemblyIndex);
                            RaiseMessageReceived(messageType, message);
                        }

                        Fire(checkSumPasses ? Trigger.CheckSumPassed : Trigger.CheckSumFailed);
                        // should maybe messageAssemblyIndex += 2, but with a failed checksum maybe
                        // the length was read corrupt, so process as normal for another localBuffer start.
                        break;
                    case State.CheckSumRead:
                        Fire(b == (byte)ControlCode.DLE ? Trigger.DataLinkEscapeRead : Trigger.UnexpectedCharacter);
                        break;
                    case State.MessageDone:
                        Fire(b == (byte)ControlCode.ETX ? Trigger.MessageFinished : Trigger.UnexpectedCharacter);
                        break;
                }
            }
        }

        private void AppendToMessage(byte data)
        {
            localBuffer[messageAssemblyIndex++] = data;
            sumOfChars += data;
        }

        private void Fire(Trigger read)
        {
            machine.Fire(read);
        }
    }
}