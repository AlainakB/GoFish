using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GoFish_
{
    internal class HumanPlayer : Player
    {
        public HumanPlayer(Game newGame) : base(newGame)
        {

        }
        public HumanPlayer(Game newGame, string name) : base(newGame, name) 
        {

        }

        public int validateAnswer(bool success, int answer, string invalidAnswerMessage, int[] bound)
        {
            success = int.TryParse(Console.ReadLine(), out answer);
            if (answer < bound[0] || answer > bound[1]) //prevents the answer from going out of bounds
            {
                success = false;
            }
            while (!success)
            {
                Console.WriteLine(invalidAnswerMessage);
                success = int.TryParse(Console.ReadLine(), out answer);
                if (answer < bound[0] || answer > bound[1])
                {
                    success = false;
                }
            }
            return answer;
        }

        public override void makeDecision()
        {
            Console.WriteLine("\n" + this.PlayerHand+"\n");
            /*Console.WriteLine("What would you like to do?\n1. Ask for a card\n2. Go fishing\n3. Pass your turn");
            bool success = false;
            int answer = 0;
            int[] bound = new int[] {1, 3};
            answer = validateAnswer(success, answer, "Sorry, that's not one of the given options", bound);*/
            int answer = 3;
            if (answer == 1)
            {
                askForCard();
                Console.WriteLine("\n" + this.PlayerHand);
                checkForBooks();
            }
            else if (answer == 2)
            {
                goFishing();
                Console.WriteLine("\n" + this.PlayerHand);
                checkForBooks();
            }
            else
            {
                flipTurn();
            }
        }

        protected override void askForCard()
        {
            bool success = false;
            int answer = 0;
            Console.WriteLine("Which player would you like to pull from?");
            for (int i = 1; i < this.Game.PlayerNum; i++)
            {
                Console.WriteLine(i + ". " + this.Game.Players[i].Name);
            }
            int[] bounds = new int[] { 1, this.Game.PlayerNum - 1 };
            answer = validateAnswer(success, answer, "Sorry, that player doesn't exist, please try again", bounds);
            Player chosenPlayer = this.Game.Players[answer];
            Console.WriteLine("You've chosen " + chosenPlayer.Name + ".\nWhich rank do you want to ask for? (numbers only)");
            //remove print line after debugging
            Console.WriteLine(chosenPlayer.PlayerHand);
            bounds[0] = 1;
            bounds[1] = 13;
            int chosenRank = validateAnswer(success, answer, "Sorry, this rank doesn't exist.", bounds);
            Console.WriteLine("You ask " + chosenPlayer.Name + " if they have any " + Card.convertToString(chosenRank).ToLower() + "s.");
            bool found = chosenPlayer.search(chosenRank);
            if (found)
            {
                Console.WriteLine(chosenPlayer.Name + " hands you all of their " + Card.convertToString(chosenRank).ToLower() + "s.");
                askedForRank(chosenRank);
                List<Card> collection = new List<Card>();
                foreach (Card c in chosenPlayer.PlayerHand.Cards)
                {
                    if (chosenRank == c.Rank)
                    {
                        try
                        {
                            Card cardBeingPulled = chosenPlayer.PlayerHand.find(c);
                            collection.Add(cardBeingPulled);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                }
                for (int i = 0; i < collection.Count; i++)
                {
                    this.PlayerHand.addCard(chosenPlayer.PlayerHand.pullCard(collection[i]));
                }
            }
            else
            {
                Console.WriteLine("Unfortunately, " + chosenPlayer.Name + " does not have any " + Card.convertToString(chosenRank).ToLower() + "s.");
                Console.WriteLine(chosenPlayer.Name + " tells you to Go Fish!");
                Card pulled = this.Game.Deck.pullCard();
                this.PlayerHand.addCard(pulled);
                if (pulled.Rank == chosenRank)
                {
                    Console.WriteLine("You've pulled your desired rank and show everyone your card: " + pulled);
                    //the CPUs are aware of all revealed cards
                    revealCard(pulled);
                }
                else
                {
                    Console.WriteLine("You have pulled a card that wasn't your desired rank, so it will not be revealed: " + pulled);
                    flipTurn();
                }
            }
        }

        public override void subscribe() { } //the method is only for the CPUs since they have no "memory"

        protected override void goFishing()
        {
            Card newCard = PlayerHand.pullCard();
            PlayerHand.addCard(newCard);
            Console.WriteLine("You've chosen to go fishing and pulled out a: " + newCard + ".");
            flipTurn();
        }
    }
}
