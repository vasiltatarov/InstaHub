namespace MyForum.Web.Controllers
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using MyForum.Services.Data;
    using MyForum.Web.ViewModels.HomePage;
    using PagedList;

    [Authorize]
    public class HomePageController : Controller
    {
        private const int ItemsPerPage = 10;

        private readonly IPostsService postsService;

        public HomePageController(IPostsService postsService)
        {
            this.postsService = postsService;
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
        public IActionResult Posts(string searchTerm, string searchFor = "latest", int page = 1)
        {
            var posts = this.postsService.GetAllPosts<HomePostViewModel>();

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
            => (searchFor) switch
            {
                "latest" => posts.OrderByDescending(x => x.CreatedOn),
                "earliest" => posts.OrderBy(x => x.CreatedOn),
                "mostVisited" => posts.OrderByDescending(x => x.VisitorsCount),
                "mostLiked" => posts.OrderByDescending(x => x.VotesCount),
                _ => posts
            };

        //[HttpGet]
        //public IActionResult GetPosts(string searchTerm, string searchFor = "latest", int page = 1)
        //{
        //    var posts = this.postsService.GetAllPosts<PostViewModel>();

        //    if (!string.IsNullOrWhiteSpace(searchTerm))
        //    {
        //        posts = posts
        //            .Where(x => x.Title.ToLower().StartsWith(searchTerm, true, CultureInfo.InvariantCulture))
        //            .ToList();
        //    }

        //    if (!string.IsNullOrWhiteSpace(searchFor))
        //    {
        //        posts = this.OrderPostsBy(posts, searchFor);
        //    }

        //    return this.View(posts.ToPagedList(page, ItemsPerPage));
        //}

        //private IEnumerable<PostViewModel> OrderPostsBy(IEnumerable<PostViewModel> posts, string searchFor)
        //    => (searchFor) switch
        //    {
        //        "latest" => posts.OrderByDescending(x => x.CreatedOn),
        //        "earliest" => posts.OrderBy(x => x.CreatedOn),
        //        "mostVisited" => posts.OrderByDescending(x => x.VisitorsCount),
        //        "mostLiked" => posts.OrderByDescending(x => x.VotesCount),
        //        _ => posts
        //    };
    }
}
