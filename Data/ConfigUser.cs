namespace Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ConfigUser")]
    public partial class ConfigUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int idUser { get; set; }

        public int? idBackground { get; set; }

        public int? idLanguage { get; set; }

        public virtual UserGame UserGame { get; set; }
    }
}
