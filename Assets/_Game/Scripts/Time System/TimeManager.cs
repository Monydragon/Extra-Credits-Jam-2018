namespace DLS.Utility
{
    /// <summary>
    /// This class contains all the timers for the main game and some methods to use for handling time.
    /// </summary>
    [System.Serializable]
    public class TimeManager
    {
        public TimeContainer GameTime = new TimeContainer(timescale: 4.8f); // Move to TimeManager
        public TimeContainer GameTimeLimit = new TimeContainer(year: 0, month: 0, day: 3, hour: 0, minute: 1, timescale: 4.8f); // Move to TimeManager
        public TimeContainer RealTime = new TimeContainer(minute: 15); // Move to TimeManager
        public TimeContainer ProcrastinationTime = new TimeContainer(timescale: 60); // Move to TimeManager

        public bool TimesUp()
        {
            if (GameTimeLimit.CheckTime(day: 0, hour: 0, minute: 0, second: 0))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Sets the time to the real system time.
        /// </summary>
        public void SetRealTime() // Move to TimeManager
        {
            var weekcheck = (RealTime.Day / 7) + 1;
            RealTime.Second = System.DateTime.Now.Second;
            RealTime.Year = System.DateTime.Now.Year;
            RealTime.Month = System.DateTime.Now.Month;
            RealTime.CurrentMonth = (Months)System.DateTime.Now.Month;
            RealTime.Day = System.DateTime.Now.Day;
            RealTime.Hour = System.DateTime.Now.Hour;
            RealTime.CurrentDay = (Days)System.DateTime.Now.DayOfWeek;
            RealTime.Minute = System.DateTime.Now.Minute;
            RealTime.Week = weekcheck;
        }
    }
}