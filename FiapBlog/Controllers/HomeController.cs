using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FiapBlog.Models;
using Lib.Net.Http.WebPush;
using FiapBlog.Store;

namespace FiapBlog.Controllers
{
    public class HomeController : Controller
    {
        private IBlogService _blogService;
        private readonly IPushSubscriptionStore _subscriptionStore;
        private readonly PushServiceClient _pushClient;

        public HomeController(IBlogService blogService, IPushSubscriptionStore subscriptionStore, PushServiceClient pushClient)
        {
            _blogService = blogService;
            _subscriptionStore = subscriptionStore;
            _pushClient = pushClient;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public JsonResult LatestBlogPosts()
        {
            var posts = _blogService.GetLatestPosts();
            return Json(posts);
        }

        public JsonResult MoreBlogPosts(int oldestBlogPostId)
        {
            var posts = _blogService.GetOlderPosts(oldestBlogPostId);
            return Json(posts);
        }

        public ContentResult Post(string link)
        {
            return Content(_blogService.GetPostText(link));
        }

        [HttpGet("publickey")]
        public ContentResult GetPublicKey()
        {
            return Content(_pushClient.DefaultAuthentication.PublicKey, "text/plain");
        }

        //armazena subscricoes
        [HttpPost("subscriptions")]
        public async Task<IActionResult> StoreSubscription([FromBody]PushSubscription subscription)
        {
            int res = await _subscriptionStore.StoreSubscriptionAsync(subscription);

            if (res > 0)
                return CreatedAtAction(nameof(StoreSubscription), subscription);

            return NoContent();
        }

        [HttpDelete("subscriptions")]
        public async Task<IActionResult> DiscardSubscription(string endpoint)
        {
            await _subscriptionStore.DiscardSubscriptionAsync(endpoint);

            return NoContent();
        }

        [HttpPost("notifications")]
        public async Task<IActionResult> SendNotification([FromBody]PushMessageViewModel messageVM)
        {
            var message = new PushMessage(messageVM.Notification)
            {
                Topic = messageVM.Topic,
                Urgency = messageVM.Urgency
            };

            await _subscriptionStore.ForEachSubscriptionAsync((PushSubscription subscription) =>
            {
                _pushClient.RequestPushMessageDeliveryAsync(subscription, message);
            });

            return NoContent();
        }
    }

}
