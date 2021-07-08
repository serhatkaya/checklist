using System;
using SerhatKaya.CheckList.Context;
using SerhatKaya.CheckList.Entities;
using SerhatKaya.CheckList.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace SerhatKaya.CheckList.Helper
{
    public class DataSeeder
    {
        public static void Seed(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                try
                {
                    CheckListContext masterContext = scope.ServiceProvider.GetRequiredService<CheckListContext>();
                    masterContext.Database.Migrate();

                    var user = new User()
                    {
                        Email = "root@kayaserhat.com",
                        UserFullName = "Serhat KAYA",
                        Role = Roles.Admin
                    };

                    if (!masterContext.Users.Where(x => true).Any())
                    {
                        byte[] passwordHash, passwordSalt;
                        CreatePasswordHash("serhatkaya", out passwordHash, out passwordSalt);
                        user.PasswordHash = passwordHash;
                        user.PasswordSalt = passwordSalt;
                        masterContext.Users.Add(user);
                    }

                    masterContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    Console.WriteLine($"Inner exception: {ex.InnerException}");
                }
            }
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hMac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hMac.Key;
                passwordHash = hMac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
    }
}