using AppWeb2.Data;
using AppWeb2.Filtros;
using AppWeb2.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace AppWeb2.Controllers
{
    public class AccountController : Controller
    {
        private readonly TiendaContext _context;

        public AccountController(TiendaContext context)
        {
            _context = context;
        }

        [SessionAuthorize]

        public IActionResult Dashboard()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(Login model)
        {
            var user = _context.Usuarios
                .FirstOrDefault(u => u.Email == model.Correo);
            if (user != null)
            {
                string saltedPassword = user.Salt + model.Password;

                using (SHA256 sha256 = SHA256.Create())
                {
                    //byte[] inputBytes = Encoding.UTF8.GetBytes(saltedPassword); 
                    byte[] inputBytes = Encoding.Unicode.GetBytes(saltedPassword);
                    byte[] hashBytes = sha256.ComputeHash(inputBytes);

                    Console.WriteLine("Salt DB: " + user.Salt);
                    Console.WriteLine("Password ingresada: " + model.Password);
                    Console.WriteLine("Password salada: " + (user.Salt + model.Password));
                    Console.WriteLine("Hash calculado: " + Convert.ToBase64String(hashBytes));
                    Console.WriteLine("Hash DB: " + Convert.ToBase64String(user.Password));

                    if (hashBytes.SequenceEqual(user.Password))
                    {
                        HttpContext.Session.SetString("usuario", user.Nombre);
                        return RedirectToAction("Dashboard", "Account");
                    }
                }
            }

            ViewBag.Error = "Correo o contraseña incorrectos";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

    }
}
