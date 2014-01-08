using System;

namespace ShinobiStockChart.Core.Presenter
{
    public class BasePresenter
    {
        public string Title { get; private set; }

        public BasePresenter (string title)
        {
            Title = title;
        }
    }
}

