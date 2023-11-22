using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Controls;
using System.Windows.Media;
using System.Globalization;
using static ESGameManager.GameListDocumentPaginator;

namespace ESGameManager
{
    public class GameListDocumentPaginator : DocumentPaginator
    {
        public GameListDocumentPaginator(FlowDocument document, Definition def)
        {
            // Create a copy of the flow document,
            // so we can modify it without modifying
            // the original.
            MemoryStream stream = new MemoryStream();
            TextRange sourceDocument = new TextRange(document.ContentStart, document.ContentEnd);
            sourceDocument.Save(stream, DataFormats.Xaml);
            FlowDocument copy = new FlowDocument();
            TextRange copyDocumentRange = new TextRange(copy.ContentStart, copy.ContentEnd);
            copyDocumentRange.Load(stream, DataFormats.Xaml);
            this.paginator = ((IDocumentPaginatorSource)copy).DocumentPaginator;
            this.definition = def;
            paginator.PageSize = def.ContentSize;

            // Change page size of the document to
            // the size of the content area
            copy.ColumnWidth = double.MaxValue; // Prevent columns
            copy.PageWidth = definition.ContentSize.Width;
            copy.PageHeight = definition.ContentSize.Height;
            copy.PagePadding = new Thickness(0);
        }

        private DocumentPaginator paginator;
        private Definition definition;


        public override DocumentPage GetPage(int pageNumber)
        {
            // Use default paginator to handle pagination
            Visual originalPage = paginator.GetPage(pageNumber).Visual;

            ContainerVisual visual = new ContainerVisual();
            ContainerVisual pageVisual = new ContainerVisual()
            {
                Transform = new TranslateTransform(
                    definition.ContentOrigin.X,
                    definition.ContentOrigin.Y
                )
            };
            pageVisual.Children.Add(originalPage);
            visual.Children.Add(pageVisual);

            // Create headers and footers
            if (definition.Header != null)
            {
                visual.Children.Add(CreateHeaderFooterVisual(definition.Header,
                                    definition.HeaderRect, pageNumber));
            }
            if (definition.Footer != null)
            {
                visual.Children.Add(CreateHeaderFooterVisual(definition.Footer,
                                    definition.FooterRect, pageNumber));
            }

            // Check for repeating table headers
            // (...will be described later in this article)

            return new DocumentPage(
                visual,
                definition.PageSize,
                new Rect(new Point(), definition.PageSize),
                new Rect(definition.ContentOrigin, definition.ContentSize)
            );
        }

        /// <summary>
        /// Creates a visual to draw the header/footer
        /// </summary>
        /// <param name="draw"></param>
        /// <param name="bounds"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        private Visual CreateHeaderFooterVisual(DrawHeaderFooter draw, Rect bounds, int pageNumber)
        {
            DrawingVisual visual = new DrawingVisual();
            using (DrawingContext context = visual.RenderOpen())
            {
                draw(context, bounds, pageNumber);
            }
            return visual;
        }

        ContainerVisual currentHeader = null;



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
        private bool PageEndsWithTable(DependencyObject element, out ContainerVisual tableVisual, out ContainerVisual headerVisual)
        {
            tableVisual = null;
            headerVisual = null;
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
        private bool PageStartsWithTable(DependencyObject element, out ContainerVisual tableVisual)
        {
            tableVisual = null;
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
            return false;
        }


        #region DocumentPaginator members

        public override bool IsPageCountValid
        {
            get { return paginator.IsPageCountValid; }
        }

        public override int PageCount
        {
            get { return paginator.PageCount; }
        }

        public override Size PageSize
        {
            get
            {
                return paginator.PageSize;
            }
            set
            {
                paginator.PageSize = value;
            }
        }

        public override IDocumentPaginatorSource Source
        {
            get { return paginator.Source; }
        }

        #endregion

    }

    public class Definition
    {

        #region Page sizes

        /// <summary>
        /// PageSize in DIUs
        /// </summary>
        public Size PageSize
        {
            get { return _PageSize; }
            set { _PageSize = value; }
        }
        private Size _PageSize = new Size(793.5987, 1122.3987); // Default: A4

        /// <summary>
        /// Margins
        /// </summary>
        public Thickness Margins
        {
            get { return _Margins; }
            set { _Margins = value; }
        }
        private Thickness _Margins = new Thickness(96); // Default: 1" margins


        /// <summary>
        /// Space reserved for the header in DIUs
        /// </summary>
        public double HeaderHeight
        {
            get { return _HeaderHeight; }
            set { _HeaderHeight = value; }
        }
        private double _HeaderHeight;

        /// <summary>
        /// Space reserved for the footer in DIUs
        /// </summary>
        public double FooterHeight
        {
            get { return _FooterHeight; }
            set { _FooterHeight = value; }
        }
        private double _FooterHeight;

        #endregion


        public DrawHeaderFooter Header, Footer;

        ///<summary>
        /// Should table headers automatically repeat?
        ///</summary>
        public bool RepeatTableHeaders
        {
            get { return _RepeatTableHeaders; }
            set { _RepeatTableHeaders = value; }
        }
        private bool _RepeatTableHeaders = true;


        #region Some convenient helper properties

        internal Size ContentSize
        {
            get
            {
                var retVal = new Size(PageSize.Width - (Margins.Left + Margins.Right), PageSize.Height - (Margins.Top + Margins.Bottom + HeaderHeight + FooterHeight));
                return retVal;
            }
        }

        internal Point ContentOrigin
        {
            get
            {
                return new Point(
                    Margins.Left,
                    Margins.Top + HeaderRect.Height
                );
            }
        }

        internal Rect HeaderRect
        {
            get
            {
                return new Rect(
                    Margins.Left, Margins.Top,
                    ContentSize.Width, HeaderHeight
                );
            }
        }

        internal Rect FooterRect
        {
            get
            {
                return new Rect(
                    Margins.Left, ContentOrigin.Y + ContentSize.Height,
                    ContentSize.Width, FooterHeight
                );
            }
        }

        #endregion

    } /// <summary>
      /// Allows drawing headers and footers
      /// </summary>
      /// <param name="context">This is the drawing context that should be used</param>
      /// <param name="bounds">The bounds of the header. You can ignore these at your own peril</param>
      /// <param name="pageNr">The page nr (0-based)</param>
    public delegate void DrawHeaderFooter(DrawingContext context, Rect bounds, int pageNr);

}
