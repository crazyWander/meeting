using System.Globalization;
using System.Text;

namespace meeting;

public class ListMeeting
{
    internal List<Meeting> listMeetings = new List<Meeting>();
    private SaveManager _manager = new SaveManager();

    /// <summary>
    /// Добавление Встречи
    /// </summary>
    /// <param name="dateStart">Дата начала dd.MM.yyyy HH:mm</param>
    /// <param name="dateEnd">Планируемая дата окончания dd.MM.yyyy HH:mm</param>
    /// <param name="title">Тема</param>
    /// <param name="description">Описание</param>
    ///<returns>Возвращает созданный и добавленный класс Meeting, или null в случае ошибки</returns>
    public Meeting addMeeting(DateTime dateStart, DateTime dateEnd, string title, string description, DateTime timeNotification)
    {
        var newMeeting = createMeeting(
            dateStart,
            dateEnd,
            title, 
            description,
            timeNotification);
        
        if(isValidDate(newMeeting))
        {
            listMeetings.Add(newMeeting);
            return newMeeting;
        }
        return null;
    }

    internal bool updateDateNotification(int id, DateTime dateTime)
    {
        Meeting meeting = listMeetings.Find(x => x._guid == id);
        if (meeting != null)
        {
            meeting.editNotification(dateTime);
            return true;
        }

        return false;
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
    
    internal StringBuilder getDayMeetingList(DateTime dateTime)
    {
        var dayMeeting = listMeetings.Where(x => x._startDateTime.Date == dateTime.Date).ToList();
        StringBuilder listMeeting = new StringBuilder();
        foreach (var meeting in dayMeeting)
        {
            listMeeting.Append(meeting);
        }

        return listMeeting;
    }

    internal int deleteMeeting(int id)
    {
        return listMeetings.RemoveAll(x => x._guid == id);
    }

    /// <summary>
    /// Изменение данных встречи
    /// </summary>
    /// <param name="id">Обязательно, ID встречи</param>
    /// <param name="dateStart">Дата начала (Пусто, если без изменения)</param>
    /// <param name="dateEnd">Дата окончания (Пусто, если без изменения)</param>
    /// <param name="title">Тема встречи (Пусто, если без изменения)</param>
    /// <param name="description">Описание (Пусто, если без изменения)</param>
    internal Meeting updateMeeting(int id, DateTime? dateStart, DateTime? dateEnd, string title, string description)
    {
        Meeting meeting = listMeetings.Find(x => x._guid == id);
        if (meeting != null)
        {
            if(dateStart != null) meeting._startDateTime = dateStart.Value;
            if (dateEnd != null) meeting._endDateTime = dateEnd.Value;
            if (String.IsNullOrEmpty(title)) meeting.title = title;
            if (String.IsNullOrEmpty(description)) meeting.description = description;
        }
        return meeting;
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

    public void saveFile(string path)
    {
        _manager.WriteDataToFileAsync(listMeetings, path);
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
    internal bool isValidDate(Meeting newMeeting)
    {
        if (listMeetings.Count == 0)
            return true;
        return !listMeetings.Any(
            m =>
                (newMeeting._startDateTime >= m._startDateTime && newMeeting._startDateTime < m._endDateTime) ||
                (newMeeting._endDateTime > m._startDateTime && newMeeting._endDateTime <= m._endDateTime) ||
                (newMeeting._startDateTime <= m._startDateTime && newMeeting._endDateTime >= m._endDateTime)
        );
    }

    internal bool isValidDate(DateTime dateTimeStart, DateTime dateTimeEnd, Meeting meeting = null)
    {
        if (dateTimeStart > dateTimeEnd) return false;
        if (listMeetings.Count == 0) return true;

        return !listMeetings.Where(m => (m != meeting) && (listMeetings.Count > 1)).Any(m =>
                (dateTimeStart >= m._startDateTime && dateTimeStart < m._endDateTime) ||
                (dateTimeEnd > m._startDateTime && dateTimeEnd <= m._endDateTime) ||
                (dateTimeStart <= m._startDateTime && dateTimeEnd >= m._endDateTime)
            );
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
            id = listMeetings[listMeetings.Count-1]._guid + 1;
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
            // Если список встреч пуст, вернуть значение по умолчанию, не
            return DateTime.MinValue;
        }

        DateTime earliestDate = listMeetings
            .Where(m => m._notification >= now)
            .Min(m => m._notification);

        return earliestDate;
    }
}