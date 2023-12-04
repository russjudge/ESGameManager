using System.IO;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace ESGameManagerLibrary
{
    public class ReportPaginator : DocumentPaginator
    {
        public ReportPaginator(FlowDocument document, ReportPageDefinition definition)
        {
            // Create a copy of the flow document,
            // so we can modify it without modifying
            // the original.
            using (MemoryStream stream = new MemoryStream())
            {
                TextRange sourceDocument = new TextRange(document.ContentStart, document.ContentEnd);
                sourceDocument.Save(stream, DataFormats.Xaml);
                FlowDocument copy = new FlowDocument();
                TextRange copyDocumentRange = new TextRange(copy.ContentStart, copy.ContentEnd);
                copyDocumentRange.Load(stream, DataFormats.Xaml);


                this.Paginator = ((IDocumentPaginatorSource)copy).DocumentPaginator;
                this.Definition = definition;
                Paginator.PageSize = definition.ContentSize;

                // Change page size of the document to
                // the size of the content area
                copy.ColumnWidth = double.MaxValue; // Prevent columns
                copy.PageWidth = Definition.ContentSize.Width;
                copy.PageHeight = Definition.ContentSize.Height;
                copy.PagePadding = new Thickness(0);

            }
        }

        private DocumentPaginator Paginator;
        private ReportPageDefinition Definition;

        public override DocumentPage GetPage(int pageNumber)
        {
            // Use default paginator to handle pagination
            Visual originalPage = Paginator.GetPage(pageNumber).Visual;

            Console.WriteLine("--- Begin Page {0} -------", pageNumber + 1);
            //originalPage.DumpVisualTree(Console.Out);
            Console.WriteLine("--- End Page {0} -------", pageNumber + 1);
            Console.WriteLine();

            ContainerVisual visual = new ContainerVisual();
            ContainerVisual pageVisual = new ContainerVisual()
            {
                Transform = new TranslateTransform(
                    Definition.ContentOrigin.X,
                    Definition.ContentOrigin.Y
                )
            };
            pageVisual.Children.Add(originalPage);


            visual.Children.Add(pageVisual);

            // Create headers and footers
            visual.Children.Add(Definition.CreateHeader(pageNumber));


            visual.Children.Add(Definition.CreateFooter(pageNumber));


            // Check for repeating table headers
            if (Definition.RepeatTableHeaders)
            {
                // Find table header
                ContainerVisual? table;
                if (PageStartsWithTable(originalPage, out table) && currentHeader != null)
                {
                    // The page starts with a table and a table header was
                    // found on the previous page. Presumably this table 
                    // was started on the previous page, so we'll repeat the
                    // table header.
                    Rect headerBounds = VisualTreeHelper.GetDescendantBounds(currentHeader);
                    Vector offset = VisualTreeHelper.GetOffset(currentHeader);
                    ContainerVisual tableHeaderVisual = new ContainerVisual();

                    // Translate the header to be at the top of the page
                    // instead of its previous position
                    tableHeaderVisual.Transform = new TranslateTransform(
                        Definition.ContentOrigin.X,
                        Definition.ContentOrigin.Y - headerBounds.Top
                    );

                    // Since we've placed the repeated table header on top of the
                    // content area, we'll need to scale down the rest of the content
                    // to accomodate this. Since the table header is relatively small,
                    // this probably is barely noticeable.
                    double yScale = (Definition.ContentSize.Height - headerBounds.Height) / Definition.ContentSize.Height;
                    TransformGroup group = new TransformGroup();
                    group.Children.Add(new ScaleTransform(1.0, yScale));
                    group.Children.Add(new TranslateTransform(
                        Definition.ContentOrigin.X,
                        Definition.ContentOrigin.Y + headerBounds.Height
                    ));
                    pageVisual.Transform = group;

                    ContainerVisual? cp = VisualTreeHelper.GetParent(currentHeader) as ContainerVisual;
                    if (cp != null)
                    {
                        cp.Children.Remove(currentHeader);
                    }
                    tableHeaderVisual.Children.Add(currentHeader);
                    visual.Children.Add(tableHeaderVisual);
                }

                // Check if there is a table on the bottom of the page.
                // If it's there, its header should be repeated
                ContainerVisual? newTable, newHeader;
                if (PageEndsWithTable(originalPage, out newTable, out newHeader))
                {
                    if (newTable == table)
                    {
                        // Still the same table so don't change the repeating header
                    }
                    else
                    {
                        // We've found a new table. Repeat the header on the next page
                        currentHeader = newHeader;
                    }
                }
                else
                {
                    // There was no table at the end of the page
                    currentHeader = null;
                }
            }


            var retVal = new DocumentPage(
                visual,
                Definition.PageSize,
                new Rect(new Point(), Definition.PageSize),
                new Rect(Definition.ContentOrigin, Definition.ContentSize)
            );
            return retVal;
        }



        ContainerVisual? currentHeader = null;

        /// <summary>
        /// Checks if the page ends with a table.
        /// </summary>
        /// <remarks>
        /// There is no such thing as a 'TableVisual'. There is a RowVisual, which
        /// is contained in a ParagraphVisual if it's part of a table. For our
        /// purposes, we'll consider this the table Visual
        /// 
        /// You'd think that if the last element on the page was a table row, 
        /// this would also be the last element in the visual tree, but this is not true
        /// The page ends with a ContainerVisual which is aparrently  empty.
        /// Therefore, this method will only check the last child of an element
        /// unless this is a ContainerVisual
        /// </remarks>
        /// <param name="originalPage"></param>
        /// <returns></returns>
        private bool PageEndsWithTable(DependencyObject element, out ContainerVisual? tableVisual, out ContainerVisual? headerVisual)
        {

            if (element.GetType().Name == "RowVisual")
            {
                tableVisual = (ContainerVisual)VisualTreeHelper.GetParent(element);
                headerVisual = (ContainerVisual)VisualTreeHelper.GetChild(tableVisual, 0);
                return true;
            }
            int children = VisualTreeHelper.GetChildrenCount(element);
            if (element.GetType() == typeof(ContainerVisual))
            {
                for (int c = children - 1; c >= 0; c--)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(element, c);
                    if (PageEndsWithTable(child, out tableVisual, out headerVisual))
                    {
                        return true;
                    }
                }
            }
            else if (children > 0)
            {
                DependencyObject child = VisualTreeHelper.GetChild(element, children - 1);
                if (PageEndsWithTable(child, out tableVisual, out headerVisual))
                {
                    return true;
                }
            }
            tableVisual = null;
            headerVisual = null;
            return false;
        }


        /// <summary>
        /// Checks if the page starts with a table which presumably has wrapped
        /// from the previous page.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="tableVisual"></param>
        /// <param name="headerVisual"></param>
        /// <returns></returns>
        private bool PageStartsWithTable(DependencyObject element, out ContainerVisual? tableVisual)
        {
            if (element.GetType().Name == "RowVisual")
            {
                tableVisual = (ContainerVisual)VisualTreeHelper.GetParent(element);
                return true;
            }
            if (VisualTreeHelper.GetChildrenCount(element) > 0)
            {
                DependencyObject child = VisualTreeHelper.GetChild(element, 0);
                if (PageStartsWithTable(child, out tableVisual))
                {
                    return true;
                }
            }
            tableVisual = null;
            return false;
        }



        public override bool IsPageCountValid
        {
            get { return Paginator.IsPageCountValid; }
        }

        public override int PageCount
        {
            get { return Paginator.PageCount; }
        }

        public override Size PageSize
        {
            get
            {
                return Paginator.PageSize;
            }
            set
            {
                Paginator.PageSize = value;
            }
        }

        public override IDocumentPaginatorSource Source
        {
            get { return Paginator.Source; }
        }


        public enum PaperSize
        {
            A4, // 793.5987 x 1122.3987  (796.8x 1123.2) (8.3 x 11.7)
            Letter, //(8.5 x 11)
            Legal
        }

        public class ReportPageDefinition
        {
            public const string PageNumberSubstitution = "{PAGENUMBER}";
            public const string DateSubstitution = "{DATE}";
            public const string DateTimeSubstitution = "{DATETIME}";
            public const string TimeSubstitution = "{TIME}";

            public string? DateFormat { get; set; } = null;
            public string? TimeFormat { get; set; } = null;
            public string? DateTimeFormat { get; set; } = null;



            List<Tuple<string, Typeface, double>> headerLines = new();
            List<Tuple<string, Typeface, double>> footerLines = new();

            public ReportPageDefinition(
                double pageWidth = 793.5987, double pageHeight = 1122.3987,
                double left = 96, double top = 96, double right = 96, double bottom = 96)
            {

                PageSize = new Size(pageWidth, pageHeight);
                Margins = new Thickness(left, top, right, bottom);

                ContentSize = new Size(PageSize.Width - (Margins.Left + Margins.Right), PageSize.Height - (Margins.Top + Margins.Bottom + HeaderHeight + FooterHeight));

                ContentOrigin = new Point(Margins.Left, Margins.Top + HeaderRect.Height);
                HeaderRect = new Rect(
                        Margins.Left, Margins.Top,
                        ContentSize.Width, HeaderHeight
                    );
                FooterRect = new Rect(
                       Margins.Left, ContentOrigin.Y + ContentSize.Height,
                       ContentSize.Width, FooterHeight
                   );

            }
            DateTime currentTime = DateTime.Now;
            internal Visual CreateHeader(int pageNumber)
            {
                DrawingVisual visual = new DrawingVisual();
                using (DrawingContext context = visual.RenderOpen())
                {
                    double offset = 0;
                    foreach (var header in headerLines)
                    {
                        FormattedText line = GetText(header.Item2, header.Item3, header.Item1, pageNumber);
                        context.DrawText(line, new Point(HeaderRect.Left, HeaderRect.Top + offset));
                        offset += line.LineHeight + line.Height;

                    }
                }
                return visual;
            }
            internal Visual CreateFooter(int pageNumber)
            {
                DrawingVisual visual = new DrawingVisual();
                using (DrawingContext context = visual.RenderOpen())
                {
                    double offset = 0;
                    foreach (var footer in footerLines)
                    {
                        FormattedText line = GetText(footer.Item2, footer.Item3, footer.Item1, pageNumber);
                        context.DrawText(line, new Point(FooterRect.Left, FooterRect.Top + offset));
                        offset += line.LineHeight + line.Height;
                    }
                }
                return visual;
            }
            private FormattedText GetText(Typeface typeface, double fontSize, string text, int pageNumber)
            {
                string txt = text
                    .Replace(PageNumberSubstitution, (pageNumber + 1).ToString())
                    .Replace(DateSubstitution, currentTime.ToString(DateFormat))
                    .Replace(DateTimeSubstitution, currentTime.ToString(DateTimeFormat))
                    .Replace(TimeSubstitution, currentTime.ToString(TimeFormat));

                FormattedText formattedText = new FormattedText(txt,
                    System.Globalization.CultureInfo.CurrentCulture,
                    FlowDirection.LeftToRight,
                    typeface, fontSize, Brushes.Black,
                    1);
                formattedText.SetFontWeight(typeface.Weight);
                formattedText.SetForegroundBrush(Brushes.Black);
                return formattedText;
            }
            public void LoadHeaderLine(Typeface typeface, double fontSize, string text)
            {
                headerLines.Add(new(text, typeface, fontSize));
                var line = GetText(typeface, fontSize, text, 0);
                HeaderHeight += line.LineHeight + line.Height;

                ContentSize = new Size(PageSize.Width - (Margins.Left + Margins.Right), PageSize.Height - (Margins.Top + Margins.Bottom + HeaderHeight + FooterHeight));
                ContentOrigin = new Point(Margins.Left, Margins.Top + HeaderRect.Height);
                HeaderRect = new Rect(
                        Margins.Left, Margins.Top,
                        ContentSize.Width, HeaderHeight
                    );
            }
            public void LoadFooterLine(Typeface typeface, double fontSize, string text)
            {
                footerLines.Add(new(text, typeface, fontSize));
                var line = GetText(typeface, fontSize, text, 0);
                FooterHeight += line.LineHeight + line.Height;

                ContentSize = new Size(PageSize.Width - (Margins.Left + Margins.Right), PageSize.Height - (Margins.Top + Margins.Bottom + HeaderHeight + FooterHeight));
                FooterRect = new Rect(
                        Margins.Left, ContentOrigin.Y + ContentSize.Height,
                        ContentSize.Width, FooterHeight
                    );
            }




            /// <summary>
            /// PageSize in DIUs
            /// </summary>
            public Size PageSize { get; set; }   // Default: A4

            /// <summary>
            /// Margins
            /// </summary>
            public Thickness Margins { get; set; } // Default: 1" margins

            /// <summary>
            /// Space reserved for the header in DIUs
            /// </summary>
            public double HeaderHeight { get; private set; } = 0;

            /// <summary>
            /// Space reserved for the footer in DIUs
            /// </summary>
            public double FooterHeight { get; private set; } = 0;

            ///<summary>
            /// Should table headers automatically repeat?
            ///</summary>
            public bool RepeatTableHeaders { get; set; } = true;


            internal Size ContentSize { get; private set; }

            internal Point ContentOrigin { get; private set; }

            private Rect HeaderRect { get; set; }

            private Rect FooterRect { get; set; }


        }

    }
}
