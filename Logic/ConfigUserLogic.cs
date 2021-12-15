using Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace Logic
{
    public class ConfigUserLogic
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ConfigUser GetConfigUserById(int idUser)
        {
            ConfigUser config = new ConfigUser();

            try
            {
                using (var context = new MemoryModel())
                {
                    var coincidences = from ConfigUser in context.ConfigUsers where ConfigUser.idUser == idUser select ConfigUser;
                    if (coincidences.Count() > 0)
                    {
                        config = coincidences.First();
                        
                    }                       
                }
            }
            catch (SystemException ex)
            {
                log.Error(ex.Message, ex);
                throw new SystemException();
            }
            return config;
        }

        public string GetBackgroundUser(ConfigUser confi)
        {

            if (confi.idBackground == 0)
            {
                string background = "Resources/Background/backgroundDefault.png";
                return background;
            }
            else if (confi.idBackground == 1)
            {
                string background = "Resources/Background/backgroundGreen.png";
                return background;
            }
            else if (confi.idBackground == 2)
            {
                string background = "Resources/Background/backgroundIce.png";
                return background;
            }
            else if (confi.idBackground == 3)
            {
                string background = "Resources/Background/backgroundPink.jpg";
                return background;
            }
            else if(confi.idBackground == 4)
            {
                string background = "Resources/Background/backgroundRed.jpg";
                return background;
            }
            else if(confi.idBackground == 5)
            {
                string background = "Resources/Background/backgroundX.jpg";
                return background;
            }
            return "0";
        }

        public void SetBackgroundUser(int idUser, int idNewBackground)
        {
            try
            {
                using (var context = new MemoryModel())
                {
                    var coincidences = from ConfigUser in context.ConfigUsers where ConfigUser.idUser == idUser select ConfigUser;
                    if (coincidences.Count() > 0)
                    {
                        ConfigUser configUser = coincidences.First();
                        configUser.idBackground = idNewBackground;
                    }
                    context.SaveChanges();
                }
            }
            catch (SystemException ex)
            {
                log.Error(ex.Message, ex);
                throw new SystemException();
            }
        }
        public bool ExistsConfigUser(int idUser)
        {
            ConfigUser config = new ConfigUser();
            bool exists = false;
            try
            {
                using (var context = new MemoryModel())
                {
                    var coincidences = from ConfigUser in context.ConfigUsers where ConfigUser.idUser == idUser select ConfigUser;
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
        public void NewConfigUser(int idUser)
        {
            try
            {
                using (var context = new MemoryModel())
                {
                    ConfigUser config = new ConfigUser()
                    {
                        idUser = idUser,
                        idBackground = 0,
                        idLanguage = 0
                    };
                    context.ConfigUsers.Add(config);
                    context.SaveChanges();                    
                }
            }
            catch (SystemException ex)
            {
                log.Error(ex.Message, ex);
                throw new SystemException();
            }
        }
    }
}
