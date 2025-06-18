namespace Domain.Events;

public class AuthorUpdatedEvent : BaseEvent
{
    public AuthorUpdatedEvent(Author author)
    {
        Author = author;
    }

    public Author Author { get; }
}
