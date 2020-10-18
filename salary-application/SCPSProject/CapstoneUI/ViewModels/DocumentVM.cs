using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneUI.ViewModels
{
  public class DocumentVM
  {
    public int Id { get; set; }
    public string Code { get; set; }
    public DateTimeOffset? SignDate { get; set; }
    public DateTimeOffset? ApplyDate { get; set; }
    public int CloseDay { get; set; }
    public int Deadline { get; set; }
    public string Description { get; set; }
    public int? FormulaId { get; set; }
    public string DocumentUrl { get; set; }
    public string Formula { get; set; }

    public FormulaVM FormulaVM { get; set; }
  }

  public class DocumentUpdateVM
  {
    public int Id { get; set; }
    public string Code { get; set; }
    public DateTimeOffset SignDate { get; set; }
    public DateTimeOffset ApplyDate { get; set; }
    public int CloseDay { get; set; }
    public int Deadline { get; set; }
    public string Description { get; set; }
    public int FormulaId { get; set; }
    public bool? Status { get; set; }
    public string DocumentUrl { get; set; }
  }
  public class DocumentUpdateImgVM
  {
    public string Code { get; set; }
    public DateTimeOffset SignDate { get; set; }
    public DateTimeOffset ApplyDate { get; set; }
    public int CloseDay { get; set; }
    public int Deadline { get; set; }
    public string Description { get; set; }
    public int FormulaId { get; set; }
    public int Status { get; set; }
    public IFormFile Files { get; set; }
  }
  public class DocumentCreateVM
  {
    public string Code { get; set; }
    public DateTimeOffset SignDate { get; set; }
    public DateTimeOffset ApplyDate { get; set; }
    public int CloseDay { get; set; }
    public int Deadline { get; set; }
    public string Description { get; set; }
    public string DocumentUrl { get; set; }
    public FormulaNCVM Formula { get; set; }
  }

  public class DocumentActiveVM
  {
    public int Id { get; set; }
    public string Code { get; set; }
    public DateTimeOffset? SignDate { get; set; }
    public DateTimeOffset? ApplyDate { get; set; }
    public DateTimeOffset FromDate { get; set; }
    public DateTimeOffset ToDate { get; set; }
    public DateTimeOffset PayDate { get; set; }
    public string Formula { get; set; }
  }
}
