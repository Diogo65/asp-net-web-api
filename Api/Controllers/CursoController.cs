using Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Api.Controllers
{
    public class CursoController : ApiController
    {
        //esse objeto vai ser usado em todos os métodos do controle p/ acessar o banco
        private CursoContext db = new CursoContext();

        //Inserção de registros com POST
        //Se o parâmetro for do tipo primitivo ex: string,int: ele vai esperar esse parâmetro da URL
        //Se for do tipo complexo, no caso de uma classe, ele espera o parâmetro no corpo da requisição
        public IHttpActionResult PostCurso(Curso curso)
        {
            //Verifica se a entidade passou nas validações que foram criadas via DataAnnotation
            if (!ModelState.IsValid)
                return BadRequest(ModelState); // E o ModelState retorna um conjunto de mensagem informando qual foi o problema da requisição

            //Se for Válido ele salva no banco de dados
            db.Cursos.Add(curso);
            db.SaveChanges();

            //retorna 201(Created-Http) e o caminho da rota padrão(URL) com o id do curso inserido
            return CreatedAtRoute("DefaultApi", new { id = curso.Id }, curso);
        }


        //Obtendo um registro pelo id
        //retorna os dados de um único curso, por isso recebe id como parâmetro
        //O parâmetro é do tipo primitivo, por isso ele espera ele pela URL
        public IHttpActionResult GetCurso (int id)
        {
            if (id <= 0)
            {
                return BadRequest("O id deve ser um número maior que zero");
            }

            //Localiza o Curso através do Id
            //O método Find busca na coleção de Cursos pelo id
            var curso = db.Cursos.Find(id);

            //Se o curso não for encontrado
            if (curso == null){
                return NotFound();
            }
            else {
                //retorna um Ok contendo no corpo da resposta o objeto curso
                return Ok(curso);
            }
        }


        //Edição de registros com PUT
        public IHttpActionResult PutCurso(int id, Curso curso)
        {
            //verifica as validações
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != curso.Id)
                return BadRequest("O id informado na URL é diferente do id informado no corpo da requisição");

            if(db.Cursos.Count(c => c.Id == id) == 0)
                return NotFound();

            //utiliza o entity para alterar no banco de dados
            //recebe um objeto do tipo curso, e altera o status dele para modified
            db.Entry(curso).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }


        //Exclusão de registros com DELETE
        public IHttpActionResult DeleteCurso(int id)
        {
            if (id <= 0)
                return BadRequest("o id deve ser um número maior que zero");

            //localiza o objeto no banco com método Find
            var curso = db.Cursos.Find(id);

            if (curso == null)
                return NotFound();

            db.Cursos.Remove(curso);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }


        //Listagem de todos os cursos
        //faz o get encima da url base desse controller que é API/Cursos
        //public IHttpActionResult GetCurso()
        //{
        //    //consulta a lista de curso ordenando ela pela data de publicação
        //    var cursos = db.Cursos.OrderBy(c => c.DataPublicacao);

        //    return Ok(cursos);
        //}

        //Adicionando paginação
        public IHttpActionResult GetCurso(int pagina = 1, int tamanhoPagina = 10)
        {
            //realiza a ordenação por dataPublicação

            //se a pagina ou tamanho pagina for menor que zero
            if (pagina <= 0 || tamanhoPagina <= 0)
                return BadRequest("Os parâmetros pagina e tamanhoPagina devem ser maiores que zero");

            if (tamanhoPagina > 10)
                return BadRequest("O tamanho máximo da página permitido é 10");

            //calculando o totalPaginas com base na quantidade de registros que tenho dividido pelo tamanho da página
            int totalPaginas = (int)Math.Ceiling(db.Cursos.Count() / Convert.ToDecimal(tamanhoPagina));

            if (pagina > totalPaginas)
                return BadRequest("A página solicitada não exite");

            System.Web.HttpContext.Current.Response.AddHeader("X-Pagination-TotalPages", totalPaginas.ToString());

            if (pagina > 1)
                System.Web.HttpContext.Current.Response.AddHeader("X-Pagination-PreviousPage", Url.Link("DefaultApi", new { pagina = pagina - 1, tamanhoPagina = tamanhoPagina }));
            
            if (pagina < totalPaginas)
                System.Web.HttpContext.Current.Response.AddHeader("X-Pagination-NextPage", Url.Link("DefaultApi", new { pagina = pagina + 1, tamanhoPagina = tamanhoPagina }));

            //Skip: vai avançar uma quantidade de registros
            //Take: pra pegar apenas uma quantidade de registros
            var cursos = db.Cursos.OrderBy(c => c.DataPublicacao).Skip(tamanhoPagina * (pagina - 1)).Take(tamanhoPagina);

            return Ok(cursos);
        }
    }
}
