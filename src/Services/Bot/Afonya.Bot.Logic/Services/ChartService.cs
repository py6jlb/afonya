using Afonya.Bot.Domain.Entities;
using Afonya.Bot.Interfaces.Services;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.SkiaSharp;
using Shared.Contracts;

namespace Afonya.Bot.Logic.Services;

public class ChartService : IChartService
{
    public Task<string> GetStatisticPng(string title, IEnumerable<MoneyTransactionDto> transactions, IEnumerable<Category> categories)
    {
        var fileName = Path.GetRandomFileName();
        var path = $"/home/py6jlb/tmp/{fileName}.png";
        var model = new PlotModel
        {
            Title = title,
            Background = OxyColors.White
        };
        var categoryAxis = new CategoryAxis { Position = AxisPosition.Left };
        foreach (var c in categories){
            categoryAxis.Labels.Add(c.HumanName);
            var s = new BarSeries { LabelFormatString = "{0}", LabelPlacement = LabelPlacement.Base, Title = "Base" };
            var minus = transactions.Where(t => t.CategoryName == c.Name && t.Sign == "-").Sum(t => t.Value);
            var plus = transactions.Where(t => t.CategoryName == c.Name && t.Sign == "+").Sum(t => t.Value);
            s.Items.Add(new BarItem() { Value = minus - plus });
            model.Series.Add(s);
        }

        var valueAxis = new LinearAxis { Position = AxisPosition.Bottom, MinimumPadding = 0.06, MaximumPadding = 0.06, ExtraGridlines = new[] { 0d } };
        model.Axes.Add(categoryAxis);
        model.Axes.Add(valueAxis);

        PngExporter.Export(model, path, 640, 1024);
        return Task.FromResult(path);
    }
}
