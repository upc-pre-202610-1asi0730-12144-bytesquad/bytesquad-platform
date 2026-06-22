namespace SpotTrack.Platform.Routines.Domain.Model;

public enum RoutinesError
{
    RoutineNotFound,
    RoutineSessionNotFound,
    ExerciseBlockNotFound,
    InvalidRoutineData,
    InvalidExerciseData,
    InvalidSessionData,
    OperationCancelled,
    DatabaseError,
    InternalServerError
}
