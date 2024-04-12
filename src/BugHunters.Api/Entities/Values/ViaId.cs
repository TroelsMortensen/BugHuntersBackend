namespace BugHunters.Api.Entities.Values;

public record ViaId
{
    public string Value { get; set; }

    private ViaId(string value)
        => Value = value;

    public static Result<ViaId> FromString(string value)
        => Result.StartValidation<ViaId>()
            .AssertThat(() => IsStudentNumberOrTeacherInitials(value), new ResultError("Hunter.ViaId", "Invalid ViaId format."))
            .ToResult()
            .WithPayloadIfSuccess(() => new ViaId(value));

    private static bool IsStudentNumberOrTeacherInitials(string value)
        => value.Length == 6 && value.All(char.IsDigit) ||
           (value.Length is >= 3 and <= 4 && value.All(char.IsLetter));
}