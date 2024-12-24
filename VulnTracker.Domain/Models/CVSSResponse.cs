using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VulnTracker.Domain.Models
{
    public class CVSSResponse
    {
        public double FinalScore { get; set; } // Nihai puan
        public string Severity { get; set; } // Ciddiyet seviyesi
    }
}
