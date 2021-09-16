using System;

namespace Navicon.TestConsole.Services
{
    public class RandomLogger : IRandomLogger
    {
        private readonly Random _rnd = new Random();

        /// <summary>
        /// Рандомное логирование
        /// </summary>
        /// <returns>Log message</returns>
        public string Log()
        {
            var way = _rnd.Next(1, 5);
            var time = DateTime.Now.ToString("dd.MM.yy hh:mm:ss");

            switch (way)
            {
                case 0:
                    return $"{time} [INF]: Запуск приложения";
                case 1:
                    return $"{time} [INF]: Открытие формы документа";
                case 2:
                    return $"{time} [INF]: Подписание документа";
                case 3:
                    return $"{time} [INF]: Нажатие на кнопку Сохранить";
                case 4:
                    return $"{time} [ERR]: Ошибка сети";
                default:
                    return $"{time} [ERR]: Ошибка пути";
            }
        }
    }
}