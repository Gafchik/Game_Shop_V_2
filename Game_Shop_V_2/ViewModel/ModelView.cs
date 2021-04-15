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
        public  ObservableCollection<Model.Base.Style> Styles { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
           => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        public ModelView()
        {
            sourse = new DB_Game();
            Update_Games();
           
        }
     

        private Game curent_game;
        public Game Curent_game
        {
            get { return curent_game; }
            set { curent_game = value; OnPropertyChanged("Curent_game"); }
        }


        private Model.Base.Style curent_style;
        public Model.Base.Style Curent_style
        {
            get { return curent_style; }
            set { curent_style = value; OnPropertyChanged("Curent_style"); }
        }

        internal  void Save() // сохранение изминений
        {
            if (MessageBox.Show("Сохранить изминения?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                return;
            else
            {
                if (sourse.Games.ToList().Exists(i => i.Id == Curent_game.Id))
                {
                    sourse.SaveChanges();
                    Update_Games();
                }
                else
                    MessageBox.Show("Что-то пошло нет так", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
           
        }

        private void Update_Games() // обновление  списка
        {
            if (Games == null)
                Games = new ObservableCollection<Game>();
            Games.Clear();
            sourse.Games.ToList().ForEach(i => Games.Add(i));
            if (Styles == null)
                Styles = new ObservableCollection<Model.Base.Style>();
            Styles.Clear();
            sourse.Styles.ToList().ForEach(i => Styles.Add(i));
           /* Curent_style = sourse.Styles.ToList().Find()*/
        }
        internal  void Dell()
        {
            if (MessageBox.Show("Удалить игру?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                return;
            else
            {
                if (sourse.Games.ToList().Exists(i => i.Id == Curent_game.Id))
                {
                    sourse.Games.Remove(sourse.Games.ToList().Find(i => i.Id == Curent_game.Id));
                    sourse.SaveChanges();
                    Update_Games();
                }
                else
                    MessageBox.Show("Что-то пошло нет так", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
