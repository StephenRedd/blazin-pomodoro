using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazinPomodoro.Server.Controllers;
using BlazinPomodoro.Server.Services;
using BlazinPomodoro.Server.Tests.Fixtures;
using BlazinPomodoro.Shared;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Xunit;


namespace BlazinPomodoro.Server.Tests
{
    public class TodosControllerShould : IClassFixture<TodoServiceFixture>
    {
        private TodoServiceFixture Fixture { get; }

        public TodosControllerShould(TodoServiceFixture fixture)
        {
            Fixture = fixture;
        }

        [Fact]
        [Trait("TestType", "Unit")]
        public void ReturnAllItemsOnGet()
        {
            var result = Fixture.TodosController.Get();
            result
                .Should().BeOfType<OkObjectResult>()
            .Which.Value
                .Should().BeAssignableTo<IEnumerable<TodoItem>>()
            .Which
                .Should().HaveCount(2);
        }

        [Fact]
        [Trait("TestType", "Unit")]
        public void Return404OnGetByIdWithBadId()
        {
            var result = Fixture.TodosController.Get(Guid.Empty);
            result
                .Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        [Trait("TestType", "Unit")]
        public void ReturnItemOnGetById()
        {
            var result = Fixture.TodosController.Get(Guid.Parse("3340556e-e9dd-46ba-8d44-3249d231bbf9"));
            result
                .Should().BeOfType<OkObjectResult>()
                .Which.Value
                .Should().BeAssignableTo<TodoItem>()
                .Which.Id.Should().Be("3340556e-e9dd-46ba-8d44-3249d231bbf9");
        }

        [Fact]
        [Trait("TestType", "Unit")]
        public void ReturnBadRequestOnDeleteWithBadId()
        {
            var result = Fixture.TodosController.Delete(Guid.Empty);
            result
                .Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        [Trait("TestType", "Unit")]
        public void ReturnNoContentOnDelete()
        {
            var result = Fixture.TodosController.Delete(Guid.Parse("3340556e-e9dd-46ba-8d44-3249d231bbf9"));
            result
                .Should().BeOfType<NoContentResult>();
        }

        [Fact]
        [Trait("TestType", "Unit")]
        public void ReturnCreatedOnPost()
        {
            var result = Fixture.TodosController.Post(Fixture.SuccessfulSaveItem);
            result
                .Should().BeOfType<CreatedResult>().Which.Location.Should().Be("/api/todos/fcbb1257-da5f-4503-8f9d-1bf0bbc762b5");
        }



        [Fact]
        [Trait("TestType", "Unit")]
        public void Return500OnPostForBadItem()
        {
            var result = Fixture.TodosController.Post(Fixture.FailSaveItem);
            result
                .Should().BeOfType<StatusCodeResult>().Which.StatusCode.Should().Be(500);
        }

        [Fact]
        [Trait("TestType", "Unit")]
        public void ReturnItemOnPut()
        {
            var result = Fixture.TodosController.Put(Fixture.SuccessfulSaveItem.Id, Fixture.SuccessfulSaveItem);
            result
                .Should().BeOfType<OkObjectResult>().Which.Value.Should().BeAssignableTo<TodoItem>()
                .Which.Id.Should().Be(Fixture.SuccessfulSaveItem.Id);
        }

        [Fact]
        [Trait("TestType", "Unit")]
        public void ReturnBadRequestOnPutWhenIdNotMatchItem()
        {
            var result = Fixture.TodosController.Put(Fixture.SuccessfulSaveItem.Id,Fixture.FailSaveItem);
            result
                .Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        [Trait("TestType", "Unit")]
        public void ReturnBadRequestOnPutItemNotExists()
        {
            var result = Fixture.TodosController.Put(Fixture.NotExistSaveItem.Id, Fixture.NotExistSaveItem);
            result
                .Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        [Trait("TestType", "Unit")]
        public void Return500OnPutForBadItem()
        {
            var result = Fixture.TodosController.Put(Fixture.FailSaveItem.Id, Fixture.FailSaveItem);
            result
                .Should().BeOfType<StatusCodeResult>().Which.StatusCode.Should().Be(500);
        }

    }
}