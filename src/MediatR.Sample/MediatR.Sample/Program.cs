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
    Console.WriteLine($"has-return 请求结束");
    return "请求完成：" + resp;
});

app.MapGet("/non-return", async ([FromServices] IServiceProvider sp, [FromServices] IMediator mediator) =>
{
    // 1、这样可以正常完成流程
    // await mediator.Send(new NonReturnEvent { Id = 2 });

    // 2、这样回到导致异常：（应该是请求完成了之后，本次请求的IServiceProvider已经被销毁了导致的）
    // Cannot access a disposed object.
    // Object name: 'IServiceProvider'.
    // _ = mediator.Send(new NonReturnEvent { Id = 2 });

    // 3、强行要用异步的形式，可以使用一下这种方式
    // 在异步中构造个ServiceProvider，这样就不会被销毁了，除非异步任务结束
    _ = Task.Run(async () =>
    {
        var scope = sp.CreateScope();
        var me = scope.ServiceProvider.GetRequiredService<IMediator>();
        await me.Send(new NonReturnEvent { Id = 2 });
    });
    Console.WriteLine($"non-return 请求结束");
    return "请求完成";
});

app.MapGet("/publish", async ([FromServices] IMediator mediator) =>
{
    // 同样，跟Send Event 一样，不支持这样的一个异步操作；
    // 跟我理解的还是稍稍有些偏差，我想像中的是发起了publish后，就不用管了，相当于发布成功就行，剩下的耗时任务后台执行；
    // 实际看来，Send 与 Publish 的区别就在于一个是单播，一个是多播
    _ = mediator.Publish(new NotificationEvent { Id = 1 });
    Console.WriteLine($"publish 请求结束");
    return "请求完成：";
});

app.Run();