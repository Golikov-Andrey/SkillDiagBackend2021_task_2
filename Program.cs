using System;
using System.Collections.Generic;

namespace SkillDiagBackend2021_task_2
{
    class Program
    {
        /*
          
              Миша работает в команде Яндекс.Маркета, которая предоставляет производителям товаров аналитику о продажах. 
            Сейчас Миша разбирается с периодизацией: нужно собирать данные по дням, неделям, месяцам, кварталам и годам. От клиентов приходят запросы, 
            в которых указан период детализации и интервал: начальная и конечная даты. Так что первоначально Мише нужно разбить интервал на периоды. 
            Так, если клиент хочет данные с 2020-01-10 по 2020-03-25 с детализацией по месяцам, то ему вернутся данные за три периода: c 2020-01-10 по 2020-01-31, 
            с 2020-02-01 по 2020-02-29 и с 2020-03-01 по 2020-03-25. Помогите Мише, а то ему еще диплом писать надо!
            Всего нужно поддержать пять видов временных интервалов:

            WEEK — неделя с понедельника по воскресенье.
            MONTH — месяц.
            QUARTER — интервалы в три месяца: январь — март, апрель — июнь, июль — сентябрь, октябрь — декабрь.
            YEAR — год c 1 января по 31 декабря.
            REVIEW — периоды, за которые оцениваются достижения сотрудников Яндекса. Летний период длится с 1 апреля по 30 сентября, зимний — с 1 октября по 31 марта.
            Формат ввода
            В первой строке дан типа интервала type — строка, принимающая одно из следующих значений: WEEK, MONTH, QUARTER, YEAR, REVIEW. Во второй строке через пробел даны начальная и конечная даты 
            start и end (start≤end) в формате yyyy-MM-dd. Гарантируется, что обе даты лежат в промежутке с 1 января 2000 года по 31 декабря 3999 года включительно.
            Формат вывода
            В первой строке ответа выведите одно целое число N — количество промежутков. В последующих N строках на i-й строке выведите через пробел дату начала и конца i-го промежутка в формате yyyy-MM-dd. 
                    Промежутки должны выводиться в порядке возрастания начальной даты.

                            Ввод	
                    MONTH
                    2020-01-10 2020-03-25
                            Вывод
                    3
                    2020-01-10 2020-01-31
                    2020-02-01 2020-02-29
                    2020-03-01 2020-03-25
                    Пример 2
                    Ввод	
                    WEEK
                    2020-01-26 2020-03-23
                            Вывод
                    10
                    2020-01-26 2020-01-26
                    2020-01-27 2020-02-02
                    2020-02-03 2020-02-09
                    2020-02-10 2020-02-16
                    2020-02-17 2020-02-23
                    2020-02-24 2020-03-01
                    2020-03-02 2020-03-08
                    2020-03-09 2020-03-15
                    2020-03-16 2020-03-22
                    2020-03-23 2020-03-23
                    Пример 3
                    Ввод	
                    REVIEW
                    2016-09-20 2022-11-30
                            Вывод
                    14
                    2016-09-20 2016-09-30
                    2016-10-01 2017-03-31
                    2017-04-01 2017-09-30
                    2017-10-01 2018-03-31
                    2018-04-01 2018-09-30
                    2018-10-01 2019-03-31
                    2019-04-01 2019-09-30
                    2019-10-01 2020-03-31
                    2020-04-01 2020-09-30
                    2020-10-01 2021-03-31
                    2021-04-01 2021-09-30
                    2021-10-01 2022-03-31
                    2022-04-01 2022-09-30
                    2022-10-01 2022-11-30

        */

        /// <summary>
        /// Делегат нужен исключительно для сокращения кода
        /// Ссылка на метод добавления необходимой даты
        /// </summary>
        delegate DateTime Delegat_AddDateTime(int count, DateTime dateTime);

