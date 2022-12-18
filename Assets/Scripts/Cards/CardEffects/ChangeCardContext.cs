public class ChangeCardContext
{
    private readonly Card _before;
    private readonly Card _after;

    public ChangeCardContext(Card before, Card after)
    {
        _before = before;
        _after = after;
    }

    public ICard Before => _before;
    public ICard After => _after;
}