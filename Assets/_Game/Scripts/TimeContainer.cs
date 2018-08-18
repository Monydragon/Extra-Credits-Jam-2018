using System;
using UnityEngine;

namespace DLS.Utility
{
    /// <summary>
    /// This is the Time Container class, This class holds time variables and methods used for custom timers and custom clocks.
    /// </summary>
    [System.Serializable]
    public class TimeContainer
    {
        public Days CurrentDay = Days.Monday;
        public Months CurrentMonth = Months.January;
        public string TimeString_H_MM_SS_MM_DD_YYYY;
        public string TimeString_H_MM_SS;
        public string TimeString_H_MM;

        public float totaltimesec;

        public float Year = 0;
        public float Month = 1;
        public float Week = 1;
        public float Day = 1;
        public float Hour = 0;
        public float Minute = 0;
        public float Second = 0;
        public float TimeScale = 1;
        public int SecondsInMinute = 60;
        public int MinutesInHour = 60;
        public int HoursInDay = 24;
        public int DaysInWeek = 7;
        public int DaysInMonth = 30;
        public int MonthsInYear = 12;

        public float TotalTimeSeconds
        {
            get
            {
                var _output = Year;
                _output = (_output * MonthsInYear) + Month;
                _output = (_output * DaysInMonth) + Day;
                _output = (_output * HoursInDay) + Hour;
                _output = (_output * MinutesInHour) + Minute;
                _output = (_output * SecondsInMinute) + Second;
                return _output;
            }
        }

        /// <summary>
        /// <para>This is class is responsible for being used as a container for a custom timer, you can Initialize optional variables for this time class.</para>
        /// <para>You can set values for example like so: [EXAMPLE: new TimeContainer(timescale: 60, day: 5, currentday: Days.Monday)] This will set the timescale to 60 and the day count to 5 and the currentday to Monday</para>
        /// <para>You can set the Following Values: Year, Month, Week, Day, Hour, Minute, Second, Time, SecondsInMinute, MinutesInHour, HoursInDay, DaysInWeek, DaysInMonth, MonthsInYear, CurrentDay, CurrentMonth if you do not set any of these values they will retain their default values.</para>
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="week"></param>
        /// <param name="day"></param>
        /// <param name="hour"></param>
        /// <param name="minute"></param>
        /// <param name="second"></param>
        /// <param name="timescale"></param>
        /// <param name="secondsInMinute"></param>
        /// <param name="minutesinhour"></param>
        /// <param name="hoursinday"></param>
        /// <param name="daysinweek"></param>
        /// <param name="daysinmonth"></param>
        /// <param name="monthsinyear"></param>
        /// <param name="currentday"></param>
        /// <param name="currentmonth"></param>

        public TimeContainer(
            int year = 0,
            int month = 1,
            int week = 1,
            int day = 1,
            int hour = 0,
            int minute = 0,
            float second = 0,
            float timescale = 1,
            int secondsInMinute = 60,
            int minutesinhour = 60,
            int hoursinday = 24,
            int daysinweek = 7,
            int daysinmonth = 30,
            int monthsinyear = 12,
            Days currentday = Days.Monday,
            Months currentmonth = Months.January
            )
        {
            Year = year;
            Week = week;
            Day = day;
            Hour = hour;
            Minute = minute;
            Second = second;
            TimeScale = timescale;
            SecondsInMinute = secondsInMinute;
            MinutesInHour = minutesinhour;
            HoursInDay = hoursinday;
            DaysInWeek = daysinweek;
            DaysInMonth = daysinmonth;
            MonthsInYear = monthsinyear;
            CurrentDay = currentday;
            if (currentmonth != Months.January)
            {
                Month = (int)currentmonth;
                CurrentMonth = currentmonth;
            }
            else if (month != 1)
            {
                Month = month;
                CurrentMonth = (Months)month;
            }
            ValidateTime();
        }

