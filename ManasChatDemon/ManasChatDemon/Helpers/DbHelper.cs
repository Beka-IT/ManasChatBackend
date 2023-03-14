using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Models;

namespace WorkerService.Helpers
{
    public class DbHelper
    {
        private const int _userVerifywaitTimeInMinutes = 30;
        private const int _hoursInDays = 24;
        private const int _minutesInHour = 60;
        private AppDbContext _dbContext;

        private DbContextOptions<AppDbContext> GetAllOptions()
        {
            DbContextOptionsBuilder<AppDbContext> optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlite(AppSettings.ConnectionString);
            return optionsBuilder.Options;
        }

        public void ClearNotVerifiedUsers()
        {
            using (_dbContext = new AppDbContext(GetAllOptions()))
            {
                try
                {
                    var users = _dbContext.Users.Where(u => !u.IsVerify);
                    if (users is not null)
                    {
                        foreach (var user in users)
                        {
                            var differenceOfDates = DateTime.Now - user.CreatedAt;
                            var differenceInMinutes = 
                                (differenceOfDates.Days * _hoursInDays * _minutesInHour) + 
                                (differenceOfDates.Hours * _minutesInHour) + differenceOfDates.Minutes;
                            if (differenceInMinutes > _userVerifywaitTimeInMinutes)
                            {
                                _dbContext.Remove(user);
                            }
                        }

                        _dbContext.SaveChanges();
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
// Seed Data - When no data is in the db, we want to populate with data        public void SeedUsers()        {            using (_dbContext = new AppDbContext(GetAllOptions()))            {                _dbContext.Users.AddRange(ListOfUsers());                _dbContext.SaveChanges();            }        }        private List<User> ListOfUsers()        {            List<User> users = new List<User> {                new User                {                    Name = "Jay Jay",                    Email = "jayjay@gmail.com"                },                new User                {                    Name = "Kanu Nwankwo",                    Email = "kanunwankwo@gmail.com"                },                new User                {                    Name = "Taribo West",                    Email = "taribowest@gmail.com"                }            };            return users;        }    }}