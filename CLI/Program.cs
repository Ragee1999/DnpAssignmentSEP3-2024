
using CLI.UI;
using FileRepositories;
using RepositoryContracts;

namespace CLI
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Instantiate
            IUserRepository userRepository = new UserFileRepository();
            IPostRepository postRepository = new PostFileRepository();
            ICommentRepository commentRepository = new CommentFileRepository();

            // Pass to CLIAPP
            CliApp cliApp = new CliApp(userRepository, postRepository, commentRepository);
            await cliApp.RunAsync();
        }
    }
}


