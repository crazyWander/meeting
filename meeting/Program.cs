using System.Globalization;
using System;
using meeting;

internal class Program
{
    private static ListMeeting _listMeeting = new ListMeeting();

    private static DateTime TimeNotification
    {
        get => _listMeeting.GetEarliestNotificationDate();
    }

    public static async Task Main(string[] args)
    {
        CheckDateTimeAsync();
        Console.WriteLine("Запуск программы");
        while (true)
        {
            Console.WriteLine("Введите команду");
            Console.WriteLine("1. Показать все совещания");
            Console.WriteLine("2. Создать совещание");
            Console.WriteLine("3. Изменить совещание");
            Console.WriteLine("4. Изменить дату напоминания");
            Console.WriteLine("5. Удалить совещание");
            Console.WriteLine("6. Сохранить в файл");
            var comand = Console.ReadLine();
            switch (comand)
            {
                case "1":
                    GetMeeting();
                    Console.WriteLine("Нажмите любую кнопку чтобы продолжить");
                    Console.ReadKey();
                    break;
                case "2":
                    CreateMeeting();
                    Console.WriteLine("Нажмите любую кнопку чтобы продолжить");
                    Console.ReadKey();
                    break;
                case "3":
                    UpdateMeeting();
                    Console.WriteLine("Нажмите любую кнопку чтобы продолжить");
                    Console.ReadKey();
                    break;
                case "4":
                    UpdateMeetingNofitication();
                    Console.WriteLine("Нажмите любую кнопку чтобы продолжить");
                    Console.ReadKey();
                    break;
                case "5":
                    RemoveMeeting();
                    Console.WriteLine("Нажмите любую кнопку чтобы продолжить");
                    Console.ReadKey();
                    break;
                case "6":
                    Console.WriteLine("Введите название файла JSON");
                    var fileName = Console.ReadLine();
                    SafeFile(fileName);
                    Console.WriteLine("Нажмите любую кнопку чтобы продолжить");
                    Console.ReadKey();
                    break;
                default:
                    Console.WriteLine("Комманда не найдена");
                    break;
            }
        }
    }

    private static void UpdateMeetingNofitication()
    {
        Meeting? meeting = null;
        int id = 0;
        DateTime dateTime = default;
        Console.WriteLine("Введите номер встечи для удаления");

        do
        {
            Console.WriteLine("Введите ID встречи");
            string input = Console.ReadLine();

            if (!Int32.TryParse(input, out id))
            {
                Console.WriteLine("Некорректный ввод. Пожалуйста, введите целое число.");
                continue;
            }

            meeting = _listMeeting.getMeeting(id);

            if (meeting == null)
            {
                Console.WriteLine("ID не найден");
                return;
            }

            dateTime = SetDate("Введите дату");

            if (_listMeeting.updateDateNotification(id, dateTime))
            {
                Console.WriteLine("Данные изменены");
            }
            else
            {
                Console.WriteLine("Неверный ID");
            }

        } while (meeting == null || !_listMeeting.updateDateNotification(id, dateTime));
    }

    private static void GetMeeting()
    {
        var date = setDateNullable("Введите дату (Пусто - показать все)");
        if (date != null)
        {
            Console.WriteLine(_listMeeting.getDayMeetingList(date.Value));
        }

        Console.WriteLine(_listMeeting.getMeetingList());
    }

    private static void RemoveMeeting()
    {
        Console.WriteLine("Введите номер встечи для удаления");
        int id = 0;
        
        
        Console.WriteLine("Введите ID встречи");
        string input = Console.ReadLine();

        if (!Int32.TryParse(input, out id))
        {
            Console.WriteLine("Некорректный ввод. Пожалуйста, введите целое число.");
            return;
        }

        var result = _listMeeting.deleteMeeting(id);

        if (result == 0)
        {
            Console.WriteLine("ID не найден");
        }
    }

    private static void SafeFile(string fileName)
    {
        _listMeeting.saveFile(fileName);
    }

