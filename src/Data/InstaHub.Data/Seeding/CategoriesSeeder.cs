namespace InstaHub.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using InstaHub.Data.Models;

    public class CategoriesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Categories.Any())
            {
                return;
            }

            var categories = new List<(string Name, string ImageUrl)>
            {
                ("Sport", "https://media.npr.org/assets/img/2020/06/10/gettyimages-200199027-001-b5fb3d8d8469ab744d9e97706fa67bc5c0e4fa40.jpg"),
                ("Coronavirus", "https://p.bnt.bg/a/b/abcfaaee646253454ba6b40e01c8dc62-466488-810x0.png"),
                ("News", "https://steamuserimages-a.akamaihd.net/ugc/312241321107872973/6CAA990B7C3BD9DCC0C4CB92DEED58A13CB640CE/"),
                ("Programming", "https://lemonop.com/uploads/15517716891365be960e846e0fb00018a998bprogrammer.jpeg"),
                ("Movies", "https://images.creativemarket.com/0.1.0/ps/7414066/1820/1214/m1/fpnw/wm1/logo-design-for-movie-production-company-01-.jpg?1575502358&s=c37b3e6a8863b415070b669f6c8a457c"),
                ("Music", "https://www.apple.com/v/apple-music/l/images/shared/og_image__pvk21jd9bj22.png?202007220135"),
                ("Games", "https://images.squarespace-cdn.com/content/v1/55ef0e29e4b099e22cdc9eea/1575909295240-VNAZXPH45PHLZFCI4WAI/ke17ZwdGBToddI8pDm48kPqQfq0L3n3wpHIsRapTfg8UqsxRUqqbr1mOJYKfIPR7LoDQ9mXPOjoJoqy81S2I8N_N4V1vUb5AoIIIbLZhVYxCRW4BPu10St3TBAUQYVKczo5Zn4xktlpMsMj-QlHXeMfNK6GwvtVkYEWiR8XAPyD3GfLCe_DXOSC_YcAacWL_/0fe20042_0bb8_4781_82f4_7130f928b021.0.jpg?format=750w"),
                ("Books", "https://img.jakpost.net/c/2019/03/02/2019_03_02_66706_1551461528._large.jpg"),
                ("Vehicles", "https://upload.wikimedia.org/wikipedia/commons/3/30/2018_BMW_M850i_xDrive_Automatic_4.4.jpg"),
            };

            foreach (var category in categories)
            {
                await dbContext.Categories.AddAsync(new Category()
                {
                    Name = category.Name,
                    Title = category.Name,
                    Description = category.Name,
                    ImageUrl = category.ImageUrl,
                });
            }
        }
    }
}
