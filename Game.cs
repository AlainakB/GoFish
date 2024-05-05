using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GoFish_
{
    internal class Game
    {
        private List<Player> players = new List<Player>();
        private Deck gameDeck = new Deck();
        const int smallGroup = 4;
        const int minCardsGiven = 5;
        bool gameEnd = false;

        public int PlayerNum
        {
            get { return players.Count; }
        }

        public string PlayersString
        {
            get 
            {
                string s = "";
                foreach (Player p in players)
                {
                    s += p;
                }
                return s; 
            }
        }

        public int CardsGiven
        {
            get 
            { 
                if (players.Count == smallGroup)
                {
                    return minCardsGiven;
                }
                else
                {
                    return minCardsGiven + 2;
                }
            }
        }

        public Deck Deck { get { return gameDeck; } }


        public List<Player> Players
        {
            get
            {
                return players;
            }
        }
        public Game(int numOfPlayers, string listOfNames)
        {
            string[] names = listOfNames.Split(',');
            for(int  i = 0; i <= numOfPlayers; i++)
            {
                if (i == 0)
                {
                    players.Add(new HumanPlayer(this, names[i]));
                }
                else
                {
                    players.Add(new ComputerPlayer(this, names[i]));
                }
               
            }
            foreach(Player p in players)
            {
                p.subscribe();
                /*this is so the computer can subscribe to events from all players/potential publishers
                 * even though human players don't need it*/
                if (p is ComputerPlayer)
                {
                    ComputerPlayer computerPlayer = (ComputerPlayer)p;
                    computerPlayer.fillDictionaries();
                }
            }
            
        }

        public void getReady()
        {
            gameDeck.shuffleDeck();
            if (players.Count <= smallGroup)
            {
                foreach (Player player in players)
                {
                    for (int i = 0; i < minCardsGiven; i++)
                    {
                        player.PlayerHand.addCard(gameDeck.pullCard());
                    }
                }
            }
            else
            {
                foreach (Player player in players)
                {
                    for (int i = 0; i < minCardsGiven+2; i++)
                    {
                        player.PlayerHand.addCard(gameDeck.pullCard());
                    }
                }
            }
        }

        public void startGame()
        {
            int currentPlayerIndex = 0;
            players[currentPlayerIndex].flipTurn(); //starts 1st player's turn
            Console.WriteLine("It's " + players[currentPlayerIndex].Name + " turn.\n");
            players[currentPlayerIndex].makeDecision();
            while(!gameEnd)
            {
                if (players[currentPlayerIndex].Turn == false)
                {
                    //if the player has ended their turn, move onto the next player
                    currentPlayerIndex = nextTurn(currentPlayerIndex);
                    Console.WriteLine("It's " + players[currentPlayerIndex].Name + " turn.\n");
                    players[currentPlayerIndex].flipTurn();
                    players[currentPlayerIndex].makeDecision();
                }
                else
                {
                    //if the player has not ended their turn, let them go again
                    players[currentPlayerIndex].makeDecision();
                }
            }
        }

        private int nextTurn(int index)
        {
            return (index + 1) % players.Count; //prevents index from exceeding the amount of players in game and wraps back to 0
        }
    }
}
