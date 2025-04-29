namespace API.Source.Libs;

public record struct Appointment(string Host, DateTime Start, DateTime End, int Room);
