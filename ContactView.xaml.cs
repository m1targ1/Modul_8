using System.Windows;

namespace Modul_8
{
    public partial class ContactView : Window
    {
        public ContactView(Contact contact)
        {
            InitializeComponent();
            DataContext = contact; // Устанавливаем контекст данных
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            // Получение контакта для удаления
            Contact contactToDelete = DataContext as Contact;

            if (contactToDelete != null)
            {
                // Удаление контакта из коллекции в родительском окне
                var mainWindow = Application.Current.MainWindow as MainWindow; // Получаем родительское окно
                if (mainWindow != null)
                {
                    mainWindow.DeleteContact(contactToDelete); // Метод для удаления контакта из коллекции
                }

                // Закрытие окна просмотра контакта
                this.Close();
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            // Открытие окна редактирования контакта
            AddContacts editContactView = new AddContacts(DataContext as Contact);
            editContactView.ShowDialog();
        }
    }
}
