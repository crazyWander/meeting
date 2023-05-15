using System.Globalization;
using meeting;

internal class Program
{
    private static ListMeeting _listMeeting = new ListMeeting();

    private static DateTime timeNotification
    {
        get => _listMeeting.GetEarliestNotificationDate();
    }

    public static async Task Main(string[] args)
    {
        await CheckDateTimeAsync();
        Console.WriteLine("Запуск программы");
        while (true)
        {
            Console.WriteLine("Введите команду");
            Console.WriteLine("1. Показать все совещания");
            Console.WriteLine("2. Создать совещание");
            Console.WriteLine("3. Изменить совещание");
            Console.WriteLine("4. Удалить совещание");
            Console.WriteLine("5. Сохранить в файл");
            var comand = Console.ReadLine();
            switch (comand)
            {
                case "1":
                    Console.WriteLine(_listMeeting.getMeetingList());
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
                    break;
                case "4":
                    break;
                case "5":
                    Console.WriteLine("Введите название файла");
                    var fileName = Console.ReadLine();
                    safeFile(fileName);
                    break;
                default:
                    Console.WriteLine("Комменда не найдена");
                    break;
            }
        }
    }

    private static void safeFile(string fileName)
    {
        if (!String.IsNullOrEmpty(fileName))
        {
            while (!File.Exists(fileName))
            {
                Console.WriteLine($"Файл не найден по пути: {Path.GetFullPath(fileName)}");

                Console.WriteLine("Введите корректный путь к файлу: ");
                fileName = Console.ReadLine();
                    
                if (String.IsNullOrEmpty(fileName))
                {
                    break;
                }
            }
        }
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
        while (Int32.TryParse(Console.ReadLine(), out id))
        {
            meeting = _listMeeting.getMeeting(id);
            if (meeting == null)
            {
                Console.WriteLine("ID не найден");
                return;
            }
        }
        
        var isValidDate = false;
        while (!isValidDate)
        {
            dateStart = setDateNullable("Введите дату начала в формате dd.MM.yyyy HH:mm") ?? meeting._startDateTime;
            dateEnd = setDateNullable("Введите планируемую дату окончания в формате dd.MM.yyyy HH:mm") ?? meeting._endDateTime;
            isValidDate = _listMeeting.isValidDate(dateStart.Value, dateEnd.Value);
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
            Console.WriteLine("Введите тему");
            title = Console.ReadLine();
            if (String.IsNullOrEmpty(title))
                meeting.title = title;
            
            Console.WriteLine("Введите описание");
            description = Console.ReadLine();
            if (String.IsNullOrEmpty(description))
                meeting.description = description;
        }
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
            dateStart = setDate("Введите дату начала в формате dd.MM.yyyy HH:mm");
            dateEnd = setDate("Введите планируемую дату окончания в формате dd.MM.yyyy HH:mm");
            isValidDate = _listMeeting.isValidDate(dateStart, dateEnd);
            if (!isValidDate)
            {
                Console.WriteLine("Встречи пересекаются");
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
        var timeNotification = setDate("Введите дату напоминания");

        _listMeeting.addMeeting(dateStart, dateEnd, title, description, timeNotification);
        
    }

    static DateTime setDate(string info)
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

            if (currentDateTime >= timeNotification)
            {
                Console.WriteLine("Напоминание, о встрече");
            }
            
            await Task.Delay(TimeSpan.FromMinutes(1));
        }
    }
}