    private static void UpdateMeeting()
    {
        bool isCorrect = true;
        DateTime? dateStart;
        DateTime? dateEnd;
        string title = "";
        string description = "";
        Meeting meeting = null;

        Console.WriteLine("Введите ID встречи");
        int id = 0;
        do
        {
            Console.WriteLine("Введите ID встречи");
            string input = Console.ReadLine();

            if (!Int32.TryParse(input, out id))
            {
                Console.WriteLine("Некорректный ввод. Пожалуйста, введите целое число.");
                continue;
            }

            meeting = _listMeeting.getMeeting(id);

            if (meeting == null)
            {
                Console.WriteLine("ID не найден");
            }

        } while (meeting == null);

        var isValidDate = false;
        while (!isValidDate)
        {
            dateStart = setDateNullable("Введите дату начала в формате dd.MM.yyyy HH:mm") ?? 
                        meeting._startDateTime;
            dateEnd = setDateNullable("Введите планируемую дату окончания в формате dd.MM.yyyy HH:mm") ??
                      meeting._endDateTime;
            isValidDate = _listMeeting.isValidDate(dateStart.Value, dateEnd.Value, meeting);
            if (!isValidDate)
            {
                Console.WriteLine("Встречи пересекаются");
                Console.WriteLine("Введите дату заного");
            }
            else
            {
                meeting._startDateTime = dateStart.Value;
                meeting._endDateTime = dateEnd.Value;
            }
        }

        Console.WriteLine("Введите тему");
            title = Console.ReadLine();
            if (!String.IsNullOrEmpty(title))
                meeting.title = title;

            Console.WriteLine("Введите описание");
            description = Console.ReadLine();
            if (!String.IsNullOrEmpty(description))
                meeting.description = description;
        
    }

    static void CreateMeeting()
    {
        bool isCorrect = true;
        DateTime dateStart = default;
        DateTime dateEnd = default;
        string title = "";
        string description = "";

        Console.WriteLine("Создание встречи");
        var isValidDate = false;
        while (!isValidDate)
        {
            dateStart = SetDate("Введите дату начала в формате dd.MM.yyyy HH:mm");
            dateEnd = SetDate("Введите планируемую дату окончания в формате dd.MM.yyyy HH:mm");
            isValidDate = _listMeeting.isValidDate(dateStart, dateEnd);
            if (!isValidDate)
            {
                Console.WriteLine("Встречи пересекаются или введены некорректно");
                Console.WriteLine("Введите дату заного");
            }
        }

        Console.WriteLine("Введите тему");
        title = Console.ReadLine();
        while (String.IsNullOrEmpty(title))
        {
            Console.WriteLine("Тема должна быть заполнена");
            Console.WriteLine("Введите тему");
            title = Console.ReadLine() ?? string.Empty;
        }

        Console.WriteLine("Введите описание");
        description = Console.ReadLine();
        Console.WriteLine("Введите время напоминания");
        var timeNotification = SetDate("Введите дату напоминания");

        var result = _listMeeting.addMeeting(dateStart, dateEnd, title, description, timeNotification);
        if (result == null)
            Console.WriteLine("Встреча не создалась, даты встречи совпадают");
    }

    static DateTime SetDate(string info)
    {
        Console.WriteLine(info);
        var data = Console.ReadLine();
        DateTime parsedData;
        while (!DateTime.TryParseExact(data,
                   "dd.MM.yyyy HH:mm",
                   CultureInfo.InvariantCulture,
                   DateTimeStyles.None,
                   out parsedData)
              )
        {
            Console.WriteLine("Некорректные данные, формат даты dd.MM.yyyy HH:mm");
            Console.WriteLine(info);
            data = Console.ReadLine();
        }

        return parsedData;
    }

    static DateTime? setDateNullable(string info)
    {
        Console.WriteLine(info);
        var data = Console.ReadLine();
        DateTime parsedData;
        if (DateTime.TryParseExact(data,
                "dd.MM.yyyy HH:mm",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out parsedData))
        {
            return parsedData;
        }

        return null;
    }

    private static async Task CheckDateTimeAsync()
    {
        while (true)
        {
            DateTime currentDateTime = DateTime.Now;
            currentDateTime = currentDateTime.AddTicks(-(currentDateTime.Ticks % TimeSpan.TicksPerMinute));
            

            if (currentDateTime == TimeNotification)
            {
                Console.WriteLine("Напоминание, о встрече");
            }

            await Task.Delay(TimeSpan.FromMinutes(1));
        }
    }
}