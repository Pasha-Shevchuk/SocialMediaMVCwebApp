using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;
using SocialMediaMVCwebApp.Models;
using System.Diagnostics;

namespace SocialMediaMVCwebApp.Data
{
    public class Seed
    {
        public static void SeedData(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AppDbContext>();

                context.Database.EnsureCreated();

                //if (!context.Posts.Any())
                //{
                //    context.Posts.AddRange(new List<Post>()
                //    {
                //        new Post()
                //        {
                //            Title = "Post1",
                //            Image = "https://www.emdocs.net/wp-content/themes/emdocs/images/default-image.jpg",
                //            PostText = "POST 1 CONTEXT",
                //            PostCategory = new PostCategory()
                //            {
                //                NameOfPostCategory = "Category 1"
                //            },
                //            Address = new Address()
                //            {
                //                Country = "Ukraine",
                //                Location = "Khmelnitsky",
                //                Region = "Khmelnitska Oblast"
                //            }
                //        },
                //        new Post()
                //        {
                //            Title = "Post 2",
                //            Image = "https://www.emdocs.net/wp-content/themes/emdocs/images/default-image.jpg",
                //            PostText = "Post 2 Context",
                //            PostCategory = new PostCategory()
                //            {
                //                NameOfPostCategory = "Category 2"
                //            },
                //            Address = new Address()
                //            {
                //                Country = "USA",
                //                Location = "Ohio",
                //                Region = "Some random region"
                //            }
                //        },
                //        new Post()
                //        {
                //            Title = "Post 3",
                //            Image = "https://www.emdocs.net/wp-content/themes/emdocs/images/default-image.jpg",
                //            PostText = "Post 3 Context",
                //            PostCategoryId = 1,
                //            AddressId = 2
                //        },
                //        new Post()
                //        {
                //            Title = "Post 3",
                //            Image = "https://www.emdocs.net/wp-content/themes/emdocs/images/default-image.jpg",
                //            PostText = "Post 3 Context",
                //            PostCategory = new PostCategory()
                //            {
                //                NameOfPostCategory = "Category 1"
                //            },
                //            Address = new Address()
                //            {
                //                Country = "Japan",
                //                Location = "Go/Jo",
                //                Region = "J-O-J-O"
                //            }
                //        },

                //    });
                //    context.SaveChanges();
                //}

                //Genders
                if (!context.Genders.Any())
                {
                    context.Genders.AddRange(new List<Gender>()
                    {
                        new Gender { NameOfGender = "male" },
                        new Gender { NameOfGender = "female" },

                    });
                    context.SaveChanges();
                }

                if (!context.PostCategories.Any())
                {
                    context.PostCategories.AddRange(new List<PostCategory>()
                    {
                        new PostCategory { NameOfPostCategory = "Games" },
                        new PostCategory { NameOfPostCategory = "Science" },
                        new PostCategory { NameOfPostCategory = "Programming" },
                        new PostCategory { NameOfPostCategory = "Technology" },
                        new PostCategory { NameOfPostCategory = "Entertainment" },
                        new PostCategory { NameOfPostCategory = "Health" },
                        new PostCategory { NameOfPostCategory = "Travel" },
                        new PostCategory { NameOfPostCategory = "Food" },
                        new PostCategory { NameOfPostCategory = "Education" },
                        new PostCategory { NameOfPostCategory = "Sports" },
                        new PostCategory { NameOfPostCategory = "Lifestyle" },
                        new PostCategory { NameOfPostCategory = "News" },
                        new PostCategory { NameOfPostCategory = "Art" },
                        new PostCategory { NameOfPostCategory = "Music" },
                        new PostCategory { NameOfPostCategory = "Finance" },
                        new PostCategory { NameOfPostCategory = "Movies" },
                        new PostCategory { NameOfPostCategory = "Fitness" },
                        new PostCategory { NameOfPostCategory = "Books" },
                        new PostCategory { NameOfPostCategory = "History" },
                        new PostCategory { NameOfPostCategory = "Fashion" },

                    });
                    context.SaveChanges();
                }



            }
        }


        public static async Task SeedUsersAndRolesAsync(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                //Roles
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                if (!await roleManager.RoleExistsAsync(UserRoles.User))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.User));

                //Users
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                string adminUserEmail = "admin@gmail.com";

                var adminUser = await userManager.FindByEmailAsync(adminUserEmail);
                if (adminUser == null)
                {
                    var newAdminUser = new AppUser()
                    {
                        UserName = "admin",
                        Email = adminUserEmail,
                        EmailConfirmed = true,
                        Address = new Address()
                        {
                            Country = "AdminCountry",
                            Region = "AdminGegion",
                            Location = "AdminLocation"
                        }
                    };
                    await userManager.CreateAsync(newAdminUser, "iAmAdminYEEEAH1234@!");
                    await userManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);
                }

            }
        }
    }
}
