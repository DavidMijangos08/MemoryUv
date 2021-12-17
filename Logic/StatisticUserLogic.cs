using Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Logic
{

    /// <summary>
    /// Clase que permite manejar la lógica de la estadística de un usuario
    /// </summary>

    public class StatisticUserLogic
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Método que permite obtener los mejores jugadores del juego
        /// </summary>
        /// <returns> Retorna una lista ordenada de mayor a menor numero de partidas ganadas</returns>

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
            catch (SystemException ex)
            {
                log.Error(ex.Message, ex);
                throw new SystemException();
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
                            }                           
                        }
                    }
                    else if (numAchievement == 2)
                    {
                        var concidences = from StatisticUser in context.StatisticsUser where StatisticUser.idUser == idUser select StatisticUser;

                        if (concidences.Count() > 0)
                        {
                            StatisticUser statisticuser = concidences.First();
                            if (statisticuser.totalWins >= 10)
                            {
                                exists = true;
                            }
                        }
                    }
                    else if (numAchievement == 3)
                    {
                        var concidences = from StatisticUser in context.StatisticsUser where StatisticUser.idUser == idUser select StatisticUser;

                        if (concidences.Count() > 0)
                        {
                            StatisticUser statisticuser = concidences.First();
                            if (statisticuser.totalWins >= 250)
                            {
                                exists = true;
                                return exists;
                            }
                        }
                    }
                    else if (numAchievement == 4)
                    {
                        var concidences = (from Friend in context.Friends where Friend.idUser == idUser select Friend).ToList();

                        if (concidences.Count() >= 2)
                        {
                            Friend statisticuser = concidences.First();
                            exists = true;
                        }
                    }
                    else if (numAchievement == 5)
                    {
                        var concidences = from StatisticUser in context.StatisticsUser where StatisticUser.idUser == idUser select StatisticUser;

                        if (concidences.Count() > 0)
                        {
                            StatisticUser statisticuser = concidences.First();
                            if (statisticuser.totalDefeat >= 1)
                            {
                                exists = true;
                            }

                        }
                    }
                }
            }
            catch (SystemException ex)
            {
                log.Error(ex.Message, ex);
                throw new SystemException();
            }
            return exists;
        }

        /// <summary>
        /// Método que permite agregar una nueva partida ganada al jugador
        /// </summary>
        /// <param name="idUser"> Corresponde al id del jugador al que se desea agregar una nueva partida ganada</param>
        /// <returns> Retorna el estado de procesamiento del método </returns>

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
            catch (SystemException ex)
            {
                log.Error(ex.Message, ex);
                throw new SystemException();
            }
            return status;
        }

        /// <summary>
        /// Método que permite agregar una nueva partida perdida a un jugador
        /// </summary>
        /// <param name="idUser"> Corresponde al id del jugador al que se le desea agregar una nueva partida perdida </param>
        /// <returns> Retorna el estado de procesamiento del método </returns>

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
            catch (SystemException ex)
            {
                log.Error(ex.Message, ex);
                throw new SystemException();
            }
            return status;
        }

        /// <summary>
        /// Método que permite agregar las estadísticas del usuario vacías
        /// </summary>
        /// <param name="idUser"> Corresponde al id del usuario al que se desea agregar sus estadísticas </param>
        /// <param name="nametag">Corresponde al nametag del usuario al que se desea agregar sus estadísticas</param>
        /// <returns> Retorna el estado de procesamiento del método </returns>

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
            catch (SystemException ex)
            {
                log.Error(ex.Message, ex);
                throw new SystemException();
            }
            return status;
        }

        /// <summary>
        /// Método que permite verificar la existencia de la estadística de un usuario
        /// </summary>
        /// <param name="idUser"> Corresponde al id del usuario a verificar </param>
        /// <returns> booleano con el resultado de la operación, true si existe, de lo contrario, false </returns>

        public bool ExistsStatisticUser(int idUser)
        {
            bool exists = false;
            try
            {
                using (var context = new MemoryModel())
                {
                    var coincidences = from StatisticUser in context.StatisticsUser where StatisticUser.idUser == idUser select StatisticUser;
                    if (coincidences.Count() > 0)
                    {
                        exists = true;
                    }
                }
            }
            catch (SystemException ex)
            {
                log.Error(ex.Message, ex);
                throw new SystemException();
            }
            return exists;
        }

        /// <summary>
        /// Enumerador que contiene dos estados (ÉXITO, FALLADO) para el retorno de diversos métodos de la lógica de estadísticas
        /// </summary>

        public enum StatisticStatus
        {
            Success = 0,
            Failed
        }
    }
}
