namespace Core.Mvvm
{
    public interface IWindowController
    {
        void Minimize();

        void Maximize();

        void Show();

        void Hide();

        bool Activate();
    }
}