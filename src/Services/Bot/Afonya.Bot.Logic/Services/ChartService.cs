using System;
using OxyPlot;
using OxyPlot.SkiaSharp;

namespace Afonya.Bot.Logic.Services;

public class ChartService
{

    public void GetPng(string title)
    {
        var plotModel = new PlotModel(){Title = title};
        using (var stream = File.Create(""))
        {
            var pdfExporter = new PngExporter { Width = 600, Height = 400 };
            pdfExporter.Export(plotModel, stream);
        }
    }
}
