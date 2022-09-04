namespace TodoApp.Domain;

public static class Errors
{
    public static class Todos
    {
        public static readonly Error TodoNotFound = new Error(nameof(TodoNotFound), "Todo not found", string.Empty);

        public static readonly Error CommentNotFound = new Error(nameof(CommentNotFound), "Comment not found", string.Empty);
    }
}
