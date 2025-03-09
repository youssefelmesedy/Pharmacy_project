namespace Option.Notification
{
    public interface INotificationService
    {
        Task SendAsync(NotificationMessage message);
    }
}
