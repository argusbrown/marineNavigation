using Stateless;

namespace Communication.Actisense
{
    internal enum State
    {
        WaitingForStart,
        Started,
        ReadingMessageType,
        ReadingMessageLength,
        BuildingMessage,
        ReadingCheckSum,
        CheckSumRead,
        MessageDone,
    }

    internal enum Trigger
    {
        DataLinkEscapeRead,
        StartOfTextRead,
        MessageTypeRead,
        MessageLengthRead,
        MessageFinished,
        MessageTextRead,
        UnexpectedCharacter,
        CheckSumPassed,
        CheckSumFailed
    }

    internal static class ActisenseState
    {
        internal static StateMachine<State, Trigger> GetStateMachine()
        {
            var machine = new StateMachine<State, Trigger>(State.WaitingForStart);

            machine.Configure(State.WaitingForStart)
                .Permit(Trigger.DataLinkEscapeRead, State.Started);

            machine.Configure(State.Started)
                .Permit(Trigger.StartOfTextRead, State.ReadingMessageType)
                .Permit(Trigger.UnexpectedCharacter, State.WaitingForStart);

            machine.Configure(State.ReadingMessageType)
                .Permit(Trigger.MessageTypeRead, State.ReadingMessageLength)
                .Permit(Trigger.UnexpectedCharacter, State.WaitingForStart);

            machine.Configure(State.ReadingMessageLength)
                .Permit(Trigger.MessageLengthRead, State.BuildingMessage);

            machine.Configure(State.BuildingMessage)
                .Permit(Trigger.MessageTextRead, State.ReadingCheckSum);

            machine.Configure(State.ReadingCheckSum)
                .Permit(Trigger.CheckSumPassed, State.CheckSumRead)
                .Permit(Trigger.CheckSumFailed, State.WaitingForStart);

            machine.Configure(State.CheckSumRead)
                .Permit(Trigger.DataLinkEscapeRead, State.MessageDone)
                .Permit(Trigger.UnexpectedCharacter, State.WaitingForStart);

            machine.Configure(State.MessageDone)
                .Permit(Trigger.MessageFinished, State.WaitingForStart)
                .Permit(Trigger.UnexpectedCharacter, State.WaitingForStart);

            return machine;
        }
    }
}