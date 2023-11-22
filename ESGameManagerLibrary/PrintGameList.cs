using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Xps;
using System.Windows;
using System.Xml.Linq;

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

        private FlowDocument CreateFlowDocument()
        {
            // Create a FlowDocument  
            FlowDocument doc = new FlowDocument();
            // Create a Section  
            Section sec = new Section();
            // Create first Paragraph  
            Paragraph p1 = new Paragraph();
            // Create and add a new Bold, Italic and Underline  
            Bold bld = new Bold();
            bld.Inlines.Add(new Run("First Paragraph"));
            Italic italicBld = new Italic();
            italicBld.Inlines.Add(bld);
            Underline underlineItalicBld = new Underline();
            underlineItalicBld.Inlines.Add(italicBld);
            // Add Bold, Italic, Underline to Paragraph  
            p1.Inlines.Add(underlineItalicBld);
            // Add Paragraph to Section  
            sec.Blocks.Add(p1);
            // Add Section to FlowDocument  
            doc.Blocks.Add(sec);
            return doc;
        }
        void PrintFlowDocument()
        {
            // Create a PrintDialog
            PrintDialog printDlg = new PrintDialog();
            // Create a FlowDocument dynamically.  
            FlowDocument doc = CreateFlowDocument();
            doc.Name = "ES Game Manager Document";
            // Create IDocumentPaginatorSource from FlowDocument  
            IDocumentPaginatorSource idpSource = doc;
            // Call PrintDocument method to send document to printer  
            printDlg.PrintDocument(idpSource.DocumentPaginator, "Hello WPF Printing.");
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
                PrintData(printDialog.PrintQueue.FullName);

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
                    OriginY = yPos
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

    }
}
