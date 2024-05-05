using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GoFish_
{
    internal class ComputerPlayer : Player
    {
        //this serves as the AI's memory for which player has which rank (based on what has been revealed)
        private Dictionary<Player, HashSet<int>> knownRanks = new Dictionary<Player, HashSet<int>>(); //this is a rank that they know someone has
        private Dictionary<Player, HashSet<int>> requestedRanks = new Dictionary<Player, HashSet<int>>(); //this is ranks that they know someone has asked for

        public ComputerPlayer(Game newGame, string name) : base(newGame, name) 
        {

        }

        private void askedForAlert(Player sender, AskedForEventArgs e)
        {
            if (requestedRanks.ContainsKey(sender))
            {
                int rank = e.Rank;
                HashSet<int> intSet = requestedRanks[sender];
                bool sucess = requestedRanks.TryGetValue(sender, out intSet);
                intSet.Add(rank); //this is for remembering what player has asked for which ranks
                Console.WriteLine("e");
            }
        }

        private void updateKnownCards(Player sender, CardRevealEventArgs e)
        {
           if (knownRanks.ContainsKey(sender))
           {
                int revealedCardRank = e.Rank; //gets the Rank property out of eventargs
                HashSet<int> cards = knownRanks[sender];
                cards.Add(revealedCardRank); //this is for remembering what rank has been revealed
                Console.WriteLine("e");
           }
        }

        private void cleanMemory(Player sender, BookCompletedEventArgs e)
        {
            if (knownRanks.ContainsKey(sender))
            {
                int bookRank = e.Rank;
                HashSet<int> rankSet = knownRanks[sender];
                HashSet<int> ranksToRemove = new HashSet<int>(rankSet);
                
                /*the 2nd hashset is a copy so that elements can be removed during the loop
                 * They are still seperate hashsets, but they have the same elements*/

                foreach (KeyValuePair<Player, HashSet<int>> kvp in knownRanks)
                {
                    foreach(int rank in ranksToRemove)
                    {
                        if (rank == this.Books.Last().Rank)
                        {
                            rankSet.Remove(rank); //removes any ranks no longer in circulation
                        }
                    }
                    
                }
                //overwrite sets for card requests
                rankSet = requestedRanks[sender];
                ranksToRemove = new HashSet<int>(requestedRanks[sender]);
                foreach (KeyValuePair<Player, HashSet<int>> kvp in requestedRanks)
                {
                    foreach (int rank in ranksToRemove)
                    {
                        if (rank == this.Books.Last().Rank)
                        {
                            rankSet.Remove(rank); //removes any ranks no longer in circulation
                        }
                    }

                }
            }
        }

        public override void subscribe()
        { 
            foreach (Player player in this.Game.Players)
            {
                if (player != this)
                {
                    player.CardRevealed += updateKnownCards;
                    player.AskedFor += askedForAlert;
                    player.BookCompleted += cleanMemory;
                }
            }
        }
        public void fillDictionaries()
        {
            foreach (Player player in this.Game.Players)
            {
                if (player != this)
                {
                    knownRanks[player] = new HashSet<int>();
                    requestedRanks[player] = new HashSet<int>();
                }
            }
        }

        public override void makeDecision()
        {
            Console.WriteLine(PlayerHand + "\n");
            Console.WriteLine("\n"+this.Name + " is making a decision...");
            //Thread.Sleep(rand.Next(1, 6) * 1000);

            int[] ranksIHave = new int[14];
            ranksIHave[0] = 0; //this value will never be pulled
            foreach (Card card in PlayerHand.Cards) //determining if there's a rank with high frequency
            {
                ranksIHave[card.Rank] += 1;
            }
            int max = ranksIHave.Max();
            int chosenRank = 0;
            if (max != 1)
            {
                List<int> maxIndex = new List<int>();
                for (int i = 0; i < 13; i++)
                {
                    //this is to find need the index(es) of max
                    if (ranksIHave.ElementAt(i) == max)
                    {
                        maxIndex.Add(i);
                    }
                }
                if (maxIndex.Count > 1)
                {
                    chosenRank = maxIndex[rand.Next(0,maxIndex.Count)];
                }
                else if (maxIndex.Count == 1)
                {
                    chosenRank = maxIndex[0];
                }
                //int decision = rand.Next(0, 10); //90% chance of asking for card
                int decision = 1;
                if (decision > 0)
                {
                    askForCard(chosenRank);
                }
                else
                {
                    goFishing();
                }
            }
            else
            {
                //int decision = rand.Next(0, 10); //70% chance of fishing
                int decision = 2;
                if (decision <= 2)
                {
                    askForCard();
                }
                else
                {
                    goFishing();
                }
            }
            checkForBooks();
            Console.WriteLine("\nPress any key to continue");
           /* Console.ReadKey();
            Console.WriteLine();*/
        }

        protected override void askForCard()
        {
            /*this is for times when there is no rank has a frequency higher than 1
            * a random card will be asked for instead*/
            int chosenRank = PlayerHand.Cards[rand.Next(0, PlayerHand.Cards.Count)].Rank;
            Player chosenPlayer = this.Game.Players[rand.Next(0, this.Game.Players.Count)];
            while (chosenPlayer == this)
            {
                chosenPlayer = this.Game.Players[rand.Next(0, this.Game.Players.Count)];
            }
            Console.WriteLine(this.Name + " asks " + chosenPlayer.Name + " if they have any " + Card.convertToString(chosenRank).ToLower() + "s.");
            if (chosenPlayer.search(chosenRank))
            {
                List<Card> collection = new List<Card>();
                foreach (Card c in chosenPlayer.PlayerHand.Cards)
                {
                    if (chosenRank == c.Rank) //if they find a card with a matching rank, they will store them into collection
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
                    //everycard in collection will be removed from the target's hand placed into the their own
                    this.PlayerHand.addCard(chosenPlayer.PlayerHand.pullCard(collection[i]));
                }
                Console.WriteLine(chosenPlayer.Name + " hands over all of their " + Card.convertToString(chosenRank) + "s.");
                OnAskedFor(new AskedForEventArgs { Rank =  chosenRank });
            }
            else
            {
                Console.WriteLine("Unfortunately, " + chosenPlayer.Name + " does not have any " + Card.convertToString(chosenRank).ToLower() + "s.");
                Console.WriteLine(chosenPlayer.Name + " tells them to Go Fish!");
                Card pulled = this.Game.Deck.pullCard();
                this.PlayerHand.addCard(pulled);
                if (pulled.Rank == chosenRank)
                {
                    Console.WriteLine(this.Name + "has pulled their desired rank and showed everyone their card: " + pulled);
                    //alert everyone what they've pulled
                    OnCardRevealed(new CardRevealEventArgs { Rank = pulled.Rank });
                }
                else
                {
                    Console.WriteLine(this.Name + " has pulled a card that wasn't their desired rank, so it will not be revealed: " + pulled);
                    OnAskedFor(new AskedForEventArgs { Rank = chosenRank });
                }
            }
        }

        protected void askForCard(int chosenRank)
        {
            Player? chosenPlayer = null;
            //this is for when the CPU has a specific card they're trying to collect
            foreach (KeyValuePair<Player, HashSet<int>> kvp in knownRanks)
            {
                Player player = kvp.Key;
                HashSet<int> rankSet = kvp.Value;
                if (rankSet.Count != 0)
                {
                    foreach(int rank in rankSet)
                    {
                        if(rank == chosenRank)
                        {
                            chosenPlayer = player; //checks awareness for any players that has a matching card and picks that player
                        }
                    }
                }
            }
            if (chosenPlayer == null) 
            {
                /*if the CPU doesn't know anyone who has a card they need
                 * then it will look for anyone who has asked for the same rank they need*/
                foreach (KeyValuePair<Player, HashSet<int>> kvp in requestedRanks)
                {
                    Player player = kvp.Key;
                    HashSet<int> rankSet = kvp.Value;
                    if (rankSet.Count != 0)
                    {
                        foreach(int rank in rankSet)
                    {
                            if (rank == chosenRank)
                            {
                                chosenPlayer = player; //checks awareness for any players that has a matching card and picks that player
                            }
                        }
                    }
                }
            }
            if (chosenPlayer == null)
            {
                //in case that the CPU doesn't know enough about the other player's hands, it will have to randomly choose
                chosenPlayer = this.Game.Players[rand.Next(0, this.Game.Players.Count)];
            }
           
            while (chosenPlayer == this)
            {
                chosenPlayer = this.Game.Players[rand.Next(0, this.Game.Players.Count)];
            }
            Console.WriteLine(this.Name + " asks " + chosenPlayer.Name + " if they have any " + Card.convertToString(chosenRank).ToLower() + "s.");
            if (chosenPlayer.search(chosenRank))
            {
                List<Card> collection = new List<Card>();
                foreach (Card c in chosenPlayer.PlayerHand.Cards)
                {
                    if (chosenRank == c.Rank) //if they find a rank with a matching rank, they will store them into collection
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
                    //every rank in collection will be removed from the target's hand placed into the their own
                    this.PlayerHand.addCard(chosenPlayer.PlayerHand.pullCard(collection[i]));
                }
                if (Card.convertToString(chosenRank) != "Six")
                {
                    Console.WriteLine(chosenPlayer.Name + " hands over all of their " + Card.convertToString(chosenRank).ToLower() + "s.");
                }
                else
                {
                    Console.WriteLine(chosenPlayer.Name + " hands over all of their " + Card.convertToString(chosenRank).ToLower() + "es.");
                }

                OnAskedFor(new AskedForEventArgs { Rank = chosenRank });
            }
            else
            {
                if (Card.convertToString(chosenRank) != "Six")
                {
                    Console.WriteLine("Unfortunately, " + chosenPlayer.Name + " does not have any " + Card.convertToString(chosenRank).ToLower() + "s.");
                }
                else
                {
                    Console.WriteLine("Unfortunately, " + chosenPlayer.Name + " does not have any " + Card.convertToString(chosenRank).ToLower() + "es.");
                }
                Console.WriteLine(chosenPlayer.Name + " tells them to Go Fish!");
                Card pulled = this.Game.Deck.pullCard();
                this.PlayerHand.addCard(pulled);
                if (pulled.Rank == chosenRank)
                {
                    Console.WriteLine(this.Name + "has pulled their desired rank and showed everyone their card: " + pulled);
                    //alert everyone what they've pulled
                    OnCardRevealed(new CardRevealEventArgs { Rank = pulled.Rank});
                }
                else
                {
                    Console.WriteLine(this.Name + " has pulled a card that wasn't their desired rank, so it will not be revealed: " + pulled);
                    OnAskedFor(new AskedForEventArgs { Rank = chosenRank });
                    flipTurn();
                }
            }
        }
    }
}
