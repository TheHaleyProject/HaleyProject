using Haley.Abstractions;
using Haley.Enums;
using Haley.Events;
using Haley.Models;
using Haley.MVVM;
using Haley.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace WPF.Test
{
    public class MainVM :ChangeNotifier
    {

        private ObservableCollection<Person> _soemthing;
        public ObservableCollection<Person> something
        {
            get { return _soemthing; }
            set { SetProp(ref _soemthing, value); }
        }

        private ObservableCollection<Person> _selecteditems;
        public ObservableCollection<Person> selecteditems
        {
            get { return _selecteditems; }
            set { SetProp(ref _selecteditems, value); }
        }

        public MainVM() 
        {
            ObservableCollection<Person> hello = new ObservableCollection<Person>();
            hello.Add(new Person("Senguttuvan", 32));
            hello.Add(new Person("Latha", 32));
            hello.Add(new Person("Bhadri", 32));
            hello.Add(new Person("Pranav", 32));
            hello.Add(new Person("Mahalingam", 32));
            hello.Add(new Person("Ramasamy", 32));
            hello.Add(new Person("Buna", 32));
            something = hello;
            selecteditems = new ObservableCollection<Person>();
        }
    }

    public class Person :ChangeNotifier
    {
        private string _Name;
        public string Name
        {
            get { return _Name; }
            set { SetProp(ref _Name, value); }
        }

        private int _age;
        public int Age
        {
            get { return _age; }
            set { SetProp(ref _age, value); }
        }


        public Person(string name, int age)
        {
            Name = name; Age = age;
        }
    }
}
