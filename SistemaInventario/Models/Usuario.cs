using RepoDb.Attributes;

namespace SistemaInventario.Models
{
    [Map("[dbo].[Usuarios]")]
    public class Usuario
    {
        [Primary, Identity]
        public int Id { get; set; }

        [Map("Usuario")]
        public string NombreUsuario { get; set; }

        public string Password { get; set; }

        public bool Activo { get; set; }
    }
}