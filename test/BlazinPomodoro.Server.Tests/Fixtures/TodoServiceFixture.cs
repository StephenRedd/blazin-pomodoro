using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazinPomodoro.Server.Controllers;
using BlazinPomodoro.Server.Services;
using BlazinPomodoro.Shared;
using NSubstitute;

namespace BlazinPomodoro.Server.Tests.Fixtures
{
    public class TodoServiceFixture
    {
        private ITodoService _substitueTodoService;
        public ITodoService SubstitueTodoService
        {
            get
            {
                if (_substitueTodoService == null)
                {
                    _substitueTodoService = Substitute.For<ITodoService>();
                    _substitueTodoService.GetAllItems().ReturnsForAnyArgs(new[]
                        {
                            new TodoItem
                            {
                                Id =  Guid.Parse("b2e9ffd8-a9ec-436d-834d-722c4e2f0944"),
                                Title = "Generated item",
                                CompletedOn = null,
                                CreatedOn = DateTimeOffset.Now.AddHours(-2)
                            },
                            new TodoItem
                            {
                                Id =  Guid.Parse("4daa58a2-d223-44b5-a8cd-a9c9eeb17ce7"),
                                Title = "Generated completed item",
                                CompletedOn = DateTimeOffset.Now.AddHours(-1),
                                CreatedOn = DateTimeOffset.Now.AddHours(-2)
                            }
                        }
                    );
                    _substitueTodoService.GetItem(Guid.Empty)
                        .Returns((TodoItem)null);
                    _substitueTodoService.GetItem(Guid.Parse("3340556e-e9dd-46ba-8d44-3249d231bbf9"))
                        .Returns(t => new TodoItem
                        {
                            Id = Guid.Parse("3340556e-e9dd-46ba-8d44-3249d231bbf9"),
                            Title = "Generated completed item",
                            CompletedOn = DateTimeOffset.Now.AddHours(-1),
                            CreatedOn = DateTimeOffset.Now.AddHours(-2)
                        });

                    _substitueTodoService.GetItem(SuccessfulSaveItem.Id)
                        .Returns(t => SuccessfulSaveItem);

                    _substitueTodoService.GetItem(FailSaveItem.Id)
                        .Returns(t => FailSaveItem);

                    _substitueTodoService.GetItem(NotExistSaveItem.Id)
                        .Returns(t => null);

                    _substitueTodoService.GetItem(Guid.Parse("3340556e-e9dd-46ba-8d44-3249d231bbf9"))
                        .Returns(t => new TodoItem
                        {
                            Id = Guid.Parse("3340556e-e9dd-46ba-8d44-3249d231bbf9"),
                            Title = "Generated completed item",
                            CompletedOn = DateTimeOffset.Now.AddHours(-1),
                            CreatedOn = DateTimeOffset.Now.AddHours(-2)
                        });


                    _substitueTodoService.DeleteItem(Guid.Empty)
                        .Returns(false);
                    _substitueTodoService.DeleteItem(Guid.Parse("3340556e-e9dd-46ba-8d44-3249d231bbf9"))
                        .Returns(true);
                    _substitueTodoService.SaveItem(Arg.Is<TodoItem>(i => i.Title != "success")).Returns(false);
                    _substitueTodoService.SaveItem(Arg.Is<TodoItem>(i => i.Title == "success")).Returns(true);


                }

                return _substitueTodoService;
            }
        }

        public TodoItem SuccessfulSaveItem { get; } = new TodoItem
        {
            Id = Guid.Parse("fcbb1257-da5f-4503-8f9d-1bf0bbc762b5"),
            Title = "success",
            CompletedOn = DateTimeOffset.Now.AddHours(-1),
            CreatedOn = DateTimeOffset.Now.AddHours(-2)
        };

        public TodoItem FailSaveItem { get; } = new TodoItem
        {
            Id = Guid.Parse("e233c503-af7a-46de-bef9-835d182296c9"),
            Title = "fail",
            CompletedOn = DateTimeOffset.Now.AddHours(-1),
            CreatedOn = DateTimeOffset.Now.AddHours(-2)
        };

        public TodoItem NotExistSaveItem { get; } = new TodoItem
        {
            Id = Guid.Parse("a48571a4-61c3-4dac-bc52-f08a32d541cc"),
            Title = "not exist",
            CompletedOn = DateTimeOffset.Now.AddHours(-1),
            CreatedOn = DateTimeOffset.Now.AddHours(-2)
        };

        public TodosController TodosController => new TodosController(SubstitueTodoService);
    }
}
