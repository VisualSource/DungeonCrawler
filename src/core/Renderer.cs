namespace Dungeon.Core;
public class Renderer {
    private List<List<char>> buffer = new List<List<char>> { };

    public bool IsDirty { get; set; } = true;

    public Renderer(string title){
        Console.CursorVisible = false;
        Console.Title = title;
        Console.OutputEncoding = System.Text.Encoding.Unicode;

        // Border
        for (int y = 0; y < 25; y++)
        {
            var row = new List<char> {};

            for (int x = 0; x < 52; x++)
            {
                char item = GetBorderChar(x,y);
                row.Add(item);
            }
            buffer.Add(row);
        }
    }

    public void WriteAt(string content, int left, int top) {
        Console.SetCursorPosition(left,top);
        Console.Write(content);
    }

    public void WriteDebug(string content){
        Console.SetCursorPosition(0,25);
        Console.Write("                                                     ");
        Console.SetCursorPosition(0,25);
        Console.Write("DEBUG: {0}",content);
    }

    /// <summary>
    ///     Write a string to the buffer. is not erased on render.
    /// </summary>
    /// <param name="content"></param>
    /// <param name="left"></param>
    /// <param name="top"></param>
    public void WriteBuffer(string content, int left , int top){
        if(content.Length > 52) throw new Exception($"Content length is larger then viewport. Length({content.Length})");


        for (int i = 0; i < content.Length; i++)
        {
            buffer[top][left + i] = content[i];
        }

        IsDirty = true;
    }

    public void WriteVer(string content, int left, int top){
        if(top - 1 > 0){
          Console.SetCursorPosition(0,top - 1);
          Console.Write(new string(buffer[top - 1].ToArray()));
        }

        WriteHor(content,left,top);

        if(top + 1 < 51){
            Console.SetCursorPosition(0,top + 1);
            Console.Write(new string(buffer[top + 1].ToArray()));
        }
    }

    /// <summary>
    /// Write a string to the screen this content is erased on render
    /// </summary>
    /// <param name="content"></param>
    /// <param name="left"></param>
    /// <param name="top"></param>
    public void WriteHor(string content, int left, int top){
        string line = "";

        for (int i = 0; i < buffer[top].Count; i++)
        {
            if(i >= left && i < left + content.Length){

                line += content[i - left];

                continue;
            }

            line += buffer[top][i];
        }

        Console.SetCursorPosition(0,top);
        Console.Write(line);
    }

    /// <summary>
    /// https://en.wikipedia.org/wiki/Box-drawing_character
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    private char GetBorderChar(int x, int y) {
        if(x == 0 && y == 0){
            return '╔';
        }
        if(x == 51 && y == 0) {
            return '╗';
        }
        if(x == 0 && y == 4) {
            return '╠';
        }
         if(x == 51 && y == 4) {
            return '╣';
        }
        if(x == 0 && y == 24){
            return '╚';
        }
        if(x == 51 && y == 24){
            return '╝';
        }
        if(x == 0 || x == 51) {
            return '║';
        }

        if(x >= 1 && x <= 51 && y == 4 || y == 0 || y == 24){
            return '=';
        }

        return ' ';
    }
    public void Render()
    {   
        Console.Clear();

        foreach (var col in buffer)
        {
            foreach (var row in col)
            {
                Console.Write(row);
            }
            Console.Write("\n");
        }

        IsDirty = false;
    }
}