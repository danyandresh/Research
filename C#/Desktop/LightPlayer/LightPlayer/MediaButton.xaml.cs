using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LightPlayer
{
    /// <summary>
    ///     Interaction logic for MediaButton.xaml
    /// </summary>
    public partial class MediaButton : Button
    {
        public MediaButton()
        {
            InitializeComponent();
        }

        // Dependency Property
        public static readonly DependencyProperty ShapeProperty =
            DependencyProperty.Register(
                "Shape",
                typeof (Geometry),
                typeof (MediaButton),
                new FrameworkPropertyMetadata(Geometry.Empty));


        public Geometry Shape
        {
            get
            {
                var result = GetValue(ShapeProperty) as Geometry ?? Geometry.Empty;

                return result;
            }
            set
            {
                if (value != null)
                {
                    SetValue(ShapeProperty, value);
                }
            }
        }
    }
}