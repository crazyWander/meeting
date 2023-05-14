// See https://aka.ms/new-console-template for more information

using System.Globalization;
using meeting;

internal class Program
{
    private static ListMeeting _listMeeting = new ListMeeting(); 
    public static void Main(string[] args)
    {
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
                    break;
                case "2":
                    CreateMeeting();
                    break;
                case "3":
                    break;
                case "4":
                    break;
            }
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

        Console.WriteLine("Введите дату начала в формате dd.MM.yyyy HH:mm");
        dateStart = setDate("Введите дату начала в формате dd.MM.yyyy HH:mm");
        dateEnd = setDate("Введите планируемую дату окончания в формате dd.MM.yyyy HH:mm");
        

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

        _listMeeting.addMeeting(dateStart, dateEnd, title, description);
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
}