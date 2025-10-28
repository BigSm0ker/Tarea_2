namespace SocialMedia.Core.CustomEntities
{
    public class GetByIdRequest
    {
        public int Id { get; set; }
    }
    namespace SocialMedia.Core.CustomEntities
    {
        public class UsuarioSinComentariosDto
        {
            public string FirstName { get; set; } = "";
            public string LastName { get; set; } = "";
            public string Email { get; set; } = "";
        }

        public class Comentario3MesesDto
        {
            public int IdComment { get; set; }
            public string CommentDescription { get; set; } = "";
            public string FirstName { get; set; } = "";
            public string LastName { get; set; } = "";
            public int Edad { get; set; }                 // usas CAST ... AS SIGNED en la SQL
        }

        public class PostSinComentariosActivosDto
        {
            public int IdPost { get; set; }
            public string PostDescription { get; set; } = "";
            public DateTime PostDate { get; set; }
        }

        public class UsuarioVariosAutoresDto
        {
            public string FirstName { get; set; } = "";
            public string LastName { get; set; } = "";
            public int UsuariosDiferentes { get; set; }
        }

        public class PostConMenoresDto
        {
            public int IdPost { get; set; }
            public string PostDescription { get; set; } = "";
            public int ComentariosMenores { get; set; }
        }

        public class DensidadPorDiaDto
        {
            public string DiaSemana { get; set; } = "";
            public long TotalComentarios { get; set; }
            public long UsuariosUnicos { get; set; }
        }

        public class CrecimientoMensualDto
        {
            public int Año { get; set; }
            public int Mes { get; set; }
            public long TotalComentarios { get; set; }
            public long? MesAnterior { get; set; }
            public decimal? CrecimientoPorcentual { get; set; }
        }

        public class TopUsuario30DiasDto
        {
            public string FirstName { get; set; } = "";
            public string LastName { get; set; } = "";
            public long Total { get; set; }
        }

        public class PromedioComentariosDto
        {
            public decimal Promedio { get; set; }
        }

        public class TiempoMedioPrimerComentarioDto
        {
            public int IdPost { get; set; }
            public decimal MinutosHastaPrimerComentario { get; set; }
        }
    }

}
