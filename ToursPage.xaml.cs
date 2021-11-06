using System;
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

namespace ToursBase
{
    /// <summary>
    /// Логика взаимодействия для ToursPage.xaml
    /// </summary>
    public partial class ToursPage : Page
    {
        /// <summary>
        /// Вывод туров с учетом фильтров
        /// </summary>
        public ToursPage()
        {
            InitializeComponent();

            var allTypes = ToursBaseEntities.GetContext().Type.ToList();
            allTypes.Insert(0, new Type
            {
                Name = "Все типы"
            });
            ComboType.ItemsSource = allTypes;

            ChekActual.IsChecked = true;
            ComboType.SelectedItem = 0;

            UpdateTours();


        }
        
        private void UpdateTours()
        {
            var currentTours = ToursBaseEntities.GetContext().Tour.ToList();
            if (ComboType.SelectedIndex > 0)
                currentTours = currentTours.Where(p => p.Type.Contains(ComboType.SelectedItem as Type)).ToList();
            currentTours = currentTours.Where(p => p.Name.ToLower().Contains(TBoxSearch.Text.ToLower())).ToList();

            if (ChekActual != null)
            {
                if (ChekActual.IsChecked.Value)
                {
                    currentTours = currentTours.Where(p => p.IsActual).ToList();
                }
            }
            //Считаем полную стоимость всех выбранных туров с учетом кол-ва билетов
            ///  У каждого отеля умножаем Цену на Кол-во билетов и прибавляем в общую переменную tourPricesum
            ///  Выводим в TextBlock сумму всех туров с 2 знаками после запятой
            decimal toursPriceSum = 0;
            foreach (Tour tour in currentTours)
            {
                decimal toursPrice = tour.Price * Convert.ToDecimal(tour.TicketCount);
                toursPriceSum += toursPrice;
            }
            sumPrice.Text = toursPriceSum.ToString("N2") + "РУБ";
            LViewTours.ItemsSource = currentTours.OrderBy(p => p.TicketCount).ToList();

        }
        /// <summary>
        /// Обновление туров по событию
        /// </summary>
        private void TBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateTours();
        }

        /// <summary>
        /// Обновление туров по событию
        /// </summary>
        private void ComboType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateTours();
        }

        /// <summary>
        /// Обновление туров по событию
        /// </summary>
        private void ChekActual_Checked(object sender, RoutedEventArgs e)
        {
            UpdateTours();
        }

        /// <summary>
        /// Обновление туров по событию
        /// </summary>
        private void ChekActual_Unchecked(object sender, RoutedEventArgs e)
        {
            UpdateTours();
        }

        /// <summary>
        /// Обновление туров по событию
        /// </summary>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new HotelsPage());
        }

       
    }
}
