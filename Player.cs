using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoFish_
{

    internal class CardRevealEventArgs : EventArgs
    {
        public int Rank { get; set; }
    }

    internal class AskedForEventArgs : EventArgs
    {
        public int Rank { get; set; }
    }
    internal class BookCompletedEventArgs : EventArgs
    {
        public int Rank { get; set; }
    }
    internal abstract class Player : IPlayer
    {
        private string name;
        private Hand playerHand;
        static int counter = 0;
        private int order;
        protected Random rand = new Random();
        bool turn = false;
        private Game currentGame;
        private List<Book> completedBooks = new List<Book>();
        public delegate void CardRevealedEventHandler(Player sender, CardRevealEventArgs e);
        public delegate void AskedForEventHandler(Player sender, AskedForEventArgs e);
        public delegate void BookCompletedEventHandler(Player sender, BookCompletedEventArgs e);


        public string Name
        { 
            get { return name; }
        }

        public bool Turn
        {
            get { return turn; }
        }
        public int Order
        {
            get { return order; }
        }

        public Hand PlayerHand
        {
            get { return playerHand; }
        }

        public Game Game
        {
            get { return currentGame; }
        }

        public List<Book> Books
        {
            get { return completedBooks; }
        }

        public Player(Game currentGame)
        {
            int num = rand.Next(0, 2);
            if (num == 0)
            {
                name = "Jane Doe";
            }
            else if (num == 1)
            {
                name = "John Doe";
            }
            playerHand = new Hand(this);
            order = counter;
            counter++;
        }

        public Player(Game currentGame, string newName)
        {
            this.name = newName;
            playerHand = new Hand(this);
            order = counter; //player order
            counter++;
            this.currentGame = currentGame;
        }

        //events
        public event CardRevealedEventHandler CardRevealed;

        public event AskedForEventHandler AskedFor;

        public event BookCompletedEventHandler BookCompleted;

        protected virtual void OnCardRevealed(CardRevealEventArgs e)
        {
            if (CardRevealed != null)
            {
                CardRevealed.Invoke(this, e);
            }
        }

        protected virtual void OnAskedFor(AskedForEventArgs e)
        {
            if (AskedFor != null)
            {
                AskedFor.Invoke(this, e);
            }
        }

        protected virtual void OnBookCompleted(BookCompletedEventArgs e)
        {
            if (BookCompleted != null)
            {
                BookCompleted.Invoke(this, e);
            }
        }

        public abstract void subscribe(); //this is so i can use subscribe on all players and not just computer ones

        public void revealCard(Card card)
        {
            CardRevealEventArgs args = new CardRevealEventArgs { Rank = card.Rank };
            OnCardRevealed(args);
        }

        public void askedForRank(int rank)
        {
            AskedForEventArgs args = new AskedForEventArgs  { Rank = rank };
            OnAskedFor(args);
        }



        public void flipTurn()
        {
            turn = !turn;
        }

        public abstract void makeDecision();

        protected virtual void goFishing()
        {
            Card newCard = PlayerHand.pullCard();
            PlayerHand.addCard(newCard);
            Console.WriteLine(this.Name + " has chosen to go fishing and pulled a : " + newCard);
            Console.WriteLine("\n" + this.PlayerHand);
            flipTurn();
        }
        protected abstract void askForCard();

        public bool search(int targetRank)
        {
            foreach (Card c in PlayerHand.Cards)
            {
                if (c.Rank == targetRank)
                {
                    return true;
                }
            }
            return false;
        }

        public virtual void checkForBooks()
        {
            int[] ranksIHave = new int[14];
            Book? book = new Book(0); //I made a book that would be overrwritten rather than turning it nullable
            ranksIHave[0] = 0; //this value will never be pulled, this is just so that the index matches the card rank
            foreach (Card card in PlayerHand.Cards) //determining if there's a rank with a high frequency
            {
                ranksIHave[card.Rank] += 1;
            }
            for (int i = 1; i >= ranksIHave.Length; i++)
            {
                if (ranksIHave[i] == 4)
                {
                    book = new Book(ranksIHave[i]);
                    completedBooks.Add(book);
                }
            }
            foreach (Card card in PlayerHand.Cards)
            {
                for (int i = 1; i >= ranksIHave.Length; i++)
                {
                    if (ranksIHave[i] == 4)
                    {
                        try
                        {
                            this.PlayerHand.pullCard(card);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                }
            }
            if (book.Rank != 0)
            {
                Console.WriteLine(this.Name + " has completed a book of " + book.Rank + "s!");
                OnBookCompleted(new BookCompletedEventArgs { Rank = book.Rank });
            }
        }
    }
}
