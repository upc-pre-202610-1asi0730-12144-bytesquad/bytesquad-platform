namespace SpotTrack.Platform.Routines.Domain.Model.Commands;

public record AddExerciseBlockCommand(int RoutineId, string ExerciseName, string ExerciseType, int Order);
