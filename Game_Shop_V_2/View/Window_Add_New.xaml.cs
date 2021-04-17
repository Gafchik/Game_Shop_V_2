using Game_Shop_V_2.Model.Base;
using Game_Shop_V_2.ViewModel;
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
using System.Windows.Shapes;

namespace Game_Shop_V_2.View
{
    /// <summary>
    /// Логика взаимодействия для Window_Add_New.xaml
    /// </summary>
    public partial class Window_Add_New : Window
    {
        public Window_Add_New()
        {
            InitializeComponent();
            this.DataContext = new ModelView();
            Button_ADD.Click += Button_ADD_Click;
        }

        private void Button_ADD_Click(object sender, RoutedEventArgs e)
        {
           // ComboBox_Game_Style
//ComboBox_Game_Studio
//ComboBox_Game_Mod

            var BD = (DataContext as ModelView);
           Game temp_Game = new Game();
            temp_Game.Game_Name = TextBlock_Game_Name.Text;
            if (TextBlock_New_Game_Studio.Text != "")
            {
                BD.sourse.Studios.Add(new Studio() { Studio_Name = TextBlock_New_Game_Studio.Text });
                BD.sourse.SaveChanges();
                temp_Game.Studio = BD.sourse.Studios.ToList().Find(i => i.Studio_Name == TextBlock_New_Game_Studio.Text);
            }
            else
                temp_Game.Studio = BD.Curent_studio;
            if (TextBlock_New_Game_Stile.Text != "")
            {
                BD.sourse.Styles.Add(new Model.Base.Style() { Style_Game_Name = TextBlock_New_Game_Stile.Text });
                BD.sourse.SaveChanges();
                temp_Game.Style = BD.sourse.Styles.ToList().Find(i => i.Style_Game_Name == TextBlock_New_Game_Stile.Text);
            }
            else
                temp_Game.Style = BD.Curent_style;

            try
            {
                temp_Game.Game_Year_Releas = Calendar.SelectedDate.Value;
            }
            catch (Exception)
            {
                temp_Game.Game_Year_Releas = DateTime.Now;
            }
            temp_Game.Mod_Game = BD.Curent_mod;
            try { temp_Game.Game_Count_Sell = Convert.ToInt32(TextBlock_Game_Sells.Text); }
           
            catch (Exception)
            {
                temp_Game.Game_Count_Sell = 0;
                MessageBox.Show("Не правильно введенное число\r\n значение установлено по умолчанию 0", "ОЙ", MessageBoxButton.OK, MessageBoxImage.Error);

            }
           
            if (BD.sourse.Games.ToList().Exists(i => i.Game_Name == temp_Game.Game_Name))
            {
                GC.Collect(GC.GetGeneration(temp_Game));
                MessageBox.Show("Игра с таким именем уже существует", "ОЙ", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
               BD.sourse.Games.Add(temp_Game);
                try
                { 
                    BD.sourse.SaveChanges();
                    MessageBox.Show("Информация успешно добавлена", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception)
                {
                   
                    MessageBox.Show("Ты что-то сделал не правильно\r\n Дибил", "ОЙ", MessageBoxButton.OK, MessageBoxImage.Error);

                }
               
            }          
         
            Close();
        }
}
}
