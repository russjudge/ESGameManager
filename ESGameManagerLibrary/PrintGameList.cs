using System.Printing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Xps;

namespace ESGameManagerLibrary
{
    public class PrintGameList
    {
        GameList theList;
        PageContent? pageContent;
        FixedPage? fixedPage;
        PrintTicket? printTicket;
        FixedDocument? fixedDocument;
        double yPos;
        const string fontFamily = "Times New";
        const double margin = 40;
        const double fontSize = 10;
        const double fontHeaderSize = 14;

        private int pageNumber = 0;

        public PrintGameList(GameList list)
        {
            theList = list;
        }
        bool printingHeader = false;
        private TableRowGroup GetGroupHeader()
        {
            TableRowGroup rowGroupHeader = new TableRowGroup();

            rowGroupHeader.Background = Brushes.LightGray;
            rowGroupHeader.FontWeight = FontWeights.Bold;
            rowGroupHeader.FontSize = 12;



            TableRow row = new TableRow();
            rowGroupHeader.Rows.Add(row);
            //row.Cells.Add(new TableCell());



            Paragraph para = new Paragraph();
            para.Inlines.Add("Name");

            TableCell cell = new TableCell(para);
            row.Cells.Add(cell);

            para = new Paragraph();
            para.Inlines.Add("Year");

            cell = new TableCell(para);
            row.Cells.Add(cell);

            para = new Paragraph();
            para.Inlines.Add("Publisher");

            cell = new TableCell(para);
            row.Cells.Add(cell);

            para = new Paragraph();
            para.Inlines.Add("Developer");

            cell = new TableCell(para);
            row.Cells.Add(cell);

            para = new Paragraph();
            para.Inlines.Add("Genre");

            cell = new TableCell(para);
            row.Cells.Add(cell);

            para = new Paragraph();
            para.Inlines.Add("Flags");

            cell = new TableCell(para);
            row.Cells.Add(cell);


            para = new Paragraph();
            para.Inlines.Add("Notes");

            cell = new TableCell(para);
            row.Cells.Add(cell);
            return rowGroupHeader;
        }
        private TableRowGroup GetGroupSummary()
        {
            TableRowGroup rowGroupSummary = new TableRowGroup();
            rowGroupSummary.FontSize = 12;
            rowGroupSummary.FontStyle = FontStyles.Italic;
            TableRow row = new TableRow();
            rowGroupSummary.Rows.Add(row);

            Paragraph para = new Paragraph();
            para.Inlines.Add(new Run("Total Games:"));
            para.Inlines.Add(new Run(theList.Games.Count.ToString()));

            TableCell cell = new TableCell(para);

            row.Cells.Add(cell);
            cell.ColumnSpan = 7;

            return rowGroupSummary;
        }
        private TableRowGroup GetGroupBody()
        {
            TableRowGroup rowGroupBody = new TableRowGroup();
            rowGroupBody.FontSize = 10;


            foreach (var game in theList.Games)
            {
                TableRow row = new TableRow();
                rowGroupBody.Rows.Add(row);


                Paragraph para = new Paragraph();
                para.Inlines.Add(game.Name);

                TableCell cell = new TableCell(para);
                row.Cells.Add(cell);

                para = new Paragraph();
                para.Inlines.Add(game.DateReleased.ToString("yyyy"));

                cell = new TableCell(para);
                row.Cells.Add(cell);

                para = new Paragraph();
                if (!string.IsNullOrEmpty(game.Publisher))
                {
                    para.Inlines.Add(game.Publisher);
                }

                cell = new TableCell(para);
                row.Cells.Add(cell);

                para = new Paragraph();
                if (!string.IsNullOrEmpty(game.Developer))
                {
                    para.Inlines.Add(game.Developer);
                }
                cell = new TableCell(para);
                row.Cells.Add(cell);

                para = new Paragraph();
                if (!string.IsNullOrEmpty(game.Genre))
                {
                    para.Inlines.Add(game.Genre);
                }
                cell = new TableCell(para);
                row.Cells.Add(cell);

                para = new Paragraph();
                if (game.Flag1)
                {
                    para.Inlines.Add(Properties.Settings.Default.Flag1Symbol);
                }
                if (game.Flag2)
                {
                    para.Inlines.Add(Properties.Settings.Default.Flag2Symbol);
                }
                if (game.Flag3)
                {
                    para.Inlines.Add(Properties.Settings.Default.Flag3Symbol);
                }
                if (game.Flag4)
                {
                    para.Inlines.Add(Properties.Settings.Default.Flag4Symbol);
                }
                if (game.Flag5)
                {
                    para.Inlines.Add(Properties.Settings.Default.Flag5Symbol);
                }
                if (game.Flag6)
                {
                    para.Inlines.Add(Properties.Settings.Default.Flag6Symbol);
                }
                cell = new TableCell(para);
                row.Cells.Add(cell);


                para = new Paragraph();
                if (!string.IsNullOrEmpty(game.Notes))
                {
                    para.Inlines.Add(game.Notes);
                }
                cell = new TableCell(para);
                row.Cells.Add(cell);

            }
            return rowGroupBody;
        }
        //private Visual CreateVisual()
        //{

