﻿namespace Common.Exceptions;

[Serializable]
public class AfonyaErrorException : Exception
{
    public AfonyaErrorException(string message) : base(message)
    { }

}