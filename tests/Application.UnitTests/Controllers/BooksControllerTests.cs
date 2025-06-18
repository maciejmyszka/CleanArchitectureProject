using NUnit.Framework;
using CleanArchitectureProject.Domain.Entities;
using CleanArchitectureProject.Web.Endpoints;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using CleanArchitectureProject.Infrastructure.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CleanArchitectureProject.Application.UnitTests.Controllers
{
    [TestFixture]
    public class BooksControllerTests
    {
        private DbContextOptions<ApplicationDbContext> _options;

        [SetUp]
        public void Setup()
        {
            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "BooksTestDb")
                .Options;

            // Wyczyść bazę przed każdym testem
            using (var context = new ApplicationDbContext(_options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                var author = new Author { Id = 1, Name = "Author One" };
                var category = new Category { Id = 1, Name = "Category One" };

                context.Authors.Add(author);
                context.Categories.Add(category);

                context.Books.AddRange(
                    new Book { Id = 1, Title = "Book One", Author = author, Category = category },
                    new Book { Id = 2, Title = "Book Two", Author = author, Category = category }
                );

                context.SaveChanges();
            }
        }

        [Test]
        public async Task GetAll_ReturnsListOfBooks()
        {
            using var context = new ApplicationDbContext(_options);
            var controller = new Books(context);

            var result = await controller.GetAll();

            Assert.IsInstanceOf<ActionResult<IEnumerable<Book>>>(result);
            var value = result.Value as List<Book>;
            Assert.NotNull(value);
            Assert.That(value.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task GetById_ReturnsBook_WhenExists()
        {
            using var context = new ApplicationDbContext(_options);
            var controller = new Books(context);

            var result = await controller.Get(1);

            Assert.IsInstanceOf<ActionResult<Book>>(result);
            Assert.NotNull(result.Value);
            Assert.That(result.Value.Id, Is.EqualTo(1));
        }

        [Test]
        public async Task GetById_ReturnsNotFound_WhenDoesNotExist()
        {
            using var context = new ApplicationDbContext(_options);
            var controller = new Books(context);

            var result = await controller.Get(999);

            Assert.IsInstanceOf<ActionResult<Book>>(result);
            Assert.IsInstanceOf<NotFoundResult>(result.Result);
        }

        [Test]
        public async Task Create_AddsBook()
        {
            using var context = new ApplicationDbContext(_options);
            var controller = new Books(context);

            var newBook = new Book
            {
                Id = 3,
                Title = "Book Three",
                AuthorId = 1,
                CategoryId = 1
            };

            var result = await controller.Create(newBook);

            var createdAt = result.Result as CreatedAtActionResult;
            Assert.NotNull(createdAt);
            Assert.That(createdAt.StatusCode, Is.EqualTo(201));

            var createdBook = createdAt.Value as Book;
            Assert.NotNull(createdBook);
            Assert.That(createdBook.Title, Is.EqualTo("Book Three"));
        }

        [Test]
        public async Task Update_UpdatesBook_WhenExists()
        {
            using var context = new ApplicationDbContext(_options);
            var controller = new Books(context);

            var updatedBook = new Book
            {
                Id = 1,
                Title = "Updated Title",
                AuthorId = 1,
                CategoryId = 1
            };

            var result = await controller.Update(1, updatedBook);

            Assert.IsInstanceOf<NoContentResult>(result);

            var book = await context.Books.FindAsync(1);
            if (book != null)
            {
                Assert.That(book.Title, Is.EqualTo("Updated Title"));
            }
        }

        [Test]
        public async Task Update_ReturnsBadRequest_WhenIdMismatch()
        {
            using var context = new ApplicationDbContext(_options);
            var controller = new Books(context);

            var updatedBook = new Book
            {
                Id = 1,
                Title = "Updated Title",
                AuthorId = 1,
                CategoryId = 1
            };

            var result = await controller.Update(2, updatedBook);

            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public async Task Delete_RemovesBook_WhenExists()
        {
            using var context = new ApplicationDbContext(_options);
            var controller = new Books(context);

            var result = await controller.Delete(1);

            Assert.IsInstanceOf<NoContentResult>(result);

            var book = await context.Books.FindAsync(1);
            Assert.Null(book);
        }

        [Test]
        public async Task Delete_ReturnsNotFound_WhenDoesNotExist()
        {
            using var context = new ApplicationDbContext(_options);
            var controller = new Books(context);

            var result = await controller.Delete(999);

            Assert.IsInstanceOf<NotFoundResult>(result);
        }
    }
}
