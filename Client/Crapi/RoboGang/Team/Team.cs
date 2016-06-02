using RoboGang.RoboGang.Helpers;
using RoboGang.Visualization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamYaffa.CRaPI;

namespace RoboGang.BasicComponents
{
   
   public class Team
    {

       /*
        * Team Properties
        */
        public int Players { get; set; }
        public String TeamName { get; set; }
        public Side TeamSide { get; set; }

        /* Field Visializer*/
        // FieldVisualizer fw = new FieldVisualizer();

        List<PlayerHandler> playerHandlerList = new List<PlayerHandler>();

       /*
        * Class Constuctor
        */
        public Team createTeam()
        { 

            Player p = new Player(this.TeamName, true);

            if (TeamSide != 0)
            {
                p.TeamSide = this.TeamSide;
            }

            //Add players to the handler list
            playerHandlerList.Add(new PlayerHandler(p, new LazyKeeper()));

            for (int i = 0; i < this.Players-1; i++)
            {
				
                p = new Player(this.TeamName, false);
                //Add players to the handler list
                playerHandlerList.Add(new PlayerHandler(p, new Renegade()));                
            }

            return this;
        }


       /*
        * Connect to Server and start Thread
        */
        public void connectAll()
        {
            this.playerHandlerList.ForEach(p =>
            {
                //fw.addPlayer(peh.Player.TeamSide, peh.Player);
                p.Player.StartUp(Constants.HostIP, Constants.Port, Constants.TimeOut);
                p.startThread();
            });

            //??
            //Dickbutt
            //fw.ShowDialog();
        }


       /*
        * Set Personality of each player of the team
        */
        public void setPersonality(int playerIndex, Personality personality)
        {
            PlayerHandler tmp;
            tmp = playerHandlerList.ElementAt(playerIndex);

           tmp.Personality = personality;
           personality.PlayerHandler = tmp;
            
        }
    }
}
