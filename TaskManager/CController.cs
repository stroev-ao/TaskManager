using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Diagnostics;
using Microsoft.Win32;

namespace TaskManager
{
    public class CController
    {
        public enum EOrder { ByExecutionDateUD = 0, ByExecutionDateDU = 1, ByPriorityUD = 2, ByPriorityDU = 3, ByCreationDateUD = 4, ByCreationDateDU = 5, ByEstimatedDurationUD = 6, ByEstimatedDurationDU = 7 };

        public delegate void DProgressChanged(int percent);
        public delegate void DWorkerCompleted();

        private const string TASKS_OLD = "tasks.json";
        private const string TASKS = "tasks.dat";
        private const string CONFIG_OLD = "config.json";
        private const string CONFIG = "config.dat";
        private const string DATA = "data";
        string tasksPath, dataPath, configPath, assemblyPath;
        private List<CTask> tasks;
        private EOrder order;
        private bool tasksChanged, configChanged;
        private Dictionary<int, string> customers;

        private const string KEY = "antisceptic";

        public enum EFilter { ByCustomer = 0, ByExecutionDate = 1 };
        private Dictionary<EFilter, object[]> activeFilters;

        public string Title { get { return "Task Manager [Alpha 18.06.2021]"; } }

        public EOrder Order { set { order = value; } }

        public Dictionary<EFilter, object[]> ActiveFilters { get { return activeFilters; } }

        public CController()
        {
            assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string storagePath = Properties.Settings.Default.storagePath;

            if (string.IsNullOrEmpty(storagePath))
            {
                Properties.Settings.Default.storagePath = $"{ assemblyPath }\\{ DATA }";
                Properties.Settings.Default.Save();

                storagePath = Properties.Settings.Default.storagePath;
            }
            
            tasksPath = assemblyPath + "\\" + TASKS_OLD;
            configPath = assemblyPath + "\\" + CONFIG_OLD;

            //проверяем наличие старого формата
            if (File.Exists(tasksPath))
            {
                using (StreamReader sr = new StreamReader(tasksPath))
                {
                    tasks = JsonConvert.DeserializeObject<List<CTask>>(sr.ReadToEnd());
                    sr.Close();
                }

                tasksPath = assemblyPath + "\\" + TASKS;

                tasksChanged = true;
            }
            else
            {
                tasksPath = assemblyPath + "\\" + TASKS;

                if (!File.Exists(tasksPath))
                {
                    using (StreamWriter sw = new StreamWriter(tasksPath))
                    {
                        sw.Write(CXORChiper.Encrypt("[]", KEY));
                        sw.Flush();
                        sw.Close();
                    }

                    tasks = new List<CTask>();
                }
                else
                {
                    using (StreamReader sr = new StreamReader(tasksPath))
                    {
                        tasks = JsonConvert.DeserializeObject<List<CTask>>(CXORChiper.Decrypt(sr.ReadToEnd(), KEY));
                        sr.Close();
                    }
                }
            }

            if (File.Exists(configPath))
            {
                using (StreamReader sr = new StreamReader(configPath))
                {
                    customers = JsonConvert.DeserializeObject<Dictionary<int, string>>(sr.ReadToEnd());
                    sr.Close();
                }

                configPath = assemblyPath + "\\" + CONFIG;

                configChanged = true;
            }
            else
            {
                configPath = assemblyPath + "\\" + CONFIG;

                if (!File.Exists(configPath))
                {
                    using (StreamWriter sw = new StreamWriter(configPath))
                    {
                        sw.Write(CXORChiper.Encrypt("{}", KEY));
                        sw.Flush();
                        sw.Close();
                    }

                    customers = new Dictionary<int, string>();
                }
                else
                {
                    using (StreamReader sr = new StreamReader(configPath))
                    {
                        customers = JsonConvert.DeserializeObject<Dictionary<int, string>>(CXORChiper.Decrypt(sr.ReadToEnd(), KEY));
                        sr.Close();
                    }
                }
            }

            dataPath = storagePath;

            if (!Directory.Exists(dataPath))
                Directory.CreateDirectory(dataPath);

            activeFilters = new Dictionary<EFilter, object[]>();
        }

