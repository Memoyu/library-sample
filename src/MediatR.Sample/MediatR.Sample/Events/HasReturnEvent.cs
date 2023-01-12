namespace MediatR.Sample.Events
{
    public class HasReturnEvent : IRequest<string>
    {
        public int Id { get; set; }
    }
}
