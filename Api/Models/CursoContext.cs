using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Api.Models
{
    public class CursoContext : DbContext
    {
        //sobescrever o contrutor padrão
        // chamando o contrutor do dbContext passando como parâmetro o nome da 
        //string de conexão, ele irá buscar no webConfig
        public CursoContext() : base("CursoLocal")
        {
            //Para verificar as instruções SQL que são geradas pelo Entity Framework
            Database.Log = d => System.Diagnostics.Debug.WriteLine(d);
        }

        public DbSet<Curso> Cursos { get; set; }
    }
}