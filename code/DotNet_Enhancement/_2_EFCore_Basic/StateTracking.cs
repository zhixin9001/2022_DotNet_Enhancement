namespace _2_EFCore_Basic;

public class StateTracking
{
    public static void Entry()
    {
        using var ctx = new TestDbContext();
        var books = ctx.Books.Take(3).ToArray();
        var b1 = books[0];
        var b2 = books[1];
        var b3 = books[2];
        var b4 = new Book() {Title = "b4"};
        var b5 = new Book() {Title = "b5"};
        b1.Title = "b1-new";
        ctx.Remove(b3);
        ctx.Add(b4);
        var entry1 = ctx.Entry(b1);
        var entry2 = ctx.Entry(b2);
        var entry3 = ctx.Entry(b3);
        var entry4 = ctx.Entry(b4);
        var entry5 = ctx.Entry(b5);
        Console.WriteLine("b1:"+ entry1.State);
        Console.WriteLine("b1:"+ entry1.DebugView.LongView);
        Console.WriteLine("b2:"+ entry2.State);
        Console.WriteLine("b2:"+ entry2.DebugView.LongView);
        Console.WriteLine("b3:"+ entry3.State);
        Console.WriteLine("b3:"+ entry3.DebugView.LongView);
        Console.WriteLine("b4:"+ entry4.State);
        Console.WriteLine("b4:"+ entry4.DebugView.LongView);
        Console.WriteLine("b5:"+ entry5.State);
        Console.WriteLine("b5:"+ entry5.DebugView.LongView);
    }
}