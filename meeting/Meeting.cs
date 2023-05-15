namespace meeting;

public class Meeting
{
    internal int _guid { get; }
    internal DateTime _startDateTime;
    internal DateTime _endDateTime;
    internal string title;
    internal string description;
    internal DateTime _notification;

    public Meeting(int id, DateTime startDateTime, DateTime endDateTime, string title, string description, DateTime timeNotification)
    {
        _guid = id;
        _startDateTime = startDateTime;
        _endDateTime = endDateTime;
        this.title = title;
        this.description = description;
        _notification = timeNotification;
    }
    

    public override string ToString()
    {
        return $"""
            -----------------------------------
            ID - {_guid}
            Начало встречи - {_startDateTime}
            Окончанее встрече - {_endDateTime}
            Тема - {title}
            Описание
            {description}
            """;
    }

    internal void editNotification(DateTime dateTimeNotifocation)
    {
        _notification = dateTimeNotifocation;
    }
    
    public void addNotification(DateTime dateTimeNotifocation)
    {
        _notification = dateTimeNotifocation;
    }
}