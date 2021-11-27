using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Client.Domain
{
    public class Card
    {
        public StatusCard status { get; set; }
        public int numberCard { get; set; }
        public Button button { get; set; }

        public Card(StatusCard status, int numberCard, Button button)
        {
            this.status = status;
            this.numberCard = numberCard;
            this.button = button;
        }
    }

    public enum StatusCard
    {
        TurnedAround,
        NotFlipped
    }
}
