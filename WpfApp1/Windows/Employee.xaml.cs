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
using System.Windows.Shapes;
using WpfApp1.EF;

namespace WpfApp1.Windows
{
    /// <summary>
    /// Логика взаимодействия для Employee.xaml
    /// </summary>
    public partial class Employee : Window
    {
        List<string> ListSort = new List<string>()
        {
        "По умолчанию","По фамилии","По имени","По телефону","По почте","По должности"
        };
        public Employee()
        {
            InitializeComponent();
            Filter();
            lvStaff.ItemsSource = ClassHelper.Class1.Context.Employee.ToList();
            cmbSort.ItemsSource = ListSort;
            cmbSort.SelectedIndex = 0;
        }
        //добавление

        private void btnStaffAdd_Click(object sender, RoutedEventArgs e)
        {
            EmployeeAdd staffAddWindow = new EmployeeAdd();
            staffAddWindow.ShowDialog();
            lvStaff.ItemsSource = ClassHelper.Class1.Context.Employee.ToList();
            Filter();
        }

        private void Filter()
        {
            List<EF.Employee> ListStaff = new List<EF.Employee>();
            ListStaff = ClassHelper.Class1.Context.Employee.Where(i => i.isDeleted == false).ToList();
            //Фильтрация
            ListStaff = ListStaff.Where(i =>
            i.LastName.ToLower().Contains(txtSearch.Text.ToLower()) ||
            i.FirstName.ToLower().Contains(txtSearch.Text.ToLower()) ||
            i.MiddleName.ToLower().Contains(txtSearch.Text.ToLower()) ||
            i.FIO.ToLower().Contains(txtSearch.Text.ToLower()) ||
            i.Phone.ToLower().Contains(txtSearch.Text.ToLower()) ||
            i.Email.ToLower().Contains(txtSearch.Text.ToLower())).ToList();

            switch (cmbSort.SelectedIndex)
            {
                case 0:
                    ListStaff = ListStaff.OrderBy(i => i.ID).ToList();
                    break;
                case 1:
                    ListStaff = ListStaff.OrderBy(i => i.LastName).ToList();
                    break;
                case 2:
                    ListStaff = ListStaff.OrderBy(i => i.FirstName).ToList();
                    break;
                case 3:
                    ListStaff = ListStaff.OrderBy(i => i.Phone).ToList();
                    break;
                case 4:
                    ListStaff = ListStaff.OrderBy(i => i.Email).ToList();
                    break;
                case 5:
                    ListStaff = ListStaff.OrderBy(i => i.IDRole).ToList();
                    break;
                default:
                    ListStaff = ListStaff.OrderBy(i => i.ID).ToList();
                    break;
            }
            lvStaff.ItemsSource = ListStaff;
        }

        private void cmbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filter();
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            Filter();
        }

        //Удаление

        private void lvStaff_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete || e.Key == Key.Back)
            {
                try
                {
                    if (lvStaff.SelectedItem is EF.Employee)
                    {
                        var resmsg = MessageBox.Show("Удалить пользователя?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (resmsg == MessageBoxResult.No)
                        {
                            return;
                        }
                        var stf = lvStaff.SelectedItem as EF.Employee;
                        stf.isDeleted = true;
                        //ClassHelper.AppData.Context.Staff.Remove(stf);
                        ClassHelper.Class1.Context.SaveChanges();
                        MessageBox.Show("Пользователь успешно удален", "Удаление", MessageBoxButton.OK, MessageBoxImage.Information);
                        Filter();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void lvStaff_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (lvStaff.SelectedItem is EF.Employee)
            {
                var stf = lvStaff.SelectedItem as EF.Employee;
                EmployeeAdd staffAddWindow = new EmployeeAdd(stf);
                staffAddWindow.ShowDialog();
                Filter();
            }
        }
    }
}
