using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using YellowbrickV6.Entities;
using System.Threading;
using System.Net.Mail;
using YellowbrickV6Cmd;


namespace YellowbrickV6
{
    class Program
    {
        static string serverName = "http://yb.tl";
        static string raceKey = "shtp2014";

        static DateTime lastUpdate = new DateTime(1970,1,1);

        static int lapse = 1800;

        static void Main(string[] args)
        {
            Race race = null;
            while (true)
            {
                try
                {
                    SMTPTools.TrySMTP();
                    SMTPTools.IsValidRecipient();

                    Console.WriteLine(DateTime.Now.ToString() + " Getting Race Information...");
                    race = YBTracker.getRaceInformation(serverName, raceKey);

                    if(race != null)
                    {
                        Console.WriteLine("Race categories: ");
                        bool foundS = false;
                        foreach (Tag t in race.tags)
                        { 
                            Console.WriteLine(t.id + " - " + t.name);
                            if(YellowbrickV6Cmd.Properties.Settings.Default.section == t.id)
                                foundS = true;
                        }
                        if(!foundS)
                        {
                            Console.WriteLine("Enter the id of the section to be reported:");
                            YellowbrickV6Cmd.Properties.Settings.Default.section = Convert.ToInt32(Console.ReadLine());
                            YellowbrickV6Cmd.Properties.Settings.Default.Save();
                        }

                        Console.WriteLine("Teams racing:");
                        bool foundT = false;
                        foreach (Team t in race.teams)
                        {
                            if (t.tags.Contains(YellowbrickV6Cmd.Properties.Settings.Default.section))
                            {
                                Console.WriteLine(t.id + " - " + t.name);
                                if (YellowbrickV6Cmd.Properties.Settings.Default.referenceteam == t.id)
                                    foundT = true;
                            }
                        }
                        if (!foundT)
                        {
                            Console.WriteLine("Enter the id of the reference team:");
                            YellowbrickV6Cmd.Properties.Settings.Default.referenceteam = Convert.ToInt32(Console.ReadLine());
                            YellowbrickV6Cmd.Properties.Settings.Default.Save();
                        }

                        string lastSentReport = "";
                        while (true)
                        {
                            double timeLapsed = (DateTime.Now - lastUpdate).TotalSeconds;
                            try
                            {
                                if (timeLapsed > lapse)
                                {
                                    Console.WriteLine("\r" + DateTime.Now.ToString() + " Getting latest position data");
                                    List<Team> teams = YBTracker.getAllPositions(serverName, raceKey);
                                    YBTracker.UpdateTeamsMoments(race.teams, teams);
                                    foreach (Team team in race.teams)
                                        YBTracker.UpdateMomentsSpeedHeading(team);

                                    string report = YBTracker.ReportSelectedTeamsHTML(race, YellowbrickV6Cmd.Properties.Settings.Default.section, YellowbrickV6Cmd.Properties.Settings.Default.referenceteam);
                                    if (report != lastSentReport)
                                    {
                                        SMTPTools.SendMail(
                                            YellowbrickV6Cmd.Properties.Settings.Default.smtpuser,
                                            SecurityTools.ToInsecureString(SecurityTools.DecryptString(YellowbrickV6Cmd.Properties.Settings.Default.smtppassword)),
                                            YellowbrickV6Cmd.Properties.Settings.Default.recipient,
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
                                    Console.Write("\r" + Math.Ceiling(lapse - timeLapsed).ToString() + " seconds");
                                }
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(DateTime.Now.ToString() + " Something went wrong" + e.Message);
                            }
                            Thread.Sleep(5000);
                        }
                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine(DateTime.Now.ToString() + " Something went wrong" + e.Message);
                }
                finally 
                {
                    if (race == null)
                        Console.WriteLine("Race information cannot be fetched...");
                }
                Thread.Sleep(5000);
            }
        }
    }
}
