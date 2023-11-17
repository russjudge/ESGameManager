using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows;

namespace ESGameManagerLibrary
{
    /// <summary>
    /// Sort Adorner.
    /// </summary>
    public class SortAdorner : Adorner
    {
        private static readonly Geometry AscGeometry =
              Geometry.Parse("M 0,0 L 10,0 L 5,5 Z");

        private static readonly Geometry DescGeometry =
            Geometry.Parse("M 0,5 L 10,5 L 5,0 Z");

        /// <summary>
        /// Initializes a new instance of the <see cref="SortAdorner"/> class.
        /// </summary>
        /// <param name="element">Element.</param>
        /// <param name="direction">Direction.</param>
        public SortAdorner(UIElement element, ListSortDirection direction)
            : base(element)
        {
            this.Initialize(direction, Brushes.LightSteelBlue);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SortAdorner"/> class.
        /// </summary>
        /// <param name="element">Element.</param>
        /// <param name="direction">Direction.</param>
        /// <param name="brushColor">Brush color.</param>
        public SortAdorner(UIElement element, ListSortDirection direction, Brush brushColor)
            : base(element)
        {
            this.Initialize(direction, brushColor);
        }

        /// <summary>
        /// Gets Direction.
        /// </summary>
        public ListSortDirection Direction { get; private set; }

        /// <summary>
        /// Gets or sets Brushcolor.
        /// </summary>
        public Brush? BrushColor { get; set; }

        /// <summary>
        /// On render.
        /// </summary>
        /// <param name="drawingContext">Drawing context.</param>
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            if (this.AdornedElement.RenderSize.Width < 20)
            {
                return;
            }

            if (drawingContext != null)
            {
                drawingContext.PushTransform(
                     new TranslateTransform(
                       this.AdornedElement.RenderSize.Width - 15,
                       (this.AdornedElement.RenderSize.Height - 5) / 2));

                drawingContext.DrawGeometry(
                    this.BrushColor,
                    null,
                    this.Direction == ListSortDirection.Ascending ? AscGeometry : DescGeometry);

                drawingContext.Pop();
            }
        }

        private void Initialize(ListSortDirection direction, Brush brushColor)
        {
            this.Direction = direction;
            this.BrushColor = brushColor;
        }
    }
}

