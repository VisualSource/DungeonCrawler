using Dungeon.Core;

namespace Dungeon.Screen;

public interface IScreen
{
    bool NeedsInit { get; set; }
    Game Context { get; set; }
    public void Init();
    public void Input();
    public void Render();
}