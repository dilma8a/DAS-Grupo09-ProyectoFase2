using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAS_Grupo09_ProyectoFase2Rest.Models
{
    [Table("Usuarios")]
    public class Usuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Apellido { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [StringLength(150)]
        public string Correo { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string Clave { get; set; } = string.Empty;

        [Required]
        public int IdRol { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        public bool EstaActivo { get; set; } = true;

        // Navigation property
        [ForeignKey("IdRol")]
        public virtual Rol? Rol { get; set; }
    }
}
