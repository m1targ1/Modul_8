using Newtonsoft.Json; // Пространство имен для работы с JSON
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel; // Используется для создания коллекций с поддержкой привязки
using System.IO; // Добавлено пространство имен для работы с файлами
using System.Linq; // Пространство имен для использования LINQ
using System.Windows; // Пространство имен для работы с WPF
using System.Windows.Controls; // Пространство имен для работы с элементами управления WPF

namespace Modul_8
{
    public partial class MainWindow : Window
    {       
        public ObservableCollection<Contact> Contacts { get; set; } = new ObservableCollection<Contact>(); // Объявление коллекции для хранения контактов с поддержкой привязки

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this; // Установка контекста данных для привязки
            LoadContacts(); // Загрузка контактов
        }
               
        public void LoadContacts() // Метод для загрузки контактов из JSON файла
        {
            string jsonFilePath = "contacts.json"; // Путь к файлу
            if (File.Exists(jsonFilePath)) // Проверка, существует ли файл
            {
                var existingJson = File.ReadAllText(jsonFilePath); // Чтение содержимого файла
                var contacts = JsonConvert.DeserializeObject<List<Contact>>(existingJson); // Десериализация JSON в список контактов
                if (contacts != null)
                {
                    Contacts.Clear(); // Очистка текущей коллекцию
                    foreach (var contact in contacts)
                    {
                        Contacts.Add(contact); // Добавление каждого контакта в ObservableCollection
                    }
                }
            }
        }
      
        public void DeleteContact(Contact contact) // Метод для удаления контакта
        {
            if (Contacts.Contains(contact)) // Проверка, существует ли контакт в коллекции
            {
                Contacts.Remove(contact); // Удаление контакта из коллекции
                SaveContacts();
            }
        }
               
        private void ListBox_Contacts_SelectionChanged(object sender, SelectionChangedEventArgs e) // Обработчик события изменения выбора в ListBox
        {
            if (ListBox_Contacts.SelectedItem is Contact selectedContact) // Проверка, выбран ли контакт
            {
                var contactView = new ContactView(selectedContact); // Создание окна для отображения выбранного контакта
                contactView.Show();
            }
        }
        
        private void btn_AddContact_Click(object sender, RoutedEventArgs e) // Обработчик события нажатия кнопки добавления контакта
        {
            var addContactWindow = new AddContacts();
            // Подписка на событие добавления контакта
            addContactWindow.ContactAdded += (s, contact) =>
            {
                Contacts.Add(contact); // Добавление нового контакта в коллекцию
                SaveContacts(); // Сохранение изменений в файл
            };
            addContactWindow.ShowDialog(); // Показ диалоговое окно
        }
       
        private void SaveContacts() // Метод для сохранения контактов в JSON файл
        {
            string jsonFilePath = "contacts.json";
            File.WriteAllText(jsonFilePath, JsonConvert.SerializeObject(Contacts.ToList(), Formatting.Indented)); // Сериализация коллекции и запись в файл
        }
       
        private void TbSearch_TextChanged(object sender, TextChangedEventArgs e) // Обработчик события изменения текста в текстовом поле для поиска
        {
            // Поиск контактов по имени
            string searchText = textBox_Search.Text.Trim(); // Получение текста из текстового поля

            if (!string.IsNullOrEmpty(searchText)) // Проверка, не пустой ли текст
            {
                // Присваивание только подходящих контактов
                ListBox_Contacts.ItemsSource = Contacts.Where(c =>
                    c.FullName.ToUpper().Contains(searchText.ToUpper()) // Поиск по полному имени
                ).ToList(); // Преобразование в список (чтобы избежать изменений в оригинальной коллекции)
            }
            else
            {
                ListBox_Contacts.ItemsSource = Contacts.ToList(); // Если текст пустой, показывает все контакты
            }
        }
    }

    // Класс для представления контакта
    public class Contact
    {
        public string LastName { get; set; } // Фамилия
        public string FirstName { get; set; } // Имя
        public string MiddleName { get; set; } // Отчество
        public List<string> PhoneNumbers { get; set; } = new List<string>(); // Список телефонных номеров
        public string Position { get; set; } // Должность
        public DateTime? BirthDate { get; set; } // Дата рождения
        public List<string> Emails { get; set; } = new List<string>(); // Список email адресов
        public string FullName => $"{LastName} {FirstName} {MiddleName}"; // Полное имя
        public string IconSource { get; set; } = "D:\\3 курс\\Учебная практика по программированию\\.Выполнение\\Модуль8\\C#\\Modul_8\\bin\\Debug\\user.png"; // Путь к иконке по умолчанию
    }
}
