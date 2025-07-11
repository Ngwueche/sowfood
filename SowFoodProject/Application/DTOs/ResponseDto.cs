﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SowFoodProject.Application.DTOs
{
    public class ResponseDto<T>
    {
        public ResponseDto(T? data, string message, bool isSuccessful, int statusStatusCode, IEnumerable<Error> errors)
        {
            IsSuccessful = isSuccessful;
            StatusCode = statusStatusCode;
            Message = message;
            Data = data;
            Errors = errors;
        }

        public bool IsSuccessful { get; private set; }
        public int StatusCode { get; private set; }
        public string Message { get; private set; }
        public T? Data { get; private set; }
        public IEnumerable<Error> Errors { get; private set; }

        public static ResponseDto<T> Failure(IEnumerable<Error> errors, int statusCode = (int)HttpStatusCode.BadRequest)
        {
            return new ResponseDto<T>(default, string.Empty, false, statusCode, errors);
        }

        public static ResponseDto<T> Success(T data, string successMessage = "", int statusCode = (int)HttpStatusCode.OK)
        {
            return new ResponseDto<T>(data, successMessage, true, statusCode, Array.Empty<Error>());
        }

        public static ResponseDto<T> Success(string successMessage = "", int statusCode = (int)HttpStatusCode.OK)
        {
            return new ResponseDto<T>(default, successMessage, true, statusCode, Array.Empty<Error>());
        }
    }
}
