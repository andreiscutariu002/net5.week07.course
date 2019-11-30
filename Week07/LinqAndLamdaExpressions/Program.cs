namespace LinqAndLamdaExpressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Models;

    internal class Program
    {
        private const string ConsoleSeparatorLine = "-----------------------------------------------------------------------";

        private static void Main(string[] args)
        {

            List<User> allUsers = ReadUsers("users.json");
            List<Post> allPosts = ReadPosts("posts.json");

            #region Demo

            // 1 - find all users having email ending with ".net".
            var users1 = from user in allUsers
                         where user.Email.EndsWith(".net")
                         select user;

            var users11 = allUsers.Where(user => user.Email.EndsWith(".net"));

            IEnumerable<string> userNames = from user in allUsers
                                            select user.Name;

            var userNames2 = allUsers.Select(user => user.Name);

            foreach (var value in userNames2)
            {
                Console.WriteLine(value);
            }

            IEnumerable<Company> allCompanies = from user in allUsers
                                                select user.Company;

            var users2 = from user in allUsers
                         orderby user.Email
                         select user;

            var netUser = allUsers.First(user => user.Email.Contains(".net"));
            Console.WriteLine(netUser.Username);

            #endregion

            // 2 - find all posts for users having email ending with ".net".
            IEnumerable<int> usersIdsWithDotNetMails = from user in allUsers
                                                       where user.Email.EndsWith(".net")
                                                       select user.Id;

            IEnumerable<Post> posts = from post in allPosts
                                      where usersIdsWithDotNetMails.Contains(post.UserId)
                                      select post;

            foreach (var post in posts)
            {
                Console.WriteLine(post.Id + " " + "user: " + post.UserId);
            }

            // 3 - print number of posts for each user.
            var countByUser = (from post in allPosts
                               group post by post.UserId into g
                               select new { count = g.Count(), userid = g.Key });

            Console.WriteLine(ConsoleSeparatorLine);
            Console.WriteLine("Number of posts by user id");
            Console.WriteLine(ConsoleSeparatorLine);
            foreach (var group in countByUser)
            {
                Console.WriteLine($"user id {group.userid} has {group.count} posts.");
            }





            // 4 - find all users that have lat and long negative.
            IEnumerable<User> allUsersWithAddress = allUsers.Where(user => user.Address != null).ToList();
            IEnumerable<User> allUsersWithAddressAndGeo = allUsersWithAddress.Where(user => user.Address.Geo != null).ToList();

            IEnumerable<User> usersGeoNegative = (from b in allUsersWithAddressAndGeo
                                                  where b.Address.Geo.Lat < 0 && b.Address.Geo.Lng < 0
                                                  select b).ToList();
            Console.WriteLine(ConsoleSeparatorLine);
            Console.WriteLine("Users with both latitude and longitude negative");
            Console.WriteLine(ConsoleSeparatorLine);

            foreach (User tmp in usersGeoNegative)
            {
                Console.WriteLine($"user id {tmp.Username} has lat and long negative.");
            }

            // 5 - find the post with longest body.
            var longestPost = (from post in allPosts
                               orderby post.Body.Length descending
                               select new { post.Body.Length, post.Title, post.Body, post.UserId }).ToList().First();

            Console.WriteLine(ConsoleSeparatorLine);
            Console.WriteLine($"Found the longest post length of {longestPost.Length} with title '{longestPost.Title}' and post body '{longestPost.Body}'");



            // 6 - print the name of the employee that have post with longest body.
            var userLongestPost = allUsers.Where(user => user.Id.Equals(longestPost.UserId)).First();

            Console.WriteLine(ConsoleSeparatorLine);
            Console.WriteLine($"The name of the employee with the longest post is '{userLongestPost.Name}'");


            // 7 - select all addresses in a new List<Address>. print the list.
            List<Address> allAddresses = (from user in allUsersWithAddress
                                          select user.Address).ToList();

            foreach (Address address in allAddresses)
            {
                Console.WriteLine(ConsoleSeparatorLine);
                Console.WriteLine($"Address details:");
                Console.WriteLine($"City {address.City}, Zip code {address.Zipcode}");
                if (address.Geo != null)
                {
                    Console.WriteLine($"Geo location: Latitude: {address.Geo.Lat}, Longitude: {address.Geo.Lng}");
                }

            }


            // 8 - print the user with min lat
            var userWithMinLat = (from user in allUsersWithAddressAndGeo
                                  orderby user.Address.Geo.Lat ascending
                                  select user).First();
            Console.WriteLine(ConsoleSeparatorLine);
            Console.WriteLine($"User with minimum geo latitude ({userWithMinLat.Address.Geo.Lat}): user id: {userWithMinLat.Id}, user name: {userWithMinLat.Name}");



            // 9 - print the user with max long
            var userWithMaxLng = (from user in allUsersWithAddressAndGeo
                                  orderby user.Address.Geo.Lng descending
                                  select user).First();
            Console.WriteLine(ConsoleSeparatorLine);
            Console.WriteLine($"User with maximum geo longitude ({userWithMaxLng.Address.Geo.Lng}): user id: {userWithMaxLng.Id}, user name: {userWithMaxLng.Name}");


            // 10 - create a new class: public class UserPosts { public User User {get; set}; public List<Post> Posts {get; set} }
            //    - create a new list: List<UserPosts>
            //    - insert in this list each user with his posts only
            List<UserPosts> allUserPostsList = (from user in allUsers
                                                join post in allPosts on user.Id equals post.UserId into postsGroup
                                                select new UserPosts { User = user as User, Posts = postsGroup.ToList() as List<Post> }).ToList();





            // 11 - order users by zip code
            IEnumerable<User> usersOrderedByZipCode = (from user in allUsersWithAddress
                                                       orderby user.Address.Zipcode ascending
                                                       select user).ToList();


            // 12 - order users by number of posts
            IEnumerable<User> usersOrderedByNumPosts = (from user in allUsers
                                                        join numPosts in countByUser on user.Id equals numPosts.userid
                                                        orderby numPosts.count descending
                                                        select user).ToList();

            Console.ReadKey();
        }

        private static List<Post> ReadPosts(string file)
        {
            return ReadData.ReadFrom<Post>(file);
        }

        private static List<User> ReadUsers(string file)
        {
            return ReadData.ReadFrom<User>(file);
        }
    }
}
