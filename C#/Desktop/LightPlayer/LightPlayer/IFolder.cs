namespace LightPlayer
{
    public interface IFolder
    {
        bool IsValid { get; }

        string Path { get; }
    }
}
