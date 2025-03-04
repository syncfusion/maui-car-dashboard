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
        private RadialAxis? radialAxis;
        protected override void OnAttachedTo(ContentPage bindable)
        {
            base.OnAttachedTo(bindable);
            radialAxis = bindable.FindByName<RadialAxis>("temperatureAxis");

            radialAxis.LabelCreated += RadialAxis_LabelCreated;
        }

        private void RadialAxis_LabelCreated(object? sender, LabelCreatedEventArgs e)
        {
            if (int.Parse(e.Text) == 75 || int.Parse(e.Text) == 125 || int.Parse(e.Text) == 175)
            {
                e.Text = string.Empty;
            }
        }

        protected override void OnDetachingFrom(ContentPage bindable)
        {
            base.OnDetachingFrom(bindable);

            if (radialAxis != null)
            {
                radialAxis.LabelCreated -= RadialAxis_LabelCreated;
                radialAxis = null;
            }
        }
    }
}
