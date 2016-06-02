using System;
using System.Collections.Generic;
using TeamYaffa.CRaPI;
using TeamYaffa.CRaPI.Commands;
using TeamYaffa.CRaPI.Utility;
using TeamYaffa.CRaPI.World.GameObjects;

namespace RoboGang.BasicComponents
{
    class ImprovedKeeper : Personality
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
            //Do the same as before kickoff to finally move him into the goal;
            return do_before_kickoff();
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
                FieldPlayer tm = findNearestTeammate();
                if (tm != null)
                    return BasicActions.KickToPoint(p, tm.Position, p.ServerParam.MaxPower);
                else return BasicActions.KickToPoint(p, new Point2D(0, 0), p.ServerParam.MaxPower);
            }
            else if (p.BallIsCatchable)
                return BasicActions.CatchBall(p);


            else
            {
                if (p.World.TheBall.SeenThisCycle)
                {

                    if (Math.Abs(p.World.TheBall.Position.Y) < 15 && p.World.TheBall.Position.X < -36)
                    {
                        if (Math.Abs(p.World.TheBall.Position.Y - p.World.MyPosition.Y) > 2)
                            return BasicActions.DashToPoint(p, new Point2D(-51, p.World.TheBall.Position.Y), p.ServerParam.MaxPower);
                        else
                            return BasicActions.TurnToObject(p, p.World.TheBall);
                    }
                    else

                        //return BasicActions.TurnToPoint(p, p.World.TheBall.Position);
                        return BasicActions.TurnToPoint(p, p.World.TheBall.Position);
                }

                else
                {
                    p.CommandQueue.Enqueue(BasicActions.DashToPoint(p, new Point2D(-50, 0), 40));
                    return BasicActions.TurnToPoint(p, new Point2D(50, 0));
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
            throw new NotImplementedException();
        }

        public override Command do_while_freekick_opponent()
        {
            throw new NotImplementedException();
        }

        public override Command do_while_goalkick_own()
        {
            throw new NotImplementedException();
        }

        public override Command do_while_goalkick_opponent()
        {
            throw new NotImplementedException();
        }

        public override Command do_after_goal_own()
        {
            throw new NotImplementedException();
        }

        public override Command do_after_goal_opponent()
        {
            throw new NotImplementedException();
        }

        public override Command do_while_kick_in_own()
        {
            throw new NotImplementedException();
        }

        public override Command do_while_kick_in_opponent()
        {
            throw new NotImplementedException();
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
