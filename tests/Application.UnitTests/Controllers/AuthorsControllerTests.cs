using NUnit.Framework;
using CleanArchitectureProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using CleanArchitectureProject.Infrastructure.Data;
using CleanArchitectureProject.Web.Endpoints;

namespace CleanArchitectureProject.Application.UnitTests.Controllers
{
    [TestFixture]
    public class AuthorsControllerTests
    {
        private DbContextOptions<ApplicationDbContext> _options;

        [SetUp]
        public void Setup()
        {
            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // unikalna baza na każdy test
                .Options;
        }

        [Test]
        public async Task GetAll_ReturnsListOfAuthors()
        {
            using (var context = new ApplicationDbContext(_options))
            {
                context.Authors.AddRange(
                    new Author { Name = "Author One" },
                    new Author { Name = "Author Two" }
                );
                context.SaveChanges();
            }

            using (var context = new ApplicationDbContext(_options))
            {
                var controller = new Authors(context);

                var result = await controller.GetAll();

                Assert.IsInstanceOf<ActionResult<IEnumerable<Author>>>(result);
                var value = result.Value as List<Author>;
                Assert.NotNull(value);
                Assert.That(value.Count, Is.EqualTo(2));
            }
        }

        [Test]
        public async Task GetById_ReturnsAuthor_WhenExists()
        {
            int authorId;
            using (var context = new ApplicationDbContext(_options))
            {
                var author = new Author { Name = "Test Author" };
                context.Authors.Add(author);
                context.SaveChanges();
                authorId = author.Id;
            }

            using (var context = new ApplicationDbContext(_options))
            {
                var controller = new Authors(context);
                var result = await controller.Get(authorId);

                Assert.IsInstanceOf<ActionResult<Author>>(result);
                Assert.NotNull(result.Value);
                Assert.That(result.Value.Id, Is.EqualTo(authorId));
            }
        }

        [Test]
        public async Task GetById_ReturnsNotFound_WhenNotExists()
        {
            using (var context = new ApplicationDbContext(_options))
            {
                var controller = new Authors(context);
                var result = await controller.Get(999);

                Assert.IsInstanceOf<NotFoundResult>(result.Result);
            }
        }

        [Test]
        public async Task Create_AddsAuthor()
        {
            using (var context = new ApplicationDbContext(_options))
            {
                var controller = new Authors(context);
                var author = new Author { Name = "New Author" };

                var result = await controller.Create(author);

                Assert.IsInstanceOf<CreatedAtActionResult>(result.Result);
                Assert.That(context.Authors.Count(), Is.EqualTo(1));
                Assert.That(context.Authors.First().Name, Is.EqualTo("New Author"));
            }
        }

        [Test]
        public async Task Update_ModifiesAuthor_WhenExists()
        {
            int authorId;
            using (var context = new ApplicationDbContext(_options))
            {
                var author = new Author { Name = "Old Name" };
                context.Authors.Add(author);
                context.SaveChanges();
                authorId = author.Id;
            }

            using (var context = new ApplicationDbContext(_options))
            {
                var controller = new Authors(context);
                var updatedAuthor = new Author { Id = authorId, Name = "Updated Name" };

                var result = await controller.Update(authorId, updatedAuthor);

                Assert.IsInstanceOf<NoContentResult>(result);

                var authorInDb = context.Authors.Find(authorId);
                if (authorInDb != null)
                {
                    Assert.That(authorInDb.Name, Is.EqualTo("Updated Name"));
                }
            }
        }

        [Test]
        public async Task Update_ReturnsBadRequest_WhenIdMismatch()
        {
            using (var context = new ApplicationDbContext(_options))
            {
                var controller = new Authors(context);
                var updatedAuthor = new Author { Id = 1, Name = "Test" };

                var result = await controller.Update(2, updatedAuthor);

                Assert.IsInstanceOf<BadRequestResult>(result);
            }
        }

        [Test]
        public async Task Delete_RemovesAuthor_WhenExists()
        {
            int authorId;
            using (var context = new ApplicationDbContext(_options))
            {
                var author = new Author { Name = "To Delete" };
                context.Authors.Add(author);
                context.SaveChanges();
                authorId = author.Id;
            }

            using (var context = new ApplicationDbContext(_options))
            {
                var controller = new Authors(context);

                var result = await controller.Delete(authorId);

                Assert.IsInstanceOf<NoContentResult>(result);
                Assert.That(context.Authors.Find(authorId), Is.Null);
            }
        }

        [Test]
        public async Task Delete_ReturnsNotFound_WhenNotExists()
        {
            using (var context = new ApplicationDbContext(_options))
            {
                var controller = new Authors(context);

                var result = await controller.Delete(999);

                Assert.IsInstanceOf<NotFoundResult>(result);
            }
        }
    }
}
