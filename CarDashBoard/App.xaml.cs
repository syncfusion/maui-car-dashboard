namespace CarDashBoard
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
#if WINDOWS || MACCATALYST
            return new Window(new CarDashBoardDemo());
#else
            return new Window();
#endif
        }
    }
}