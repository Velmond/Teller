namespace Teller.Common.DataGenerators
{
    using System;

    public interface IRandomGenerator
    {
        string RandomString(int minLength, int maxLength);

        DateTime RandomDate(DateTime minDate, DateTime maxDate);

        int RandomNumber(int min, int max);
    }
}
