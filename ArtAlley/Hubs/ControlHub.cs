using ArtAlley.Data;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtAlley.Hubs
{
    public class ControlHub : Hub
    {
        public async Task StartLine(Guid userId, int line)
        {
            var state = State.getInstance();
            state.ClearByTimeout();

            if (!state.startedTime.HasValue)
            {
                state.startedTime = DateTime.Now;
                state.currentTrackTime = 0;
            }
            else
            {
                state.currentTrackTime = (DateTime.Now - state.startedTime).Value.TotalSeconds;
            }
            state.AliveLines.Add(new Line()
            {
                userId = userId,
                LineNum = line,
                lastPoint = DateTime.Now
            });
            await Clients.All.SendAsync("UpdateState", state);
            await Clients.Caller.SendAsync("CurrentTime", state.currentTrackTime, 0);
        }

        public async Task StopLine(Guid userId)
        {
            var state = State.getInstance();
            var currentLine = state.AliveLines.Find(p => p.userId == userId);
            if (currentLine != null)
            {
                state.AliveLines.Remove(currentLine);
            }
            state.ClearByTimeout();
            await Clients.All.SendAsync("UpdateState", state);
        }


        public async Task Alive(Guid userId, double localTime)
        {
            var state = State.getInstance();
            state.ClearByTimeout();
            var currentLine = state.AliveLines.Find(p => p.userId == userId);
            if (currentLine != null)
            {
                currentLine.lastPoint = DateTime.Now;
            }
            await Clients.Caller.SendAsync("UpdateState", state);
            if (state.startedTime.HasValue)
            {
                var currentTrackTime = (DateTime.Now - state.startedTime).Value.TotalSeconds;
                var diff = currentTrackTime - localTime;
                Console.WriteLine(diff);
                await Clients.Caller.SendAsync("DiffTime", diff);
            }
        }
    }
}
