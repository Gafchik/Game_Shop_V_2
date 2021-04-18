using Game_Shop_V_2.Model.Base;
using Game_Shop_V_2.ViewModel;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using Dapper;
using System.Linq;

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


            var BD = (DataContext as ModelView);
            Game temp_Game = new Game();
            temp_Game.Game_Name = TextBlock_Game_Name.Text; // присваиваем новое имя выбраную в временную игру


            if (TextBlock_New_Game_Studio.Text != "")  // присваиваем новую студию или выбраную в временную игру
            {
                using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DB_Game"].ConnectionString))
                {
                    db.Open();
                    using (var transaction = db.BeginTransaction())
                    {
                        try
                        {
                            db.Execute("INSERT INTO [Studio]([Studio_Name]) VALUES (@Studio_Name)",
                               new { Studio_Name = TextBlock_New_Game_Studio.Text }, transaction);
                            transaction.Commit();

                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw ex;
                        }
                    }
                    temp_Game.Game_Studio_id = db.Query<Studio>("SELECT * FROM [Studio]").ToList().Find(i => i.Studio_Name == TextBlock_New_Game_Studio.Text).Id;
                }
            }
            else
                temp_Game.Game_Studio_id = BD.Studios.ToList().Find(i => i.Studio_Name == BD.Curent_studio.Studio_Name).Id;



            if (TextBlock_New_Game_Stile.Text != "")  // присваиваем новый стиль или выбраный в временную игру
            {
                using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DB_Game"].ConnectionString))
                {
                    db.Open();
                    using (var transaction = db.BeginTransaction())
                    {
                        try
                        {
                            db.Execute("INSERT INTO [Style]([Style_Game_Name]) VALUES (@Style_Game_Name)",
                               new { Style_Game_Name = TextBlock_New_Game_Stile.Text }, transaction);
                            transaction.Commit();

                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw ex;
                        }
                    }
                    temp_Game.Game_Style_id = db.Query<Model.Base.Style>("SELECT * FROM [Style]").ToList().Find(i => i.Style_Game_Name == TextBlock_New_Game_Stile.Text).Id;
                }
            }
            else
                temp_Game.Game_Style_id = BD.Styles.ToList().Find(i => i.Style_Game_Name == BD.Curent_style.Style_Game_Name).Id;



            try // присваиваем новую дату релиза в временную игру
            {
                temp_Game.Game_Year_Releas = Calendar.SelectedDate.Value;
            }
            catch (Exception)
            {
                temp_Game.Game_Year_Releas = DateTime.Now;
            }

            temp_Game.Game_Mod_id = BD.Mod_s.ToList().Find(i => i.Mod_Game_Name == BD.Curent_mod.Mod_Game_Name).Id;// присваиваем новый онлайн мод  в временную игру
           
            
            try { temp_Game.Game_Count_Sell = Convert.ToInt32(TextBlock_Game_Sells.Text); } // присваиваем количество продаж в временную игру
            catch (Exception)
            {
                temp_Game.Game_Count_Sell = 0;
                MessageBox.Show("Не правильно введенное число\r\n значение установлено по умолчанию 0", "ОЙ", MessageBoxButton.OK, MessageBoxImage.Error);

            }


            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DB_Game"].ConnectionString)) // заносим в бд новую игру
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {

                        db.Execute("INSERT INTO [Game]  ([Game_Name], [Game_Studio_id], [Game_Style_id], [Game_Year_Releas], [Game_Mod_id], [Game_Count_Sell])" +
                            " VALUES( @Game_Name, @Game_Studio_id, @Game_Style_id, @Game_Year_Releas, @Game_Mod_id, @Game_Count_Sell)",
                            new
                            {

                                Game_Name = temp_Game.Game_Name,
                                Game_Studio_id = temp_Game.Game_Studio_id,
                                Game_Style_id = temp_Game.Game_Style_id,
                                Game_Year_Releas = temp_Game.Game_Year_Releas,
                                Game_Mod_id = temp_Game.Game_Mod_id,
                                Game_Count_Sell = temp_Game.Game_Count_Sell
                            }, transaction);
                        transaction.Commit();
                    }
                    catch (System.Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
           
            Close();
        }
    }
}
