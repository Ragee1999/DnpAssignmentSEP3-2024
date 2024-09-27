using Entities;
using RepositoryContracts;

namespace CLI.UI
{
    public class ManageUserView
    {
        private readonly IUserRepository _userRepository;

        public ManageUserView(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task ShowMenuAsync()
        {
            Console.Clear();
            Console.WriteLine("=== Manage Users ===");
            Console.WriteLine("1. Create User");
            Console.WriteLine("2. List Users");
            Console.WriteLine("0. Back");

            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    await CreateUserAsync();
                    break;
                case "2":
                    await ListUsersAsync();  // Add this case for listing users
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Invalid choice, try again.");
                    break;
            }
        }

        private async Task CreateUserAsync()
        {
            Console.Write("Enter username: ");
            string username = Console.ReadLine();
            Console.Write("Enter password: ");
            string password = Console.ReadLine();

            User newUser = new User { UserName = username, Password = password };
            User createdUser = await _userRepository.AddAsync(newUser);

            Console.WriteLine($"User created with ID: {createdUser.Id}");
            Console.ReadKey();
        }

        private async Task ListUsersAsync()
        {
            var users = _userRepository.GetMany().ToList(); 

            if (users.Any())
            {
                Console.WriteLine("=== User List ===");
                foreach (var user in users)
                {
                    Console.WriteLine($"ID: {user.Id}, Username: {user.UserName}");
                }
            }
            else
            {
                Console.WriteLine("No users found.");
            }

            Console.ReadKey();  
        }
    }
}
