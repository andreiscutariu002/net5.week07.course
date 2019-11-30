using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqAndLamdaExpressions.Models
{
    public class UserPosts
    {
        private User user;
        private List<Post> posts;


        public UserPosts()
        {
            this.Posts = new List<Post>();
        }

        public User User { get => user; set => user = value; }
        public List<Post> Posts { get => posts; set => posts = value; }
    }
}
