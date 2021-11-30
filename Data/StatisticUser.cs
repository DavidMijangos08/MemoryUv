namespace Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    /// <summary>
    /// Clase correspondiente a la tabla estad�stica de usuario de la BD
    /// </summary>

    [Table("StatisticUser")]
    public partial class StatisticUser
    {
        public int idUser { get; set; }

        public int totalGames { get; set; }

        public int totalWins { get; set; }

        public int totalDefeat { get; set; }

        [Required]
        [StringLength(20)]
        public string nameTag { get; set; }

        public int id { get; set; }

        public virtual UserGame UserGame { get; set; }
    }
}
