using System;
namespace TimeNazi
{
    public interface IConfiguration
    {
        int ClockOpacity { get; set; }
        int NumberOfSnoozes { get; set; }
        int PauseTimeout { get; set; }
        int RestTime { get; set; }
        bool ShowClock { get; set; }
        int SnoozeTime { get; set; }
        int WorkTime { get; set; }
    }
}