        public void SaveTasksAndConfig()
        {
            if (tasksChanged)
            {
                using (StreamWriter sw = new StreamWriter(tasksPath))
                {
                    string text = CXORChiper.Encrypt(JsonConvert.SerializeObject(tasks), KEY);
                    sw.Write(text);
                    sw.Flush();
                    sw.Close();
                }

                tasksChanged = false;
            }
                

            if (configChanged)
            {
                using (StreamWriter sw = new StreamWriter(configPath))
                {
                    string text = CXORChiper.Encrypt(JsonConvert.SerializeObject(customers), KEY);
                    sw.Write(text);
                    sw.Flush();
                    sw.Close();
                }

                configChanged = false;
            }

            DeleteOldFile(assemblyPath + "\\" + TASKS_OLD);
            DeleteOldFile(assemblyPath + "\\" + CONFIG_OLD);

            void DeleteOldFile(string path)
            {
                if (File.Exists(path))
                {
                    try
                    {
                        File.Delete(path);
                    }
                    catch { }
                }
            }
        }

        public CTask[] GetTasks(bool showActive, bool showPaused, bool showDone, bool showOutstanding)
        {
            List<CTask> list = new List<CTask>();

            if (showActive)
                list.AddRange(tasks.Where(e => e.Status == CTask.EStatus.Активная));

            if (showPaused)
                list.AddRange(tasks.Where(e => e.Status == CTask.EStatus.Приостановлена));

            if (showDone)
                list.AddRange(tasks.Where(e => e.Status == CTask.EStatus.Завершена));

            if (showOutstanding)
                list.AddRange(tasks.Where(e => e.Status == CTask.EStatus.Просрочена));

            switch (order)
            {
                case EOrder.ByExecutionDateUD:
                    list = list.OrderBy(e => e.ExecutionDate).ToList();
                    break;
                case EOrder.ByExecutionDateDU:
                    list = list.OrderByDescending(e => e.ExecutionDate).ToList();
                    break;
                case EOrder.ByPriorityUD:
                    list = list.OrderByDescending(e => e.Priority).ToList();
                    break;
                case EOrder.ByPriorityDU:
                    list = list.OrderBy(e => e.Priority).ToList();
                    break;
                case EOrder.ByCreationDateUD:
                    list = list.OrderBy(e => e.CreationDate).ToList();
                    break;
                case EOrder.ByCreationDateDU:
                    list = list.OrderByDescending(e => e.CreationDate).ToList();
                    break;
                case EOrder.ByEstimatedDurationUD:
                    list = list.OrderBy(e => e.EstimatedDuration).ToList();
                    break;
                case EOrder.ByEstimatedDurationDU:
                    list = list.OrderByDescending(e => e.EstimatedDuration).ToList();
                    break;
            }

            int filterCount = activeFilters.Count;
            
            if (filterCount > 0)
            {
                for (int i = 0; i < filterCount; i++)
                {
                    switch (activeFilters.ElementAt(i).Key)
                    {
                        case EFilter.ByCustomer:
                            {
                                int[] idxs = activeFilters.ElementAt(i).Value.Select(e => Convert.ToInt32(e)).ToArray();

                                list.RemoveAll(e => !idxs.Contains(e.Customer));

                                break;
                            }
                        case EFilter.ByExecutionDate:
                            {
                                DateTime[] dates = activeFilters.ElementAt(i).Value.Select(e => Convert.ToDateTime(e)).ToArray();

                                for (int j = 0; j < list.Count; j++)
                                {
                                    CTask t = list[j];

                                    bool success = false;

                                    for (int k = 0; !success && k < dates.Length; k++)
                                        success = success || dates[k].IsTheSameDay(t.ExecutionDate);

                                    if (!success)
                                    {
                                        list.Remove(t);
                                        j--;
                                    }
                                }

                                break;
                            }
                    }
                }
            }

            return list.ToArray();
        }

        public int AddTask()
        {
            int id = tasks.Count == 0 ? 0 : tasks.Max(e => e.ID) + 1;

            tasks.Add(new CTask(id, "Новая задача"));

            return id;
        }

        public CTask GetTask(int id)
        {
            return tasks.FirstOrDefault(e => e.ID == id);
        }

