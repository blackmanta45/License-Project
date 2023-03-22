using System;
using System.Collections.Generic;

namespace RecSysApi.Domain.Entities;

public class RequestUrl<T>
{
    public Guid Id { get; set; }
    public T Content { get; set; }
    public string Protocol { get; set; }
    public string Domain { get; set; }
    public string Path { get; set; }
    public Dictionary<string, string> QueryParams { get; set; } = new();
    public Dictionary<string, string> Headers { get; set; } = new();
    public string Anchor { get; set; }
}