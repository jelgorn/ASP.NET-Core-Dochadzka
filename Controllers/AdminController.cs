using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using EvidenciaStudentov.Domain.Constants;
using Microsoft.EntityFrameworkCore;
using EvidenciaStudentov.Infrastructure.Persistence;
using EvidenciaStudentov.Models;
using System.Diagnostics;
using System.IO;
using System;
using System.Text;
using System.Globalization;

namespace EvidenciaStudentov.Controllers
{
    [Authorize(Roles = RoleNames.Admin)]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        // Konfigurácia MySQL zálohy
        private readonly string _mysqlUsername = "root";
        private readonly string _mysqlPassword = "1234";
        private readonly string _mysqlDatabase = "bakalarka";
        private readonly string _mysqldumpPath = @"C:\Program Files\MySQL\MySQL Server 8.0\bin\mysqldump.exe";
        private readonly string _backupDirectory = @"C:\Backups";

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Zobrazenie hlavnej stránky administratívneho panela.
        /// </summary>
        public async Task<IActionResult> Index()
        {
            var users = await _context.Pouzivatelia.ToListAsync();
            var pocetPredmetov = await _context.Predmety.CountAsync();
            ViewBag.PocetPredmetov = pocetPredmetov;
            return View(users);
        }

        /// <summary>
        /// Export všetkých používateľov do CSV súboru.
        /// </summary>
        public IActionResult ExportUsersToCSV()
        {
            var users = _context.Pouzivatelia.ToList();
            var csv = new StringBuilder();

            // Hlavička CSV súboru
            csv.AppendLine("PouzivatelId,Meno,Priezvisko,Email,DatumNarodenia,Rola");

            // Pridanie údajov používateľov
            foreach (var user in users)
            {
                string date = user.DatumNarodenia.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);
                csv.AppendLine($"{user.PouzivatelId},{user.Meno},{user.Priezvisko},{user.Email},{date},{user.Rola}");
            }

            byte[] buffer = Encoding.UTF8.GetBytes(csv.ToString());
            return File(buffer, "text/csv", "users.csv");
        }

        /// <summary>
        /// Vytvorí zálohu databázy pomocou mysqldump a umožní stiahnutie súboru.
        /// </summary>
        public IActionResult CreateDatabaseBackup()
        {
            try
            {
                // Over, či zálohovací priečinok existuje, ak nie - vytvor ho
                if (!Directory.Exists(_backupDirectory))
                {
                    Directory.CreateDirectory(_backupDirectory);
                }

                // Vytvorenie názvu zálohy
                string backupFileName = $"Backup_{DateTime.Now:yyyyMMddHHmmss}.sql";
                string backupPath = Path.Combine(_backupDirectory, backupFileName);

                // Príprava argumentov pre mysqldump (heslo bez medzery)
                string arguments = $"-u{_mysqlUsername} -p{_mysqlPassword} {_mysqlDatabase}";

                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = _mysqldumpPath,
                    Arguments = arguments,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (Process? process = Process.Start(psi))
                {
                    if (process is null)
                    {
                        return BadRequest("Nepodarilo sa spustiť mysqldump.");
                    }

                    string output = process.StandardOutput.ReadToEnd();
                    string errors = process.StandardError.ReadToEnd();
                    process.WaitForExit();

                    // Ak chybový výstup obsahuje len varovanie, ignoruj ho
                    if (!string.IsNullOrWhiteSpace(errors))
                    {
                        string lowerErrors = errors.ToLower();
                        if (lowerErrors.Contains("error") || lowerErrors.Contains("failed"))
                        {
                            return BadRequest("Chyba pri zálohovaní: " + errors);
                        }
                    }

                    // Ulož výstup do súboru
                    System.IO.File.WriteAllText(backupPath, output);
                }

                byte[] fileBytes = System.IO.File.ReadAllBytes(backupPath);
                return File(fileBytes, "application/octet-stream", backupFileName);
            }
            catch (Exception ex)
            {
                return BadRequest("Zálohovanie zlyhalo: " + ex.Message);
            }
        }
    }
}



