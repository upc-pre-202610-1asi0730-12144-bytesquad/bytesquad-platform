namespace SpotTrack.Platform.Routines.Domain.Model.Commands;

public record SetExerciseBlockCompletionCommand(int RoutineSessionId, int ExerciseBlockId, bool Completed);
