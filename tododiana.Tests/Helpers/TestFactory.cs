﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using tododiana.Common.Models;
using tododiana.Functions.Entities;

namespace tododiana.Tests.Helpers
{
    public class TestFactory
    {
        public static TodoEntity GetTodoEntity()
        {
            return new TodoEntity
            {
                ETag = "*",
                PartitionKey = "TODO",
                RowKey = Guid.NewGuid().ToString(),
                CreatedTime = DateTime.UtcNow,
                IsCompleted = false,
                TaskDescription = "Task: kill the humans."
            };
        }

        public static DefaultHttpRequest CreateHttpRequest(Guid todoId, Todo todoRequest)
        {
            string request = JsonConvert.SerializeObject(todoRequest);
            return new DefaultHttpRequest(new DefaultHttpContext())
            {
                Body = GenerateStreamFromString(request),
                Path = $"/{todoId}"
            };
        }

        //for function Delete or GetTodoById
        public static DefaultHttpRequest CreateHttpRequest(Guid todoId)
        {
            return new DefaultHttpRequest(new DefaultHttpContext())
            {
                Path = $"/{todoId}"
            };
        }

        //for function Create
        public static DefaultHttpRequest CreateHttpRequest(Todo todoRequest)
        {
            string request = JsonConvert.SerializeObject(todoRequest);
            return new DefaultHttpRequest(new DefaultHttpContext())
            {
                Body = GenerateStreamFromString(request)
            };
        }

        //for function GetAllTodos
        public static DefaultHttpRequest CreateHttpRequest()
        {
            return new DefaultHttpRequest(new DefaultHttpContext());
        }

        //for function GetTodo
        public static Todo GetTodoRequest()
        {
            return new Todo
            {
                CreatedTime = DateTime.UtcNow,
                IsCompleted = false,
                TaskDescription = "Try to conquer the world."
            };
        }

        public static Stream GenerateStreamFromString(string stringToConvert)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(stringToConvert);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        public static ILogger CreateLogger(LoggerTypes type = LoggerTypes.Null)
        {
            ILogger logger;
            if (type == LoggerTypes.List)
            {
                logger = new ListLogger();
            }
            else
            {
                logger = NullLoggerFactory.Instance.CreateLogger("Null Logger");
            }

            return logger;
        }
    }
}
