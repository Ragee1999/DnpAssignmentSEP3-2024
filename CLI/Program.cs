
using CLI.UI;
using InMemoryRepositories;
using RepositoryContracts;

namespace CLI
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Instantiate
            IPostRepository postRepository = new PostInMemoryRepository();
            IUserRepository userRepository = new UserInMemoryRepository();
            ICommentRepository commentRepository = new CommentInMemoryRepository();

            // Pass to CLIAPP
            CliApp cliApp = new CliApp(userRepository, postRepository, commentRepository);
            await cliApp.RunAsync();
        }
    }
}


