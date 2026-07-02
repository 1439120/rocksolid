using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp.Models
{
    public class CardInfo
    {
        public string Title { get; set; } ="";
        public string Description { get; set; } ="";
        public string Icon { get; set; } ="";
        public string SmallDescription { get; set; } ="";
        public LoanTypes LoanType{get;set;} = LoanTypes.None;
    }
}