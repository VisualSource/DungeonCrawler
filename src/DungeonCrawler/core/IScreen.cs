namespace Dungeon.Core;
public interface ScreenInterface
{
    void Update(Renderer renderer);
    void Input(ConsoleKeyInfo input, Renderer renderer);

    void Init(Renderer renderer);
}