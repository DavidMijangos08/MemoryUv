using Data;
using System;
using System.Collections.Generic;
using System.Data.Common;
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
        public ConfigUser GetConfigUserById(int idUser)
        {
            ConfigUser confi = new ConfigUser();

            try
            {
                using (var context = new MemoryModel())
                {
                    var coincidences = from ConfigUser in context.ConfigUser where ConfigUser.idUser == idUser select ConfigUser;
                    if (coincidences.Count() > 0)
                    {
                        confi = coincidences.first();
                        
                    }
                        
                }
            }
            catch (DbException)
            {

            }
            return confi;
        }

        public string GetBackgroundUser(ConfigUser confi)
        {

            if (confi.idBackgroud == 0)
            {
                string background = "Resources/Background/backgroundDefault.png";
                return background;
            }
            else if (confi.idBackgroud == 1)
            {
                string background = "Resources/Background/backgroundGreen.png";
                return background;
            }
            else if (confi.idBackgroud == 2)
            {
                string background = "Resources/Background/backgroundIce.png";
                return background;
            }
            else if (confi.idBackgroud == 3)
            {
                string background = "Resources/Background/backgroundPink.jpg";
                return background;
            }
            else if(confi.idBackgroud == 4)
            {
                string background = "Resources/Background/backgroundRed.jpg";
                return background;
            }
            else if(confi.idBackgroud == 5)
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
                    var coincidences = from ConfigUser in context.ConfigUser where ConfigUser.idUser == idUser select ConfigUser;
                    if (coincidences.Count() > 0)
                    {
                        ConfigUser configUser = coincidences.First();
                        configUser.idBackgroud = idNewBackground;

                        
                    }
                    context.SaveChanges();
                    
                }
            }
            catch (DbException)
            {

            }
        }
    }
}
