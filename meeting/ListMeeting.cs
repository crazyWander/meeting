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
    public Meeting addMeeting(DateTime dateStart, DateTime dateEnd, string title, string description,
        DateTime timeNotification)
    {
        var newMeeting = createMeeting(
            dateStart,
            dateEnd,
            title,
            description,
            timeNotification);

        if (isValidDate(newMeeting))
        {
            listMeetings.Add(newMeeting);
            return newMeeting;
        }

        return null;
    }

    internal bool updateDateNotification(int id, DateTime dateTime)
    {
        Meeting meeting = listMeetings.Find(x => x.Guid == id);
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
        var dayMeeting = listMeetings.Where(x => x.StartDateTime.Date == dateTime.Date).ToList();
        StringBuilder listMeeting = new StringBuilder();
        foreach (var meeting in dayMeeting)
        {
            listMeeting.Append(meeting);
        }

        return listMeeting;
    }

    internal int deleteMeeting(int id)
    {
        return listMeetings.RemoveAll(x => x.Guid == id);
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
        Meeting meeting = listMeetings.Find(x => x.Guid == id);
        if (meeting != null)
        {
            if (dateStart != null) meeting.StartDateTime = dateStart.Value;
            if (dateEnd != null) meeting.EndDateTime = dateEnd.Value;
            if (String.IsNullOrEmpty(title)) meeting.Title = title;
            if (String.IsNullOrEmpty(description)) meeting.Description = description;
        }

        return meeting;
    }

    public Meeting? getMeeting(int guid)
    {
        var meeting = listMeetings.Find(x => x.Guid == guid);
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
                (newMeeting.StartDateTime >= m.StartDateTime && newMeeting.StartDateTime < m.EndDateTime) ||
                (newMeeting.EndDateTime > m.StartDateTime && newMeeting.EndDateTime <= m.EndDateTime) ||
                (newMeeting.StartDateTime <= m.StartDateTime && newMeeting.EndDateTime >= m.EndDateTime)
        );
    }

    internal bool isValidDate(DateTime dateTimeStart, DateTime dateTimeEnd, Meeting? meeting = null)
    {
        if (dateTimeStart > dateTimeEnd) return false;
        if (listMeetings.Count == 0) return true;

        return !listMeetings.Where(m => (m != meeting) && (listMeetings.Count > 1)).Any(m =>
            (dateTimeStart >= m.StartDateTime && dateTimeStart < m.EndDateTime) ||
            (dateTimeEnd > m.StartDateTime && dateTimeEnd <= m.EndDateTime) ||
            (dateTimeStart <= m.StartDateTime && dateTimeEnd >= m.EndDateTime)
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
    Meeting createMeeting(DateTime dateStart, DateTime dateEnd, string title, string description,
        DateTime timeNotification)
    {
        var id = 1;
        if (listMeetings.Count > 0)
        {
            id = listMeetings[listMeetings.Count - 1].Guid + 1;
        }

        return new Meeting(
            id,
            dateStart,
            dateEnd,
            title, description,
            timeNotification);
    }

    internal Meeting GetEarliestNotificationDate()
    {
        //Вычитается минута
        DateTime now = DateTime.Now.AddMinutes(-1);

        if (listMeetings.Count == 0)
        {
            // Если список встреч пуст, вернуть значение по умолчанию, не
            return null;
        }

        Meeting earliestDate = listMeetings
            .Where(m => m.Notification >= now)
            .OrderBy(m => m.Notification)
            .FirstOrDefault();

        return earliestDate;
    }
}