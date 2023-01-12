using MediatR;
using MediatR.Sample.Events;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

var app = builder.Build();

// Configure the HTTP request pipeline.

app.MapGet("/has-return", async ([FromServices] IMediator mediator) =>
{
    var resp = await mediator.Send(new HasReturnEvent { Id = 1 });
    Console.WriteLine($"has-return �������");
    return "������ɣ�" + resp;
});

app.MapGet("/non-return", async ([FromServices] IServiceProvider sp, [FromServices] IMediator mediator) =>
{
    // 1���������������������
    // await mediator.Send(new NonReturnEvent { Id = 2 });

    // 2�������ص������쳣����Ӧ�������������֮�󣬱��������IServiceProvider�Ѿ��������˵��µģ�
    // Cannot access a disposed object.
    // Object name: 'IServiceProvider'.
    // _ = mediator.Send(new NonReturnEvent { Id = 2 });

    // 3��ǿ��Ҫ���첽����ʽ������ʹ��һ�����ַ�ʽ
    // ���첽�й����ServiceProvider�������Ͳ��ᱻ�����ˣ������첽�������
    _ = Task.Run(async () =>
    {
        var scope = sp.CreateScope();
        var me = scope.ServiceProvider.GetRequiredService<IMediator>();
        await me.Send(new NonReturnEvent { Id = 2 });
    });
    Console.WriteLine($"non-return �������");
    return "�������";
});

app.MapGet("/publish", async ([FromServices] IMediator mediator) =>
{
    // ͬ������Send Event һ������֧��������һ���첽������
    // �������Ļ���������Щƫ��������е��Ƿ�����publish�󣬾Ͳ��ù��ˣ��൱�ڷ����ɹ����У�ʣ�µĺ�ʱ�����ִ̨�У�
    // ʵ�ʿ�����Send �� Publish �����������һ���ǵ�����һ���Ƕಥ
    _ = mediator.Publish(new NotificationEvent { Id = 1 });
    Console.WriteLine($"publish �������");
    return "������ɣ�";
});

app.Run();