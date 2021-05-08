namespace MyForum.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;
    using MyForum.Services.Data;
    using MyForum.Web.ViewModels.HomePage;
    using PagedList;

    [Authorize]
    public class HomePageController : Controller
    {
        private const int ItemsPerPage = 5;

        private readonly IPostsService postsService;
        private readonly IMemoryCache cache;

        public HomePageController(IPostsService postsService, IMemoryCache memoryCache)
        {
            this.postsService = postsService;
            this.cache = memoryCache;
        }

        /// <summary>
        /// This method returns all posts.
        /// </summary>
        /// <param name="searchTerm"></param>
        /// searchTerm is a key word to search in the post's titles.
        /// <param name="searchFor"></param>
        /// searchFor is a key word to search in the post's titles.
        /// <param name="page"></param>
        /// Install PagedList.Mvc and PagedList to do pagination.
        /// In cshtml file use IPagedList<T>.
        /// <returns></returns>
        [HttpGet]
        public IActionResult Posts(string searchTerm, string searchFor = "Latest", int page = 1)
        {
            if (!this.cache.TryGetValue<IEnumerable<HomePostViewModel>>("Posts", out var posts))
            {
                posts = this.postsService.GetAllPosts<HomePostViewModel>();

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(100));

                this.cache.Set("Posts", posts, cacheEntryOptions);
            }

            this.ViewData.Add("searchFor", searchFor);
            this.ViewData.Add("searchTerm", searchTerm);
            this.ViewData.Add("page", page);

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                posts = posts
                    .Where(x => x.Title.ToLower().StartsWith(searchTerm, true, CultureInfo.InvariantCulture))
                    .ToList();
            }

            if (!string.IsNullOrWhiteSpace(searchFor))
            {
                posts = this.OrderHomePostsBy(posts, searchFor);
            }

            return this.View(posts.ToPagedList(page, ItemsPerPage));
        }

        private IEnumerable<HomePostViewModel> OrderHomePostsBy(IEnumerable<HomePostViewModel> posts, string searchFor)
            => searchFor switch
            {
                "Latest" => posts.OrderByDescending(x => x.CreatedOn),
                "Earliest" => posts.OrderBy(x => x.CreatedOn),
                "Most-Visited" => posts.OrderByDescending(x => x.VisitorsCount),
                "Most-Liked" => posts.OrderByDescending(x => x.VotesCount),
                _ => posts,
            };
    }
}
