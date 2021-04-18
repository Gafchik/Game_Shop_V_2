using System;
using Game_Shop_V_2.Model.Base;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using Game_Shop_V_2.View.ViewModel;
using System.Data.SqlClient;
using System.Data;
using Dapper;
using System.Configuration;
using Game_Shop_V_2.View;

namespace Game_Shop_V_2.ViewModel
{
    public class ModelView : INotifyPropertyChanged
    {     
        public ObservableCollection<Game> Games { get; set; }
        public ObservableCollection<Studio> Studios { get; set; }
        public ObservableCollection<Model.Base.Style> Styles { get; set; }
        public ObservableCollection<Mod_Game> Mod_s { get; set; }

        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged; // ивент обновления
        public void OnPropertyChanged([CallerMemberName] string prop = "")
           => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        #endregion

        public ModelView()
        {
            InitializeComponent();
        }

        public void InitializeComponent()
        {
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DB_Game"].ConnectionString))
            {
                if (Studios != null)
                    Studios.Clear();
                Studios = new ObservableCollection<Studio>(db.Query<Studio>("SELECT * FROM [Studio]").ToList());
                if (Styles != null)
                    Styles.Clear();
                Styles = new ObservableCollection<Model.Base.Style>(db.Query<Model.Base.Style>("SELECT * FROM [Style]").ToList());
                if (Mod_s != null)
                    Mod_s.Clear();
                Mod_s = new ObservableCollection<Mod_Game>(db.Query<Mod_Game>("SELECT * FROM [Mod_Game]").ToList());
                if (Games != null)
                    Games.Clear();
                Games = new ObservableCollection<Game>(db.Query<Game>("SELECT * FROM [Game]").ToList());
                Games.ToList().ForEach(i => i.Studio = Studios.ToList().Find(j => j.Id == i.Game_Studio_id));
                Games.ToList().ForEach(i => i.Style = Styles.ToList().Find(j => j.Id == i.Game_Style_id));
                Games.ToList().ForEach(i => i.Mod_Game = Mod_s.ToList().Find(j => j.Id == i.Game_Mod_id));
                OnPropertyChanged("Games");
            }
        }      

      

        #region serch_srt
        private string serch_srt;

        public string Serch_srt
        {
            get { return serch_srt; }
            set 
            { 
                serch_srt = value; OnPropertyChanged("Serch_srt");               
                    using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DB_Game"].ConnectionString))
                    {
                        Games = new ObservableCollection<Game>(db.Query<Game>("SELECT * FROM [Game]").ToList());
                        Games.ToList().ForEach(i => i.Studio = Studios.ToList().Find(j => j.Id == i.Game_Studio_id));
                        Games.ToList().ForEach(i => i.Style = Styles.ToList().Find(j => j.Id == i.Game_Style_id));
                        Games.ToList().ForEach(i => i.Mod_Game = Mod_s.ToList().Find(j => j.Id == i.Game_Mod_id));
                    }
                    Games = new ObservableCollection<Game>(Find(i => i.Game_Name.ToLower().Contains(Serch_srt.ToLower())));
                    OnPropertyChanged("Games");
                
            }
        }
        #endregion

        #region curent_game
        private Game curent_game;
        public Game Curent_game
        {
            get { return curent_game; }
            set { curent_game = value; OnPropertyChanged("Curent_game"); }
        }
        #endregion

        #region curent_mod
        private Mod_Game curent_mod;
        public Mod_Game Curent_mod
        {
            get { return curent_mod; }
            set { curent_mod = value; OnPropertyChanged("Curent_game"); }
        }
        #endregion

        #region curent_studio
        private Studio curent_studio;
        public Studio Curent_studio
        {
            get { return curent_studio; }
            set { curent_studio = value; OnPropertyChanged("Curent_game"); }
        }
        #endregion

        #region curent_style
        private Model.Base.Style curent_style;
        public Model.Base.Style Curent_style
        {
            get { return curent_style; }
            set { curent_style = value; OnPropertyChanged("Curent_style"); }
        }
        #endregion

        #region Save

