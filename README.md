# ContactBookTest

Приложение разработано с использованием паттерна MVVM (Model-View-ViewModel) и Entity Framework для работы с базой данных. Приложение позволяет пользователю добавлять, редактировать, удалять, сортировать, фильтровать и искать контакты по различным параметрам.

# Архитектура Приложения
## Паттерн MVVM

Model: Представляет данные. В данном случае, моделью является класс Contact, который описывает контактную информацию.

View: Отвечает за отображение данных пользователю. Включает в себя файлы XAML, такие как MainWindow.xaml, AddContactWindow.xaml, и EditContactWindow.xaml.

ViewModel: Связывает Model и View, предоставляя данные и команды для управления ими. Включает в себя классы ContactViewModel, AddContactViewModel, и EditContactViewModel.

## Entity Framework
Для работы с базой данных используется Entity Framework, который обеспечивает взаимодействие с базой данных PostgreSQL. Контекст базы данных реализован в классе ContactContext.

## Асинхронные Команды
Все операции с базой данных и вьюшкой выполняются асинхронно с использованием асинхронных команд, реализованных в классе AsyncRelayCommand.

# Структура Проекта
Commands: Содержит класс AsyncRelayCommand, который реализует асинхронные команды для ViewModel.

Data: Содержит класс ContactContext, который представляет контекст базы данных.

Models: Содержит класс Contact, который представляет модель данных контакта.

ViewModels: Содержит ViewModel-классы:

ContactViewModel: Основной ViewModel для управления контактами.

AddContactViewModel: ViewModel для добавления нового контакта.

EditContactViewModel: ViewModel для редактирования существующего контакта.

Views: Содержит XAML-файлы для отображения:

AddContactWindow.xaml: Окно для добавления нового контакта.

EditContactWindow.xaml: Окно для редактирования существующего контакта.

MainWindow.xaml: Основное окно приложения, отображающее список контактов.

# Реализация Требований

## Добавление: 
Пользователь может добавить новый контакт через окно AddContactWindow. Данные валидируются и сохраняются в базу данных с использованием Entity Framework.

## Редактирование: 
Пользователь может редактировать существующий контакт через окно EditContactWindow. Изменения валидируются и сохраняются в базу данных.

## Удаление: 
Пользователь может удалить контакт, выбрав его в списке и нажав кнопку "Delete". Контакт удаляется из базы данных.

## Сортировка: 
Контакты могут быть отсортированы по имени с использованием кнопки "Sort".

## Фильтрация: 
Контакты могут быть отфильтрованы по типу (например, Friend, Coworker, Unknowned) с использованием выпадающего списка.

## Поиск: 
Пользователь может искать контакты по имени, фамилии, телефону или email с использованием поля поиска.
