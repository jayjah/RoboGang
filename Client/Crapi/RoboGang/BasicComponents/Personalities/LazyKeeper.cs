using System;
using System.Collections.Generic;
using TeamYaffa.CRaPI;
using TeamYaffa.CRaPI.Commands;
using TeamYaffa.CRaPI.Utility;
using TeamYaffa.CRaPI.World.GameObjects;

namespace RoboGang.BasicComponents
{
    class LazyKeeper : Personality
    {
      
        

        /*
       * ********************************************************************************************
       * Fill with actions for each situation and return command to execute in the following code
       * ********************************************************************************************
       */

        //Before a kickoff
        public override Command do_before_kickoff()
        {
                //Move the goalie into the goal
                return BasicCommands.Move(-50, 0);
        }

        //While a kickoff when own team has to kick off.
        public override Command do_while_kickoff_own()
        {
			Player p = this.PlayerHandler.Player;
			//FieldPlayer temp = findNearestTeammate ();
			if (p.BallIsKickable) {
				return BasicCommands.Kick(100, 30);
			}
			return null;
				
            //Do the same as before kickoff to finally move him into the goal;
            //return do_before_kickoff();
        }

        //While a kickoff when opponent team has to kick off.
        public override Command do_while_kickoff_opponent()
        {
            return null;
        }

        //While play on
        public override Command do_while_playon()
        {
            Player p = this.PlayerHandler.Player;

            if (p.BallIsKickable)
            {
                FieldPlayer fp = findNearestTeammate();
                if (fp != null)
                    return BasicActions.KickToPoint(p, fp.Position, p.ServerParam.MaxPower);
                else
                    return BasicActions.KickToPoint(p, new Point2D(0, 0), p.ServerParam.MaxPower);
            }
            else if (p.BallIsCatchable)
                return BasicActions.CatchBall(p);
            else
            {
                if (p.World.TheBall.SeenThisCycle && p.World.TheBall.Position.X > -52 && p.World.TheBall.Position.X < 52)
                {

                    if (Math.Abs(p.World.TheBall.Position.Y) < 20 && p.World.TheBall.Position.X < -35)
                    {
                        if (p.World.TheBall.ForecastPosition(5, p.World.Positioner).X < -35 && p.World.TheBall.ForecastPosition(2, p.World.Positioner).X > -48)
                        {

                            return BasicActions.DashToPoint(p, new Point2D(p.World.TheBall.ForecastPosition(2, p.World.Positioner)), p.ServerParam.MaxPower);
                        }
                        else
                            return BasicActions.DashToPoint(p, new Point2D(-49, p.World.TheBall.ForecastPosition(8, p.World.Positioner).Y), p.ServerParam.MaxPower);
                    }
                    else

                        //return BasicActions.TurnToPoint(p, p.World.TheBall.Position);
                        return BasicActions.TurnToPoint(p, p.World.TheBall.Position);
                }
                else
                {
                    return BasicActions.DashToPoint(p, new Point2D(-49, 0), 40);

                }
            }
        }


        public override Command do_while_corner_own()
        {
            throw new NotImplementedException();
        }

        public override Command do_while_corner_opponent()
        {
            throw new NotImplementedException();
        }

        public override Command do_while_freekick_own()
        {
			Player p = this.PlayerHandler.Player;
			FieldPlayer otherplayer = findNearestTeammate();
			if (p.World.TheBall.Distance < 15) {
				if (p.BallIsKickable) {
					if (otherplayer != null) {
						//p.Turn();
						//return BasicCommands.Kick(100,50);
						return BasicActions.KickToObject (p, otherplayer, p.ServerParam.MaxPower);

					} else {
						return BasicActions.KickToPoint (p, new Point2D (50,0), p.ServerParam.MaxPower);
					}
				}
				return BasicActions.DashToPoint (p, p.World.TheBall.Position, p.ServerParam.MaxPower);
			}
			return null;
        }

        public override Command do_while_freekick_opponent()
        {
			return null;
        }

        public override Command do_while_goalkick_own()
        {
			Player p = this.PlayerHandler.Player;
			FieldPlayer otherplayer = findNearestTeammate ();
			if (p.World.TheBall.Distance < 15) {
				if (p.BallIsKickable) {
					if (otherplayer != null) {
						return BasicActions.KickToObject (p, otherplayer, 100);
					} else {
						return BasicActions.KickToPoint (p, new Point2D (0,0), p.ServerParam.MaxPower);
							
					}
				}
				return BasicActions.DashToPoint (p, p.World.TheBall.Position, p.ServerParam.MaxPower);
			}
			return null;
        }

        public override Command do_while_goalkick_opponent()
        {
			return null;
        }

        public override Command do_after_goal_own()
        {
            this.PlayerHandler.Player.CommandQueue.Clear();
            return null;
        }

        public override Command do_after_goal_opponent()
        {
            this.PlayerHandler.Player.CommandQueue.Clear();
            return null;
        }

        public override Command do_while_kick_in_own()
        {
			return null;
        }

        public override Command do_while_kick_in_opponent()
        {
			return null;
        }

        public override Command do_while_unknown_playmode()
        {
            throw new NotImplementedException();
        }

        public override Command do_if_offside_own()
        {
            throw new NotImplementedException();
        }

        public override Command do_if_offside_opponent()
        {
            throw new NotImplementedException();
        }

        public override Command do_after_time_out()
        {
            throw new NotImplementedException();
        }
    }
}