        static void Main(string[] args)
        {
            //Режим работы программы
            String regimDate = Console.ReadLine();

            //Входная строчка с интервалом дат
            String[] dateInput = Console.ReadLine().Split(" ");

            //Дата начала введенного периода
            DateTime dateStartInput = Convert.ToDateTime(dateInput[0]);
            //Дата окончания введенного периода
            DateTime dateStopInput = Convert.ToDateTime(dateInput[1]);
            //Минемальная дата
            DateTime DBegin = new DateTime(2000, 1, 1);//DateTime.Parse("2000-01-01");
            //Максимальная дата
            DateTime DEnd = new DateTime(3999, 12, 31);// DateTime.Parse("3999-12-32");

            //Проверяем корректность ввода режима работы программы
            bool regimTest = false;
            if (regimDate == "WEEK" || regimDate == "MONTH" || regimDate == "QUARTER" || regimDate == "YEAR" || regimDate == "REVIEW") regimTest = true;

            //Проверяем чтобы входные даты соответствовали допустимому диаппазону
            bool timeTest = true;
            if (!(dateStartInput <= dateStopInput && dateStartInput >= DBegin && dateStopInput <= DEnd)) timeTest = false;

            //Ссылка на метод добавления даты. По умолчанию метод пустышка
            Delegat_AddDateTime AddDateTime = AddNothing;


            //Если все данные введены верно запускаем логику
            if (regimTest && timeTest)
            {
                //Список начальных дат интервалов времени
                List<DateTime> DTListStart = new List<DateTime>();
                //Список конечных дат интервалов времени
                List<DateTime> DTListStop = new List<DateTime>();

                //Дата итератор
                DateTime bufferDateTime = DateTime.Now;
                //Количество добавляемых периодов времени
                int duration = 0;

                //Инициализируем начальную дату периода и ссылку на метод при итерации и ее шаг
                InitState(regimDate, dateStartInput, ref bufferDateTime,ref AddDateTime, out duration);

                //Добавляем начальную дату периода как стартову.
                DTListStart.Add(dateStartInput);
                //Добавляем нужное количество периодов времени
                bufferDateTime = AddDateTime(duration, bufferDateTime);


                //Выполняем цыкл, пока не достигнем или превзайдем дату окончания входного периода
                while (bufferDateTime <= dateStopInput)
                {
                    //Вычетаем один день. Нужно для приведения к нужному формату выводимой даты
                    DTListStop.Add(bufferDateTime.AddDays(-1));
                    //Добавляем дату в список начальных дат интервалов времени
                    DTListStart.Add(bufferDateTime);
                    //Смещаем дату итератор на нужный период времени
                    bufferDateTime = AddDateTime(duration, bufferDateTime);
                }
                //Добавляем конечную дату входного периода как окончание в последнем выводимом периоде времени
                DTListStop.Add(dateStopInput);



                //Выводим количество периодов
                Console.WriteLine(DTListStart.Count);
                //Выводим периоды в формате "yyyy-MM-dd"
                for (int i = 0; i < DTListStart.Count; i++)
                {
                    Console.WriteLine(DTListStart[i].ToString("yyyy-MM-dd") + " " + DTListStop[i].ToString("yyyy-MM-dd"));
                }

            }
            else
            {
                //Выводим ошибку
                Console.WriteLine("input error");
            }

        }

        //=========================================================================================================================================
        // Инициализация режима работы программы
        //=========================================================================================================================================

