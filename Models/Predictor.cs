using Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Models
{
    public class Predictor : IPredictor
    {
        private IDataProvider dataProvider;

        public Predictor(IDataProvider dataProvider)
        {
            this.dataProvider = dataProvider;
        }
        public IEnumerable<(DateTime date, decimal amount, ICategory category)> Predict(int year, int month)
        {
            yield return (DateTime.Now.AddDays(1), 5555, dataProvider.Categories.First());
            // TODO !!!
        }
    }
}
