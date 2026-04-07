namespace Finlay.PharmaVigilance.Domain.Enum;

public static class EnumHelper<T>
{
    // public static bool IsValid(string t) =>
    //     System.Enum.TryParse(typeof(T), t, out _);

    public static IReadOnlyCollection<string> AllRoles() =>
        System.Enum.GetNames(typeof(T));

    public static bool IsValid(string role)
    {
        var all = AllRoles();

        foreach (var a in all)
        {
            if (role == a) return true;
        }

        return false;
    }
}