using NUnit.Framework;
using CleanArchitectureProject.Domain.Entities;
using CleanArchitectureProject.Web.Endpoints;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using CleanArchitectureProject.Infrastructure.Data;

namespace CleanArchitectureProject.Application.UnitTests.Controllers
{
    [TestFixture]
    public class CategoriesControllerTests
    {
        private DbContextOptions<ApplicationDbContext> _options;

        [SetUp]
        public void Setup()
        {
            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "CategoriesTestDb")
                .Options;

            using var context = new ApplicationDbContext(_options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            context.Categories.AddRange(
                new Category { Id = 1, Name = "Category One" },
                new Category { Id = 2, Name = "Category Two" }
            );
            context.SaveChanges();
        }

        [Test]
        public async Task GetAll_ReturnsListOfCategories()
        {
            using var context = new ApplicationDbContext(_options);
            var controller = new Categories(context);

            var result = await controller.GetAll();

            Assert.IsInstanceOf<ActionResult<IEnumerable<Category>>>(result);
            var value = result.Value as List<Category>;
            Assert.NotNull(value);
            Assert.That(value.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task GetById_ReturnsCategory_WhenExists()
        {
            using var context = new ApplicationDbContext(_options);
            var controller = new Categories(context);

            var result = await controller.Get(1);

            Assert.IsInstanceOf<ActionResult<Category>>(result);
            Assert.NotNull(result.Value);
            Assert.That(result.Value.Id, Is.EqualTo(1));
        }

        [Test]
        public async Task GetById_ReturnsNotFound_WhenNotExists()
        {
            using var context = new ApplicationDbContext(_options);
            var controller = new Categories(context);

            var result = await controller.Get(999);

            Assert.IsInstanceOf<ActionResult<Category>>(result);
            Assert.IsInstanceOf<NotFoundResult>(result.Result);
        }

        [Test]
        public async Task Create_AddsCategory()
        {
            using var context = new ApplicationDbContext(_options);
            var controller = new Categories(context);

            var newCategory = new Category
            {
                Id = 3,
                Name = "Category Three"
            };

            var result = await controller.Create(newCategory);

            var createdAt = result.Result as CreatedAtActionResult;
            Assert.NotNull(createdAt);
            Assert.That(createdAt.StatusCode, Is.EqualTo(201));

            var createdCategory = createdAt.Value as Category;
            Assert.NotNull(createdCategory);
            Assert.That(createdCategory.Name, Is.EqualTo("Category Three"));
        }

        [Test]
        public async Task Update_UpdatesCategory_WhenExists()
        {
            using var context = new ApplicationDbContext(_options);
            var controller = new Categories(context);

            var updatedCategory = new Category
            {
                Id = 1,
                Name = "Updated Name"
            };

            var result = await controller.Update(1, updatedCategory);

            Assert.IsInstanceOf<NoContentResult>(result);

            var category = await context.Categories.FindAsync(1);
            if (category != null) 
            {
                Assert.That(category.Name, Is.EqualTo("Updated Name"));
            }
        }

        [Test]
        public async Task Update_ReturnsBadRequest_WhenIdMismatch()
        {
            using var context = new ApplicationDbContext(_options);
            var controller = new Categories(context);

            var updatedCategory = new Category
            {
                Id = 1,
                Name = "Updated Name"
            };

            var result = await controller.Update(2, updatedCategory);

            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public async Task Delete_RemovesCategory_WhenExists()
        {
            using var context = new ApplicationDbContext(_options);
            var controller = new Categories(context);

            var result = await controller.Delete(1);

            Assert.IsInstanceOf<NoContentResult>(result);

            var category = await context.Categories.FindAsync(1);
            Assert.Null(category);
        }

        [Test]
        public async Task Delete_ReturnsNotFound_WhenNotExists()
        {
            using var context = new ApplicationDbContext(_options);
            var controller = new Categories(context);

            var result = await controller.Delete(999);

            Assert.IsInstanceOf<NotFoundResult>(result);
        }
    }
}