        /// <summary>
        /// This sets the Seconds to Time.deltaTime * TimeScale. This effectively will set the proper second value when called in update.
        /// this method also calls ValidateTime. <see cref="ValidateTime"/>
        /// (Must be called in Update for the Time to be properly set.)
        /// </summary>
        /// <returns></returns>
        public virtual void StartTime(TimeType timetype = TimeType.Second)
        {
            switch (timetype)
            {
                case TimeType.Second:
                    Second += UnityEngine.Time.deltaTime * TimeScale;
                    break;
                case TimeType.Minute:
                    Minute += UnityEngine.Time.deltaTime * TimeScale;
                    break;
                case TimeType.Hour:
                    Hour += UnityEngine.Time.deltaTime * TimeScale;
                    break;
                case TimeType.Day:
                    Day += UnityEngine.Time.deltaTime * TimeScale;
                    break;
                case TimeType.Week:
                    Week += UnityEngine.Time.deltaTime * TimeScale;
                    break;
                case TimeType.Month:
                    Month += UnityEngine.Time.deltaTime * TimeScale;
                    break;
                case TimeType.Year:
                    Year += UnityEngine.Time.deltaTime * TimeScale;
                    break;
            }
            ValidateTime();
        }

        public virtual void ReverseTime(TimeType timetype = TimeType.Second)
        {
            switch (timetype)
            {
                case TimeType.Second:
                    Second -= UnityEngine.Time.deltaTime * TimeScale;
                    break;
                case TimeType.Minute:
                    Minute -= UnityEngine.Time.deltaTime * TimeScale;
                    break;
                case TimeType.Hour:
                    Hour -= UnityEngine.Time.deltaTime * TimeScale;
                    break;
                case TimeType.Day:
                    Day -= UnityEngine.Time.deltaTime * TimeScale;
                    break;
                case TimeType.Week:
                    Week -= UnityEngine.Time.deltaTime * TimeScale;
                    break;
                case TimeType.Month:
                    Month -= UnityEngine.Time.deltaTime * TimeScale;
                    break;
                case TimeType.Year:
                    Year -= UnityEngine.Time.deltaTime * TimeScale;
                    break;
            }
            ValidateTime();
        }

        /// <summary>
        /// This method checks the time counting to make sure that after 60 seconds 1 minute is added (by default) and 60 Minutes adds 1 hour, etc etc.
        /// </summary>
        public virtual void ValidateTime()
        {
            if ((int)Second < 0 && Minute >= 0)
            {
                Minute--;
                Second += SecondsInMinute;
            }

            if ((int)Minute < 0 && Hour >= 0)
            {
                Hour--;
                Minute += MinutesInHour;
            }

            if ((int)Hour < 0 && Day >= 1)
            {
                Day--;
                if (CurrentDay == Days.Monday)
                {
                    CurrentDay = Days.Sunday;
                }
                else
                {
                    CurrentDay--;
                }
                CurrentDay--;
                Hour += HoursInDay;
            }

            if ((int)Day < 1 && Month >= 1)
            {
                Month--;
                Day = DaysInMonth;
            }

            if ((int)Month < 1 && (int)Year > 0)
            {
                Year--;
                Month = MonthsInYear;
            }

            if ((int)Second + 1 > SecondsInMinute)
            {
                Minute++;
                Second = 0;
            }
            if ((int)Minute + 1 > MinutesInHour)
            {
                Hour++;
                Minute = 0;
            }
            if ((int)Hour >= HoursInDay)
            {
                Day++;
                if (CurrentDay == Days.Sunday)
                {
                    CurrentDay = Days.Monday;
                }
                else
                {
                    CurrentDay++;
                }
                Hour = 0;
            }
            if ((int)Day > DaysInMonth)
            {
                Month++;
                Day = 1;
            }
            if ((int)Month > MonthsInYear)
            {
                Year++;
                Month = 1;
            }

            Week = (int)(Day / DaysInWeek) + 1;
            CurrentMonth = (Months)Month;
            TimeString_H_MM_SS_MM_DD_YYYY = System.String.Format("{0}:{1:00}:{2:00} {3}/{4}/{5}", (int)Hour % HoursInDay, (int)Minute % MinutesInHour, (int)Second % SecondsInMinute, Month, Day, Year);
            TimeString_H_MM_SS = System.String.Format("{0}:{1:00}:{2:00}", (int)Hour % HoursInDay, (int)Minute % MinutesInHour, (int)Second % SecondsInMinute);
            TimeString_H_MM = System.String.Format("{0}:{1:00}", (int)Hour % HoursInDay, (int)Minute % MinutesInHour);

        }

