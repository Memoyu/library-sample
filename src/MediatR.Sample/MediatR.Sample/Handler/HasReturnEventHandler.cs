using MediatR.Sample.Events;

namespace MediatR.Sample.Handler
{
    public class HasReturnEventHandler : IRequestHandler<HasReturnEvent, string>
    {
        public async Task<string> Handle(HasReturnEvent request, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            Console.WriteLine($"{nameof(HasReturnEventHandler)} Handler 触发");
            return $"Id：{request.Id} 的数据返回";
        }
    }
}
