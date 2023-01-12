using MediatR.Sample.Events;

namespace MediatR.Sample.Handler
{
    public class NotificationEventHandler : INotificationHandler<NotificationEvent>
    {
        private readonly IMediator _mediator;
        public NotificationEventHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Handle(NotificationEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                Console.WriteLine($"{nameof(NotificationEventHandler)}:开始耗时操作");
                await Task.Delay(3000);
                var resp = await _mediator.Send(new HasReturnEvent { Id = notification.Id });
                Console.WriteLine($"{nameof(NotificationEventHandler)}:调用HasReturnEvent响应：" + resp);
                Console.WriteLine($"{nameof(NotificationEventHandler)}:结束耗时操作");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
