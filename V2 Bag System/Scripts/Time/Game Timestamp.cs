using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameTimestamp : MonoBehaviour
{
    public int year;
    public enum Season
    {
        Spring,
        Summer,
        Fall,
        Winter
    }
    public Season season;

    public enum DayOfTheWeek
    {
        Saturday,
        Sunday,
        Monday,
        Tueday,
        Wednesday,
        Thursday,
        Friday
    }

    public int day;
    public int hour;
    public int minute;

    public GameTimestamp(int year, Season season,int day, int hour, int minute)
    {
        this.year = year;
        this.season = season;
        this.day = day;
        this.hour = hour;
        this.minute = minute;
    }

    public GameTimestamp(GameTimestamp timestamp) {  
        this.year = timestamp.year; 
        this.season = timestamp.season;
        this.day = timestamp.day; 
        this.hour = timestamp.hour; 
        this.minute = timestamp.minute; 
    }

    public void UpdateClock()
    {
        minute++;
        if(minute >= 60)
        {
            minute = 0;
            hour++;
        }

        if(hour >= 24)
        {
            hour = 0;
            day++;
        }

        if(day > 30)
        {
            day = 1;
            if (season == Season.Winter)
            {
                season = Season.Spring;
                year++;
            }
            else
            {
                season++;
            }
        }

    }

    public DayOfTheWeek GetDayOfTheWeek()
    {
        int daysPassed = YearsToDays(year) + SeasonsToDays(season) + day;

        int dayIndex = daysPassed % 7;

        return (DayOfTheWeek)dayIndex;

    }

    public static int HoursToMinutes(int hours)
    {
        return hours * 60;
    }

    public static int DaysToHours(int days)
    {
        return days * 24;
    }

    public static int SeasonsToDays(Season season)
    {
        int seasonIndex = (int)season;
        return seasonIndex * 30;
    }

    public static int YearsToDays(int years)
    {
        return years * 30 * 4;
    }

    ///<summary>
    ///Calculate the difference between 2 timestamps
    ///</summary>
    ///<param name = "timestamp1"> The first timestamp</param>
    ///<param name = "timestamp2"> The second timestamp</param>
    ///<param name = "displayInHours">Whether to calculate in terms of hours. Else it will return in minutes </param>
    ///<returns></returns>

    public static int CompareTimestamps(GameTimestamp timestamp1, GameTimestamp timestamp2)
    {
        int timestamp1Hours = DaysToHours(YearsToDays(timestamp1.year)) + DaysToHours(SeasonsToDays(timestamp1.season)) + DaysToHours(timestamp1.day) + timestamp1.hour;
        int timestamp2Hours = DaysToHours(YearsToDays(timestamp2.year)) + DaysToHours(SeasonsToDays(timestamp2.season)) + DaysToHours(timestamp2.day) + timestamp2.hour;

        //if (displayInHours)
        //{
        //    return Mathf.Abs(timestamp2Hours - timestamp1Hours);
        //}

        //return HoursToMinutes(timestamp2Hours - timestamp1Hours);

        int difference = timestamp2Hours - timestamp1Hours;
        return Mathf.Abs(difference);

    }


}
