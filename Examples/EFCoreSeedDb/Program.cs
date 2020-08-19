using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using Bogus.Extensions;
using Microsoft.EntityFrameworkCore;
using static System.Console;

namespace EFCoreSeedDb
{
   class Program
   {
      static void Main(string[] args)
      {
         WriteLine("Bogus Blog Example!");

         using var ctx = new BloggingContext();

         var blogs = ctx.Blogs
            .Include(b => b.Posts)
            .ToList();

         foreach( var blog in blogs)
         {
            WriteLine($"Blog Id: {blog.BlogId}");
            WriteLine($"Blog Url: {blog.Url}");
            foreach( var post in blog.Posts )
            {
               WriteLine($"  Post Id: {post.PostId}");
               WriteLine($"       Title: {post.Title}");
               WriteLine($"       Content: {post.Content}");
            }

            WriteLine();
            WriteLine();
         }
      }

   }

   public class BloggingContext : DbContext
   {
      public DbSet<Blog> Blogs { get; set; }
      public DbSet<Post> Posts { get; set; }

      protected override void OnConfiguring(DbContextOptionsBuilder options)
      {
         options.UseSqlite("Data Source=blogging.db");
         options.EnableSensitiveDataLogging();
      }

      protected override void OnModelCreating(ModelBuilder modelBuilder)
      {
         base.OnModelCreating(modelBuilder);

         FakeData.Init(10);

         modelBuilder.Entity<Blog>().HasData(FakeData.Blogs);
         modelBuilder.Entity<Post>().HasData(FakeData.Posts);
      }

   }

   /// <summary>
   /// Example uses Faker[T]
   /// </summary>
   public static class FakeData
   {
      public static List<Blog> Blogs = new List<Blog>();
      public static List<Post> Posts = new List<Post>();

      public static void Init(int count)
      {
         var postId = 1;
         var postFaker = new Faker<Post>()
            .RuleFor(p => p.PostId, _ => postId++)
            .RuleFor(p => p.Title, f => f.Hacker.Phrase())
            .RuleFor(p => p.Content, f => f.Lorem.Sentence());

         var blogId = 1;
         var blogFaker = new Faker<Blog>()
            .RuleFor(b => b.BlogId, _ => blogId++)
            .RuleFor(b => b.Url, f => f.Internet.Url())
            .RuleFor(b => b.Posts, (f, b) =>
               {
                  postFaker.RuleFor(p => p.BlogId, _ => b.BlogId);

                  var posts = postFaker.GenerateBetween(3, 5);

                  FakeData.Posts.AddRange(posts);

                  return null; // Blog.Posts is a getter only. The return value has no impact.
               });

         var blogs = blogFaker.Generate(count);

         FakeData.Blogs.AddRange(blogs);
      }
   }

   /// <summary>
   /// Example uses Faker facade
   /// </summary>
   public static class FakeData2
   {
      public static int BlogId = 1;
      public static List<Blog> Blogs = new List<Blog>();

      public static int PostId = 1;
      public static List<Post> Posts = new List<Post>();

      private static Faker f;

      public static void Init(int count)
      {
         f = new Faker();

         GenerateBlogs(count);
      }

      private static void GenerateBlogs(int blogCount)
      {
         for( var i = 0; i < blogCount; i++, BlogId++ )
         {
            var blog = new Blog
               {
                  BlogId = BlogId,
                  Url = f.Internet.Url()
               };

            Blogs.Add(blog);

            var postCount = f.Random.Number(3, 5);
            GeneratePost(blog, postCount);
         };
      }

      private static void GeneratePost(Blog b, int postCount)
      {
         for( var i = 0; i < postCount; i++, PostId++ )
         {
            var post = new Post
               {
                  PostId = PostId,
                  BlogId = b.BlogId,
                  Title = f.Hacker.Phrase(),
                  Content = f.Lorem.Sentence()
               };

            Posts.Add(post);
         }
      }
   }

   public class Blog
   {
      public int BlogId { get; set; }
      public string Url { get; set; }

      public List<Post> Posts { get; } = new List<Post>();
   }

   public class Post
   {
      public int PostId { get; set; }
      public string Title { get; set; }
      public string Content { get; set; }

      public int BlogId { get; set; }
      public Blog Blog { get; set; }
   }
}
