using CleanArchitectureProject.Domain.Common;
using CleanArchitectureProject.Domain.Entities;

namespace CleanArchitectureProject.Domain.Events;

public class AuthorCreatedEvent : BaseEvent
{
    public AuthorCreatedEvent(Author author)
    {
        Author = author;
    }

    public Author Author { get; }
}
