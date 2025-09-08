namespace EventApp.Frontend.Services.LoadingServ
{
    public class LoadingService
    {
        public event Action? OnChange;

        private bool _isLoading;
        private string _message = "Processing...";

        public bool IsLoading => _isLoading;
        public string Message => _message;

        public void Show(string message = "Processing...")
        {
            _message = message;
            _isLoading = true;
            OnChange?.Invoke();
        }

        public void Hide()
        {
            _isLoading = false;
            OnChange?.Invoke();
        }
    }
}