        public void RemoveTask(int id, bool deleteFiles = false)
        {
            CTask t = tasks.FirstOrDefault(e => e.ID == id);
            
            if (t != null)
            {
                if (deleteFiles)
                {
                    string target = $"{ dataPath }\\{ id }";

                    if (Directory.Exists(target))
                        Directory.Delete(target, true);
                }

                tasks.Remove(t);
            }
        }

        public void UpdateTask(int id, string name, string customer, string comment, DateTime executionDate, int estimatedDuration, List<CReminder> reminders, List<CPreFile> preFiles, DProgressChanged progressChanged, DWorkerCompleted workerCompleted)
        {
            CTask t = tasks.FirstOrDefault(e => e.ID == id);
            if (t == null)
            {
                workerCompleted?.Invoke();
                throw new Exception("Задача не существует");
            }

            string dir = dataPath + "\\" + id;

            if (preFiles.Count == 0)
            {
                if (Directory.Exists(dir))
                {
                    try
                    {
                        Directory.Delete(dir, true);
                    }
                    catch (Exception ex)
                    {
                        workerCompleted?.Invoke();
                        throw new Exception($"Не удалось удалить папку для хранения файлов задачи\n\n{ ex.Message }");
                    }
                }

                t.Files.Clear();
            }
            else
            {
                //удаление файлов
                string[] fileNames = preFiles.Select(e => Path.GetFileNameWithoutExtension(e.FullPath)).ToArray();
                for (int i = 0; i < t.Files.Count; i++)
                {
                    CFile f = t.Files[i];

                    if (!fileNames.Contains(f.Name))
                    {
                        try
                        {
                            File.Delete(f.FullPath);
                        }
                        catch (Exception ex)
                        {
                            workerCompleted?.Invoke();
                            throw new Exception($"Не удалось удалить файл { f.FullPath }\n\n{ ex.Message }");
                        }

                        t.Files.Remove(f);
                    }
                }
                fileNames = null;

                if (!Directory.Exists(dir))
                {
                    try
                    {
                        Directory.CreateDirectory(dir);
                    }
                    catch (Exception ex)
                    {
                        workerCompleted?.Invoke();
                        throw new Exception($"Не удалось создать папку для хранения файлов задачи\n\n{ ex.Message }");
                    }
                }

                //добавление файлов
                int filesCount = preFiles.Count;
                for (int i = 0; i < filesCount; i++)
                {
                    CPreFile pf = preFiles[i];

                    if (pf.FullPath.Contains(dir))
                        continue;

                    string file = pf.FullPath;
                    string fileFullName = Path.GetFileName(file);
                    string fileName = Path.GetFileNameWithoutExtension(file);

                    byte[] hash = null;

                    try
                    {
                        using (FileStream s = new FileStream(file, FileMode.Open, FileAccess.Read))
                        {
                            hash = MD5.Create().ComputeHash(s);
                            s.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        workerCompleted?.Invoke();
                        throw new Exception($"Не удалось создать хеш файла\n\n{ ex.Message }");
                    }
                    finally
                    {
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                    }

                    bool needToCopy = true;

                    string destFileName = dir;
                    if (!string.IsNullOrEmpty(pf.AdditionalPath))
                        destFileName += pf.AdditionalPath;
                    destFileName += $"\\{ fileFullName }";

                    if (File.Exists(destFileName))
                    {
                        CFile f = t.Files.FirstOrDefault(e => e.Name == fileName);
                        byte[] existingHash = f?.Hash ?? null;

                        if (IsArrayEquals(hash, existingHash))
                            needToCopy = false;
                        else
                        {
                            string targetDir = Path.GetDirectoryName(destFileName);
                            int count = Directory.GetFiles(targetDir).Count(e => e.Contains(fileName));
                            destFileName = $"{ targetDir }\\{ fileName }_{ count }{ Path.GetExtension(destFileName) }";

                            fileName = Path.GetFileNameWithoutExtension(destFileName);
                        }
                    }

                    if (needToCopy)
                    {
                        string destPath = Path.GetDirectoryName(destFileName);

                        try
                        {
                            if (!Directory.Exists(destPath))
                                Directory.CreateDirectory(destPath);

                            File.Copy(file, destFileName, true);
                        }
                        catch (Exception ex)
                        {
                            workerCompleted?.Invoke();
                            throw new Exception($"Не удалось создать копию файла \"{ file }\"\n\n{ ex.Message }");
                        }

                        string addPath = destFileName.Replace(dir, "").Replace($"\\{ fileFullName }", "");

                        t.AddFile(destFileName, addPath, fileName, hash);
                    }

                    progressChanged?.Invoke((int)(100.0 * (i + 1) / filesCount));
                }
            }

            t.Name = name;

            if (!customers.ContainsValue(customer))
            {
                customers.Add(customers.Count, customer);
                configChanged = true;
            }

            t.Customer = customers.First(e => e.Value == customer).Key;
            t.Comment = comment;
            //t.Location = location;

            DateTime date = t.ExecutionDate;
            if (!date.Equals(executionDate))
            {
                t.PreviousExecutionDate = date.GetDate();

                t.Status = CTask.EStatus.Приостановлена;
            }
            t.ExecutionDate = executionDate;

            t.EstimatedDuration = estimatedDuration;
            t.AddReminders(reminders);

            workerCompleted?.Invoke();

            tasksChanged = true;

            SaveTasksAndConfig();

            //void DeleteFolder(string path)
            //{
            //    string[] _files = Directory.GetFiles(path);
            //    for (int i = 0; i < _files.Length; i++)
            //        File.Delete(_files[i]);

            //    string[] subDirs = Directory.GetDirectories(path);
            //    for (int i = 0; i < subDirs.Length; i++)
            //        DeleteFolder(subDirs[i]);

            //    Directory.Delete(path);
            //}
        }

        private bool IsArrayEquals(byte[] arr1, byte[] arr2)
        {
            bool result = true;

            if (arr1 == null || arr2 == null)
                return false;

            int count = arr1.Length;
            if (count != arr2.Length)
                result = false;

            for (int i = 0; result && i < count; i++)
                if (arr1[i] != arr2[i])
                    result = false;

            return result;
        }

        public string GetActiveReminder()
        {
            string message = null;

            bool needToSearch = true;
            for (int i = 0; needToSearch && i < tasks.Count; i++)
            {
                CTask task = tasks[i];

                for (int j = 0; needToSearch && j < task.Reminders.Count; j++)
                {
                    CReminder reminder = task.Reminders[j];

                    if (!reminder.Shown)
                    {
                        int seconds = reminder.Seconds;
                        int now = Convert.ToInt32(task.ExecutionDate.Subtract(DateTime.Now).TotalSeconds);

                        if (now <= seconds)
                        {
                            if (string.IsNullOrEmpty(reminder.Comment))
                                message = $"Для выполнения задачи \"{ task.Name }\" осталось времени:{ GetReminder(now) }";
                            else
                                message = reminder.Comment;
                            
                            reminder.Shown = true;
                            
                            tasksChanged = true;

                            needToSearch = false;
                        }
                    }
                }
            }

            return message;
        }

        public string GetReminder(int time, string prefix = "")
        {
            string result = prefix;

            if (time >= 86400)
            {
                int days = time / 86400;
                result += $" { days } д.";

                time -= 86400 * days;
            }

            if (time >= 3600)
            {
                int hours = time / 3600;
                result += $" { hours } ч.";

                time -= hours * 3600;
            }
            
            if (time >= 60)
                result += $" { time / 60 } мин.";

            return result;
        }

        public void SetTaskStatus(int id, CTask.EStatus status)
        {
            CTask t = tasks.FirstOrDefault(e => e.ID == id);
            if (t != null)
            {
                switch (status)
                {
                    case CTask.EStatus.Активная:
                        if (t.StartDate == DateTime.MinValue)
                            t.StartDate = DateTime.Now;
                        break;
                    case CTask.EStatus.Завершена:
                        if (t.FinishDate == DateTime.MinValue)
                            t.FinishDate = DateTime.Now;
                        break;
                }

                t.Status = status;
            }

            tasksChanged = true;

            SaveTasksAndConfig();
        }

        public int GetEstimatedDuration(DateTime dateTime, int currentTaskId)
        {
            int totalTime = 0;

            foreach (CTask t in tasks.Where(e => e.ID != currentTaskId && e.ExecutionDate.IsTheSameDay(dateTime)))
                totalTime += t.EstimatedDuration;

            return totalTime;
        }

        public void OpenFolder(int taskId)
        {
            string dir = $"{ dataPath }\\{ taskId }";
            if (Directory.Exists(dir))
                Process.Start(dir);
        }

        public void SetPriority(int taskId, int priority)
        {
            GetTask(taskId).Priority = priority;

            tasksChanged = true;

            SaveTasksAndConfig();
        }

        public string[] GetCustomerNames()
        {
            return customers.Values.ToArray();
        }

        public string GetCustomerName(int id)
        {
            return customers.ContainsKey(id) ? customers[id] : "-1";
        }

        public bool IsFolderExists(string path)
        {
            return Directory.Exists(path);
        }

        public bool IsFolderEmpty(string path)
        {
            return Directory.GetDirectories(path).Length == 0 && Directory.GetFiles(path).Length == 0;
        }

        public void CopyStorage(string newPath, DProgressChanged progressChanged, DWorkerCompleted workerCompleted)
        {
            string path = Properties.Settings.Default.storagePath;

            string[] directories = GetDirectories(path).Select(e => e.Replace(path, "").Insert(0, newPath)).ToArray();
            string[] originalFiles = GetFiles(path);
            string[] files = originalFiles.Select(e => e.Replace(path, "").Insert(0, newPath)).ToArray();

            int dirCount = directories.Length;
            int fileCount = originalFiles.Length;

            int maxProgress = + dirCount + fileCount;
            int progress = 0;

            for (int i = 0; i < dirCount; i++)
            {
                try
                {
                    Directory.CreateDirectory(directories[i]);
                }
                catch (Exception ex)
                {
                    workerCompleted?.Invoke();
                    throw new Exception($"Не удалось выполнить перенос хранилища\n\n{ ex.Message }");
                }

                progressChanged?.Invoke((int)(100.0 * ++progress / maxProgress));
            }

            for (int i = 0; i < fileCount; i++)
            {
                try
                {
                    File.Copy(originalFiles[i], files[i]);
                }
                catch (Exception ex)
                {
                    workerCompleted?.Invoke();
                    throw new Exception($"Не удалось выполнить перенос хранилища\n\n{ ex.Message }");
                }

                progressChanged?.Invoke((int)(100.0 * ++progress / maxProgress));
            }

            try
            {
                Directory.Delete(path, true);
            }
            catch (Exception ex)
            {
                workerCompleted?.Invoke();
                throw new Exception($"Не удалось выполнить перенос хранилища\n\n{ ex.Message }");
            }

            workerCompleted?.Invoke();
        }

        private string[] GetDirectories(string _path)
        {
            List<string> dirs = new List<string>(Directory.GetDirectories(_path));

            string[] subDirs = Directory.GetDirectories(_path);
            for (int i = 0; i < subDirs.Length; i++)
                dirs.AddRange(GetDirectories(subDirs[i]));

            return dirs.ToArray();
        }

        private string[] GetFiles(string _path)
        {
            List<string> fls = new List<string>(Directory.GetFiles(_path));

            string[] subDirs = Directory.GetDirectories(_path);
            for (int i = 0; i < subDirs.Length; i++)
                fls.AddRange(GetFiles(subDirs[i]));

            return fls.ToArray();
        }

        public void SetStoragePath(string path)
        {
            Properties.Settings.Default.storagePath = path;
            Properties.Settings.Default.Save();

            dataPath = path;
        }

        public void CheckFilesInTaskFolder(int id, out int foundCount, out int changedCount, out int failedToCheck, out int changedPath, out int deletedCount)
        {
            foundCount = 0;
            changedCount = 0;
            failedToCheck = 0;
            deletedCount = 0;
            changedPath = 0;

            string path = $"{ dataPath }\\{ id }";

            if (!Directory.Exists(path))
                return;

            CTask t = GetTask(id);

            string[] files = GetFiles(path);

            int listedFilesCount = t.Files.Count;
            int filesCount = files.Length;

            if (listedFilesCount < filesCount)
                foundCount = filesCount - listedFilesCount;

            if (listedFilesCount > filesCount)
                deletedCount = listedFilesCount - filesCount;

            for (int i = 0; i < filesCount; i++)
            {
                string file = files[i];
                string fileFullName = Path.GetFileName(file);

                CFile f = t.Files.FirstOrDefault(e => Path.GetFileName(e.FullPath) == fileFullName);
                if (f == null)
                    continue;

                byte[] hash = null;

                try
                {
                    using (FileStream s = new FileStream(file, FileMode.Open, FileAccess.Read))
                    {
                        hash = MD5.Create().ComputeHash(s);
                        s.Close();
                    }
                }
                catch
                {
                    failedToCheck++;
                }
                finally
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }

                if (hash == null || hash.Length == 0)
                    continue;

                if (!IsArrayEquals(f.Hash, hash))
                    changedCount++;

                string additionalPath = file.Replace(path, "").Replace($"\\{ fileFullName }", "");

                if (additionalPath != f.AdditionalPath)
                {
                    f.FullPath = file;
                    f.AdditionalPath = additionalPath;
                    changedPath++;
                }
            }
        }

