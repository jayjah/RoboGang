using System;
using System.Collections.Generic;
using TeamYaffa.CRaPI;
using RoboGang.BasicComponents;
using TeamYaffa.CRaPI.Commands;
using TeamYaffa.CRaPI.World.GameObjects;

namespace RoboGang.BasicComponents
{

    public abstract class Personality{

        public PlayerHandler PlayerHandler { get; set; }

		protected FieldPlayer findTeammatefarbehind(Player p){
			double distance = -1;
			FieldPlayer tempPlayer = null;

			foreach (FieldPlayer temp in p.World.TeamMates) {
				//double x = p.World.MyPosition.SignedXDistanceTo (temp.Position.X);
				//double y = p.World.MyPosition.SignedYDistanceTo (temp.Position.Y);

				if (distance < temp.Distance) {
					distance = temp.Distance;
					tempPlayer = temp;
				}
			}
			return tempPlayer;
		}

        //Find nearest player
        protected FieldPlayer findNearestTeammate()
        {
            double distance = -1;
            FieldPlayer tempPlayer = null;
            Player p = this.PlayerHandler.Player;

            foreach (FieldPlayer t in p.World.TeamMates)
            {
                if (distance == -1 || t.Distance < distance)
                {
                    tempPlayer = t;
                    distance = t.Distance;
                }
            }
            return tempPlayer;
        }

       /*
       * ********************************************************************************************
       * Fill with actions for each situation and return command to execute in the following code
       * ********************************************************************************************
       */

        //Before a kickoff
        abstract public Command do_before_kickoff();

        //While a kickoff when own team has to kick off.
        abstract public Command do_while_kickoff_own();

        //While a kickoff when opponent team has to kick off.
        abstract public Command do_while_kickoff_opponent();

        //While play on
        abstract  public Command do_while_playon();

        //While corner for own team
        abstract public Command do_while_corner_own();

        //While corner for opponent team
        abstract public Command do_while_corner_opponent();

        //While freekick for own team
        abstract public Command do_while_freekick_own();

        //While freekick for opponent team
        abstract public Command do_while_freekick_opponent();

        //While goalkick for own team
        abstract public Command do_while_goalkick_own();

        //While goalkick for opponent team
        abstract public Command do_while_goalkick_opponent();

        //After goal for own team
        abstract public Command do_after_goal_own();

        //After goal for opponent team
        abstract public Command do_after_goal_opponent();

        //While kick in for own team
        abstract public Command do_while_kick_in_own();

        //While kick in for opponent team
        abstract public Command do_while_kick_in_opponent();

        //While unknown playmode
        abstract public Command do_while_unknown_playmode();

        //If own team is offside
        abstract public Command do_if_offside_own();

        //If opponent team is offside
        abstract public Command do_if_offside_opponent();

        //If the game is finished
        abstract public Command do_after_time_out();


    }

}