        //}
        private FlowDocument CreateFlowDocument()
        {
            // Create a FlowDocument  
            FlowDocument doc = new FlowDocument();
            //doc.PageWidth = width;
            //doc.PageHeight = height;
            Table table = new Table();
            table.TextAlignment = TextAlignment.Justify;
            doc.Blocks.Add(table);

            table.Columns.Add(new TableColumn());
            table.Columns.Add(new TableColumn());
            table.Columns.Add(new TableColumn());
            table.Columns.Add(new TableColumn());
            table.Columns.Add(new TableColumn());
            table.Columns.Add(new TableColumn());
            table.Columns.Add(new TableColumn());





            table.RowGroups.Add(GetGroupHeader());
            table.RowGroups.Add(GetGroupBody());
            table.RowGroups.Add(GetGroupSummary());



            return doc;
        }
        void PrintFlowDocument()
        {
            // Create a PrintDialog
            PrintDialog printDlg = new PrintDialog();
            // Create a FlowDocument dynamically.  
            FlowDocument doc = CreateFlowDocument();

            doc.Name = "ES_Game_Manager_Document";
            // Create IDocumentPaginatorSource from FlowDocument  
            IDocumentPaginatorSource idpSource = doc;

            //doc.PageWidth = printDlg.PrintableAreaWidth;
            //doc.PageHeight = printDlg.PrintableAreaHeight;


            // Call PrintDocument method to send document to printer  
            printDlg.PrintDocument(idpSource.DocumentPaginator, "Hello WPF Printing.");
        }
        public void SimpleReporting()
        {
            Report1 visual = new();
            visual.DataContext = theList;
            var paginator = new ProgramPaginator(visual);
            var dlg = new PrintDialog();
            if (dlg.ShowDialog() == true)
            {
                paginator.PageSize = new Size(dlg.PrintableAreaWidth, dlg.PrintableAreaWidth);
                dlg.PrintDocument(paginator, "ESManagerDocument");
            }

            //SimpleWPFReporting.Report.ExportVisualAsPdf(visual);
            //SimpleWPFReporting.Report.PrintReport(visual, theList, SimpleWPFReporting.ReportOrientation.Portrait);


        }

        void StartNewPage()
        {
            pageNumber++;
            pageContent = new PageContent();
            fixedPage = new FixedPage();
            if (printTicket != null && printTicket.PageMediaSize != null && printTicket.PageMediaSize.Width != null && printTicket.PageMediaSize.Height != null)
            {
                fixedPage.Width = printTicket.PageMediaSize.Width.Value;
                fixedPage.Height = printTicket.PageMediaSize.Height.Value;
            }
            yPos = margin;

            PrintHeader();
        }
        void PrintHeader()
        {
            printingHeader = true;
            PrintRow("\t\t\tEmulaiton Station Game List", fontHeaderSize, FontWeights.Bold);
            PrintRow("Page " + pageNumber.ToString() + "\tFolder: " + theList.Folder + "\tSystem: " + theList.Provider.System, fontHeaderSize, FontWeights.Normal);

            PrintRow(string.Format("Name\tYear\tPublisher\tDeveloper\tGenre\t{0}\t{1}\t{2}\tNotes",
                Properties.Settings.Default.Flag1, Properties.Settings.Default.Flag2,
                Properties.Settings.Default.Flag3), fontSize, FontWeights.Bold);

            printingHeader = false;
        }
        public void Print()
        {
            PrintDialog printDialog = new PrintDialog();

            if (printDialog.ShowDialog() == true)
            {
                PrintFlowDocument();
                //PrintData(printDialog.PrintQueue.FullName);

                //PrintData(printDialog.PrintQueue);
                MessageBox.Show("Printing complete.");
            }

        }
        private void PrintRow(string text, double fntSz, FontWeight weight)
        {
            if (fixedPage != null && pageContent != null && fixedDocument != null)
            {
                Typeface typeface = new Typeface(new FontFamily(fontFamily), FontStyles.Normal, weight, FontStretches.Normal);
                // Set up the formatted text
                FormattedText formattedText = new FormattedText(text,
                    System.Globalization.CultureInfo.CurrentCulture,
                    FlowDirection.LeftToRight,
                    typeface, fontSize, Brushes.Black,
                    1.25);
                formattedText.SetFontWeight(weight);
                formattedText.SetForegroundBrush(Brushes.Black);
                // Draw the text on the fixed page
                Glyphs data = new()
                {
                    Fill = System.Windows.Media.Brushes.Black,
                    UnicodeString = formattedText.Text,
                    FontUri = typeface.FontFamily.BaseUri,
                    FontRenderingEmSize = fontSize,
                    OriginX = margin,
                    OriginY = yPos,

                };

                fixedPage.Children.Add(data);

                // Move to the next row
                yPos += formattedText.Height;

                // Check if another page is needed
                if (!printingHeader && yPos + formattedText.Height > fixedPage.Height)
                {
                    // Add the current fixed page to the document
                    pageContent.Child = fixedPage;
                    fixedDocument.Pages.Add(pageContent);
                    // Start a new page
                    StartNewPage();
                }
            }
        }
        private void PrintGames()
        {
            if (fixedPage != null && pageContent != null && fixedDocument != null)
            {

                // Iterate through each row
                foreach (var game in theList.Games)
                {
                    PrintRow(game.ToString(), fontSize, FontWeights.Normal);

                }
                PrintRow("Total Game Count:" + theList.Games.Count.ToString(), fontSize, FontWeights.Normal);
                // Add the last page to the document
                pageContent.Child = fixedPage;
                fixedDocument.Pages.Add(pageContent);
            }
        }

