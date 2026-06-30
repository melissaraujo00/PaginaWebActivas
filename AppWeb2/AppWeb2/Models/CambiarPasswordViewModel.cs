using System.ComponentModel.DataAnnotations;

namespace AppWeb2.Models
{
    public class CambiarPasswordViewModel
    {
        [Required(ErrorMessage = "La contraseña actual es obligatoria.")]
        public string PasswordActual { get; set; }

        [Required(ErrorMessage = "La nueva contraseña es obligatoria.")]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
        public string NuevaPassword { get; set; }

        [Required(ErrorMessage = "Debes confirmar tu nueva contraseña.")]
        [Compare("NuevaPassword", ErrorMessage = "Las contraseñas nuevas no coinciden.")]
        public string ConfirmarPassword { get; set; }
    }
}