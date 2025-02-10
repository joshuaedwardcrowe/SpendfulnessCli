using System.Reflection;

namespace YnabProgressConsole.Commands;

public static class AssemblyExtensions
{
    public static List<Type> WhereClassTypesImplementType(this Assembly assembly, Type thatImplementType)
        => assembly
            .GetTypes()
            .WhereClassTypesImplement(thatImplementType)
            .ToList();
}