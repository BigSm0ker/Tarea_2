using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Infrastructure.Queries
{
    public static class PostQueries
    {
        public static string PostQuerySqlServer = @"
                        select Id, UserId, Date, Description, Imagen 
                        from post 
                        order by Date desc
                        OFFSET 0 ROWS FETCH NEXT @Limit ROWS ONLY;";
        public static string PostQueryMySQl = @"
                        select Id, UserId, Date, Description, Imagen 
                        from post 
                        order by Date desc
                        LIMIT @Limit
                    ";
        public static string PostComentadosUsuariosActivos = @"
                        SELECT 
                        p.Id AS PostId,
                        p.Description,
                        COUNT(c.Id) AS TotalComentarios
                    FROM Post p
                    JOIN Comment c ON p.Id = c.PostId
                    JOIN User u ON c.UserId = u.Id
                    WHERE u.IsActive = 1        
                    GROUP BY p.Id, p.Description
                    HAVING COUNT(c.Id) > 2
                    ORDER BY TotalComentarios DESC;            
                    ";

        public static string UsuariosActivosSinComentarios_MySql = @"
SELECT u.FirstName, u.LastName, u.Email
FROM `User` u
LEFT JOIN Comment c ON c.UserId = u.Id
WHERE u.IsActive = 1 AND c.Id IS NULL;";


        public static string Comentarios3MesesMayores25_MySql = @"
SELECT
  c.Id AS IdComment,
  c.Description AS CommentDescription,
  u.FirstName, u.LastName,
  CAST(TIMESTAMPDIFF(YEAR, u.DateOfBirth, CURDATE()) AS SIGNED) AS Edad
FROM Comment c
JOIN `User` u ON u.Id = c.UserId
WHERE c.Date >= DATE_SUB(CURDATE(), INTERVAL 3 MONTH)
  AND TIMESTAMPDIFF(YEAR, u.DateOfBirth, CURDATE()) > 25;";

        public static string PostsSinComentariosDeActivos_MySql = @"
SELECT p.Id AS IdPost, p.Description AS PostDescription, p.Date AS PostDate
FROM Post p
LEFT JOIN Comment c ON c.PostId = p.Id
LEFT JOIN `User` u ON u.Id = c.UserId AND u.IsActive = 1
WHERE u.Id IS NULL;";

        public static string UsuariosQueComentanVariosAutores_MySql = @"
SELECT u.FirstName, u.LastName,
       COUNT(DISTINCT p.UserId) AS UsuariosDiferentes
FROM Comment c
JOIN `User` u ON u.Id = c.UserId
JOIN Post p ON p.Id = c.PostId
WHERE p.UserId <> u.Id
GROUP BY u.Id, u.FirstName, u.LastName
HAVING COUNT(DISTINCT p.UserId) >= 3;
";

        public static string PostsConComentariosDeMenores_MySql = @"
SELECT p.Id AS IdPost, p.Description AS PostDescription,
       COUNT(*) AS ComentariosMenores
FROM Comment c
JOIN `User` u ON u.Id = c.UserId
JOIN Post p ON p.Id = c.PostId
WHERE TIMESTAMPDIFF(YEAR, u.DateOfBirth, CURDATE()) < 18
GROUP BY p.Id, p.Description;";

        public static string DensidadComentariosPorDia_MySql = @"
SELECT DAYNAME(c.Date) AS DiaSemana,
       COUNT(*) AS TotalComentarios,
       COUNT(DISTINCT c.UserId) AS UsuariosUnicos
FROM Comment c
GROUP BY DAYOFWEEK(c.Date), DAYNAME(c.Date)
ORDER BY DAYOFWEEK(c.Date);";

        public static string CrecimientoMensualComentarios_MySql = @"
WITH m AS (
  SELECT YEAR(c.Date) AS Año,
         MONTH(c.Date) AS Mes,
         COUNT(*) AS TotalComentarios
  FROM Comment c
  GROUP BY YEAR(c.Date), MONTH(c.Date)
)
SELECT Año, Mes, TotalComentarios,
       LAG(TotalComentarios) OVER (ORDER BY Año, Mes) AS MesAnterior,
       CASE
         WHEN LAG(TotalComentarios) OVER (ORDER BY Año, Mes) IS NULL THEN NULL
         WHEN LAG(TotalComentarios) OVER (ORDER BY Año, Mes) = 0 THEN NULL
         ELSE ROUND(
           (TotalComentarios - LAG(TotalComentarios) OVER (ORDER BY Año, Mes))
           / LAG(TotalComentarios) OVER (ORDER BY Año, Mes) * 100, 2)
       END AS CrecimientoPorcentual
FROM m
ORDER BY Año, Mes;";

        public static string Top5Usuarios30Dias_MySql = @"
SELECT u.FirstName, u.LastName, COUNT(*) AS Total
FROM Comment c
JOIN `User` u ON u.Id = c.UserId
WHERE c.Date >= DATE_SUB(CURDATE(), INTERVAL 30 DAY)
GROUP BY u.Id, u.FirstName, u.LastName
ORDER BY Total DESC
LIMIT 5;
";

        public static string PromedioComentariosPorPost_MySql = @"
SELECT ROUND(AVG(t.cnt), 2) AS Promedio
FROM (
  SELECT p.Id, COUNT(c.Id) AS cnt
  FROM Post p
  LEFT JOIN Comment c ON c.PostId = p.Id
  GROUP BY p.Id
  HAVING COUNT(c.Id) > 0
) AS t;";

        public static string TiempoMedioPrimerComentario_MySql = @"
SELECT p.Id AS IdPost,
       ROUND(AVG(TIMESTAMPDIFF(MINUTE, p.Date, fc.FirstCommentDate)), 2) AS MinutosHastaPrimerComentario
FROM Post p
JOIN (
  SELECT c.PostId, MIN(c.Date) AS FirstCommentDate
  FROM Comment c
  GROUP BY c.PostId
) fc ON fc.PostId = p.Id
GROUP BY p.Id";
    }
}
    

