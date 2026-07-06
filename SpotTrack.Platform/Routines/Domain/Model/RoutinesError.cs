namespace SpotTrack.Platform.Routines.Domain.Model;

public enum RoutinesError
{
    RoutineNotFound,
    RoutineSessionNotFound,
    ExerciseBlockNotFound,
    ClientNotFound,
    AccessDenied,
    InvalidRoutineData,
    InvalidExerciseData,
    InvalidSessionData,
    OperationCancelled,
    DatabaseError,
    InternalServerError
}
