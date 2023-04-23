﻿using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

namespace BookStore.Application.Common.Behaviours
{
    public class LoggingBehaviour<TRequest> : IRequestPreProcessor<TRequest> where TRequest : notnull
    {
        private readonly ILogger _logger;
      

        public LoggingBehaviour(ILogger<TRequest> logger //, 
          
            )
        {
            _logger = logger;
           
        }

        public async Task Process(TRequest request, CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;
           
            string userName = string.Empty;

           
            _logger.LogInformation(" Request: {Name} {@Request}",
                requestName,  request);
        }
    }
}