        /// <summary>
        /// This will check the time provided and return true or false based on the check. This has optional parameters that can be called like this
        /// EXAMPLE: CheckFullTIme(day:5,minute:30); - This will check to make sure Day 5 and 30 Minutes is true and execute.
        /// Passing -1 on a parameter will always evaluate the selected check to true for the check call.
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="week"></param>
        /// <param name="day"></param>
        /// <param name="hour"></param>
        /// <param name="minute"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public virtual bool CheckTime(int year = -1, int month = -1, int week = -1, int day = -1, int hour = -1, int minute = -1, int second = -1)
        {
            bool yearchecked = (int)Year == year;
            bool monthchecked = (int)Month == month;
            bool weekchecked = (int)Week == week;
            bool daychecked = (int)Day == day;
            bool hourchecked = (int)Hour == hour;
            bool minutechecked = (int)Minute == minute;
            bool secondchecked = (int)Second == second;

            if (year == -1)
            {
                yearchecked = true;
            }
            if (month == -1)
            {
                monthchecked = true;
            }
            if (week == -1)
            {
                weekchecked = true;
            }
            if (day == -1)
            {
                daychecked = true;
            }
            if (hour == -1)
            {
                hourchecked = true;
            }
            if (minute == -1)
            {
                minutechecked = true;
            }
            if (second == -1)
            {
                secondchecked = true;
            }
            if (yearchecked && monthchecked && weekchecked && daychecked && hourchecked && minutechecked && secondchecked)
            {
                return true;
            }

            return false;
        }


        public void SetTime(int year = -1, int month = -1, int week = -1, int day = -1, int hour = -1, int minute = -1, int second = -1)
        {
            if (year != -1)
            {
                Year = year;
            }
            if (month != -1)
            {
                Month = month;
            }
            if (week != -1)
            {
                Week = week;
            }
            if (day != -1)
            {
                Day = day;
            }
            if (hour != -1)
            {
                Hour = hour;
            }
            if (minute != -1)
            {
                Minute = minute;
            }
            if (second != -1)
            {
                Second = second;
            }

        }

