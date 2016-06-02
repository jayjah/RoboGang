using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using TeamYaffa;
using TeamYaffa.CRaPI;

using RoboGang.BasicComponents;
using RoboGang.Visualization;


namespace RoboCupProject
{
    class Program
    {
        static void Main(string[] args)
        {

           /*
            * Create 2 Team
            * goalie  = "lazykeeper"
            * players = "renegade"
            */
            Team testTeam = new Team { Players = 11, TeamName = "Testteam", TeamSide = 0 }.createTeam();
            Team gegnerTeam = new Team { Players = 11, TeamName = "Gegnerteam", TeamSide = Side.Right }.createTeam();

            testTeam.setPersonality(1, new Renegade());
            testTeam.connectAll();
            gegnerTeam.connectAll();
            
            Console.WriteLine("Maybe connected!");

            for (; ; ) ;
           
        }
    }
}