        private static void InitState(string regimDate, DateTime dateStartInput, ref DateTime bufferDateTime, ref Delegat_AddDateTime AddDateTime, out int duration)
        {
            duration = 1;
            //Выбираем режим работы программы
            switch (regimDate)
            {
                //Режим работы по месяцам
                case "MONTH":
                    //Дата старта периода начало месяца
                    bufferDateTime = FindStartDataMonth(dateStartInput);
                    //Получаем ссылку на метод добавления по месяцам
                    AddDateTime = AddMonths;
                    //Количество добавляемых режимов времени 1 месяц
                    duration = 1;
                    break;

                //Режим работы по неделям
                case "WEEK":
                    //Определяем дату начала недели
                    bufferDateTime = FindStartDataWeek(dateStartInput);
                    //Получаем ссылку на метод добавления по дням
                    AddDateTime = AddDays;
                    //Количество добавляемых режимов времени 7 дней
                    duration = 7;
                    break;

                //Режим работы по годам
                case "YEAR":
                    //Дата старта периода начало года
                    bufferDateTime = FindStartDataYear(dateStartInput);
                    //Получаем ссылку на метод добавления по годам
                    AddDateTime = AddYears;
                    //Количество добавляемых режимов времени 1 год
                    duration = 1;
                    break;

                //Режим работы по REVIEW
                case "REVIEW":
                    //Дата старта периода начало REVIEW
                    bufferDateTime = FindStartDataReview(dateStartInput);
                    //Получаем ссылку на метод добавления по месяцам
                    AddDateTime = AddMonths;
                    //Количество добавляемых режимов времени 6 месяцев
                    duration = 6;
                    break;

                //Режим работы по кварталам
                case "QUARTER":
                    //Дата старта периода начало квартала
                    bufferDateTime = FindStartDataQuarter(dateStartInput);
                    //Получаем ссылку на метод добавления по месяцам
                    AddDateTime = AddMonths;
                    //Количество добавляемых режимов времени 3 месца
                    duration = 3;
                    break;
            }

        }

        //=========================================================================================================================================
        // Делегаты методов добавления периода времени
        //=========================================================================================================================================


        //Метод пустой
        private static DateTime AddNothing(int count, DateTime dateTime)
        {
            //Выводим входную дату
            return dateTime;
        }

        //Метод добавления месяцев
        private static DateTime AddMonths(int count, DateTime dateTime)
        {
            //Добавляем нужное количество месяцев
            return dateTime.AddMonths(count);
        }

        //Метод добавления лет
        private static DateTime AddYears(int count, DateTime dateTime)
        {
            //Добавляем нужное количество лет
            return dateTime.AddYears(count);
        }

        //Метод добавления дней
        private static DateTime AddDays(int count, DateTime dateTime)
        {
            //Добавляем нужное количество дней
            return dateTime.AddDays(count);
        }


        //=========================================================================================================================================
        // Методы определения начальной даты согласно режима работы
        //=========================================================================================================================================

        //Метод определения начальной даты периодов в режиме "MONTH"
        private static DateTime FindStartDataMonth(DateTime dateStartInput)
        {
            return new DateTime(dateStartInput.Year, dateStartInput.Month, 1);
        }

        //Метод определения начальной даты периодов в режиме "WEEK"
        private static DateTime FindStartDataWeek(DateTime dateStartInput)
        {
            DateTime bufferDateTime = new DateTime(dateStartInput.Year, dateStartInput.Month, dateStartInput.Day);

            while (bufferDateTime.DayOfWeek != DayOfWeek.Monday)
            {
                bufferDateTime = bufferDateTime.AddDays(-1);
            }

            return bufferDateTime;
        }

        //Метод определения начальной даты периодов в режиме "YEAR"
        private static DateTime FindStartDataYear(DateTime dateStartInput)
        {
            return new DateTime(dateStartInput.Year, 1, 1);
        }

        //Метод определения начальной даты периодов в режиме "REVIEW"
        private static DateTime FindStartDataReview(DateTime dateStartInput)
        {
            DateTime bufferDateTime = new DateTime(dateStartInput.Year, dateStartInput.Month, dateStartInput.Day);

            while (!(((bufferDateTime.Month == 4) || (bufferDateTime.Month == 10)) && (bufferDateTime.Day == 1)))
            {
                bufferDateTime = bufferDateTime.AddDays(-1);
            }
            return bufferDateTime;
        }

        //Метод определения начальной даты периодов в режиме "QUARTER"
        private static DateTime FindStartDataQuarter(DateTime dateStartInput)
        {
            DateTime bufferDateTime = new DateTime(dateStartInput.Year, dateStartInput.Month, dateStartInput.Day);

            while (!(((bufferDateTime.Month == 1) || (bufferDateTime.Month == 4) || (bufferDateTime.Month == 7) || (bufferDateTime.Month == 10)) && (bufferDateTime.Day == 1)))
            {
                bufferDateTime = bufferDateTime.AddDays(-1);
            }
            return bufferDateTime;
        }
        //=========================================================================================================================================

    }
}
