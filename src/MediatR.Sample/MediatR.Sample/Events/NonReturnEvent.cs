namespace MediatR.Sample.Events
{
    public class NonReturnEvent : IRequest
    {
        public int Id { get; set; }
    }
}
