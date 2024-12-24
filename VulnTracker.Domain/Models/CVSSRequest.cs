using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VulnTracker.Domain.Models
{
    public class CVSSRequest
    {
        public double BaseScore { get; set; } // Taban puanı
        public double TemporalScore { get; set; } // Zamana bağlı puan
        public double EnvironmentalScore { get; set; } // Çevresel puan
    }
}
