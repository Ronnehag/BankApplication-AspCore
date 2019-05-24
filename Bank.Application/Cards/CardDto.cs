using System;

namespace Bank.Application.Cards
{
    public class CardDto
    {
        public string Type { get; set; }
        public DateTime Issued { get; set; }
        public string CcType { get; set; }
        public int ExpM { get; set; }
        public int ExpY { get; set; }
        public string CcNumber { get; set; }

        //TODO visa bara 4 första sen ******
    }
}