        /// <summary>
        /// <para>Checks the Exact Second.</para>
        /// Returns True if the time has been reached otherwise returns False.
        /// </summary>
        /// <param name="second"></param>
        /// <returns></returns>
        public bool CheckExactTime(int second)
        {
            if ((int)Second == second)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// <para>Checks the Exact Minute, Second.</para>
        /// Returns True if the time has been reached otherwise returns False.
        /// </summary>
        /// <param name="minute"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public bool CheckExactTime(int minute, int second)
        {
            if (Minute == minute && (int)Second == second)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// <para>Checks the Exact Hour, Minute, Second.</para>
        /// Returns True if the time has been reached otherwise returns False.
        /// </summary>
        /// <param name="hour"></param>
        /// <param name="minute"></param>
        /// <param name="second"></param>
        /// <returns>test</returns>
        public bool CheckExactTime(int hour, int minute, int second)
        {
            if (Hour == hour && Minute == minute && (int)Second == second)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// <para>Checks the Exact Day, Hour, Minute, Second.</para>
        /// Returns True if the time has been reached otherwise returns False.
        /// </summary>
        /// <param name="day"></param>
        /// <param name="hour"></param>
        /// <param name="minute"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public bool CheckExactTime(int day, int hour, int minute, int second)
        {
            if (Day == day && Hour == hour && Minute == minute && (int)Second == second)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// <para>Checks the Exact Week, Day, Hour, Minute, Second.</para>
        /// Returns True if the time has been reached otherwise returns False.
        /// </summary>
        /// <param name="week"></param>
        /// <param name="day"></param>
        /// <param name="hour"></param>
        /// <param name="minute"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public bool CheckExactTime(int week, int day, int hour, int minute, int second)
        {
            if (Week == week && Day == day && Hour == hour && Minute == minute && (int)Second == second)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// <para>Checks the Exact Month, Week, Day, Hour, Minute, Second.</para>
        /// Returns True if the time has been reached otherwise returns False.
        /// </summary>
        /// <param name="month"></param>
        /// <param name="week"></param>
        /// <param name="day"></param>
        /// <param name="hour"></param>
        /// <param name="minute"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public bool CheckExactTime(int month, int week, int day, int hour, int minute, int second)
        {
            if (Month == month && Week == week && Day == day && Hour == hour && Minute == minute && (int)Second == second)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// <para>Checks the Exact Year, Month, Week, Day, Hour, Minute, Second.</para>
        /// Returns True if the time has been reached otherwise returns False.
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="week"></param>
        /// <param name="day"></param>
        /// <param name="hour"></param>
        /// <param name="minute"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public bool CheckExactTime(int year, int month, int week, int day, int hour, int minute, int second)
        {
            if (Month == month && Week == week && Day == day && Hour == hour && Minute == minute && (int)Second == second)
            {
                return true;
            }
            return false;
        }
        public virtual void ResetTime(bool year = false, bool month = false, bool week = false, bool day = false, bool hour = false, bool minute = false, bool second = false)
        {
            if (year)
            {
                Year = 0;
            }
            if (month)
            {
                Month = 1;
                CurrentMonth = Months.January;
            }
            if (week)
            {
                Week = 1;
            }
            if (day)
            {
                Day = 1;
            }
            if (hour)
            {
                Hour = 0;
            }
            if (minute)
            {
                Minute = 0;
            }
            if (second)
            {
                Second = 0;
            }

        }

        public virtual void ResetMinute()
        {
            Second = 0;
        }

        public virtual void ResetHour()
        {
            Minute = 0;
            Second = 0;
        }

        public virtual void ResetDay()
        {
            Hour = 0;
            Minute = 0;
            Second = 0;
        }

        public virtual void ResetWeek()
        {
            Day -= (int)Day % DaysInWeek;  // Not sure if this works yet.? Untested
        }

        public virtual void ResetMonth()
        {
            Day = 1;
            CurrentDay = Days.Monday; ;
            Hour = 0;
            Minute = 0;
            Second = 0;
        }

        public virtual void ResetFullDate()
        {
            CurrentDay = Days.Monday;
            CurrentMonth = Months.January;
            Year = 1;
            Month = 1;
            Week = 1;
            Day = 1;
            Hour = 0;
            Minute = 0;
            Second = 0;
        }

        public virtual void SetToDateTime(DateTime date_time)
        {
            Second = date_time.Second;
            Year = date_time.Year;
            Month = date_time.Month;
            CurrentMonth = (Months)date_time.Month;
            Day = date_time.Day;
            var weekcheck = (Day / 7) + 1;
            Hour = date_time.Hour;
            CurrentDay = (Days)date_time.DayOfWeek;
            Minute = date_time.Minute;
            Week = weekcheck;
        }
        public virtual void SetToNow()
        {
            Second = DateTime.Now.Second;
            Year = DateTime.Now.Year;
            Month = DateTime.Now.Month;
            CurrentMonth = (Months)DateTime.Now.Month;
            Day = DateTime.Now.Day;
            var weekcheck = (Day / 7) + 1;
            Hour = DateTime.Now.Hour;
            CurrentDay = (Days)DateTime.Now.DayOfWeek;
            Minute = DateTime.Now.Minute;
            Week = weekcheck;
        }
    }

}



