using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace MastersApp2021
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static Entities.DataBaseNameEntities Context
        {
            get;
        }
        = new Entities.DataBaseNameEntities();
        public static Entities.User CurrentUser = null;
    }
}
