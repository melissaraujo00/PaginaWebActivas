using AppWeb2.Data;
using AppWeb2.Filtros;
using AppWeb2.Helpers;
using AppWeb2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;
using System.Globalization;
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
            var rol = HttpContext.Session.GetInt32("idRol");
            if (rol != 1)
            {
                return RedirectToAction("JuegosVenta");
            }
            else
            {
                return View();
            }
            
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
                        HttpContext.Session.SetInt32("UsuarioId", user.Id);
                        HttpContext.Session.SetInt32("idRol", user.idRol);

                        if (user.idRol == 1)
                        {
                            return RedirectToAction("Dashboard", "Account");

                        }
                        else if (user.idRol == 2)
                        {
                            return RedirectToAction("JuegosVenta", "Account");
                        }

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



        [RoleAuthorize(1)]
        public IActionResult GetCategorias()
        {
            var categorias = _context.Categorias
                .Select(c => new { c.id, c.Nombre })
                .ToList();
            return Json(categorias);
        }

        [RoleAuthorize(1)]
        public IActionResult ObtenerVentasPorCategoria(int categoriaId)
        {
            var ventas = _context.DetalleCompras
                .Where(d => d.estadoCompra == "Completado" && d.VideoJuego.Categoria.id == categoriaId)
                .GroupBy(d => d.VideoJuego.Titulo)
                .Select(g => new {
                    nombre = g.Key,
                    cantidad = g.Sum(x => x.cantidad)
                })
                .OrderByDescending(x => x.cantidad)
                .Take(5).ToList();

            return Json(ventas);
        }

        [RoleAuthorize(1)]
        public IActionResult ObtenerDatosDashboard()
        {
            var ventas = _context.DetalleCompras
                .Where(d => d.estadoCompra == "Completado")
                .GroupBy(d => d.VideoJuego.Titulo)
                .Select(g => new {
                    nombre = g.Key,
                    cantidad = g.Sum(x => x.cantidad)
                })
                .OrderByDescending(x => x.cantidad)
                .Take(5).ToList();

            var usuarios = _context.Usuarios
                .GroupBy(u => u.idRol)
                .Select(g => new {
                    rol = g.Key == 1 ? "Administradores" : "Clientes",
                    total = g.Count()
                }).ToList();

            var categorias = _context.VideoJuegos
                .GroupBy(v => v.Categoria.Nombre)
                .Select(g => new {
                    categoria = g.Key,
                    total = g.Count()
                }).ToList();

        
            var estados = _context.DetalleCompras
                .GroupBy(d => d.estadoCompra)
                .Select(g => new {
                    estado = g.Key,
                    total = g.Count()
                }).ToList();


            return Json(new { ventas, usuarios, categorias, estados });
        }

        [RoleAuthorize(1)]
        public async Task<IActionResult> DetalleVentas(DateTime? desde, DateTime? hasta, string searchString, string estado, int pagina = 1)
        {
            int paginador = 30;
           
            var query = _context.DetalleCompras
                .Include(d => d.Compra)
                .Include(d => d.VideoJuego)
                .AsQueryable();

            if (desde.HasValue)
                query = query.Where(d => d.Compra.FechaCompra >= desde.Value);

            if (hasta.HasValue)
                query = query.Where(d => d.Compra.FechaCompra <= hasta.Value);

            if (!string.IsNullOrEmpty(estado))
                query = query.Where(d => d.estadoCompra == estado);

            if (!string.IsNullOrEmpty(searchString)) 
                query = query.Where(d => d.Compra.Usuario.Nombre.Contains(searchString));


            var totalRegistros = await query.CountAsync();

            var datos = await query
                .OrderByDescending(d => d.Compra.FechaCompra)
                .Skip((pagina - 1) * paginador)
                .Take(paginador)
                .Select(d => new VentaViewModel
                {
                    idCompra = d.idCompra,
                    cantidad = d.cantidad,
                    total = d.total,
                    estadoCompra = d.estadoCompra,
                    fechaHoraTransaccion = d.fechaHoraTransaccion,
                    codigoTransaccion = d.codigoTransaccion,
                    clienteNombre = d.Compra.Usuario.Nombre,
                    TituloJuego = d.VideoJuego.Titulo
                }).ToListAsync();

            ViewBag.TotalPaginas = (int)Math.Ceiling((double)totalRegistros / paginador);
            ViewBag.PaginaActual = pagina;
            ViewBag.Desde = desde?.ToString("yyyy-MM-dd");
            ViewBag.Hasta = hasta?.ToString("yyyy-MM-dd");
            ViewBag.SearchString = searchString; 
            ViewBag.Estado = estado;            

            return View(datos);
        }

        [RoleAuthorize(1)]
        public async Task<IActionResult> DetalleVentasPdf(DateTime? desde, DateTime? hasta, string searchString, string estado)
        {
            var query = _context.DetalleCompras
                .Include(d => d.Compra)
                .ThenInclude(c => c.Usuario)
                .Include(d => d.VideoJuego)
                .AsQueryable();


            if (desde.HasValue) query = query.Where(d => d.Compra.FechaCompra >= desde.Value);
            if (hasta.HasValue) query = query.Where(d => d.Compra.FechaCompra <= hasta.Value);
            if (!string.IsNullOrEmpty(estado)) query = query.Where(d => d.estadoCompra == estado);

            if (!string.IsNullOrEmpty(searchString)) query = query.Where(d => d.Compra.Usuario.Nombre.Contains(searchString));


            var datos = await query.OrderByDescending(d => d.Compra.FechaCompra)
                .Select(d => new VentaViewModel
                {
                    codigoTransaccion = d.codigoTransaccion,
                    clienteNombre = d.Compra.Usuario.Nombre,
                    TituloJuego = d.VideoJuego.Titulo,
                    cantidad = d.cantidad,
                    total = d.total,
                    estadoCompra = d.estadoCompra,
                    fechaHoraTransaccion = d.fechaHoraTransaccion
                }).ToListAsync();


            var filtros = new FiltrosPaginacionViewModel
            {
                Desde = desde?.ToString("dd/MM/yyyy"),
                Hasta = hasta?.ToString("dd/MM/yyyy"),
                SearchString = searchString,
                Estado = estado,
                LabelSearch = "CLIENTE"
            };

            var pdfModel = new ReporteVentasPdfViewModel
            {
                Filtros = filtros,
                Ventas = datos
            };

            string nombreArchivo = FileNamingHelper.GenerarNombrePdf("Reporte_Ventas", searchString, estado, desde, hasta);

            return new ViewAsPdf("ReporteVentasPdf", pdfModel)
            {
                FileName = nombreArchivo,
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape,
                CustomSwitches = "--footer-right \"Página [page] de [toPage]\" --footer-font-size 9"
            };
        }


        [RoleAuthorize(2)]
        public async Task<IActionResult> JuegosVenta(int pagina = 1)
        {
            int registrosPorPagina = 10;
            var query = _context.VideoJuegos
                .Include(j => j.Categoria)
                .OrderByDescending(j => j.FechaRegistro);

            int totalRegistros = await query.CountAsync();
            var juegos = await query
                .Skip((pagina - 1) * registrosPorPagina)
                .Take(registrosPorPagina)
                .ToListAsync();

            ViewBag.TotalPaginas = (int)Math.Ceiling((double)totalRegistros / registrosPorPagina);
            ViewBag.PaginaActual = pagina;

            return View(juegos);
        }


        [RoleAuthorize(2)]
        public async Task<IActionResult> MisCompras(DateTime? desde, DateTime? hasta, string searchString, int pagina = 1)
        {
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");
            if (usuarioId == null) return RedirectToAction("Login");

            int registrosPorPagina = 10;
            var query = _context.DetalleCompras
                .Include(d => d.VideoJuego)
                .Where(d => d.Compra.UsuarioId == usuarioId.Value);

            if (!string.IsNullOrEmpty(searchString))
                query = query.Where(d => d.VideoJuego.Titulo.Contains(searchString));
            if (desde.HasValue)
                query = query.Where(d => d.fechaHoraTransaccion.Date >= desde.Value.Date);
            if (hasta.HasValue)
                query = query.Where(d => d.fechaHoraTransaccion.Date <= hasta.Value.Date);

            int totalRegistros = await query.CountAsync();
            var compras = await query
                .OrderByDescending(d => d.fechaHoraTransaccion)
                .Skip((pagina - 1) * registrosPorPagina)
                .Take(registrosPorPagina)
                .Select(d => new VentaViewModel
                {
                    idCompra = d.idCompra,
                    VideoJuegoId = d.VideoJuegoId,
                    TituloJuego = d.VideoJuego.Titulo,
                    cantidad = d.cantidad,
                    total = d.total,
                    estadoCompra = d.estadoCompra,
                    fechaHoraTransaccion = d.fechaHoraTransaccion,
                    codigoTransaccion = d.codigoTransaccion
                }).ToListAsync();

            ViewBag.TotalPaginas = (int)Math.Ceiling((double)totalRegistros / registrosPorPagina);
            ViewBag.PaginaActual = pagina;
            ViewBag.Desde = desde?.ToString("yyyy-MM-dd");
            ViewBag.Hasta = hasta?.ToString("yyyy-MM-dd");
            ViewBag.SearchString = searchString;

            return View(compras);
        }


        [RoleAuthorize(2)]
        public async Task<IActionResult> MisComprasPdf(DateTime? desde, DateTime? hasta, string searchString)
        {
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == usuarioId);
            string nombreCompleto = usuario?.Nombre ?? "Cliente";

            var query = _context.DetalleCompras
                .Include(d => d.VideoJuego)
                .Where(d => d.Compra.UsuarioId == usuarioId.Value);


            if (!string.IsNullOrEmpty(searchString))
                query = query.Where(d => d.VideoJuego.Titulo.Contains(searchString));

            if (desde.HasValue)
                query = query.Where(d => d.fechaHoraTransaccion.Date >= desde.Value.Date);

            if (hasta.HasValue)
                query = query.Where(d => d.fechaHoraTransaccion.Date <= hasta.Value.Date);

            var compras = await query
                .OrderByDescending(d => d.fechaHoraTransaccion)
                .Select(d => new VentaViewModel
                {
                    TituloJuego = d.VideoJuego.Titulo,
                    cantidad = d.cantidad,
                    total = d.total,
                    estadoCompra = d.estadoCompra,
                    fechaHoraTransaccion = d.fechaHoraTransaccion,
                    codigoTransaccion = d.codigoTransaccion
                }).ToListAsync();

 
            var pdfModel = new ReporteVentasPdfViewModel
            {
                Ventas = compras,
                NombreUsuario = nombreCompleto,
                Filtros = new FiltrosPaginacionViewModel
                {
                    Desde = desde?.ToString("dd/MM/yyyy"),
                    Hasta = hasta?.ToString("dd/MM/yyyy"),
                    SearchString = searchString,
                    LabelSearch = "VIDEOJUEGO" 
                }
            };

            

            string nombreArchivo = FileNamingHelper.GenerarNombrePdf("Mis_Compras", searchString, null, desde, hasta);

            return new ViewAsPdf("ReporteComprasPdf", pdfModel)
            {
                FileName = nombreArchivo,
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                CustomSwitches = "--footer-right \"Página [page] de [toPage]\" --footer-font-size 9"
            };
        }


        [RoleAuthorize(1)]
        public async Task<IActionResult> Clientes(string searchString, int pagina = 1)
        {
            int registrosPorPagina = 15;


            var query = _context.Usuarios.Where(u => u.idRol == 2).AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(u => u.Nombre.Contains(searchString) || u.Email.Contains(searchString));
            }

            int totalRegistros = await query.CountAsync();

            var clientes = await query
                .OrderByDescending(u => u.Id)
                .Skip((pagina - 1) * registrosPorPagina)
                .Take(registrosPorPagina)
                .ToListAsync();

            ViewBag.TotalPaginas = (int)Math.Ceiling((double)totalRegistros / registrosPorPagina);
            ViewBag.PaginaActual = pagina;
            ViewBag.SearchString = searchString;

            return View(clientes);
        }


        [SessionAuthorize]
        public IActionResult CambiarPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionAuthorize]
        public async Task<IActionResult> CambiarPassword(CambiarPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");
            var user = await _context.Usuarios.FindAsync(usuarioId);

            if (user == null) return RedirectToAction("Login");

            using (SHA256 sha256 = SHA256.Create())
            {

                string saltedActual = user.Salt + model.PasswordActual;
                byte[] hashActual = sha256.ComputeHash(Encoding.Unicode.GetBytes(saltedActual));

                if (!hashActual.SequenceEqual(user.Password))
                {
                    ViewBag.Error = "La contraseña actual es incorrecta.";
                    return View(model);
                }

                string saltedNueva = user.Salt + model.NuevaPassword;
                byte[] hashNueva = sha256.ComputeHash(Encoding.Unicode.GetBytes(saltedNueva));

                user.Password = hashNueva;
                _context.Update(user);
                await _context.SaveChangesAsync();
            }

            ViewBag.Exito = "Tu contraseña ha sido actualizada correctamente.";
            ModelState.Clear();

            return View(new CambiarPasswordViewModel());
        }

        
        [RoleAuthorize(1)]
        public IActionResult CrearCliente()
        {
            return View(new ClienteAdminViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RoleAuthorize(1)]
        public async Task<IActionResult> CrearCliente(ClienteAdminViewModel model)
        {
            if (string.IsNullOrEmpty(model.Password))
                ModelState.AddModelError("Password", "La contraseña es obligatoria para un nuevo cliente.");

            if (ModelState.IsValid)
            {
               
                if (_context.Usuarios.Any(u => u.Email == model.Email))
                {
                    ViewBag.Error = "Este correo ya está registrado.";
                    return View(model);
                }

                var nuevoCliente = new Usuario
                {
                    Nombre = model.Nombre,
                    Email = model.Email,
                    idRol = 2, 
                    Salt = Guid.NewGuid().ToString() 
                };

                using (SHA256 sha256 = SHA256.Create())
                {
                    string saltedPassword = nuevoCliente.Salt + model.Password;
                    nuevoCliente.Password = sha256.ComputeHash(Encoding.Unicode.GetBytes(saltedPassword));
                }

                _context.Usuarios.Add(nuevoCliente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Clientes));
            }
            return View(model);
        }

        
        [RoleAuthorize(1)]
        public async Task<IActionResult> EditarCliente(int id)
        {
            var cliente = await _context.Usuarios.FindAsync(id);
            if (cliente == null || cliente.idRol != 2) return NotFound();

            var model = new ClienteAdminViewModel
            {
                Id = cliente.Id,
                Nombre = cliente.Nombre,
                Email = cliente.Email
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RoleAuthorize(1)]
        public async Task<IActionResult> EditarCliente(ClienteAdminViewModel model)
        {
            if (ModelState.IsValid)
            {
                var clienteDB = await _context.Usuarios.FindAsync(model.Id);
                if (clienteDB == null) return NotFound();

                clienteDB.Nombre = model.Nombre;
                clienteDB.Email = model.Email;

                // Si el admin escribió una contraseña, la actualizamos
                if (!string.IsNullOrEmpty(model.Password))
                {
                    using (SHA256 sha256 = SHA256.Create())
                    {
                        string saltedPassword = clienteDB.Salt + model.Password;
                        clienteDB.Password = sha256.ComputeHash(Encoding.Unicode.GetBytes(saltedPassword));
                    }
                }

                _context.Update(clienteDB);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Clientes));
            }
            return View(model);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [RoleAuthorize(1)]
        public async Task<IActionResult> EliminarCliente(int id)
        {
            var cliente = await _context.Usuarios.FindAsync(id);
            if (cliente != null && cliente.idRol == 2)
            {
                _context.Usuarios.Remove(cliente);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Clientes));
        }

        
        [RoleAuthorize(1,2)]
        public async Task<IActionResult> MiPerfil()
        {
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");
            var user = await _context.Usuarios.FindAsync(usuarioId);
            if (user == null) return RedirectToAction("Login");

            var model = new MiPerfilViewModel
            {
                Nombre = user.Nombre,
                Email = user.Email
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RoleAuthorize(2)]
        public async Task<IActionResult> MiPerfil(MiPerfilViewModel model)
        {
            if (ModelState.IsValid)
            {
                var usuarioId = HttpContext.Session.GetInt32("UsuarioId");
                var user = await _context.Usuarios.FindAsync(usuarioId);
                if (user == null) return RedirectToAction("Login");

                user.Nombre = model.Nombre;
                user.Email = model.Email;

                _context.Update(user);
                await _context.SaveChangesAsync();


                HttpContext.Session.SetString("usuario", user.Nombre);

                ViewBag.Exito = "Tu información personal ha sido actualizada correctamente.";
            }
            return View(model);
        }

    }
}
