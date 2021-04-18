using System;
using Game_Shop_V_2.Model.Base;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Game_Shop_V_2.View.ViewModel;

namespace Game_Shop_V_2.ViewModel
{
    public class ModelView : INotifyPropertyChanged
    {
        public DB_Game sourse;
        public ObservableCollection<Game> Games { get; set; }


        public ObservableCollection<Studio> Studios { get; set; }


        public ObservableCollection<Model.Base.Style> Styles { get; set; }


        public ObservableCollection<Mod_Game> Mod_s { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
           => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        public ModelView()
        {
            sourse = new DB_Game();
            /*  Games = new ObservableCollection<Game>();
              sourse.Games.ToList().ForEach(i => Games.Add(i));
              Styles = new ObservableCollection<Model.Base.Style>();
              sourse.Styles.ToList().ForEach(i => Styles.Add(i));
              Studios = new ObservableCollection<Studio>();
              sourse.Studios.ToList().ForEach(i => Studios.Add(i));*/
            Update_Games();


        }

        private RelayCommand serch_comand;
        public RelayCommand Serch_comand
        {
            get
            {
                return serch_comand ?? (serch_comand = new RelayCommand(act =>
                {
                    if (Games != null)
                        Games = new ObservableCollection<Game>(Find(i => i.Game_Name.ToLower().Contains(Serch_srt.ToLower())));
                    OnPropertyChanged("Games");
                }));


            }
        }

       IEnumerable<Game> Find(Func<Game, bool> filter) => sourse.Games.ToList().Where(filter).ToList();
       


        private string serch_srt;

        public string Serch_srt
        {
            get { return serch_srt; }
            set { serch_srt = value; OnPropertyChanged("Serch_srt"); Games = new ObservableCollection<Game>(Find(i => i.Game_Name.ToLower().Contains(Serch_srt.ToLower())));
                OnPropertyChanged("Games"); 
            }
        }




        private Game curent_game;
        public Game Curent_game
        {
            get { return curent_game; }
            set { curent_game = value; OnPropertyChanged("Curent_game"); }
        }


        private Mod_Game curent_mod;
        public Mod_Game Curent_mod
        {
            get { return curent_mod; }
            set { curent_mod = value; OnPropertyChanged("Curent_game"); }
        }





        private Studio curent_studio;
        public Studio Curent_studio
        {
            get { return curent_studio; }
            set { curent_studio = value; OnPropertyChanged("Curent_game"); }
        }







        private Model.Base.Style curent_style;
        public Model.Base.Style Curent_style
        {
            get { return curent_style; }
            set { curent_style = value; OnPropertyChanged("Curent_style"); }
        }

        internal void Save() // сохранение изминений
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

        public void Update_Games() // обновление  списка
        {
            if (Games == null)
                Games = new ObservableCollection<Game>();
            Games.Clear();
            sourse.Games.ToList().ForEach(i => Games.Add(i));
            if (Styles == null)
                Styles = new ObservableCollection<Model.Base.Style>();
            Styles.Clear();
            sourse.Styles.ToList().ForEach(i => Styles.Add(i));
            if (Studios == null)
                Studios = new ObservableCollection<Studio>();
            Studios.Clear();
            sourse.Studios.ToList().ForEach(i => Studios.Add(i));
            if (Mod_s == null)
                Mod_s = new ObservableCollection<Mod_Game>();
            Mod_s.Clear();
            sourse.Mod_Game.ToList().ForEach(i => Mod_s.Add(i));

        }
        internal void Dell()
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
