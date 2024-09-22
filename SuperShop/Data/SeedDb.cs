using Microsoft.AspNetCore.Identity;
using SuperShop.Data.Entities;
using SuperShop.Helpers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SuperShop.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;        
        private Random _random;

        public SeedDb(DataContext context,IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
            _random = new Random();
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();

            await _userHelper.CheckRoleAsync("Admin");
            await _userHelper.CheckRoleAsync("Customer");

            var user = await _userHelper.GetUserByEmailAsync("cssalvador29@gmail.com");
            if(user == null)
            {
                user = new User
                {
                    FirstName = "César",
                    LastName = "Salvador",
                    Email = "cssalvador29@gmail.com",
                    UserName = "cssalvador29@gmail.com",
                    PhoneNumber = "123456789"
                };

                var result = await _userHelper.AddUserAsync(user, "12345");
                if(result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("could not create the user in seeder");
                }
                await _userHelper.AddUserToRoleAsync(user, "Admin");
            }

            var isInRole = await _userHelper.IsUserInRoleAsync(user, "Admin");
            if(!isInRole)
            {
                await _userHelper.AddUserToRoleAsync(user, "Admin");
            }            

            if (!_context.Products.Any())
            {
                AddProduct("iphone X", user);
                AddProduct("ipad Mini", user);
                AddProduct("Macbook Air", user);
                AddProduct("iWatch Series 4", user);
                await _context.SaveChangesAsync();
            }
        }

        private void AddProduct(string name, User user)
        {
            _context.Products.Add(new Product
            {
                Name = name,
                Price = _random.Next(1000),
                IsAvaiable = true,
                Stock = _random.Next(100),
                User = user
            });
        }
    }
}
