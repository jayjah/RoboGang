using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using TeamYaffa.CRaPI;
using TeamYaffa.CRaPI.Utility;
using TeamYaffa.CRaPI.Commands;



namespace RoboGang.BasicComponents
{
    public class PlayerHandler
    {
        private Player p;
        private Thread playerThread;
        private Personality personality;
        private Point2D randpos;
        private int runcycles = 0;

        public Point2D Randpos
        {
            get { return randpos; }
            set { randpos = value; }
        }
        
        public int Runcycles
        {
            get { return runcycles; }
            set { runcycles = value; }
        }

        public Player Player
        {
            get { return p; }
            set { p = value; }
        }

        public Personality Personality
        {
            get { return personality; }
            set { personality = value; }
        }

        public PlayerHandler(Player p, Personality personality)
        {
            this.p = p;
            personality.PlayerHandler = this;
            this.personality = personality;
            
            
            this.playerThread = new Thread(new ThreadStart(p.PlayGame));
            p.AfterNewCycle += new TimeEventHandler(this.tick);
            

        }
        
        public void startThread(){
            this.playerThread.Start();
        }
      
        /*
         * ********************************************************************************************
         * Method to call every cycle, containing the logic of the connected player
         * ********************************************************************************************
         */

        private void tick(Object sender, TimeEventArgs arg)
        {
            Random rnd=new Random();
            Command commandToExecute = null;

            //Do stuff before kickoff
            if (this.Player.PlayMode == PlayMode.before_kick_off)
            {
                commandToExecute = personality.do_before_kickoff();
            }

            //Do stuff while kickoff left
            else if (this.Player.PlayMode == PlayMode.kick_off_l)
            {
                //if kickoff is same side as team of player
                if (this.Player.TeamSide == Side.Left)
                {
                    commandToExecute = personality.do_while_playon();
                }
                else
                {
                    commandToExecute = personality.do_while_kickoff_opponent();
                }

            }

            //Do stuff while kickoff right
            else if (this.Player.PlayMode == PlayMode.kick_off_r)
            {
                //if kickoff is same side as team of player
                if (this.Player.TeamSide == Side.Right)
                {
                    commandToExecute = personality.do_while_playon();
                }
                else
                {
                    commandToExecute = personality.do_while_kickoff_opponent();
                }

            }

            //TODO: Implement functionality for other situations!!
            //Do stuff for kickinleft
            else if (this.Player.PlayMode == PlayMode.kick_in_l)
            {
                if (this.Player.TeamSide == Side.Left)
                {
                    commandToExecute = personality.do_while_kick_in_own();
                }
                else
                {
                    commandToExecute = personality.do_while_kick_in_opponent();
                }
            }

            //Do stuff for kickin right
            else if (this.Player.PlayMode == PlayMode.kick_in_r)
            {
                if (this.Player.TeamSide == Side.Right)
                {
                    commandToExecute = personality.do_while_kick_in_own();
                }
                else
                {
                    commandToExecute = personality.do_while_kick_in_opponent();
                }
            }

            //Do stuff for freekick left
            else if (this.Player.PlayMode == PlayMode.free_kick_l)
            {
                if (this.Player.TeamSide == Side.Left)
                {
                    commandToExecute = personality.do_while_freekick_own();
                }
                else
                {
                    commandToExecute = personality.do_while_freekick_opponent();
                }
            }

            //Do stuff for freekick right
            else if (this.Player.PlayMode == PlayMode.free_kick_r)
            {
                if (this.Player.TeamSide == Side.Right)
                {
                    commandToExecute = personality.do_while_freekick_own();
                }
                else
                {
                    commandToExecute = personality.do_while_freekick_opponent();
                }
            }

            //Do stuff for goalkick left
            else if (this.Player.PlayMode == PlayMode.goal_kick_l)
            {
                if (this.Player.TeamSide == Side.Left)
                {
                    commandToExecute = personality.do_while_goalkick_own();
                }
                else
                {
                    commandToExecute = personality.do_while_goalkick_opponent();
                }
            }

            //Do stuff for goalkick right
            else if (this.Player.PlayMode == PlayMode.goal_kick_r)
            {
                if (this.Player.TeamSide == Side.Right)
                {
                    commandToExecute = personality.do_while_goalkick_own();
                }
                else
                {
                    commandToExecute = personality.do_while_goalkick_opponent();
                }
            }

            else if (this.Player.PlayMode == PlayMode.goal_l || this.Player.PlayMode == PlayMode.goal_l)
            {
                this.Player.CommandQueue.Clear();
              
                commandToExecute = BasicActions.DashToPoint(this.Player, new Point2D(-50, 0), 50);
            }
            //Do stuff for playon
            else if (this.Player.PlayMode == PlayMode.play_on)
            {
                commandToExecute = personality.do_while_playon();
            }

            //Execute the command
            if (commandToExecute != null)
            {
                p.CommandQueue.Enqueue(commandToExecute);
                p.Send();
            }
            runcycles = (runcycles + 1) % 60;
            if (runcycles == 0) {
                randpos=new Point2D(rnd.Next(-50,50), rnd.Next(-25,25));
            }
        }
    }
}