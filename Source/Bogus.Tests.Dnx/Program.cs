using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus;

namespace Bogus.Tests.Dnx
{
    public class Program
    {
        public static void Main(string[] args)
        {

                var fakeUser = new Faker<User>("en")
                    .RuleFor(u => u.FirstName, f => f.Name.FirstName())
                    .RuleFor(u => u.LastName, f => f.Name.LastName());


                var users = fakeUser.Generate(4);

                foreach (var user in users)
                {
                    Console.WriteLine($"{user.FirstName} {user.LastName}");
                }



            Console.ReadLine();


        }

        public class User
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }
    }
}
