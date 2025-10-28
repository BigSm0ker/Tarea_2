using SocialMedia.Core.CustomEntities;
using SocialMedia.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using static SocialMedia.Core.CustomEntities.PostComentariosUsersResponse;

namespace SocialMedia.Core.Interfaces
{
    public interface IPostRepository : IBaseRepository<Post>
    {
        // Existentes
        Task<IEnumerable<Post>> GetAllPostByUserAsync(int idUser);
        Task<IEnumerable<Post>> GetAllPostDapperAsync(int limit = 10);
        Task<IEnumerable<PostComentariosUsersResponse>> GetPostCommentUserAsync();
        Task<Post> GetPostAsync(int id);
        Task InsertPostAsync(Post post);
        Task UpdatePostAsync(Post post);
        Task DeletePostAsync(Post post);

        // Reportes Dapper (10)
        Task<IEnumerable<UsuarioSinComentariosDto>> GetUsuariosActivosSinComentariosAsync();
        Task<IEnumerable<Comentario3MesesDto>> GetComentarios3MesesMayores25Async();
        Task<IEnumerable<PostSinComentariosActivosDto>> GetPostsSinComentariosDeActivosAsync();
        Task<IEnumerable<UsuarioVariosAutoresDto>> GetUsuariosQueComentanVariosAutoresAsync();
        Task<IEnumerable<PostConMenoresDto>> GetPostsConComentariosDeMenoresAsync();
        Task<IEnumerable<DensidadPorDiaDto>> GetDensidadComentariosPorDiaAsync();
        Task<IEnumerable<CrecimientoMensualDto>> GetCrecimientoMensualComentariosAsync();
        Task<IEnumerable<TopUsuario30DiasDto>> GetTop5Usuarios30DiasAsync();
        Task<PromedioComentariosDto?> GetPromedioComentariosPorPostAsync();
        Task<IEnumerable<TiempoMedioPrimerComentarioDto>> GetTiempoMedioPrimerComentarioAsync();
    }
}
