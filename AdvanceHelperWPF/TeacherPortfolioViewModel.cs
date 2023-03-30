using System.Collections.Generic;
using AHlibrary;
using Caliburn.Micro;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace AdvanceHelperWPF
{
    class TeacherPortfolioViewModel : ObservableObject
    {
        private Teacher selectedTeacher;
        private BindableCollection<Teacher> teachers;
        private string imageSource;

        public BindableCollection<Teacher> Teachers
        {
            get { return teachers; }
            set { SetProperty(ref teachers, value); }
        }

        public Teacher SelectedTeacher
        {
            get { return selectedTeacher; }
            set
            {
                selectedTeacher = value;
                ImageSource = selectedTeacher.ImagePath;
                OnPropertyChanged(nameof(SelectedTeacher));
            }
        }

        public string ImageSource
        {
            get { return imageSource; }
            set { SetProperty(ref imageSource, value); }
        }

        public TeacherPortfolioViewModel()
        {
            DBconnect dBconnect = new DBconnect();
            List<Teacher> teachersFromDatabase = dBconnect.GetTeachersFromDatabase("преподаватели");
            // Загрузка преподавателей из базы данных и добавление их в коллекцию Teachers
            Teachers = new BindableCollection<Teacher>(teachersFromDatabase);
            // Выбор первого преподавателя из коллекции
            if (Teachers.Count > 0)
            {
                SelectedTeacher = Teachers[0];
            }
        }
    }
}
