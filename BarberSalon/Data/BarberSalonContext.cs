using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BarberSalon.Models;

namespace BarberSalon.Data
{
    public class BarberSalonContext : DbContext
    {
        public BarberSalonContext (DbContextOptions<BarberSalonContext> options)
            : base(options)
        {
        }

        public DbSet<Appointment> Appointment { get; set; }
        public DbSet<Service> Service { get; set; }

        public DbSet<Worker> Worker { get; set; }

        public DbSet<Review> Review { get; set; }

        public DbSet<PortofolioItem> PortofolioItem { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed Services
            modelBuilder.Entity<Service>().HasData(
                new Service { Id = 1, Name = "Tuns Barbat", Description = "Tuns modern, adaptat preferințelor tale", Price = 50 },
                new Service { Id = 2, Name = "Tuns Barbat + Barba", Description = "Tuns complet cu ras și îngrijire a bărbii", Price = 70 },
                new Service { Id = 3, Name = "Spalat", Description = "Spălare și massage pentru relaxare", Price = 30 },
                new Service { Id = 4, Name = "Vopsit", Description = "Vopsire păr cu produse profesionale", Price = 60 },
                new Service { Id = 5, Name = "Barba", Description = "Ras și îngrijire profesională a bărbii", Price = 40 },
                new Service { Id = 6, Name = "Tratament Păr", Description = "Tratament hidratant și nutritiv pentru păr", Price = 45 },
                new Service { Id = 7, Name = "Pomadă", Description = "Aplicare produse de finisare și styling", Price = 15 },
                new Service { Id = 8, Name = "Tuns Copil", Description = "Tunsoare specială pentru copii cu îngrijire", Price = 35 },
                new Service { Id = 9, Name = "Masaj Scalp", Description = "Masaj relaxant cu produse profesionale", Price = 50 }
            );

            // Seed Workers
            modelBuilder.Entity<Worker>().HasData(
                new Worker 
                { 
                    Id = 1, 
                    Name = "Andrei Popescu", 
                    Position = "Frizer Senior",
                    Address = "Strada Primaverii, Craiova",
                    PhoneNumber = 0740123456,
                    HireDate = new DateTime(2014, 1, 15)
                },
                new Worker 
                { 
                    Id = 2, 
                    Name = "Mihai Ionescu", 
                    Position = "Frizer Specialist",
                    Address = "Strada Primaverii, Craiova",
                    PhoneNumber = 0740123457,
                    HireDate = new DateTime(2016, 3, 22)
                },
                new Worker 
                { 
                    Id = 3, 
                    Name = "Cristian Georgescu", 
                    Position = "Frizer",
                    Address = "Strada Primaverii, Craiova",
                    PhoneNumber = 0740123458,
                    HireDate = new DateTime(2018, 6, 10)
                },
                new Worker 
                { 
                    Id = 4, 
                    Name = "Daniel Vasilescu", 
                    Position = "Frizer Apprentice",
                    Address = "Strada Primaverii, Craiova",
                    PhoneNumber = 0740123459,
                    HireDate = new DateTime(2023, 9, 1)
                }
            );
        }
    }
}

