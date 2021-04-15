using Game_Shop_V_2.Model.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Game_Shop_V_2.ViewModel
{
    public class ModelView : INotifyPropertyChanged
    {
        public  DB_Game sourse { get; set; }
        public  ObservableCollection<Game> Games { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
           => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        private Game curent_game;
        public ModelView()
        {
            sourse = new DB_Game();
            Update_Games();
           // Games = sourse.Games;
        }

      

        public Game Curent_game
        {
            get { return curent_game; }
            set { curent_game = value; OnPropertyChanged("Curent_game"); }
        }
        private void Update_Games()
        {
            if (Games != null)
                Games.Clear();
            Games = new ObservableCollection<Game>();
            sourse.Games.ToList().ForEach(i => Games.Add(i));
        }

    }
}
