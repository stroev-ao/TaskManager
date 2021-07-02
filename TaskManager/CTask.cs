using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace TaskManager
{
    public class CTask
    {
        public enum EStatus { Приостановлена = 0, Активная = 1, Завершена = 2, Просрочена = 3 };

        /// <summary>
        /// Идентификатор задачи
        /// </summary>
        [JsonProperty("1")]
        public int ID { get; set; }

        /// <summary>
        /// Название задачи
        /// </summary>
        [JsonProperty("2")]
        public string Name { get; set; }

        /// <summary>
        /// Заказчик (постановщик задачи)
        /// </summary>
        [JsonProperty("3")]
        public int Customer { get; set; } //*

        /// <summary>
        /// Комментарий
        /// </summary>
        [JsonProperty("4")]
        public string Comment{ get; set; }

        /// <summary>
        /// Место размещения
        /// </summary>
        //[JsonProperty("5")]
        //public string Location { get; set; } //*

        /// <summary>
        /// Дата создания
        /// </summary>
        [JsonProperty("6")]
        public DateTime CreationDate { get; set; } //*

        /// <summary>
        /// Дата начала исполнения
        /// </summary>
        [JsonProperty("7")]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Дата окончания выполнения задачи
        /// </summary>
        [JsonProperty("8")]
        public DateTime FinishDate { get; set; }

        /// <summary>
        /// Срок исполнения
        /// </summary>
        [JsonProperty("9")]
        public DateTime ExecutionDate { get; set; } //*

        /// <summary>
        /// Предыдущий срок исполнения
        /// </summary>
        [JsonProperty("10")]
        public string PreviousExecutionDate { get; set; }

        /// <summary>
        /// Файлы
        /// </summary>
        [JsonProperty("11")]
        public List<CFile> Files { get; set; }

        /// <summary>
        /// Оценочная длительность
        /// </summary>
        [JsonProperty("12")]
        public int EstimatedDuration { get; set; }

        /// <summary>
        /// Напоминания
        /// </summary>
        [JsonProperty("13")]
        public List<CReminder> Reminders { get; set; }

        /// <summary>
        /// Статус задачи
        /// </summary>
        [JsonProperty("14")]
        public EStatus Status { get; set; } //*

        /// <summary>
        /// Приоритет задачи
        /// </summary>
        [JsonProperty("24")]
        public int Priority { get; set; }

        public CTask(int id, string name)
        {
            ID = id;
            Name = name;
            Status = EStatus.Приостановлена;
            CreationDate = DateTime.Now;
            ExecutionDate = DateTime.Now.AddDays(1);
            StartDate = FinishDate = DateTime.MinValue;
            Files = new List<CFile>();
            EstimatedDuration = 3600;
            Reminders = new List<CReminder>();
            Customer = -1;
        }

        public override string ToString()
        {
            return Name;
        }

        public void AddFile(string fullPath, string additionalPath, string name, byte[] hash)
        {
            Files.Add(new CFile(Files.Count, ID, fullPath, additionalPath, name, hash));
        }

        public void AddReminders(List<CReminder> time)
        {
            Reminders.Clear();

            Reminders.AddRange(time);
        }

        public void ClearReminders()
        {
            Reminders.Clear();
        }

        public CPreFile[] GetFiles()
        {
            return Files.Select(e => new CPreFile(e.FullPath, e.AdditionalPath)).ToArray();
        }
    }
}
