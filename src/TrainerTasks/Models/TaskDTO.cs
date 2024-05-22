namespace MmsApiV2.TrainerTasks.Models;

/// <summary>
/// Represents a task in the system.
/// </summary>
public class TaskDTO
{
    /// <summary>
    /// The ID of the task.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// The name of the task.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Task type<br/>
    /// Allowed values: GENERAL, ANAMNESIS, COORDINATION_FITNESS_TEST, HEALTH_FITNESS_TEST,<br/>
    /// PWC_FITNESS_TEST, MAX_FORCE_FITNESS_TEST, MOBILITY_SCREEN_FITNESS_TEST,<br/>
    /// BLOOD_PRESSURE_FITNESS_TEST, POLAR_FITNESS_TEST, INBODY_FITNESS_TEST, JAWON_FITNESS_TEST
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    /// Task completion deadline date.<br/>
    /// Format: yyyy-MM-dd
    /// </summary>
    public string DueDate { get; set; }

    /// <summary>
    /// The author of the task. EGYM account ID.
    /// </summary>
    public string AuthorId { get; set; }

    /// <summary>
    /// The account of task related to. EGYM account ID.
    /// </summary>
    public string TargetId { get; set; }

    /// <summary>
    /// The account who solved the task. EGYM account ID.
    /// </summary>
    public string? CompleterId { get; set; }

    /// <summary>
    /// If this task has been completed in the gym this will be true, false otherwise.
    /// </summary>
    public bool Completed { get; set; }
}
