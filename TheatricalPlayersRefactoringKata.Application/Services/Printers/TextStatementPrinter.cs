﻿
using System.Globalization;
using TheatricalPlayersRefactoringKata.Application.Interfaces;
using TheatricalPlayersRefactoringKata.Application.Services.Factories;
using TheatricalPlayersRefactoringKata.Domain.Entities;

namespace TheatricalPlayersRefactoringKata.Application.Services.Printers
{
    public class TextStatementPrinter : IStatementPrinter
    {
        public string Print(Invoice invoice, Dictionary<string, Play> plays)
        {
            var totalAmount = 0m;
            var volumeCredits = 0;
            var result = string.Format("Statement for {0}\n", invoice.Customer);
            CultureInfo cultureInfo = new CultureInfo("en-US");

            foreach (var perf in invoice.Performances)
            {
                var play = plays[perf.PlayId];
                var calculator = GenreCalculatorFactory.GetCalculator(play.Type);

                var thisAmount = calculator.CalculateAmount(perf, play);
                totalAmount += thisAmount;

                volumeCredits += calculator.CalculateVolumeCredits(perf);

                result += string.Format(cultureInfo, "  {0}: {1:C} ({2} seats)\n", play.Name, Convert.ToDecimal(thisAmount / 100), perf.Audience);
            }

            result += string.Format(cultureInfo, "Amount owed is {0:C}\n", Convert.ToDecimal(totalAmount / 100));
            result += string.Format("You earned {0} credits\n", volumeCredits);

            return result;
        }
    }
}
