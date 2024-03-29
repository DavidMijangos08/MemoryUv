namespace Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    /// <summary>
    /// Clase correspondiente a la tabla de la BD de Solicitud de amistad
    /// </summary>

    [Table("FriendRequest")]
    public partial class FriendRequest
    {
        public int id { get; set; }

        public int idApplicant { get; set; }

        public int idReceiver { get; set; }

        [StringLength(20)]
        public string requestStatus { get; set; }

        public virtual UserGame UserGame { get; set; }
    }
}
