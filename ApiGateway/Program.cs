using static AuthService.Grpc.AuthService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.WebHost.UseUrls("http://0.0.0.0:5000");

builder.Services.AddGrpcClient<AuthServiceClient>(o =>
{
    o.Address = new Uri(builder.Configuration["AuthService:GrpcUrl"]);
    o.ChannelOptionsActions.Add(opt => opt.HttpHandler = new SocketsHttpHandler
    {
        PooledConnectionIdleTimeout = Timeout.InfiniteTimeSpan,
        KeepAlivePingDelay = TimeSpan.FromSeconds(15),
        KeepAlivePingTimeout = TimeSpan.FromSeconds(5),
        EnableMultipleHttp2Connections = true
    });
    o.ChannelOptionsActions.Add(opt => opt.Credentials = Grpc.Core.ChannelCredentials.Insecure);
});

var app = builder.Build();

//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseAuthorization();
app.MapControllers();
app.UseRouting();

app.Run();
