namespace API.Source.Libs;

public record struct Appointment(Guid Id, string Host, DateTime Start, DateTime End, int Room);
