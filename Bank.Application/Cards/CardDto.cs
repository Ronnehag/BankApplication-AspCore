using System;
using System.Linq;

namespace Bank.Application.Cards
{
    public class CardDto
    {
        private string _ccNumber;
        public string Type { get; set; }
        public DateTime Issued { get; set; }
        public string CcType { get; set; }
        public int ExpM { get; set; }
        public int ExpY { get; set; }

        public string CcNumber
        {
            get => _ccNumber.Substring(0, 4) + " **** **** ***" + _ccNumber.Last();
            set => _ccNumber = value;
        }
    }
}
