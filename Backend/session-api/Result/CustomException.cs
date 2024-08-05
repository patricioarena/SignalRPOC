﻿using System;
using System.Collections.Generic;

namespace session_api.Result
{
    public class CustomException : Exception
    {
        private static readonly Dictionary<ErrorsEnum, string> _errors;

        public static Dictionary<ErrorsEnum, string> Errors => _errors;

        public enum ErrorsEnum : int
        {
            NotAddedConnectionOnUrl = 1000,
            NotAddedMapping = 1001,
            NotFoundException = 1002,
            UserNotFoundException = 1003
        }

        public int ErrorCode { get; }

        static CustomException()
        {
            _errors = new Dictionary<ErrorsEnum, string>
            {
                { ErrorsEnum.NotAddedConnectionOnUrl, "Conexión no agregada." },
                { ErrorsEnum.NotAddedMapping, "Mapeo no agregado." },
                { ErrorsEnum.NotFoundException, "No encontrado." },
                { ErrorsEnum.UserNotFoundException, "Usuario no encontrado." }
            };

            EnsureAllErrorDescriptions();
        }

        public CustomException(ErrorsEnum errorsEnum) : base(_errors[errorsEnum])
        {
            ErrorCode = (int)errorsEnum;
        }

        private static void EnsureAllErrorDescriptions()
        {
            if (Enum.GetNames(typeof(ErrorsEnum)).Length != _errors.Count)
            {
                foreach (ErrorsEnum errorName in Enum.GetValues(typeof(ErrorsEnum)))
                {
                    if (!_errors.ContainsKey(errorName))
                    {
                        _errors.Add(errorName, errorName.ToString());
                    }
                }
            }
        }
    }
}