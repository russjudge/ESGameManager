using RussJudge.SimpleWPFReportPrinter;
using System.Printing;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

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

        public PrintGameList(GameList list)
        {
            theList = list;
        }
        bool printingHeader = false;
        private static TableRowGroup GetGroupHeader()
        {
            TableRowGroup rowGroupHeader = new TableRowGroup();

            rowGroupHeader.FontWeight = FontWeights.Bold;
            rowGroupHeader.FontSize = 12;
            TableRow row = new TableRow();
            rowGroupHeader.Rows.Add(row);
            row.Cells.Add(BuildHeaderCell("Name"));
            row.Cells.Add(BuildHeaderCell("Year"));
            row.Cells.Add(BuildHeaderCell("Publisher"));
            row.Cells.Add(BuildHeaderCell("Developer"));
            row.Cells.Add(BuildHeaderCell("Genre"));
            row.Cells.Add(BuildHeaderCell("Flags"));
            row.Cells.Add(BuildHeaderCell("Notes"));
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
        private static TableCell BuildHeaderCell(string text)
        {
            var para = new Paragraph();
            para.Background = Brushes.LightGray;
            para.Inlines.Add(text);

            var cell = new TableCell(para);
            return cell;
        }
        private static TableCell BuildCell(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                return new(new Paragraph(new Run(text)));
            }
            else
            {
                return new(new Paragraph());
            }
        }
        private TableRowGroup GetGroupBody()
        {
            TableRowGroup rowGroupBody = new();
            rowGroupBody.FontSize = 10;
            bool isAlt = false;
            foreach (var game in theList.Games)
            {
                TableRow row = new TableRow();

                row.Background = isAlt ? Brushes.AliceBlue : Brushes.Transparent;
                isAlt = !isAlt;


                rowGroupBody.Rows.Add(row);
                row.Cells.Add(BuildCell(game.Name));
                row.Cells.Add(BuildCell(game.DateReleased.ToString("yyyy")));
                row.Cells.Add(BuildCell(game.Publisher));
                row.Cells.Add(BuildCell(game.Developer));
                row.Cells.Add(BuildCell(game.Genre));

                var para = new Paragraph();
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

                row.Cells.Add(new(para));
                row.Cells.Add(BuildCell(game.Notes));
            }
            return rowGroupBody;
        }

        private FlowDocument CreateFlowDocument()
        {
            // Create a FlowDocument  
            FlowDocument doc = new FlowDocument();
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

        public static void GenerateReport(GameList gameList)
        {
            PrintDialog printDlg = new PrintDialog();
            if (printDlg.ShowDialog().GetValueOrDefault())
            {
                var pgl = new PrintGameList(gameList);
                var doc = pgl.CreateFlowDocument();

                Typeface tpBold = new(new("Times New"), FontStyles.Normal, FontWeights.Bold, FontStretches.Normal);
                Typeface tpNormal = new(new("TImes New"), FontStyles.Normal, FontWeights.Normal, FontStretches.Normal);
                Typeface tpItalic = new(new("TImes New"), FontStyles.Italic, FontWeights.Normal, FontStretches.Normal);

                ReportPageDefinition def = new(printDlg.PrintableAreaWidth, printDlg.PrintableAreaHeight, 48, 48, 48, 48);

                ReportHeaderFooterTextData pageDataSection = new("Page " + ReportPageDefinition.PageNumberSubstitution + " of " + ReportPageDefinition.TotalPagesSubstitution + " pages", tpNormal, 10);

                def.AddHeaderLine(new(new(DateTime.Now.ToString(), tpNormal, 10), new("ES Game Manager", tpBold, 18), pageDataSection));
                string folder = string.Empty;
                if (!string.IsNullOrEmpty(gameList.Folder))
                {
                    folder = gameList.Folder;
                }

                def.AddHeaderLine(new(new("Folder:" + folder, tpItalic, 10)));

                StringBuilder sb = new StringBuilder();
                if (!string.IsNullOrEmpty(Properties.Settings.Default.Flag1Symbol) && !string.IsNullOrEmpty(Properties.Settings.Default.Flag1))
                {
                    sb.AppendFormat("{0}: {1}  ", Properties.Settings.Default.Flag1Symbol, Properties.Settings.Default.Flag1);
                }
                if (!string.IsNullOrEmpty(Properties.Settings.Default.Flag2Symbol) && !string.IsNullOrEmpty(Properties.Settings.Default.Flag2))
                {
                    sb.AppendFormat("{0}: {1}  ", Properties.Settings.Default.Flag2Symbol, Properties.Settings.Default.Flag2);
                }
                if (!string.IsNullOrEmpty(Properties.Settings.Default.Flag3Symbol) && !string.IsNullOrEmpty(Properties.Settings.Default.Flag3))
                {
                    sb.AppendFormat("{0}: {1}  ", Properties.Settings.Default.Flag3Symbol, Properties.Settings.Default.Flag3);
                }
                if (!string.IsNullOrEmpty(Properties.Settings.Default.Flag4Symbol) && !string.IsNullOrEmpty(Properties.Settings.Default.Flag4))
                {
                    sb.AppendFormat("{0}: {1}  ", Properties.Settings.Default.Flag4Symbol, Properties.Settings.Default.Flag4);
                }
                if (!string.IsNullOrEmpty(Properties.Settings.Default.Flag5Symbol) && !string.IsNullOrEmpty(Properties.Settings.Default.Flag5))
                {
                    sb.AppendFormat("{0}: {1}  ", Properties.Settings.Default.Flag5Symbol, Properties.Settings.Default.Flag5);
                }
                if (!string.IsNullOrEmpty(Properties.Settings.Default.Flag6Symbol) && !string.IsNullOrEmpty(Properties.Settings.Default.Flag6))
                {
                    sb.AppendFormat("{0}: {1}  ", Properties.Settings.Default.Flag6Symbol, Properties.Settings.Default.Flag6);
                }

                def.AddFooterLine(new(new(sb.ToString(), tpNormal, 8), null, new(folder, tpNormal, 10)));

                ReportPaginator paginator = new(doc, def);

                printDlg.PrintDocument(paginator, "ES Game Manager");
                MessageBox.Show("Printing complete.");
            }
        }
    }
}
