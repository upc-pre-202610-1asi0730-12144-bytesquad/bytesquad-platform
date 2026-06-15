namespace SpotTrack.Platform.Routines.Domain.Model.Commands;

public record CreateRoutineCommand(
    int ClientId, 
    string RoutineName)
{
}