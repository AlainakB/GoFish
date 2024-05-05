using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace GoFish_
{
    internal class Deck : IDeck
    {
        private List<Card> cards = new List<Card>();
        private string[] suit_names = new string[] { "Diamond", "Club", "Hearts", "Spades" };
        private int?[] rank_names = { null, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13};
        Random r = new Random();

        public Deck() 
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 13; j++)
                {
                    Card card = new Card((int)rank_names[j+1], suit_names[i]);
                    cards.Add(card);
                }
            }
        }
        public Card pullCard()
        {

            if (cards.Count != 0)
            {
                int index = r.Next(cards.Count);
                Card c = cards[index];
                cards.RemoveAt(index);
                return c;
            }
            else
            {
                throw new Exception("Attempting to pull from empty deck");
            }
        }

        public void addCard(Card card)
        {

            cards.Add(card);

        }

        public void shuffleDeck()
        {
            Random r = new Random();
            for(int i = cards.Count-1; i > 0 ; i--) 
            {
                int index = r.Next(i + 1);  
                if (i != index)
                {
                    Card temp = cards[i];
                    cards[i] = cards[index];
                    cards[index] = temp;
                }
            }
        }

        public override string ToString()
        {
            string s = "";
            foreach (Card c in cards)
            {
                s += "\n" + c.ToString();
            }
            return s;
        }
    }
}
