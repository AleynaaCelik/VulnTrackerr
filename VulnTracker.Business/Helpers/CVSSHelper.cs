using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VulnTracker.Domain.Models;

namespace VulnTracker.Business.Helpers
{
    public static class CVSSHelper
    {
        public static CVSSResponse CalculateCVSS(CVSSRequest request)
        {
            // Nihai puan hesaplama formülü
            var finalScore = request.BaseScore * request.TemporalScore * request.EnvironmentalScore;

            // Ciddiyet seviyesini belirleme
            var severity = finalScore switch
            {
                <= 3.9 => "Low",
                <= 6.9 => "Medium",
                <= 8.9 => "High",
                _ => "Critical"
            };

            return new CVSSResponse
            {
                FinalScore = Math.Round(finalScore, 2),
                Severity = severity
            };
        }
    }
}