        private RelayCommand save_comand;
        public RelayCommand Save_comand
        {
            get
            {
                return save_comand ?? (serch_comand = new RelayCommand(act =>
                {
                    if (MessageBox.Show("Сохранить изминения?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                        return;
                    else
                    {
                        if (Games.ToList().Exists(i => i.Id == Curent_game.Id))
                        {
                            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DB_Game"].ConnectionString))
                            {
                                db.Open();
                                using (var transaction = db.BeginTransaction())
                                {
                                    try
                                    {

                                        db.Execute("UPDATE [Game] SET Game_Name = @Game_Name," + "Game_Studio_id = @Game_Studio_id," +
                                                    "Game_Style_id = @Game_Style_id," + "Game_Year_Releas = @Game_Year_Releas," +
                                                    "Game_Mod_id = @Game_Mod_id," + "Game_Count_Sell = @Game_Count_Sell " +
                                                      "WHERE Id = @Id",
                                            new
                                            {
                                                Id = Curent_game.Id,
                                                Game_Name = Curent_game.Game_Name,
                                                Game_Studio_id = Curent_game.Studio.Id,
                                                Game_Style_id = Curent_game.Style.Id,
                                                Game_Year_Releas = Curent_game.Game_Year_Releas,
                                                Game_Mod_id = Curent_game.Mod_Game.Id,
                                                Game_Count_Sell = Curent_game.Game_Count_Sell
                                            }, transaction);
                                        transaction.Commit();
                                    }
                                    catch (System.Exception ex)
                                    {
                                        transaction.Rollback();
                                        throw ex;
                                    }
                                }
                                Games = new ObservableCollection<Game>(db.Query<Game>("SELECT * FROM [Game]").ToList());
                                Games.ToList().ForEach(i => i.Studio = Studios.ToList().Find(j => j.Id == i.Game_Studio_id));
                                Games.ToList().ForEach(i => i.Style = Styles.ToList().Find(j => j.Id == i.Game_Style_id));
                                Games.ToList().ForEach(i => i.Mod_Game = Mod_s.ToList().Find(j => j.Id == i.Game_Mod_id));
                                OnPropertyChanged("Games");
                            }

                        }
                        else
                            MessageBox.Show("Что-то пошло нет так", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    OnPropertyChanged("Games");
                }));


            }
        }

       /* internal void Save() // сохранение изминений
        {
            if (MessageBox.Show("Сохранить изминения?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                return;
            else
            {
                if (Games.ToList().Exists(i => i.Id == Curent_game.Id))
                {
                    using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DB_Game"].ConnectionString))
                    {
                        db.Open();
                        using (var transaction = db.BeginTransaction())
                        {
                            try
                            {

                                db.Execute("UPDATE [Game] SET Game_Name = @Game_Name," + "Game_Studio_id = @Game_Studio_id," +
                                            "Game_Style_id = @Game_Style_id," + "Game_Year_Releas = @Game_Year_Releas," +
                                            "Game_Mod_id = @Game_Mod_id," + "Game_Count_Sell = @Game_Count_Sell " +
                                              "WHERE Id = @Id",
                                    new
                                    {
                                        Id = Curent_game.Id,
                                        Game_Name = Curent_game.Game_Name,
                                        Game_Studio_id = Curent_game.Studio.Id,
                                        Game_Style_id = Curent_game.Style.Id,
                                        Game_Year_Releas = Curent_game.Game_Year_Releas,
                                        Game_Mod_id = Curent_game.Mod_Game.Id,
                                        Game_Count_Sell = Curent_game.Game_Count_Sell
                                    }, transaction);
                                transaction.Commit();
                            }
                            catch (System.Exception ex)
                            {
                                transaction.Rollback();
                                throw ex;
                            }
                        }
                        Games = new ObservableCollection<Game>(db.Query<Game>("SELECT * FROM [Game]").ToList());
                        Games.ToList().ForEach(i => i.Studio = Studios.ToList().Find(j => j.Id == i.Game_Studio_id));
                        Games.ToList().ForEach(i => i.Style = Styles.ToList().Find(j => j.Id == i.Game_Style_id));
                        Games.ToList().ForEach(i => i.Mod_Game = Mod_s.ToList().Find(j => j.Id == i.Game_Mod_id));
                        OnPropertyChanged("Games");
                    }

                }
                else
                    MessageBox.Show("Что-то пошло нет так", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }*/
        #endregion

        #region Dell
        internal void Dell()
        {
            if (MessageBox.Show("Удалить игру?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                return;
            else
            {
                if ( Games.ToList().Exists(i => i.Id == Curent_game.Id))
                {
                    using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DB_Game"].ConnectionString))
                    {
                        db.Open();
                        using (var transaction = db.BeginTransaction())
                        {
                            try
                            {
                                db.Execute(" DELETE FROM [Game] WHERE Id = @Id",
                                    new { Id = Curent_game.Id }, transaction); 
                                transaction.Commit();
                            }
                            catch (System.Exception ex)
                            {
                                transaction.Rollback();
                                throw ex;
                            }
                        }
                    }
                }
                else
                    MessageBox.Show("Что-то пошло нет так", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DB_Game"].ConnectionString))
            {
                Games = new ObservableCollection<Game>(db.Query<Game>("SELECT * FROM [Game]").ToList());
                Games.ToList().ForEach(i => i.Studio = Studios.ToList().Find(j => j.Id == i.Game_Studio_id));
                Games.ToList().ForEach(i => i.Style = Styles.ToList().Find(j => j.Id == i.Game_Style_id));
                Games.ToList().ForEach(i => i.Mod_Game = Mod_s.ToList().Find(j => j.Id == i.Game_Mod_id));
                OnPropertyChanged("Games");
            }
        }
        #endregion

        #region ADD
        internal void ADD()
        {
            new Window_Add_New().ShowDialog();
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DB_Game"].ConnectionString))
            {
                Games = new ObservableCollection<Game>(db.Query<Game>("SELECT * FROM [Game]").ToList());
                Games.ToList().ForEach(i => i.Studio = Studios.ToList().Find(j => j.Id == i.Game_Studio_id));
                Games.ToList().ForEach(i => i.Style = Styles.ToList().Find(j => j.Id == i.Game_Style_id));
                Games.ToList().ForEach(i => i.Mod_Game = Mod_s.ToList().Find(j => j.Id == i.Game_Mod_id));
                OnPropertyChanged("Games");
            }
        }
        #endregion

        #region serch_comand
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
        IEnumerable<Game> Find(Func<Game, bool> filter) => Games.ToList().Where(filter).ToList();
        #endregion
    }
}
