using MediatR.Sample.Events;

namespace MediatR.Sample.Handler
{
    public class NonReturnEventHandler : AsyncRequestHandler<NonReturnEvent>
    {
        private readonly IMediator _mediator;
        public NonReturnEventHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        protected override async Task Handle(NonReturnEvent request, CancellationToken cancellationToken)
        {
            try
            {
                Console.WriteLine($"{nameof(NonReturnEventHandler)}:开始耗时操作");
                await Task.Delay(3000);
                var resp = await _mediator.Send(new HasReturnEvent { Id = request.Id });
                Console.WriteLine($"{nameof(NonReturnEventHandler)}:调用HasReturnEvent响应：" + resp);
                Console.WriteLine($"{nameof(NonReturnEventHandler)}:结束耗时操作");
            }
            catch (Exception ex)
            {
                // 
                throw ex;
            }

        }
    }
}
