using Microsoft.AspNetCore.SignalR.Client;

HubConnection connection;
int total = 0;

Console.WriteLine("Введите количество тасков");
int tasks = Convert.ToInt16(Console.ReadLine());
Console.WriteLine("Выполнять их параллельно?\n1 - true\t2-false");
bool parallel = Console.ReadLine() == "1";

connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7079/check")
                .Build();
connection.On<ResponseView>("Receive", (result) => {
    var newMess = $"{result.order} = {result.time}";
    total += result.time;
    Console.WriteLine(newMess);
});

connection.On<string>("ReceiveError", (result) => {
    Console.WriteLine(result);
});

try
{
    await connection.StartAsync();
    Console.WriteLine("Произошло подключение");
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
}

try
{
    await connection.InvokeAsync("ProcessTasks", new RequestView { tasks = tasks, parallel = parallel });
}
catch (Exception e)
{
    Console.WriteLine($"{e.Message}");
}
await connection.StopAsync();
Console.WriteLine($"total = {total}");
Console.ReadLine();

public class RequestView
{
    public int tasks { get; set; }
    public bool parallel { get; set; }
}

public class ResponseView
{
    public int order { get; set; }
    public int time { get; set; }
}