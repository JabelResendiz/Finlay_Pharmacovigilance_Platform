namespace Finlay.PharmaVigilance.Domain.Helper;


public static class IdentityNumberHelper
{
    public static DateTime DateOfBirth { get; }
    public static int Age => CalculateAge(DateOfBirth);

    public static void Validate(string ci)
    {
        if (string.IsNullOrWhiteSpace(ci))
            throw new ArgumentException();

        if (ci.Length != 11)
            throw new ArgumentException();

        if (!ci.All(char.IsDigit))
            throw new ArgumentException();

        ExtractDateOfBirth(ci);
    }


    public static DateTime ExtractDateOfBirth(string ci)
    {
        string yy = ci.Substring(0, 2);
        string mm = ci.Substring(2, 2);
        string dd = ci.Substring(4, 2);

        int year = int.Parse(yy);
        int month = int.Parse(mm);
        int day = int.Parse(dd);

        int currentYearTwoDigits = DateTime.Now.Year % 100;
        int fullYear = (year > currentYearTwoDigits) ? 1900 + year : 2000 + year;

        try
        {
            return new DateTime(fullYear, month, day);
        }
        catch
        {
            throw new ArgumentException("Invalid date encoded in identity number.");
        }
    }


    public static int CalculateAge(DateTime dateOfBirth)
    {
        var today = DateTime.Today;
        int age = today.Year - dateOfBirth.Year;

        if (dateOfBirth.Date > today.AddYears(-age))
            age--;

        return age;
    }

}