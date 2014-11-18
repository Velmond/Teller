namespace Teller.Common.DataGenerators
{
    using System;
    using System.Text;

    public class RandomGenerator : IRandomGenerator
    {
        private const string Letters = "ABCDEFGHIJKLMNOPQRSTUWXVYZabcdefghijklmnopqrstuwxvyz";

        private readonly Random random;

        public RandomGenerator()
        {
            this.random = new Random();
        }

        public string RandomString(int minLength = 5, int maxLength = 50)
        {
            var result = new StringBuilder();
            var length = this.RandomNumber(minLength, maxLength + 1);

            for (int i = 0; i <= length; i++)
            {
                result.Append(Letters[this.RandomNumber(0, Letters.Length - 1)]);
            }

            return result.ToString();
        }

        public int RandomNumber(int min, int max)
        {
            if (max < min)
            {
                var temp = min;
                min = max;
                max = temp;
            }

            return this.random.Next(min, max + 1);
        }

        public DateTime RandomDate(DateTime minDate, DateTime maxDate)
        {
            if (maxDate < minDate)
            {
                var temp = minDate;
                minDate = maxDate;
                maxDate = temp;
            }
            
            var timeDiffInHours = (maxDate - minDate).Hours;

            return minDate.AddHours(this.RandomNumber(0, timeDiffInHours));
        }
    }
}
