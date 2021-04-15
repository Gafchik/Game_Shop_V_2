using Game_Shop_V_2.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Shop_V_2.Model
{
   public class Game_Model_Final
    {
        public Game_Model_Final(Game value)
        {
            /*sourse = new DB_Game();
             Id = value.Id;
            Game_Name = sourse.Games.ToList().Find(i => i.Id == value.Id).Game_Name;
            Game_Studio = sourse.Studios.ToList().Find(j => j.Id == sourse.Games.ToList().Find(i => i.Id == value.Id).Studio));
            Game_Style = game_Style;
            Game_Year_Releas = game_Year_Releas;
            Game_Mod = game_Mod;
            Game_Count_Sell = game_Count_Sell;
            Mod_Game = mod_Game;
            Studio = studio;
            Style = style;*/
        }

        public DB_Game sourse { get; private set; }
        public int Id { get; set; }     
        public string Game_Name { get; set; }
        public string Game_Studio { get; set; }
        public string Game_Style { get; set; }       
        public DateTime Game_Year_Releas { get; set; }
        public string Game_Mod { get; set; }
        public string Game_Count_Sell { get; set; }
        public string Mod_Game  { get; set; }
        public string Studio  { get; set; }
        public string Style  { get; set; }
    }
}
