using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using YellowbrickV6.Entities;
using System.Threading;


namespace YellowbrickV6
{
    class Program
    {
        static string serverName = "http://yb.tl";
        static string raceKey = "shtp2014";

        static DateTime lastUpdate = new DateTime(1970,1,1);

        static void Main(string[] args)
        {
            Console.WriteLine(DateTime.Now.ToString() + " Getting Race Information");
            Race race = YBTracker.getRaceInformation(serverName, raceKey);
            string lastSentReport = "";
            while (true)
            {
                double timeLapsed = (DateTime.Now - lastUpdate).TotalSeconds;
                if (timeLapsed > 1800)
                {
                    Console.WriteLine("\r" + DateTime.Now.ToString() + " Getting latest position data");
                    List<Team> teams = YBTracker.getAllPositions(serverName, raceKey);
                    YBTracker.UpdateTeamsMoments(race.teams, teams);
                    foreach (Team team in race.teams)
                        YBTracker.UpdateMomentsSpeedHeading(team);
                    string report = YBTracker.ReportSelectedTeamsHTML(race, new List<int>(), 5);
                    if (report != lastSentReport)
                    {
                        EmailTools.SendMail(
                            "",
                            "",
                            "",
                            "Race Positions at " + DateTime.Now.ToString(),
                            report,
                            true);
                        lastSentReport = report;
                        Console.WriteLine(DateTime.Now.ToString() + " Email Sent");
                    }
                    else
                        Console.WriteLine(DateTime.Now.ToString() + " No changes, no need to send email");
                    lastUpdate = DateTime.Now;
                }
                else
                {
                    Console.Write("\r" + Math.Ceiling(1800 - timeLapsed).ToString() + " seconds");
                }
                Thread.Sleep(1000);
            }
        }
    }
}
