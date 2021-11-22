using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServerAdmin.Filters
{

    public class ExceptionFilter : Attribute, IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            int statusCode = 500;
            string exceptionMessage = context.Exception.Message;

            if (context.Exception is RpcException)
            {
                RpcException exception = ((RpcException)context.Exception);
                statusCode = ParseCommand(exception.StatusCode);
                exceptionMessage = exception.Status.Detail;
            }
            if (context.Exception is ArgumentNullException)
            {
                statusCode = 400;
                exceptionMessage = "Por favor ingrese parametros requeridos para realizar este pedido";
            }

            context.Result = new ContentResult
            {
                StatusCode = statusCode,
                Content = exceptionMessage
            };
        }


        private Dictionary<StatusCode, int> _GRPCStatusCodeMap = new Dictionary<StatusCode, int>()
        {
            { StatusCode.NotFound, 404 },
            { StatusCode.AlreadyExists, 409 }
        };

        private int ParseCommand(StatusCode statusCode)
        {
            if (_GRPCStatusCodeMap.ContainsKey(statusCode))
            {
                return _GRPCStatusCodeMap[statusCode];
            }
            return 500;
        }
    }

}
