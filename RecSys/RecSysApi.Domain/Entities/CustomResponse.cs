using System;
using System.Net;

namespace RecSysApi.Domain.Entities
{
    public class CustomResponse<T>
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public HttpStatusCode Status { get; set; }
        public string Message { get; set; }
        public T Content { get; set; }
    }
}