using System.ComponentModel.DataAnnotations;

namespace SCHAPP.Models
{
    public class RegistrarseViewModel
    {
        [Required]
        public string inpNombreUsu { get; set; }
        [Required]
        public string inpNombre { get; set; }
        [Required]
        public string inpApPaterno { get; set; }
        [Required]
        public string inpApMaterno { get; set; }
        [Required]
        public string inpEmail { get; set; }
        [Required]
        public DateTime inpFdeNacimiento { get; set; }
        [Required]
        public string inpTelefono { get; set; }
        [Required]
        public string inpDomicilio { get; set; }
        public int slctRol { get; set; }
        public string inpOtrServ { get; set; }
        [Required]
        public string Sexo { get; set; }
        [Required]
        public string inpContraseña { get; set; }
        [Required]
        public string inpContraseña2 { get; set; }

    }
}
