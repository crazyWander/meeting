using System.Globalization;
using System.Text;

namespace meeting;

class Meeting
{
    internal int _guid { get; }
    internal DateTime _startDateTime;
    internal DateTime _endDateTime;
    internal string title;
    internal string description;

    public Meeting(int id, DateTime startDateTime, DateTime endDateTime, string title, string description)
    {
        _guid = id;
        _startDateTime = startDateTime;
        _endDateTime = endDateTime;
        this.title = title;
        this.description = description;
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
}

public class ListMeeting
{
    private List<Meeting> listMeetings = new List<Meeting>();

    /// <summary>
    /// Добавление Встречи
    /// </summary>
    /// <param name="dateStart">Дата начала dd.MM.yyyy HH:mm</param>
    /// <param name="dateEnd">Планируемая дата окончания dd.MM.yyyy HH:mm</param>
    /// <param name="title">Тема</param>
    /// <param name="description">Описание</param>
    ///<returns>true - если добавлено успешно</returns>
    public bool addMeeting(DateTime dateStart, DateTime dateEnd, string title, string description)
    {
        var newMeeting = new Meeting(
            listMeetings.Count + 1,
            dateStart,
            dateEnd,
            title, description);
        
        if(listMeetings.Count == 0 || isValidDate(listMeetings, newMeeting))
        {
            listMeetings.Add(
                new Meeting(
                    listMeetings.Count + 1,
                    dateStart,
                    dateEnd,
                    title, description));
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

    void deleteMeeting(int id)
    {
        listMeetings.RemoveAll(x => x._guid == id);
    }

    void updateMeeting(int id, string dateStart, string dateEnd, string title, string description)
    {
        Meeting meeting = listMeetings.Find(x => x._guid == id);
        if (meeting != null)
        {
            meeting._startDateTime = parseData(dateStart);
            meeting._endDateTime = parseData(dateEnd);
            meeting.title = title;
            meeting.description = description;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
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
    bool isValidDate(List<Meeting> listMeeting, Meeting newMeeting)
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
    
}