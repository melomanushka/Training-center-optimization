﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Practice.view
{
    /// <summary>
    /// Логика взаимодействия для HelpPage.xaml
    /// </summary>
    public partial class HelpPage : Page
    {
        public HelpPage()
        {
            InitializeComponent();
            using(var context = new PracticeStudyCenterEntities())
            {
                var info = context.CompanyInfo.FirstOrDefault(u => u.CompanyID == 1);
                txblAdress.Text = info.PhysicalAddress;
                txblName.Text = info.ContactPerson;
                txblNumber.Text = info.PhoneNumber;
            }
        }
    }
}
