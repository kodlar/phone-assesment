using MediatR;
using Microsoft.Extensions.Logging;

namespace PhoneBook.Application.PipelineBehaviours
{
    public class UnhandledExceptionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<TRequest> _logger;

        public UnhandledExceptionBehavior(ILogger<TRequest> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            try
            {
                return await next();
            }
            catch (Exception ex)
            {

                var requestName = typeof(TRequest).Name;

                if (_logger.IsEnabled(LogLevel.Error))
                {
                    _logger.LogError(ex, $"Clean Architecture Request: Unhandled exception  for  Request {requestName} {request}");
                }

                throw;

            }

        }

   
    }
}
