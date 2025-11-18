using RepoDb.Attributes;
using System;

namespace SistemaInventario.Models
{
    [Map("[dbo].[Productos]")]
    public class Producto
    {
        [Primary, Identity]
        public int Id { get; set; }

        public string Nombre { get; set; }

        public string Codigo { get; set; }

        public string Categoria { get; set; }

        public decimal PrecioUnitario { get; set; }

        public int Cantidad { get; set; }

        public int StockMinimo { get; set; }

        public DateTime FechaCreacion { get; set; }
    }
}