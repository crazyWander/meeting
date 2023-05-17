namespace meeting;

public class Meeting
{
    //Переменная _guid названа так, в расчете на будущее
    //(по хорошему для ID лучше использовать GUID, но пользователю вводить будет сложно)
    internal int Guid { get; }
    internal DateTime StartDateTime;
    internal DateTime EndDateTime;
    internal string Title;
    internal string Description;
    internal DateTime Notification;

    public Meeting(int id, DateTime startDateTime, DateTime endDateTime, string title, string description,
        DateTime timeNotification)
    {
        Guid = id;
        StartDateTime = startDateTime;
        EndDateTime = endDateTime;
        Title = title;
        Description = description;
        Notification = timeNotification;
    }


    public override string ToString()
    {
        return $"""
            -----------------------------------
            ID - {Guid}
            Начало встречи - {StartDateTime}
            Окончанее встрече - {EndDateTime}
            Тема - {Title}
            Описание
            {Description}
            Напоминание - {Notification}

            """;
    }

    internal void editNotification(DateTime dateTimeNotifocation)
    {
        Notification = dateTimeNotifocation;
    }

    public void addNotification(DateTime dateTimeNotifocation)
    {
        Notification = dateTimeNotifocation;
    }
}