using Dungeon.Core;

namespace Dungeon.Screen;

public interface IScreen 
{
    public void Input(Renderer renderer);
    public void Render(Renderer renderer);
}