using System;

namespace CourseProgram.Exceptions
{
    public class RepeatConflictException<T> : Exception
    {
        public T ExistingModel { get; }
        public T IncomingModel { get; }

        public RepeatConflictException(T existingModel, T incomingModel)
        {
            ExistingModel = existingModel;
            IncomingModel = incomingModel;
        }

        public RepeatConflictException(string? message, T existingModel, T incomingModel) : base(message)
        {
            ExistingModel = existingModel;
            IncomingModel = incomingModel;
        }

        public RepeatConflictException(string? message, Exception? innerException, T existingModel, T incomingModel) : base(message, innerException)
        {
            ExistingModel = existingModel;
            IncomingModel = incomingModel;
        }
    }
}