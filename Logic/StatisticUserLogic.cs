using Data;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Logic
{
    public class StatisticUserLogic
    {
        public List<StatisticUser> GetBetterUsers()
        {
            List<StatisticUser> users = new List<StatisticUser>();
            try
            {
                using (var context = new MemoryModel())
                {
                    var coincidences = (from StatisticUser in context.StatisticsUser orderby StatisticUser.totalWins descending select StatisticUser).ToList();
                    users = coincidences;
                }
            }
            catch (DbException)
            {

            }
            return users;
        }

        public bool GetStatisticUser(int idUser, int numAchievement)
        {
            bool exists = false;
            
            try
            {
                using (var context = new MemoryModel())
                {
                    
                    if (numAchievement == 1)
                    {
                        var coincidences = from StatisticUser in context.StatisticsUser where StatisticUser.idUser == idUser select StatisticUser;
                        if (coincidences.Count() > 0)
                        {
                            StatisticUser statisticuser = coincidences.First();
                            if (statisticuser.totalWins > 1)
                            {
                                exists = true;
                                
                                return exists;
                            }
                            
                        }
                    }
                    else if (numAchievement == 2)
                    {
                        var concidences = (from StatisticUser in context.StatisticsUser where StatisticUser.idUser == idUser select StatisticUser);

                        if (concidences.Count() > 0)
                        {
                            StatisticUser statisticuser = concidences.First();
                            if (statisticuser.totalWins > 10)
                            {
                                exists = true;
                                return exists;
                            }

                        }
                    }
                    else if (numAchievement == 3)
                    {
                        var concidences = (from StatisticUser in context.StatisticsUser where StatisticUser.idUser == idUser select StatisticUser);

                        if (concidences.Count() > 0)
                        {
                            StatisticUser statisticuser = concidences.First();
                            if (statisticuser.totalWins > 250)
                            {
                                exists = true;
                                return exists;
                            }

                        }
                    }
                    else if (numAchievement == 4)
                    {
                        var concidences = (from Friend in context.Friends where Friend.idUser == idUser select Friend).ToList();

                        if (concidences.Count() > 2)
                        {
                            Friend statisticuser = concidences.First();
                            exists = true;
                            return exists;
                               
                            

                        }
                    }
                    else if (numAchievement == 5)
                    {
                        var concidences = (from StatisticUser in context.StatisticsUser where StatisticUser.idUser == idUser select StatisticUser);

                        if (concidences.Count() > 0)
                        {
                            StatisticUser statisticuser = concidences.First();
                            if (statisticuser.totalDefeat > 1)
                            {
                                exists = true;
                                return exists;
                            }

                        }
                    }



                }
            }
            catch (DbException)
            {

            }
            return exists;
        }

        public StatisticStatus IncreaseGameWon(int idUser)
        {
            StatisticStatus status = StatisticStatus.Failed;
            try
            {
                using (var context = new MemoryModel())
                {
                    var coincidences = from StatisticUser in context.StatisticsUser where StatisticUser.idUser == idUser select StatisticUser;
                    if (coincidences.Count() > 0)
                    {
                        StatisticUser statisticUser = coincidences.First();
                        statisticUser.totalWins++;
                        statisticUser.totalGames++;
                    }
                    int numberChanges = context.SaveChanges();
                    if (numberChanges > 0)
                    {
                        status = StatisticStatus.Success;
                    }
                }
            }
            catch (DbException)
            {

            }
            return status;
        }

        public StatisticStatus IncreaseLosingGame(int idUser)
        {
            StatisticStatus status = StatisticStatus.Failed;
            try
            {
                using (var context = new MemoryModel())
                {
                    var coincidences = from StatisticUser in context.StatisticsUser where StatisticUser.idUser == idUser select StatisticUser;
                    if(coincidences.Count() > 0)
                    {
                        StatisticUser statisticUser = coincidences.First();
                        statisticUser.totalDefeat++;
                        statisticUser.totalGames++;
                    }
                    int numberChanges = context.SaveChanges();
                    if (numberChanges > 0)
                    {
                        status = StatisticStatus.Success;
                    }
                }
            }
            catch (DbException)
            {

            }
            return status;
        }

        public StatisticStatus AddStatisticUser(int idUser, string nametag)
        {
            StatisticStatus status = StatisticStatus.Failed;
            try
            {
                using(var context = new MemoryModel())
                {
                    StatisticUser statisticUser = new StatisticUser()
                    {
                        idUser = idUser,
                        totalGames = 0,
                        totalWins = 0,
                        totalDefeat = 0,
                        nameTag = nametag
                    };
                    context.StatisticsUser.Add(statisticUser);
                    int numberChanges = context.SaveChanges();
                    if (numberChanges > 0)
                    {
                        status = StatisticStatus.Success;
                    }
                }
            }
            catch (DbException)
            {

            }
            return status;
        }

        public enum StatisticStatus
        {
            Success = 0,
            Failed
        }
    }
}
