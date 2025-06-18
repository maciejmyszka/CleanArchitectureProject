namespace Domain.Events;

public class AuthorDeletedEvent : BaseEvent
{
    public AuthorDeletedEvent(Author author)
    {
        Author = author;
    }

    public Author Author { get; }
}
