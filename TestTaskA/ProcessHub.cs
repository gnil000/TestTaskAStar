using Microsoft.AspNetCore.SignalR;
using TestTaskA.Models;

namespace TestTaskA
{
    public class ProcessHub:Hub
    {
        public async Task ProcessTasks(RequestView req)
        {
            if (req.tasks < 3 || req.tasks > 25)
                await this.Clients.Caller.SendAsync("ReceiveError", "Количество тасков должно быть не меньше 3 и не больше 25");
            {

                int countr = 0;
                Random random = new Random();
                List<Task> tasks = new List<Task>();
                List<ResponseView> responses = new List<ResponseView>();
                int time = 0;
                for (int i = 0; i < req.tasks; i++)
                {
                    var t = Task.Run(async () =>
                    {
                        time = random.Next(100, 1000);
                        countr = Interlocked.Increment(ref countr);
                        ResponseView resp = new ResponseView(countr, time);
                        await Task.Delay(resp.time);
                        await this.Clients.Caller.SendAsync("Receive", resp);
                    });
                    if (req.parallel)
                        tasks.Add(t);
                    else
                        t.Wait();
                }
                Task.WaitAll(tasks.ToArray());
            }
        }
    }
}
