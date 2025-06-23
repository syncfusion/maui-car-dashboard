using Syncfusion.Maui.Gauges;

namespace CarDashBoard
{
    public class CarDashBoardBehavior : Behavior<ContentPage>
    {
        /// <summary>
        /// Declaration of radial axis control.
        /// </summary>
        private RadialAxis? radialAxis;

        /// <summary>
        /// Declaration of temparature indicator needle.
        /// </summary>
        private NeedlePointer? needlePointer;

        /// <summary>
        /// Declaration of rpm indicator needle.
        /// </summary>
        private NeedlePointer? rpmPointer;

        /// <summary>
        /// Declaration of speed indicator needle.
        /// </summary>
        private NeedlePointer? speedPointer;

        /// <summary>
        /// Declaration of fuel indicator needle.
        /// </summary>
        private NeedlePointer? fuelPointer;

        /// <summary>
        /// Declaration of Distance digital Gauge.
        /// </summary>
        private SfDigitalGauge? distanceGauge;

        /// <summary>
        /// Declaration of label which denotes current hour.
        /// </summary>
        private Span? currentHourLabel;

        /// <summary>
        /// Declaration of label which denotes current minute and .
        /// </summary>
        private Span? currentMinuteMeridianLabel;

        /// <summary>
        /// Declaration of label which denotes separator between current hour and current minute.
        /// </summary>
        private Span? separatorLabel;

        /// <summary>
        /// Declaration of timer to update the current time.
        /// </summary>
        private IDispatcherTimer? timer;

        /// <summary>
        /// Declaration timer update gauge animation.
        /// </summary>
        private IDispatcherTimer? needleTimer;

        /// <summary>
        /// Indicates whether the object is currently in a blinking state.
        /// </summary>
        private bool isBlinking = false;

        /// <summary>
        /// Represents the current content page being displayed.
        /// </summary>
        private ContentPage? contentPage;

        /// <summary>
        /// To Check Animation Completion.
        /// </summary>
        private bool isLoaded = false;

        /// <summary>
        /// Begins when the behavior attached to the view.
        /// </summary>
        /// <param name="bindable"></param>

        protected override void OnAttachedTo(ContentPage bindable)
        {
            base.OnAttachedTo(bindable);

            // Initialize variable.
            this.radialAxis = bindable.FindByName<RadialAxis>("temperatureAxis");
            this.currentHourLabel = bindable.FindByName<Span>("currentHour");
            this.currentMinuteMeridianLabel = bindable.FindByName<Span>("currentMinuteandMeridian");
            this.separatorLabel = bindable.FindByName<Span>("separator");
            this.needlePointer = bindable.FindByName<NeedlePointer>("temperatureNeedle");
            this.rpmPointer = bindable.FindByName<NeedlePointer>("rpmNeedle");
            this.speedPointer = bindable.FindByName<NeedlePointer>("speedNeedle");
            this.fuelPointer = bindable.FindByName<NeedlePointer>("fuelNeedle");
            this.distanceGauge = bindable.FindByName<SfDigitalGauge>("digitalGauge");
            this.contentPage = bindable;

            // Setting default appearance.
            this.currentHourLabel.Text = DateTime.Now.ToString("hh");
            this.currentMinuteMeridianLabel.Text = DateTime.Now.ToString("mm tt");

            // Started a timer to update the current time. This will help to update the current time in the UI.
            this.timer = bindable.Dispatcher.CreateTimer();
            this.timer.Interval = TimeSpan.FromSeconds(1);
            this.timer.Tick += Timer_Tick;
            this.timer.Start();

            // Started a timer to add animation to gauges.
            this.needleTimer = bindable.Dispatcher.CreateTimer();
            this.needleTimer.Interval = TimeSpan.FromSeconds(2);
            this.needleTimer.Tick += NeedleTimer_Tick;
            this.needleTimer.Start();

            // Wire Required events.
            this.radialAxis.LabelCreated += RadialAxis_LabelCreated;
            this.contentPage.Appearing += ContentPage_Appearing;
            this.contentPage.Disappearing += ContentPage_Disappearing;

        }

        /// <summary>
        /// It is used to add animtaion to time label.
        /// </summary>
        /// <param name="sender">represents content page</param>
        /// <param name="e">represents dispatcher event args</param>
        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (this.currentHourLabel != null)
            {
                this.currentHourLabel.Text = DateTime.Now.ToString("hh");
            }

            if (this.currentMinuteMeridianLabel != null && this.separatorLabel != null)
            {
                this.currentMinuteMeridianLabel.Text = DateTime.Now.ToString("mm tt");

                if (this.isBlinking)
                {
                    this.separatorLabel.TextColor = Colors.Transparent;
                }
                else
                {
                    this.separatorLabel.TextColor = this.currentMinuteMeridianLabel.TextColor;
                }
            }
            this.isBlinking = !this.isBlinking;
        }

