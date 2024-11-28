namespace SQLlite
{
    public partial class App : Application
    {
        public static CategoryRepository CategoryRepo { get; private set; }
        public App(CategoryRepository repo)
        {
            InitializeComponent();

            MainPage = new AppShell();

            CategoryRepo = repo;
        }
    }
}
