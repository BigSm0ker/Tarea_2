using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Api.Responses;
using SocialMedia.Core.CustomEntities;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Core.QueryFilters;
using SocialMedia.Infrastructure.DTOs;
using SocialMedia.Infrastructure.Validators;
using System.Net;
using static SocialMedia.Core.CustomEntities.PostComentariosUsersResponse;

namespace SocialMedia.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly IMapper _mapper;
        private readonly IValidationService _validationService;
        public PostController(IPostService postService,
            IMapper mapper,
            IValidationService validationService)
        {
            _postService = postService;
            _mapper = mapper;
            _validationService = validationService;
        }

        #region Sin DTOs
        //[HttpGet]
        //public async Task<IActionResult> GetPost()
        //{
        //    var posts = await _postService.GetAllPostAsync();
        //    return Ok(posts);
        //}

        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetPostId(int id)
        //{
        //    var post = await _postService.GetPostAsync(id);
        //    return Ok(post);
        //}

        //[HttpPost]
        //public async Task<IActionResult> InsertPost(Post post)
        //{
        //    await _postService.InsertPostAsync(post);
        //    return Ok(post);
        //}
        #endregion

        #region Con DTO
        //[HttpGet("dto")]
        //public async Task<IActionResult> GetPostsDto()
        //{
        //    var posts = await _postService.GetAllPostAsync();
        //    var postsDto = posts.Select(p => new PostDto
        //    {
        //        Id = p.Id,
        //        UserId = p.UserId,
        //        Date = p.Date.ToString("dd-MM-yyyy"),
        //        Description = p.Description,
        //        Imagen = p.Imagen
        //    });

        //    return Ok(postsDto);
        //}

        //[HttpGet("dto/{id}")]
        //public async Task<IActionResult> GetPostIdDto(int id)
        //{
        //    var post = await _postService.GetPostAsync(id);
        //    var postDto = new PostDto
        //    {
        //        Id = post.Id,
        //        UserId = post.UserId,
        //        Date = post.Date.ToString("dd-MM-yyyy"),
        //        Description = post.Description,
        //        Imagen = post.Imagen
        //    };

        //    return Ok(postDto);
        //}

        //[HttpPost("dto")]
        //public async Task<IActionResult> InsertPostDto(PostDto postDto)
        //{
        //    var post = new Post
        //    {
        //        Id = postDto.Id,
        //        UserId = postDto.UserId,
        //        Date = Convert.ToDateTime(postDto.Date),
        //        Description = postDto.Description,
        //        Imagen = postDto.Imagen
        //    };

        //    await _postService.InsertPostAsync(post);
        //    return Ok(post);
        //}

        //[HttpPut("dto/{id}")]
        //public async Task<IActionResult> UpdatePostDto(int id, 
        //    [FromBody]PostDto postDto)
        //{
        //    if (id != postDto.Id)
        //        return BadRequest("El Id del Post no coincide");

        //    var post = await _postService.GetPostAsync(id);
        //    if (post == null)
        //        return NotFound("Post no encontrado");

        //    post.Id = postDto.Id;
        //    post.UserId = postDto.UserId;
        //    post.Date = Convert.ToDateTime(postDto.Date);
        //    post.Description = postDto.Description;
        //    post.Imagen = postDto.Imagen;

        //    await _postService.UpdatePostAsync(post);
        //    return Ok(post);
        //}

        //[HttpDelete("dto/{id}")]
        //public async Task<IActionResult> UpdatePostDto(int id)
        //{
        //    var post = await _postService.GetPostAsync(id);
        //    if (post == null)
        //        return NotFound("Post no encontrado");

        //    await _postService.DeletePostAsync(post);
        //    return NoContent();
        //}
        #endregion

        #region Dto Mapper
        [HttpGet("dto/mapper")]
        public async Task<IActionResult> GetPostsDtoMapper(
            [FromQuery]PostQueryFilter postQueryFilter)
        {
            var posts = await _postService.GetAllPostAsync(postQueryFilter);
            var postsDto = _mapper.Map<IEnumerable<PostDto>>(posts);

            var response = new ApiResponse<IEnumerable<PostDto>>(postsDto);

            return Ok(response);
        }

        [HttpGet("dto/dapper")]
        public async Task<IActionResult> GetPostsDtoMapper()
        {
            var posts = await _postService.GetAllPostDapperAsync();
            var postsDto = _mapper.Map<IEnumerable<PostDto>>(posts);

            var response = new ApiResponse<IEnumerable<PostDto>>(postsDto);

            return Ok(response);
        }

        [HttpGet("dapper/1")]
        public async Task<IActionResult> GetPostCommentUserAsync()
        {
            var posts = await _postService.GetPostCommentUserAsync();
           

            var response = new ApiResponse<IEnumerable<PostComentariosUsersResponse>>(posts);

            return Ok(response);
        }

        [HttpGet("dto/mapper/{id}")]
        public async Task<IActionResult> GetPostsDtoMapperId(int id)
        {
            #region Validaciones
            var validationRequest = new GetByIdRequest { Id = id };
            var validationResult = await _validationService.ValidateAsync(validationRequest);

            if (!validationResult.IsValid)
            {
                return BadRequest(new
                {
                    Message = "Error de validación del ID",
                    Errors = validationResult.Errors
                });
            }
            #endregion

            var post = await _postService.GetPostAsync(id);
            var postDto = _mapper.Map<PostDto>(post);

            var response = new ApiResponse<PostDto>(postDto);

            return Ok(response);
        }

        [HttpPost("dto/mapper/")]
        public async Task<IActionResult> InsertPostDtoMapper([FromBody]PostDto postDto)
        {
            try
            {
                #region Validaciones
                // La validación automática se hace mediante el filtro
                // Esta validación manual es opcional
                var validationResult = await _validationService.ValidateAsync(postDto);

                if (!validationResult.IsValid)
                {
                    return BadRequest(new { Errors = validationResult.Errors });
                }
                #endregion

                var post = _mapper.Map<Post>(postDto);
                await _postService.InsertPostAsync(post);

                var response = new ApiResponse<Post>(post);

                return Ok(response);
            }
            catch (Exception err)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, err.Message);
            }
        }

        [HttpPut("dto/mapper/{id}")]
        public async Task<IActionResult> UpdatePostDtoMapper(int id,
            [FromBody] PostDto postDto)
        {
            if (id != postDto.Id)
                return BadRequest("El Id del Post no coincide");

            var post = await _postService.GetPostAsync(id);
            if (post == null)
                return NotFound("Post no encontrado");
      
            _mapper.Map(postDto, post);
            await _postService.UpdatePostAsync(post);

            var response = new ApiResponse<Post>(post);

            return Ok(response);
        }

        [HttpDelete("dto/mapper/{id}")]
        public async Task<IActionResult> DeletePostDtoMapper(int id)
        {
            //var post = await _postService.GetPostAsync(id);
            //if (post == null)
            //    return NotFound("Post no encontrado");

            await _postService.DeletePostAsync(id);
            return NoContent();
        }
        #endregion

        #region Consultas Avanzadas Dapper

        // 1) Usuarios activos sin comentarios
        [HttpGet("reportes/usuarios-sin-comentarios")]
        public async Task<IActionResult> UsuariosSinComentarios()
        {
            var data = await _postService.GetUsuariosActivosSinComentariosAsync();
            return Ok(new ApiResponse<IEnumerable<UsuarioSinComentariosDto>>(data));
        }

        // 2) Comentarios de los últimos 3 meses por usuarios >25 años
        [HttpGet("reportes/comentarios-3m-mayores25")]
        public async Task<IActionResult> Comentarios3MesesMayores25()
        {
            var data = await _postService.GetComentarios3MesesMayores25Async();
            return Ok(new ApiResponse<IEnumerable<Comentario3MesesDto>>(data));
        }

        // 3) Posts sin comentarios de usuarios activos
        [HttpGet("reportes/posts-sin-comentarios-activos")]
        public async Task<IActionResult> PostsSinComentariosActivos()
        {
            var data = await _postService.GetPostsSinComentariosDeActivosAsync();
            return Ok(new ApiResponse<IEnumerable<PostSinComentariosActivosDto>>(data));
        }

        // 4) Usuarios que comentaron en ≥3 autores diferentes
        [HttpGet("reportes/usuarios-varios-autores")]
        public async Task<IActionResult> UsuariosVariosAutores()
        {
            var data = await _postService.GetUsuariosQueComentanVariosAutoresAsync();
            return Ok(new ApiResponse<IEnumerable<UsuarioVariosAutoresDto>>(data));
        }

        // 5) Posts con comentarios de menores de edad
        [HttpGet("reportes/posts-menores-edad")]
        public async Task<IActionResult> PostsConMenores()
        {
            var data = await _postService.GetPostsConComentariosDeMenoresAsync();
            return Ok(new ApiResponse<IEnumerable<PostConMenoresDto>>(data));
        }

        // 6) Densidad de comentarios por día
        [HttpGet("reportes/densidad-comentarios-dia")]
        public async Task<IActionResult> DensidadPorDia()
        {
            var data = await _postService.GetDensidadComentariosPorDiaAsync();
            return Ok(new ApiResponse<IEnumerable<DensidadPorDiaDto>>(data));
        }

        // 7) Crecimiento mensual de comentarios
        [HttpGet("reportes/crecimiento-mensual-comentarios")]
        public async Task<IActionResult> CrecimientoMensual()
        {
            var data = await _postService.GetCrecimientoMensualComentariosAsync();
            return Ok(new ApiResponse<IEnumerable<CrecimientoMensualDto>>(data));
        }

        // 8) Top 5 usuarios últimos 30 días
        [HttpGet("reportes/top5-usuarios-30dias")]
        public async Task<IActionResult> Top5Usuarios()
        {
            var data = await _postService.GetTop5Usuarios30DiasAsync();
            return Ok(new ApiResponse<IEnumerable<TopUsuario30DiasDto>>(data));
        }

        // 9) Promedio de comentarios por post
        [HttpGet("reportes/promedio-comentarios-post")]
        public async Task<IActionResult> PromedioComentarios()
        {
            var data = await _postService.GetPromedioComentariosPorPostAsync();
            return Ok(new ApiResponse<PromedioComentariosDto?>(data));
        }

        // 10) Tiempo medio hasta el primer comentario
        [HttpGet("reportes/tiempo-medio-primer-comentario")]
        public async Task<IActionResult> TiempoMedioPrimerComentario()
        {
            var data = await _postService.GetTiempoMedioPrimerComentarioAsync();
            return Ok(new ApiResponse<IEnumerable<TiempoMedioPrimerComentarioDto>>(data));
        }

        #endregion


    }
}