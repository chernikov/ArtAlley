using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtAlley.Data
{
    public class Line
    {
        public Guid userId { get; set; }

        public int LineNum { get; set; }

        public DateTime lastPoint { get; set; }
    }

    public class State
    {
        public List<Line> AliveLines { get; set; } = new List<Line>();

        public DateTime? startedTime { get; set; }

        public double currentTrackTime { get; set; }

        private static State instance;

        private State()
        { }

        public static State getInstance()
        {
            if (instance == null)
                instance = new State();
            return instance;
        }


        public void ClearByTimeout()
        {
            var i = 0;
            while (i < AliveLines.Count)
            {
                var aliveLine = AliveLines[i];
                if ((DateTime.Now - aliveLine.lastPoint).TotalSeconds < 10)
                {
                    i++;
                }
                else
                {
                    AliveLines.Remove(aliveLine);
                }
            }

            if (AliveLines.Count == 0)
            {
                startedTime = null;
                currentTrackTime = 0;
            }
        }

    }
}
