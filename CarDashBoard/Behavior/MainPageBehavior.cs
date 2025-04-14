using Syncfusion.Maui.Gauges;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDashBoard
{
    public class MainPageBehavior : Behavior<ContentPage>
    {
        /// <summary>
        /// Declaration of radial axis control.
        /// </summary>
        private RadialAxis? radialAxis;

        /// <summary>
        /// Declaration of label which denotes current time.
        /// </summary>
        private Label? currentTimeLabel;

        /// <summary>
        /// Declaration of timer to update the current time.
        /// </summary>
        private IDispatcherTimer? timer;
        
        /// <summary>
        /// Begins when the behavior attached to the view.
        /// </summary>
        /// <param name="bindable"></param>

        protected override void OnAttachedTo(ContentPage bindable)
        {
            base.OnAttachedTo(bindable);

            // Initialize variable.
            this.radialAxis = bindable.FindByName<RadialAxis>("temperatureAxis");
            this.currentTimeLabel = bindable.FindByName<Label>("currentTime");

            // Setting default appearance.
            this.currentTimeLabel.Text = DateTime.Now.ToString("hh:mm tt");

            // Started a timer to update the current time. This will help to update the current time in the UI.
            this.timer = bindable.Dispatcher.CreateTimer();
            this.timer.Interval = TimeSpan.FromSeconds(1);
            this.timer.Tick += (s, e) =>
            {
                this.currentTimeLabel.Text = DateTime.Now.ToString("hh:mm tt");
            };
            this.timer.Start();

            // Wire Required events.
            this.radialAxis.LabelCreated += RadialAxis_LabelCreated;
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
        }
    }
}
