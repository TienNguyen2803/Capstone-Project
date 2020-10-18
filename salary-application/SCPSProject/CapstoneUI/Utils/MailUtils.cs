using CapstoneUI.ViewModels;
using DataAccess.Entities;
using HtmlAgilityPack;
using Mammoth;
using MimeKit;
using SautinSoft;
using SautinSoft.Document;
using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using Syncfusion.DocIORenderer;
using Syncfusion.Pdf;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CapstoneUI.Utils
{
  public class MailUtils
  {

    public void SendEmail(string messs, string email,string month,string year)
    {

      var mimeMessage = new MimeMessage();

      mimeMessage.From.Add(new MailboxAddress("Tien Nguyen", "TienNMSE62756@fpt.edu.vn"));

      mimeMessage.To.Add(new MailboxAddress(email));

 
      mimeMessage.Subject = "Bảng Lương Tháng " +month + "/ " + year;
 

      mimeMessage.Body = new TextPart("html")
      {
        Text = messs
      };

      using (var client = new MailKit.Net.Smtp.SmtpClient())
      {
        client.ServerCertificateValidationCallback = (s, c, h, e) => true;
        //host , port , usse
        client.Connect("smtp.gmail.com", 465, true);

        client.Authenticate("TienNMSE62756@fpt.edu.vn", "hqzcvkypjpnbqbcn");
        client.Send(mimeMessage);
        client.Disconnect(true);
      }


    }

    public void OpenWord(string[] fieldNames, string[] fieldValues, string email, string template)
    {
      var fullPath = Directory.GetCurrentDirectory() + @"\Resources\DocumentTemplate";
      if ((fullPath + "/Result.pdf").Equals(fullPath + "/Result.pdf"))
      {
        File.Delete(fullPath + "/Result.pdf");
      }
      //string[] listValue = item.Split(",");

      //Opens the Word template document
      FileStream fileStreamPath = new FileStream(fullPath + "/" + template, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
      using (WordDocument document = new WordDocument(fileStreamPath, FormatType.Docx))
      {
        //Performs the mail merge
        document.MailMerge.Execute(fieldNames, fieldValues);

        //Saves the Word document to  MemoryStream
        MemoryStream stream = new MemoryStream();
        document.Save(stream, FormatType.Docx);

        //convert word to pdf

        DocIORenderer render = new DocIORenderer();
        PdfDocument pdfDocument = render.ConvertToPDF(document);
        //Release the resources used by the Word document and DocIO Renderer objects.
        render.Dispose();
        string path = fullPath + "/Result.pdf";
        //Saves the PDF file.
        using (FileStream outputStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
        {
          pdfDocument.Save(outputStream);
          //Closes the instance of PDF document object.
          pdfDocument.Close();
          //Dispose the instance of FileStream.
          outputStream.Dispose();
        }



        //// convert pdf to html 
        PdfFocus f = new PdfFocus();
        f.OpenPdf(path);
        if (f.PageCount > 0)
        {
          f.HtmlOptions.IncludeImageInHtml = true;
          f.HtmlOptions.InlineCSS = true;
          string html = f.ToHtml();
          //SendEmail(html, email);

        }
        f.ClosePdf();
        File.Delete(path);
      }


    }
    public string ReviewDataTemplate(Dictionary<string, string> list, string filename)
    {
      var path = Directory.GetCurrentDirectory() + @"\Resources\DocumentTemplate\";
      var inputFile = path + filename;
      //string outFile = path+@"Result.pdf";

      var html = File.ReadAllText(inputFile, Encoding.UTF8);
      HtmlDocument doc = new HtmlDocument();
      doc.LoadHtml(html);


      foreach (var item in list)
      {
        string pattern = @"^[-+]?[0-9]*\.?[0-9]*$";


        if (Regex.IsMatch(item.Value, pattern) && !item.Key.Equals("&lt;&lt;Tháng&gt;&gt;") && !item.Key.Equals("&lt;&lt;Năm&gt;&gt;") && !item.Value.Equals("0")) {
          CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");   // try with "en-US"
          string a = Math.Round(double.Parse(item.Value)).ToString("#,###", cul.NumberFormat);

          doc.Text = doc.Text.Replace(item.Key, a);
        }
        else
        {
          doc.Text = doc.Text.Replace(item.Key, item.Value);
        }
        
        
      }
      return doc.Text;
    }
    public void SendMailHtml(Dictionary<string, string> list, string filename, string email,string month,string year)
    {
      var path = Directory.GetCurrentDirectory() + @"\Resources\DocumentTemplate\";
      var inputFile = path + filename;
      //string outFile = path+@"Result.pdf";

      var html = File.ReadAllText(inputFile, Encoding.UTF8);
      HtmlDocument doc = new HtmlDocument();
      doc.LoadHtml(html);


      foreach (var item in list)
      {
        string pattern = @"^[-+]?[0-9]*\.?[0-9]*$";

        if (Regex.IsMatch(item.Value, pattern) && !item.Key.Equals("&lt;&lt;Tháng&gt;&gt;") && !item.Key.Equals("&lt;&lt;Năm&gt;&gt;") && !item.Value.Equals("0"))
        {
          CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");   // try with "en-US"
          string a = Math.Round(double.Parse(item.Value)).ToString("#,###", cul.NumberFormat);

          doc.Text = doc.Text.Replace(item.Key, a);
        }
        else
        {
          doc.Text = doc.Text.Replace(item.Key, item.Value);
        }
      }

      doc.Text = doc.Text.Replace("<table>", "<table style='width: 100%;border-spacing: 0;'>");
      doc.Text = doc.Text.Replace("<td>", "<td style=' padding: 5px 10px;width: 100px;border: 1px solid #000000 !important;'>");
      doc.Text = doc.Text.Replace("<span>", "<span style='background-color: #ffffff !important;color: #000000 !important;'>");

      SendEmail(doc.Text, email,month,year);

    }
    public void DeleteImage(string path)
    {
      File.Delete(path);
    }


    public bool DeleteTemplateDocument(string path)
    {
      var fullPath = Directory.GetCurrentDirectory();
      File.Delete(fullPath + @"\Resources\DocumentTemplate\" + path);
      return true;
    }
    public Dictionary<string, string> AddValue(Dictionary<string, string> keyValues, BusinessLogic.Implement.FormulaTypeSSVM result)
    {
      foreach (var field in result.Details)
      {
        switch (field.Type)
        {
          case 1:
            if (keyValues.ContainsKey("&lt;&lt;" + field.FieldType.Name.Trim().Replace(" ", "_")+ "&gt;&gt;"))
            {
              break;
            }
            else
            {
              keyValues.Add("&lt;&lt;" + field.FieldType.Name.Trim().Replace(" ", "_")+ "&gt;&gt;", field.FieldType.Value);
            }
            break;
          case 2:
            if (keyValues.ContainsKey("&lt;&lt;" + field.RefType.Name.Trim().Replace(" ", "_")+ "&gt;&gt;"))
            {
              break;
            }
            else
            {
              keyValues.Add("&lt;&lt;" + field.RefType.Name.Trim().Replace(" ", "_")+ "&gt;&gt;", field.RefType.Value);
            }
            break;
          case 3:
            if (keyValues.ContainsKey("&lt;&lt;" + field.FormulaType.Name.Trim().Replace(" ", "_")+ "&gt;&gt;"))
            {
              break;
            }
            else
            {
              keyValues.Add("&lt;&lt;" + field.FormulaType.Name.Trim().Replace(" ", "_")+ "&gt;&gt;", field.FormulaType.Value.ToString());
              BusinessLogic.Implement.FormulaTypeSSVM formula = field.FormulaType;
              AddValue(keyValues, formula);

            }
            break;
          case 4:
            break;
        }

      }
      return keyValues;
    }
    public Dictionary<string, string> AddValue(Dictionary<string, string> keyValues, FormulaTypeSSVM result)
    {
      foreach (var field in result.Details)
      {
        switch (field.Type)
        {
          case 1:
            if (keyValues.ContainsKey("&lt;&lt;" + field.FieldType.Name.Replace(" ", "_")+ "&gt;&gt;"))
            {
              break;
            }
            else
            {
              keyValues.Add("&lt;&lt;" + field.FieldType.Name.Replace(" ", "_")+ "&gt;&gt;", field.FieldType.Value);
            }
            break;
          case 2:
            if (keyValues.ContainsKey("&lt;&lt;" + field.RefType.Name.Replace(" ", "_")+ "&gt;&gt;"))
            {
              break;
            }
            else
            {
              keyValues.Add("&lt;&lt;" + field.RefType.Name.Replace(" ", "_")+ "&gt;&gt;", field.RefType.Value);
            }
            break;
          case 3:
            if (keyValues.ContainsKey("&lt;&lt;" + field.FormulaType.Name.Replace(" ", "_") + "&gt;&gt;"))
            {
              break;
            }
            else
            {
              keyValues.Add("&lt;&lt;" + field.FormulaType.Name.Replace(" ", "_")+ "&gt;&gt;", field.FormulaType.Value.ToString());
              FormulaTypeSSVM formula = field.FormulaType;
              AddValue(keyValues, formula);

            }
            break;
          case 4:
            break;
        }

      }
      return keyValues;
    }

    public void Upload(string template, string name)
    {

      if (name.Split(".")[1].Equals("docx"))
      {
        var fullPath = Directory.GetCurrentDirectory();
        byte[] decodedBytes = Convert.FromBase64String(template);
        File.WriteAllBytes(fullPath + @"\Resources\DocumentTemplate\" + name, decodedBytes);
      }
      else
      {
        DocumentCore dc = new DocumentCore();
        var fullPath = Directory.GetCurrentDirectory() + @"\Resources\DocumentTemplate\test.docx";
        dc.Save(fullPath);
        byte[] decodedBytes = Convert.FromBase64String(template);
        File.WriteAllBytes(fullPath, decodedBytes);
      }

    }

    public void SaveTemplate(string path)
    {
      var fullPath = Directory.GetCurrentDirectory();
      List<string> files = new DirectoryInfo(fullPath + @"\Resources\DocumentTemplate").GetFiles().Select(o => o.Name).ToList();
      if (files.Count > 0)
      {
        foreach (var item in files)
        {
          var x = item.Split("_")[1].Split(".")[0];
          var k = path.Split(".")[0];
          if (x.Equals(k))
          {
            File.Delete(fullPath + @"\Resources\DocumentTemplate\" + item);
          }
        }

      }
    }
    public string HtmlString(string fileName)
    {
      //get root path
      var fullPath = Directory.GetCurrentDirectory() + @"\Resources\DocumentTemplate\";
      //get list file name
      List<string> files = new DirectoryInfo(fullPath).GetFiles().Select(o => o.Name).ToList();
      var pathFile = "";
      // compare file name with list file name
      //get file name .doc or html
      foreach (var item in files)
      {
        if (item.Split(".")[0].Equals(fileName))
        {
          pathFile = item;
        }
      }
      //compare html or docx to return string
      if (pathFile.Split(".")[1].Equals("html"))
      {
        //convert html file to docx
        //DocumentCore dc = DocumentCore.Load(fullPath+pathFile);
        var html = File.ReadAllText(fullPath + pathFile, Encoding.UTF8);
        HtmlDocument doc = new HtmlDocument();
        doc.LoadHtml(html);
        return doc.Text;
      }
      else
      {
        //convert docx to html string make up
        var converter = new DocumentConverter();
        var result = converter.ConvertToHtml(fullPath + pathFile);
        var html = result.Value; // The generated HTML
        return html;
      }

    }

  }
}
