using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Api.Models
{
    [Table("Cursos")] //diz o nome da tabela que será gerado no DB
    public class Curso
    {
        //Data Annotation
        public int Id { get; set; }

        //curstomização da mensagem de erro caso a propriedade não seja preenchida
        [Required(ErrorMessage = "O título do curso deve ser preenchido.")]
        //impede que o texto receba mais de 100 caracteres
        [MaxLength(100, ErrorMessage ="O título do curso só pode conter até 100 caracteres.")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "A URL do curso deve ser preenchida.")]
        [Url(ErrorMessage = "A URL do curso deve conter um endereço válido.")]
        public string URL { get; set; }

        [Required(ErrorMessage = "O canal do curso deve ser preenchido")]
        //Convertendo enum para string no GET
        [JsonConverter(typeof(StringEnumConverter))]
        public Canal Canal { get; set; }

        [Required(ErrorMessage = "A data de publicação do curso deve ser preenchida")]
        public DateTime DataPublicacao { get; set; }

        [Required(ErrorMessage = "A carga horária do curso deve ser preenchida.")]
        [Range(1, Int32.MaxValue, ErrorMessage ="A carga horária deve ser de pelo menos 1h")]
        public int CargaHoraria { get; set; }
    }
}