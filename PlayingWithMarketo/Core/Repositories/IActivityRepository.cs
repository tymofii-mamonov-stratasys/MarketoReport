namespace PlayingWithMarketo.Core.Repositories
{
    public interface IActivityRepository
    {
        string GetActivity(int activityId);
        int GetId(int activityId);
    }
}