        public void AddFoundFiles(int taskId)
        {
            string path = $"{ dataPath }\\{ taskId }";

            if (!Directory.Exists(path))
                return;

            CTask t = GetTask(taskId);

            string[] files = GetFiles(path);

            for (int i = 0; i < files.Length; i++)
            {
                string file = files[i];
                string fileFullName = Path.GetFileName(file);

                CFile f = t.Files.FirstOrDefault(e => Path.GetFileName(e.FullPath) == fileFullName);
                if (f != null)
                    continue;

                byte[] hash = null;

                try
                {
                    using (FileStream s = new FileStream(file, FileMode.Open, FileAccess.Read))
                    {
                        hash = MD5.Create().ComputeHash(s);
                        s.Close();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Не удалось добавить новые файлы в список. { ex.Message }");
                }
                finally
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }

                t.Files.Add
                (
                    new CFile
                    (
                        t.Files.Count,
                        taskId,
                        file,
                        file.Replace(path, "").Replace($"\\{ fileFullName }", ""),
                        Path.GetFileNameWithoutExtension(file),
                        hash
                    )
                );
            }

            tasksChanged = true;
        }

        public void ExcludeDeletedFiles(int taskId)
        {
            string path = $"{ dataPath }\\{ taskId }";

            if (!Directory.Exists(path))
                return;

            CTask t = GetTask(taskId);

            string[] files = GetFiles(path);

            for (int i = 0; i < t.Files.Count; i++)
            {
                CFile f = t.Files[i];

                if (files.Count(e => Path.GetFileName(e) == Path.GetFileName(f.FullPath)) == 0)
                    t.Files.Remove(f);
            }

            tasksChanged = true;
        }

