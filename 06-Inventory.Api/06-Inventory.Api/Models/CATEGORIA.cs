using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _06_Inventory.Api.Models
{
    public class CATEGORIA
    {
        [Key]
        [Column(Order = 1)]
        public int CODIGO { get; set; }
        public string DESCRIPCION { get; set; }

        public DateTime? CREACION_TSTAMP { get; set; }
        public string CREACION_USUARIO { get; set; }
        public DateTime? ULT_MODIF_TSTAMP { get; set; }
        public string ULT_MODIF_USUARIO { get; set; }
    }
}

// String por defecto es nulo
// numericos y fechas nopueden ser nulas
