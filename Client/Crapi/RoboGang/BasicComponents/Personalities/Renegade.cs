using System;
using System.Collections.Generic;
using TeamYaffa.CRaPI;
using TeamYaffa.CRaPI.Commands;
using TeamYaffa.CRaPI.Utility;
using TeamYaffa.CRaPI.World.GameObjects;

namespace RoboGang.BasicComponents
{
    class Renegade : Personality
    {
        private bool hasStartPosition = false;
        private bool turned = false;
        private bool caught = false;


        /*
        * ********************************************************************************************
        * Fill with actions for each situation and return command to execute in the following code
        * ********************************************************************************************
        */

        //Before a kickoff
        public override Command do_before_kickoff()
        {
            Player p = this.PlayerHandler.Player;

            if (!p.IsGoalie)
            {
                if (!hasStartPosition)
                {
                    hasStartPosition = true;
                    return BasicCommands.Move(p.World.TheBall.Position);

                }
                else
                    return null;
            }
            else if (p.IsGoalie)
                return BasicCommands.Move(-50, 0);
            else
                return null;
        }

        //While a kickoff when own team has to kick off.
        public override Command do_while_kickoff_own()
        {
            Player p = this.PlayerHandler.Player;

            if (p.BallIsKickable)
                //Random value for testing.
                return BasicCommands.Kick(40, 4);
            else
                if (p.UniformNumber == 2 || p.UniformNumber == 3)
                    return BasicCommands.Move(p.World.TheBall.Position);
                else
                    return BasicCommands.Move(p.World.TheBall.Position);
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
            Random rnd = new Random();
            if (p.World.TheBall.SeenThisCycle)
                if (p.BallIsKickable)
                {
                    FieldPlayer nearestTeammate = findNearestTeammate();
                    if (nearestTeammate != null)
                    {
                        //Crapi is using double: I assume that is in radians? The server expects degrees, so i just converted that.
                        if (this.turned)
                        {
                            if (rnd.Next(40) % 40 != 0 && nearestTeammate.Goalie == false)

                                return BasicActions.KickToObject(p, nearestTeammate, (int)nearestTeammate.Distance * 2);
                            else
                                return BasicActions.KickToPoint(p, new Point2D(52, rnd.Next(-8, +8)), rnd.Next(180));
                        }
                        else
                        {
                            turned = true;
                            return BasicActions.TurnToObject(p, nearestTeammate);
                        }
                    }
                    else
                        return BasicActions.KickToPoint(p, new Point2D(52, rnd.Next(-8, +8)), rnd.Next(180));

                }
                else
                {
                    turned = false;
                    if (!p.IsGoalie)
                        if (p.World.TheBall.Distance < 20)
                            if (findNearestTeammate() != null)
                            {
                                if (findNearestTeammate().Distance > 20 || (findNearestTeammate().Position.XDistanceTo(p.World.TheBall.Position) > 15 && findNearestTeammate().Position.YDistanceTo(p.World.TheBall.Position) > 15) || p.World.TheBall.Distance < 5)
                                    return BasicActions.DashToPoint(p, p.World.TheBall.Position, 30 * (p.UniformNumber * 5));
                                else
                                    return BasicActions.DashToPoint(p, this.PlayerHandler.Randpos, 50);
                            }
                            else
                                return BasicActions.DashToPoint(p, p.World.TheBall.Position, 30 * (p.UniformNumber * 5));
                        else
                        {
                            return BasicActions.DashToPoint(p, this.PlayerHandler.Randpos, 50);
                        }

                    else
                    {
                        if (p.BallIsCatchable)
                            return BasicActions.CatchBall(p);
                        else
                        {

                            if (Math.Abs(p.World.TheBall.Position.Y) < 15 && p.World.TheBall.Position.X < -35)
                                return BasicActions.DashToPoint(p, new Point2D(-50, p.World.TheBall.Position.Y), 300);
                            else
                                if (p.World.TheBall.SeenThisCycle)
                                    return BasicActions.TurnToPoint(p, p.World.TheBall.Position);
                                else
                                    return BasicCommands.Turn(30);
                        }
                    }

                }

            else
                if (!p.IsGoalie)
                {
                    if (this.PlayerHandler.Runcycles % 12 == 0)
                        return BasicActions.DashToPoint(p, this.PlayerHandler.Randpos, 50);
                    return BasicCommands.Turn(30);
                }
                else
                {
                    return BasicActions.TurnToPoint(p, p.World.TheBall.Position);
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
            if (p.World.TheBall.SeenThisCycle)
            {
                if (p.World.TheBall.Distance < 15)
                {
                    if (p.BallIsKickable)
                    {
                        if (otherplayer != null)
                        {
                            return BasicActions.KickToObject(p, otherplayer, 100);
                        }
                        else
                        {
                            return BasicActions.KickToPoint(p, new Point2D(0, 0), p.ServerParam.MaxPower);
                        }
                    }
                    return BasicActions.DashToPoint(p, p.World.TheBall.Position, p.ServerParam.MaxPower);
                }
                return BasicActions.DashToPoint(p, p.World.TheBall.Position, p.ServerParam.MaxPower);
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
            FieldPlayer otherplayer = findNearestTeammate();
            if (p.World.TheBall.Distance < 15)
            {
                if (p.BallIsKickable)
                {
                    if (otherplayer != null)
                    {
                        return BasicActions.KickToObject(p, otherplayer, 100);
                    }
                    else
                    {
                        BasicActions.KickToPoint(p, new Point2D(50, 0), p.ServerParam.MaxPower);
                    }
                }
                return BasicActions.DashToPoint(p, p.World.TheBall.Position, p.ServerParam.MaxPower);
            }
            return null;
        }

        public override Command do_while_goalkick_opponent()
        {
            return null;
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
            Player p = this.PlayerHandler.Player;
            if (p.World.TheBall.SeenThisCycle)
            {
                if (p.World.TheBall.Distance < 15)
                {
                    if (p.BallIsKickable)
                    {
                        if (findNearestTeammate() != null)
                            return BasicActions.KickToObject(p, findNearestTeammate(), 80);
                        else
                            return BasicActions.KickToPoint(p, new Point2D(50, 0), 100);
                    }
                    else
                        return BasicActions.DashToPoint(p, p.World.TheBall.Position, p.ServerParam.MaxPower);
                }
                return BasicActions.DashToPoint(p, p.World.TheBall.Position, p.ServerParam.MaxPower);
            }
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