        public void SetAutorun(bool value)
        {
            try
            {
                RegistryKey key = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run\\", true);
                
                string keyName = "TaskManager";

                if (value)
                    key.SetValue(keyName, $"{ assemblyPath }\\Task Manager.exe -t");
                else
                    key.DeleteValue(keyName, false);

                key.Close();

                Properties.Settings.Default.autorun = value;
                Properties.Settings.Default.Save();
            }
            catch (System.Security.SecurityException se)
            {
                throw new Exception($"Не удалось получить доступ к реестру. { se.Message } Попробуйте запустить приложение от имени Администратора.");
            }
        }

        public void SetFilterByCustomer(string[] names)
        {
            object[] idxs = customers.Where(e => names.Contains(e.Value)).Select(e => (object)e.Key).ToArray();

            if (activeFilters.Keys.Contains(EFilter.ByCustomer))
                activeFilters[EFilter.ByCustomer] = idxs;
            else
                activeFilters.Add(EFilter.ByCustomer, idxs);
        }

        public int GetTaskCountByExecutionDate(DateTime dateTime)
        {
            return tasks.Where(e => e.Status != CTask.EStatus.Завершена).Count(e => e.ExecutionDate.IsTheSameDay(dateTime));
        }

        public void SetFilterByExecutionDate(DateTime dateTime)
        {
            if (activeFilters.Keys.Contains(EFilter.ByExecutionDate))
            {
                List<DateTime> list = activeFilters[EFilter.ByExecutionDate].Select(e => Convert.ToDateTime(e)).ToList();

                if (list.Count(e => e.IsTheSameDay(dateTime)) == 0)
                {
                    list.Add(dateTime);

                    activeFilters[EFilter.ByExecutionDate] = list.Select(e => (object)e).ToArray();
                }

                list.Clear();
            }
            else
                activeFilters.Add(EFilter.ByExecutionDate, new object[] { dateTime });
        }
    }
}
