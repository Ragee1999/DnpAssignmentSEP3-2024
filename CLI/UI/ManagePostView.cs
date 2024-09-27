using Entities;
using RepositoryContracts;

namespace CLI.UI
{
    public class ManagePostView
    {
        private readonly IPostRepository _postRepository;
        private readonly IUserRepository _userRepository;

        public ManagePostView(IPostRepository postRepository, IUserRepository userRepository)
        {
            _postRepository = postRepository;
            _userRepository = userRepository;
        }

        public async Task ShowMenuAsync()
        {
            Console.Clear();
            Console.WriteLine("=== Manage Posts ===");
            Console.WriteLine("1. Create Post");
            Console.WriteLine("2. List Posts");
            Console.WriteLine("0. Back");

            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    await CreatePostAsync();
                    break;
                case "2":
                    await ListPostsAsync();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Invalid choice, try again.");
                    break;
            }
        }

        private async Task CreatePostAsync()
        {
            Console.Write("Enter title: ");
            string title = Console.ReadLine();
            Console.Write("Enter body: ");
            string body = Console.ReadLine();
            Console.Write("Enter user ID: ");
            int userId = int.Parse(Console.ReadLine());

            Post newPost = new Post { Title = title, Body = body, UserId = userId };
            Post createdPost = await _postRepository.AddAsync(newPost);

            Console.WriteLine($"Post created with ID: {createdPost.Id}");
            Console.ReadKey();
        }

        private async Task ListPostsAsync()
        {
            var posts = _postRepository.GetMany();
            Console.WriteLine("=== Posts ===");
            foreach (var post in posts)
            {
                Console.WriteLine($"ID: {post.Id}, Title: {post.Title}");
            }
            Console.ReadKey();
        }
    }
}
