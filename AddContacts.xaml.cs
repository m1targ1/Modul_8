using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;

namespace Modul_8
{
    public partial class AddContacts : Window
    {
        private Contact _existingContact; // Переменная для хранения редактируемого контакта

        public event EventHandler<Contact> ContactAdded; // Событие для добавления контакта

        public AddContacts(Contact existingContact = null) // Конструктор, принимающий существующий контакт
        {
            InitializeComponent();
            _existingContact = existingContact;

            if (_existingContact != null)
            {
                // Заполнение полей для редактирования существующего контакта
                LastNameTextBox.Text = _existingContact.LastName;
                FirstNameTextBox.Text = _existingContact.FirstName;
                MiddleNameTextBox.Text = _existingContact.MiddleName;
                PositionTextBox.Text = _existingContact.Position;
                BirthDatePicker.SelectedDate = _existingContact.BirthDate;

                // Заполнение телефонных номеров
                foreach (var phone in _existingContact.PhoneNumbers)
                {
                    TextBox phoneTextBox = new TextBox { Text = phone, Margin = new Thickness(0, 0, 0, 5) };
                    PhoneNumbersPanel.Items.Add(phoneTextBox);
                }

                // Заполнение email адресов
                foreach (var email in _existingContact.Emails)
                {
                    TextBox emailTextBox = new TextBox { Text = email, Margin = new Thickness(0, 0, 0, 5) };
                    EmailsPanel.Items.Add(emailTextBox);
                }
            }
        }

        // Сохранение контакта при нажатии на кнопку
        private void SaveContact_Click(object sender, RoutedEventArgs e)
        {
            if (_existingContact != null)
            {
                // Обновляем существующий контакт
                _existingContact.LastName = LastNameTextBox.Text.Trim();
                _existingContact.FirstName = FirstNameTextBox.Text.Trim();
                _existingContact.MiddleName = MiddleNameTextBox.Text.Trim();
                _existingContact.Position = PositionTextBox.Text.Trim();
                _existingContact.BirthDate = BirthDatePicker.SelectedDate;

                _existingContact.PhoneNumbers.Clear();
                foreach (var item in PhoneNumbersPanel.Items)
                {
                    if (item is TextBox phoneTextBox && !string.IsNullOrWhiteSpace(phoneTextBox.Text))
                    {
                        _existingContact.PhoneNumbers.Add(phoneTextBox.Text.Trim());
                    }
                }

                _existingContact.Emails.Clear();
                foreach (var item in EmailsPanel.Items)
                {
                    if (item is TextBox emailTextBox && !string.IsNullOrWhiteSpace(emailTextBox.Text))
                    {
                        _existingContact.Emails.Add(emailTextBox.Text.Trim());
                    }
                }
            }
            else
            {
                // Создаем новый контакт
                _existingContact = new Contact
                {
                    LastName = LastNameTextBox.Text.Trim(),
                    FirstName = FirstNameTextBox.Text.Trim(),
                    MiddleName = MiddleNameTextBox.Text.Trim(),
                    Position = PositionTextBox.Text.Trim(),
                    BirthDate = BirthDatePicker.SelectedDate
                };

                foreach (var item in PhoneNumbersPanel.Items)
                {
                    if (item is TextBox phoneTextBox && !string.IsNullOrWhiteSpace(phoneTextBox.Text))
                    {
                        _existingContact.PhoneNumbers.Add(phoneTextBox.Text.Trim());
                    }
                }

                foreach (var item in EmailsPanel.Items)
                {
                    if (item is TextBox emailTextBox && !string.IsNullOrWhiteSpace(emailTextBox.Text))
                    {
                        _existingContact.Emails.Add(emailTextBox.Text.Trim());
                    }
                }
            }

            // Сохранение контакта в JSON
            SaveToJson(_existingContact);

            // Сообщаем, что контакт был добавлен или обновлен
            ContactAdded?.Invoke(this, _existingContact);

            // Закрываем окно
            this.Close();
        }

        // Сохранение списка контактов в JSON
        private void SaveToJson(Contact contact)
        {
            string jsonFilePath = "contacts.json";
            List<Contact> contacts;

            if (File.Exists(jsonFilePath))
            {
                var existingJson = File.ReadAllText(jsonFilePath);
                contacts = JsonConvert.DeserializeObject<List<Contact>>(existingJson) ?? new List<Contact>();

                var existingContact = contacts.Find(c => c.FullName == contact.FullName);
                if (existingContact != null)
                {
                    contacts.Remove(existingContact);
                }
            }
            else
            {
                contacts = new List<Contact>();
            }

            contacts.Add(contact);
            File.WriteAllText(jsonFilePath, JsonConvert.SerializeObject(contacts, Formatting.Indented));
        }

        // Добавление телефонного номера
        private void AddPhoneNumber_Click(object sender, RoutedEventArgs e)
        {
            TextBox phoneTextBox = new TextBox { Margin = new Thickness(0, 0, 0, 5) };
            PhoneNumbersPanel.Items.Add(phoneTextBox);
        }

        // Добавление email адреса
        private void AddEmail_Click(object sender, RoutedEventArgs e)
        {
            TextBox emailTextBox = new TextBox { Margin = new Thickness(0, 0, 0, 5) };
            EmailsPanel.Items.Add(emailTextBox);
        }
    }
}
