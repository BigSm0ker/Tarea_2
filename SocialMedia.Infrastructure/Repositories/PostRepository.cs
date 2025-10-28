using Microsoft.EntityFrameworkCore;
using SocialMedia.Core.CustomEntities;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Enum;
using SocialMedia.Core.Interfaces;
using SocialMedia.Infrastructure.Data;
using SocialMedia.Infrastructure.Queries;


namespace SocialMedia.Infrastructure.Repositories
{
    public class PostRepository : BaseRepository<Post>, IPostRepository
    {
        private readonly IDapperContext _dapper;
        private readonly SocialMediaContext _context;

        public PostRepository(SocialMediaContext context, IDapperContext dapper) : base(context)
        {
            _dapper = dapper;
            _context = context;
        }

        public async Task<IEnumerable<Post>> GetAllPostByUserAsync(int idUser)
        {
            var posts = await _context.Posts.Where(x => x.UserId == idUser).ToListAsync();
            return posts;
        }

        public async Task<IEnumerable<Post>> GetAllPostDapperAsync(int limit = 10)
        {
            try
            {
                var sql = _dapper.Provider switch
                {
                    DatabaseProvider.SqlServer => PostQueries.PostQuerySqlServer,
                    DatabaseProvider.MySql => PostQueries.PostQueryMySQl,
                    _ => throw new NotSupportedException("Provider no soportado")
                };

                return await _dapper.QueryAsync<Post>(sql, new { Limit = limit });
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }

        public async Task<IEnumerable<PostComentariosUsersResponse>> GetPostCommentUserAsync()
        {
            try
            {
                var sql = PostQueries.PostComentadosUsuariosActivos;
                return await _dapper.QueryAsync<PostComentariosUsersResponse>(sql);
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }

        public async Task<Post> GetPostAsync(int id)
        {
            var post = await _context.Posts.FirstOrDefaultAsync(x => x.Id == id);
            return post;
        }

        public async Task InsertPostAsync(Post post)
        {
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePostAsync(Post post)
        {
            _context.Update(post);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePostAsync(Post post)
        {
            _context.Remove(post);
            await _context.SaveChangesAsync();
        }

        // Usuarios activos sin comentarios
        public async Task<IEnumerable<UsuarioSinComentariosDto>> GetUsuariosActivosSinComentariosAsync()
        {
            var sql = PostQueries.UsuariosActivosSinComentarios_MySql;
            return await _dapper.QueryAsync<UsuarioSinComentariosDto>(sql);
        }

        // Comentarios 3 meses atrás de usuarios >25 años
        public async Task<IEnumerable<Comentario3MesesDto>> GetComentarios3MesesMayores25Async()
        {
            var sql = PostQueries.Comentarios3MesesMayores25_MySql;
            return await _dapper.QueryAsync<Comentario3MesesDto>(sql);
        }

        // Posts sin comentarios de usuarios activos
        public async Task<IEnumerable<PostSinComentariosActivosDto>> GetPostsSinComentariosDeActivosAsync()
        {
            var sql = PostQueries.PostsSinComentariosDeActivos_MySql;
            return await _dapper.QueryAsync<PostSinComentariosActivosDto>(sql);
        }

        // Usuarios que comentan posts de ≥3 autores
        public async Task<IEnumerable<UsuarioVariosAutoresDto>> GetUsuariosQueComentanVariosAutoresAsync()
        {
            var sql = PostQueries.UsuariosQueComentanVariosAutores_MySql;
            return await _dapper.QueryAsync<UsuarioVariosAutoresDto>(sql);
        }

        // Posts con comentarios de menores de edad
        public async Task<IEnumerable<PostConMenoresDto>> GetPostsConComentariosDeMenoresAsync()
        {
            var sql = PostQueries.PostsConComentariosDeMenores_MySql;
            return await _dapper.QueryAsync<PostConMenoresDto>(sql);
        }

        // Densidad de comentarios por día
        public async Task<IEnumerable<DensidadPorDiaDto>> GetDensidadComentariosPorDiaAsync()
        {
            var sql = PostQueries.DensidadComentariosPorDia_MySql;
            return await _dapper.QueryAsync<DensidadPorDiaDto>(sql);
        }

        // Crecimiento mensual
        public async Task<IEnumerable<CrecimientoMensualDto>> GetCrecimientoMensualComentariosAsync()
        {
            var sql = PostQueries.CrecimientoMensualComentarios_MySql;
            return await _dapper.QueryAsync<CrecimientoMensualDto>(sql);
        }

        // Top 5 usuarios últimos 30 días
        public async Task<IEnumerable<TopUsuario30DiasDto>> GetTop5Usuarios30DiasAsync()
        {
            var sql = PostQueries.Top5Usuarios30Dias_MySql;
            return await _dapper.QueryAsync<TopUsuario30DiasDto>(sql);
        }

        // Promedio de comentarios por post
        public async Task<PromedioComentariosDto?> GetPromedioComentariosPorPostAsync()
        {
            var sql = PostQueries.PromedioComentariosPorPost_MySql;
            return (await _dapper.QueryAsync<PromedioComentariosDto>(sql)).FirstOrDefault();
        }

        // Tiempo medio hasta primer comentario
        public async Task<IEnumerable<TiempoMedioPrimerComentarioDto>> GetTiempoMedioPrimerComentarioAsync()
        {
            var sql = PostQueries.TiempoMedioPrimerComentario_MySql;
            return await _dapper.QueryAsync<TiempoMedioPrimerComentarioDto>(sql);
        }
    }
}