        /// <summary>
        /// It is used to add animtaion to needles and gauge in the sample.
        /// </summary>
        /// <param name="sender">represents content page</param>
        /// <param name="e">represents dispatcher event args</param>
        private void NeedleTimer_Tick(object? sender, EventArgs e)
        {
            if (this.isLoaded)
            {
                // Behavior for temperature needle.
                if (this.needlePointer?.Value >= 125)
                {
                    this.needlePointer.Value = 40;
                }
                else
                {
                    if (this.needlePointer != null)
                    {
                        this.needlePointer.Value += 25;
                    }
                }

                // Behavior for rpm needle.
                if (this.rpmPointer?.Value >= 4.8)
                {
                    this.rpmPointer.Value = 0.8;
                }
                else
                {
                    if (this.rpmPointer != null)
                    {
                        this.rpmPointer.Value += 0.8;
                    }
                }

                // Behavior for speed needle.
                if (this.speedPointer?.Value >= 200)
                {
                    this.speedPointer.Value = 60;
                }
                else
                {
                    if (this.speedPointer != null)
                    {
                        this.speedPointer.Value += 20;
                    }
                }

                // Behavior for fuel needle.
                if (this.fuelPointer?.Value <= 5.4)
                {
                    this.fuelPointer.Value += 0.2;
                    
                }

                // Behavior for Distance Gauge.
                if (this.distanceGauge?.Text.Length > 5)
                {
                    this.distanceGauge.Text = "00000";
                }
                else
                {
                    if (this.distanceGauge != null)
                    {
                        this.distanceGauge.Text = (Convert.ToInt32(this.distanceGauge.Text) + 1).ToString();
                    }
                }
            }
            else
            {
                // Added for Loading Delay.
                isLoaded = true;
            }
        }


        /// <summary>
        /// Reset the minimum width and height of the window when the page disappears.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContentPage_Disappearing(object? sender, EventArgs e)
        {
            if (App.Current?.Windows.Count == 0 || App.Current?.Windows[0] == null)
            {
                return;
            }

            App.Current.Windows[0].MinimumWidth = 0;
            App.Current.Windows[0].MinimumHeight = 0;
        }

        /// <summary>
        /// Added to set the minimum width and height of the window when the page appears.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContentPage_Appearing(object? sender, EventArgs e)
        {
            if (App.Current?.Windows.Count == 0 || App.Current?.Windows[0] == null)
            {
                return;
            }
#if WINDOWS
            App.Current.Windows[0].MinimumWidth = 1067;
            App.Current.Windows[0].MinimumHeight = 629;
#elif MACCATALYST
            App.Current.Windows[0].MinimumWidth = 1700;
            App.Current.Windows[0].MinimumHeight = 1000;
#endif
        }


        /// <summary>
        /// Used to empty required strings.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadialAxis_LabelCreated(object? sender, LabelCreatedEventArgs e)
        {
            if (int.Parse(e.Text) == 75 || int.Parse(e.Text) == 125 || int.Parse(e.Text) == 175)
            {
                e.Text = string.Empty;
            }
        }

        /// <summary>
        /// Begins when the behavior gets detached from the view.
        /// Unwire Events and Nullify Variables
        /// </summary>
        /// <param name="bindable"></param>
        protected override void OnDetachingFrom(ContentPage bindable)
        {
            base.OnDetachingFrom(bindable);

            if (this.radialAxis != null)
            {
                this.radialAxis.LabelCreated -= RadialAxis_LabelCreated;
                this.radialAxis = null;
            }

            if (this.timer != null)
            {
                this.timer.Stop();
                this.timer = null;
            }

            if (this.needleTimer != null)
            {
                this.needleTimer.Stop();
                this.needleTimer.Tick -= NeedleTimer_Tick;
                this.needleTimer = null;
            }

            if (this.currentHourLabel != null)
            {
                this.currentHourLabel = null;
            }

            if (this.currentMinuteMeridianLabel != null)
            {
                this.currentMinuteMeridianLabel = null;
            }

            if (this.separatorLabel != null)
            {
                this.separatorLabel = null;
            }

            if (this.contentPage != null)
            {
                this.contentPage.Appearing -= ContentPage_Appearing;
                this.contentPage.Disappearing -= ContentPage_Disappearing;
                this.contentPage = null;
            }

            if (this.rpmPointer != null)
            {
                this.rpmPointer = null;
            }

            if (this.speedPointer != null)
            {
                this.speedPointer = null;
            }

            if (this.needlePointer != null)
            {
                this.needlePointer = null;
            }

            if (this.fuelPointer != null)
            {
                this.fuelPointer = null;
            }
        }
    }
}
