using static BugHunters.Api.Common.Result.ResultExt;

namespace BugHunters.Api.Entities.Values.Hunter;

public record ViaId
{
    public string Value { get; set; }

    private ViaId(string value) =>
        Value = value;

    public static Result<ViaId> FromString(string value) =>
        Create(Prepare(value));

    private static string Prepare(string value) =>
        value
            .ToLower()
            .Trim()
            .Replace("@via.dk", "")
            .Replace("@viauc.dk", "");

    private static Result<ViaId> Create(string value) =>
        StartValidation()
            .AssertThat(IsStudentNumberOrTeacherInitials(value))
            .WithPayloadIfSuccess(() => new ViaId(value));

    private static Func<Result<None>> IsStudentNumberOrTeacherInitials(string value) =>
        () => IsSixDigits(value) || IsValidInitials(value)
            ? Success()
            : new ResultError("Hunter.ViaId", "Invalid Via ID format. Must be 6 digits, or 3-4 letters.");

    private static bool IsValidInitials(string value) =>
        value.Length is >= 3 and <= 4 && value.All(char.IsLetter);

    private static bool IsSixDigits(string value) =>
        value.Length == 6 && value.All(char.IsDigit);

    private ViaId()
    {
    } // EFC
}