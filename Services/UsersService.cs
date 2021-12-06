using Microsoft.Extensions.Configuration;
using Okta.Sdk;
using Okta.Sdk.Configuration;
using OktaDemo.Models;
using System.Threading.Tasks;

namespace OktaDemo.Services
{
    public class UsersService
    {
        private readonly IConfiguration configuration;

        public UsersService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        //public async Task<IUser> RegisterUser(RegisterUserRequest model)
        //{
        //    var clientConfig = GetOktaClientConfig();
        //    var client = new OktaClient(clientConfig);

        //    var options = new CreateUserWithoutCredentialsOptions
        //    {
        //        Profile = new UserProfile
        //        {
        //            Login = model.Email,
        //            Email = model.Email,
        //            FirstName = model.FirstName,
        //            LastName = model.LastName,
        //        },
        //    };

        //    var result = await client.Users.CreateUserAsync(options);

        //    return result;
        //}

        public async Task<IUser> GetUserAsync(string userId)
        {
            var clientConfig = GetOktaClientConfig();
            var client = new OktaClient(clientConfig);

            var user = await client.Users.GetUserAsync(userId);

            return user;
        }

        public async Task<IUser> UpdateUser(UpdateUserRequest model)
        {
            var clientConfig = GetOktaClientConfig();
            var client = new OktaClient(clientConfig);

            var user = await client.Users.GetUserAsync(model.Email);
            user.Profile.FirstName = model.FirstName;
            user.Profile.LastName = model.LastName;

            var result = await client.Users.UpdateUserAsync(user, user.Id, true);
            return result;
        }

        private OktaClientConfiguration GetOktaClientConfig()
        {
            var clientConfig = new OktaClientConfiguration
            {
                Token = configuration["Okta:ApiToken"],
                ClientId = configuration["Okta:ClientId"],
                OktaDomain = configuration["Okta:Domain"],
            };
            return clientConfig;
        }
    }
}
