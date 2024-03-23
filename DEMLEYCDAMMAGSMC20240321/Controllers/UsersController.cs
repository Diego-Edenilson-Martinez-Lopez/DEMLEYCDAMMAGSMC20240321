using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DEMLEYCDAMMAGSMC20240321.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace DEMLEYCDAMMAGSMC20240321.Controllers
{

    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {

            //var users = await _context.Users.Include(u => u.Roles).ToListAsync();
            //return View(users);

            var AplicationDBContext = _context.Users
                .Include(p => p.Roles);
            return View(await AplicationDBContext.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            ViewData["RolesId"] = new SelectList(_context.Roles, "Id", "Name");
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserName,Password,Email,Status,Image,RolesId")] Users user, IFormFile imagen)
        {

            if (imagen != null && imagen.Length > 0)
            {

                using (var memoryStream = new MemoryStream())
                {
                    await imagen.CopyToAsync(memoryStream);

                    user.Image = memoryStream.ToArray();

                }
            }
                    _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            
            
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            ViewData["RolesId"] = new SelectList(_context.Roles, "RolesId", "Name", user.RolesId);
            return View(user);
        }

        // POST: Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserName,Password,Email,Status,RolesId")] Users user, IFormFile imagen)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (imagen != null && imagen.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await imagen.CopyToAsync(memoryStream); // Copia el contenido de la imagen al MemoryStream
                    user.Image = memoryStream.ToArray();
                }
                _context.Update(user);
                await _context.SaveChangesAsync();
            }
            else
            {
                var registroFind = await _context.Users.FirstOrDefaultAsync(s => s.Id == user.Id);

                if (registroFind?.Image?.Length > 0)
                    user.Image = registroFind.Image;

                registroFind.UserName = user.UserName;
                registroFind.Email = user.Email;              
                registroFind.Status = user.Status;
                registroFind.RolesId = user.RolesId;
                

                _context.Update(registroFind);
                await _context.SaveChangesAsync();

            }
            return RedirectToAction(nameof(Index));

            //    catch (DbUpdateConcurrencyException);
            //    {
            //        if (!UserExists(user.Id))
            //        {
            //            return NotFound();
            //        }
            //        else
            //        {
            //            throw;
            //        }
            //    }
            //    return RedirectToAction(nameof(Index));
            //}
            //return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

        private string CalcularHashMD5(string texto)
        {
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(texto);
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }

        // ---- ---- ---- ---- ---- ---- Login ---- ---- ---- ---- ---- ---- 
        // ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- -

        [AllowAnonymous]
        public async Task<IActionResult> Login(string ReturnUrl)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([Bind("UserName,Password")] Users user, string ReturnUrl)
        {
            user.Password = CalcularHashMD5(user.Password);
            var usuarioAut = await _context.Users.Include(u => u.Roles).FirstOrDefaultAsync(u => u.UserName == user.UserName && u.Password == user.Password);
            if (usuarioAut?.Id > 0 && usuarioAut.UserName == user.UserName)
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, usuarioAut.UserName),
                    new Claim(ClaimTypes.Role, usuarioAut.Roles.FirstOrDefault()?.Name),
                    new Claim("Id", usuarioAut.Id.ToString())
                };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), new AuthenticationProperties { IsPersistent = true });
                if (!string.IsNullOrWhiteSpace(ReturnUrl))
                    return Redirect(ReturnUrl);
                else
                    return RedirectToAction("Index", "Home");
            }
            else
                ViewBag.Error = "Credenciales incorrectas";
            ViewBag.pReturnUrl = ReturnUrl;
            return View(user);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
