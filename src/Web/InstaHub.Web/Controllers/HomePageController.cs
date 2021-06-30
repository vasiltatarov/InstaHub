namespace InstaHub.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using InstaHub.Services.Data;
    using InstaHub.Web.ViewModels.HomePage;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;
    using PagedList;

    using static InstaHub.Common.GlobalConstants;

    [Authorize]
    public class HomePageController : Controller
    {
        private readonly IPostService postService;
        private readonly IMemoryCache cache;

        public HomePageController(IPostService postService, IMemoryCache memoryCache)
        {
            this.postService = postService;
            this.cache = memoryCache;
        }

        /// <summary>
        /// This method returns all posts.
        /// </summary>
        /// <param name="searchTerm"></param>
        /// searchTerm is a key word to search in the post's titles.
        /// <param name="orderBy"></param>
        /// orderBy is a key word to search in the post's titles.
        /// <param name="page"></param>
        /// Install PagedList.Mvc and PagedList to do pagination.
        /// In cshtml file use IPagedList<T>.
        /// <returns></returns>
        [HttpGet]
        public IActionResult Posts(string searchTerm, string orderBy = "Latest", int page = DefaultPage)
        {
            if (!this.cache.TryGetValue<IEnumerable<HomePostViewModel>>("Posts", out var posts))
            {
                posts = this.postService.GetAllPosts<HomePostViewModel>();

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(10));

                this.cache.Set("Posts", posts, cacheEntryOptions);
            }

            if (posts.Count() != this.postService.GetAllPosts<HomePostViewModel>().Count())
            {
                posts = this.postService.GetAllPosts<HomePostViewModel>();
            }

            this.ViewData.Add("orderBy", orderBy);
            this.ViewData.Add("searchTerm", searchTerm);
            this.ViewData.Add("page", page);

            this.TempData.Add("AllPosts", posts.Count());

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                posts = posts
                    .Where(x => x.Title.ToLower().StartsWith(searchTerm, true, CultureInfo.InvariantCulture))
                    .ToList();
            }

            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                posts = this.OrderHomePostsBy(posts, orderBy);
            }

            return this.View(posts.ToPagedList(page, ItemsOnPaged));
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
