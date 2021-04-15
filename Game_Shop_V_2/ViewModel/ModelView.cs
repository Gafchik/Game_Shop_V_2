using Game_Shop_V_2.Model.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Game_Shop_V_2.ViewModel
{
    public class ModelView : INotifyPropertyChanged
    {
        DB_Game sourse;
        public  ObservableCollection<Game> Games { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
           => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        private Game curent_game;
        public ModelView()
        {
            sourse = new DB_Game();
            Update_Games();        
        }

      

        public Game Curent_game
        {
            get { return curent_game; }
            set { curent_game = value; OnPropertyChanged("Curent_game"); }
        }

        internal  void Save()
        {
            sourse.SaveChanges();
            Update_Games();
        }

        private void Update_Games()
        {
            if (Games != null)
                Games.Clear();
            Games = new ObservableCollection<Game>();
            sourse.Games.ToList().ForEach(i => Games.Add(i));
        }
        internal static void Dell_Game(object SelectedItem)
        {
            if (MessageBox.Show("Удалить игру?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                return;
            else
            {
                lock (typeof(ModelView))
                {
                    using (DB_Game sourse = new DB_Game())
                    {
                        if (sourse.Games.ToList().Exists(i => i.Game_Name == (SelectedItem as string)))
                        {
                            sourse.Games.Remove(sourse.Games.ToList().Find(i => i.Game_Name == (SelectedItem as string)));
                            sourse.SaveChanges();
                        }
                        else
                            MessageBox.Show("Что-то пошло нет так", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

      /*  internal static void Info_Game(object SelectedItem)
        {
            var window_info = new Window_Rezult();

            if (BD.Games.ToList().Exists(i => i.Game_Name == (SelectedItem as string)))
            {
                window_info.Get_Game(BD.Games.ToList().Find(i => i.Game_Name == (SelectedItem as string)));
                window_info.ShowDialog();
            }
            else
                MessageBox.Show("Что-то пошло нет так", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }*/
        #region edit row game
       /* internal  void In_Game_Edit_Style_id(Game curent_game, string Game_Style)
        {
            lock (this)
            {
                using (DB_Game sourse = new DB_Game())
                {
                    curent_game.Game_Studio_id = sourse.Styles.ToList().Find(i => i.Style_Game_Name == Game_Style).Id;
                    sourse.SaveChanges();
                }
            }
        }*/

      /*  internal static void In_Game_Edit_Game_Count_Sell(Game curent_game, string Game_Sells)
        {
            lock (typeof(View_Model_Game))
            {
                using (Model_Game_Shop sourse = new Model_Game_Shop())
                {
                    curent_game.Game_Count_Sell = Convert.ToInt32(Game_Sells);
                    sourse.SaveChanges();
                }
            }
        }*/

       /* internal static void In_Game_Edit_Game_Mod_id(Game curent_game, string Game_Mod)
        {
            lock (typeof(View_Model_Game))
            {
                using (Model_Game_Shop sourse = new Model_Game_Shop())
                {
                    curent_game.Game_Mod_id = BD.Mod_Game.ToList().Find(i => i.Mod_Game_Name == Game_Mod).Id;
                    sourse.SaveChanges();
                }
            }
        }*/

        /*internal static void In_Game_Edit_Year_Releas(Game curent_game, Calendar calendar)
        {
            lock (typeof(View_Model_Game))
            {
                using (Model_Game_Shop sourse = new Model_Game_Shop())
                {
                    curent_game.Game_Year_Releas = new DateTime(calendar.SelectedDate.Value.Year, calendar.SelectedDate.Value.Month, calendar.SelectedDate.Value.Day);
                    sourse.SaveChanges();
                }
            }
        }*/

       /* internal static void In_Game_Edit_Studio_id(Game curent_game, string Game_Studio)
        {
            lock (typeof(View_Model_Game))
            {
                using (Model_Game_Shop sourse = new Model_Game_Shop())
                {
                    curent_game.Game_Studio_id = BD.Studios.ToList().Find(i => i.Studio_Name == Game_Studio).Id;
                    sourse.SaveChanges();
                }
            }
        }*/

       /* internal static void In_Game_Edit_Name(Game curent_game, string text)
        {
            lock (typeof(View_Model_Game))
            {
                using (Model_Game_Shop sourse = new Model_Game_Shop())
                {
                    curent_game.Game_Name = text;
                    sourse.SaveChanges();
                }
            }
        }*/
        #endregion
    }
}
