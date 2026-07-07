namespace SpotTrack.Platform.Routines.Domain.Model.Commands;

public record CompleteExerciseBlockCommand(int RoutineSessionId, int ExerciseBlockId, bool IsCompleted);