        private void PrintData(string printerName)
        {
            PrintQueue printQueue = new PrintQueue(new PrintServer(), printerName);
            printTicket = printQueue.UserPrintTicket;
            // Create a FixedDocument for printing
            //GameListFlowDocument doc = new();
            StartNewPage();
            // Add a page to the document
            PrintGames();

            // Print the document
            XpsDocumentWriter xpsWriter = PrintQueue.CreateXpsDocumentWriter(printQueue);
            xpsWriter.Write(fixedDocument, printTicket);
        }
        /*  *************** */

        public static void GenerateReport(GameList gameList)
        {

            //var ppd = VisualTreeHelper.GetDpi(baseControl).PixelsPerDip;


            //// Create a FlowDocument
            //FlowDocument doc = new FlowDocument();

            //// Create a Section
            //Section sec = new Section();

            //// Create first (header) paragraph
            //Paragraph para1 = new Paragraph();

            //para1.Inlines.Add(new Bold(new Run($"Provider: {gameList.Provider.System}\nFolder: {gameList.Folder}")) { FontSize = 16, FontFamily = new FontFamily("Courier New") });

            //sec.Blocks.Add(para1);

            //// Create second (column headers) paragraph
            //Paragraph para2 = new Paragraph();




            //para2.Inlines.Add(new Bold(new Run("Name\t\tPath")) { FontSize = 14 });
            //sec.Blocks.Add(para2);

            //// Create rest of the paragraphs (each game)
            //foreach (Game game in gameList.Games)
            //{
            //    Paragraph para = new Paragraph();
            //    para.Inlines.Add(new Run($"{game.Name}\t\t{game.Path}"));
            //    sec.Blocks.Add(para);
            //}

            //// Add Section to FlowDocument
            //doc.Blocks.Add(sec);




            // Call PrintDocument method to send document to printer
            PrintDialog printDlg = new PrintDialog();
            if ((bool)printDlg.ShowDialog().GetValueOrDefault())
            {
                var pgl = new PrintGameList(gameList);
                var doc = pgl.CreateFlowDocument();


                ReportPaginator.ReportPageDefinition def = new(printDlg.PrintableAreaWidth, printDlg.PrintableAreaHeight);

                def.LoadHeaderLine(new(new FontFamily("Courier New"), FontStyles.Normal, FontWeights.Bold, FontStretches.Normal), 18, "ES Game Manager");
                def.LoadHeaderLine(new(new FontFamily("Courier New"), FontStyles.Normal, FontWeights.Normal, FontStretches.Normal), 10, "Page " + ReportPaginator.ReportPageDefinition.PageNumberSubstitution);

                def.LoadFooterLine(new(new FontFamily("Courier New"), FontStyles.Normal, FontWeights.Bold, FontStretches.Normal), 10, "Test Footer");
                ReportPaginator paginator = new ReportPaginator(doc, def);

                //printDlg.PrintableAreaHeight
                printDlg.PrintDocument(paginator, "ES Game Manager");
            }
        }

        public static void DoThePrint()
        {

        }


    }


}
