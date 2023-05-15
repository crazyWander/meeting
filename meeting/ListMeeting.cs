using System.Globalization;
using System.Text;

namespace meeting;

public class ListMeeting
{
    internal List<Meeting> listMeetings = new List<Meeting>();
    /// <summary>
    /// Добавление Встречи
    /// </summary>
    /// <param name="dateStart">Дата начала dd.MM.yyyy HH:mm</param>
    /// <param name="dateEnd">Планируемая дата окончания dd.MM.yyyy HH:mm</param>
    /// <param name="title">Тема</param>
    /// <param name="description">Описание</param>
    ///<returns>true - если добавлено успешно</returns>
    public Meeting addMeeting(DateTime dateStart, DateTime dateEnd, string title, string description, DateTime timeNotification)
    {
        var newMeeting = createMeeting(
            dateStart,
            dateEnd,
            title, 
            description,
            timeNotification);
        
        if(listMeetings.Count == 0 || isValidDate(listMeetings, newMeeting))
        {
            listMeetings.Add(newMeeting);
            return newMeeting;
        }
        return null;
    }

    /// <summary>
    /// Получение всего списка встреч
    /// </summary>
    /// <returns></returns>
    internal StringBuilder getMeetingList()
    {
        StringBuilder listMeeting = new StringBuilder();
        foreach (var meeting in listMeetings)
        {
            listMeeting.Append(meeting);
        }

        return listMeeting;
    }

    void deleteMeeting(int id)
    {
        listMeetings.RemoveAll(x => x._guid == id);
    }

    void updateMeeting(int id, DateTime? dateStart, DateTime? dateEnd, string title, string description)
    {
        Meeting meeting = listMeetings.Find(x => x._guid == id);
        if (meeting != null)
        {
            if(dateStart != null) meeting._startDateTime = dateStart.Value;
            if (dateEnd != null) meeting._endDateTime = dateEnd.Value;
            if (String.IsNullOrEmpty(title)) meeting.title = title;
            if (String.IsNullOrEmpty(description)) meeting.description = description;
        }
    }

    public Meeting? getMeeting(int guid)
    {
        var meeting = listMeetings.Find(x => x._guid == guid);
        if (meeting != null)
        {
            return meeting;
        }
        return null;
    }
    
    DateTime parseData(string data)
    {
        return DateTime.ParseExact(data, "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Проверка что дата не пересекается
    /// </summary>
    /// <param name="listMeeting">Существующий список</param>
    /// <param name="newMeeting">Проверяемая встреча</param>
    /// <returns></returns>
    internal bool isValidDate(List<Meeting> listMeeting, Meeting newMeeting)
    {
        if (listMeetings.Count == 0)
            return true;
        return listMeetings.Any(
            m =>
                (m._startDateTime >= newMeeting._startDateTime
                 && newMeeting._startDateTime < m._endDateTime)
                || (newMeeting._endDateTime > m._startDateTime
                    && newMeeting._endDateTime <= m._endDateTime)
        );
    }

    internal bool isValidDate(DateTime dateTimeStart, DateTime dateTimeEnd)
    {
        if (listMeetings.Count == 0)
            return true;
        
        return !listMeetings.Any(m =>
                (dateTimeStart >= m._startDateTime && dateTimeStart < m._endDateTime) ||
                (dateTimeEnd > m._startDateTime && dateTimeEnd <= m._endDateTime) ||
                (dateTimeStart <= m._startDateTime && dateTimeEnd >= m._endDateTime)
            );
    }

    void addNotification(DateTime dateTimeNotifocation, int guid)
    {
        Meeting meeting = listMeetings.Find(x => x._guid == guid);
        if (meeting != null)
        {
            meeting.addNotification(dateTimeNotifocation);
        }
    }

    void editNotification(DateTime dateTimeNotifocation, int guid)
    {
        Meeting meeting = listMeetings.Find(n => n._guid == guid);
        if (meeting != null)
        {
            meeting.editNotification(dateTimeNotifocation);
        }
    }

    /// <summary>
    /// Создает встречу, ID едет по порядку создания
    /// </summary>
    /// <param name="dateStart"></param>
    /// <param name="dateEnd"></param>
    /// <param name="title"></param>
    /// <param name="description"></param>
    /// <returns></returns>
    Meeting createMeeting(DateTime dateStart, DateTime dateEnd, string title, string description, DateTime timeNotification)
    {
        var id = 1;
        if (listMeetings.Count > 1)
        {
            id = listMeetings[listMeetings.Count]._guid + 1;
        }
        return new Meeting(
            id,
            dateStart,
            dateEnd,
            title, description,
            timeNotification);
    }

    internal DateTime GetEarliestNotificationDate()
    {
        DateTime now = DateTime.Now;

        if (listMeetings.Count == 0)
        {
            // Если список встреч пуст, вернуть значение по умолчанию
            return DateTime.MinValue;
        }

        DateTime earliestDate = listMeetings
            .Where(m => m._notification >= now)
            .Min(m => m._notification);

        return earliestDate;
    }
}