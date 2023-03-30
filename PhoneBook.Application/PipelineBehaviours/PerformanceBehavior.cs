using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace PhoneBook.Application.PipelineBehaviours
{
    public class PerformanceBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly Stopwatch _timer;
        private readonly ILogger<TRequest> _logger;

        public PerformanceBehavior(ILogger<TRequest> logger)
        {
            _timer = new Stopwatch();
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            _timer.Start();

            var response = await next();

            _timer.Stop();

            var elapsedMiliSeconds = _timer.ElapsedMilliseconds;

            if (elapsedMiliSeconds > 500)
            {
                var requestName = typeof(TRequest).Name;

                if (_logger.IsEnabled(LogLevel.Warning))
                {
                    _logger.LogWarning($"Long Running Request: {requestName} ({elapsedMiliSeconds}  miliseconds) {requestName}");
                }
            }


            return response;
        }

  
    }
}
