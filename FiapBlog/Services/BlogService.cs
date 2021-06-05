using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FiapBlog.Controllers
{
    public class BlogService : IBlogService
    {
        private IWebHostEnvironment _env;
        public BlogService(IWebHostEnvironment env)
        {
            _env = env;
        }

        private List<BlogPost> Posts
        {
            get
            {
                return new List<BlogPost>() {
                new BlogPost { PostId = 1, Title = "Obter posts via API", ShortDescription = "Como usar fetch para obter uma lista de posts do blog" },
                new BlogPost { PostId = 2, Title = "Usando Indexed DB", ShortDescription = "Como salvar lista de posts utilizando indexed DB" },
                new BlogPost { PostId = 3, Title = "Gravando posts do blog no cache", ShortDescription = "Como usar a Cache API para salvar posts de blog que podem ser acessados offline" },
                new BlogPost { PostId = 4, Title = "Obtendo dado em cache com Service Worker", ShortDescription = "Como utilizar Service Worker para obter dado do cache quando o usuário está offline" },
                new BlogPost { PostId = 5, Title = "Criando uma Web App instalável", ShortDescription = "Como criar arquivos que permitem que você instale o seu aplicativo no seu celular" },
                new BlogPost { PostId = 6, Title = "Enviando notificações push", ShortDescription = "Como enviar notificações push que permitem notificar o usuário que tem o seu aplicativo instalado" },
                new BlogPost { PostId = 7, Title = "Micro front ends", ShortDescription = "Como criar Micro front ends" },
                new BlogPost { PostId = 8, Title = "Blazor", ShortDescription = "Como implementar uma SPA com Blazor client-side" },
                new BlogPost { PostId = 9, Title = "Xamarim", ShortDescription = "Como implementar uma aplicação Xamarim" },
                new BlogPost { PostId = 10, Title = "Unity", ShortDescription = "Como implementar uma aplicação Unity" },
                new BlogPost { PostId = 11, Title = "Angular", ShortDescription = "Como implementar uma aplicação Angular" },
                new BlogPost { PostId = 12, Title = "React", ShortDescription = "Como implementar uma aplicação React" }
            };
            }
        }

        public string GetPostText(string link)
        {
            var post = Posts.FirstOrDefault(_ => _.Link == link);

            return File.ReadAllText($"{_env.WebRootPath}/Posts/{post.PostId}_post.md");
        }

        public List<BlogPost> GetLatestPosts()
        {
            return Posts.OrderByDescending(_ => _.PostId).Take(3).ToList();
        }

        public List<BlogPost> GetOlderPosts(int oldestPostId)
        {
            var posts = Posts.Where(_ => _.PostId < oldestPostId).OrderByDescending(_ => _.PostId).ToList();

            if (posts.Count < 3)
                return posts;

            return posts.Take(3).ToList();
        }
    }
}
