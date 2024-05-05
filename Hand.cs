using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GoFish_
{
    internal class Hand : IDeck
    {
        private List<Card> cards = new List<Card> ();
        private string playerName;
        private Player player;
        Random r = new Random ();

        public Hand(Player player)
        {
            playerName = player.Name;
            this.player = player;
        }

        public List<Card> Cards
        {
            get { return cards; }
        }

        public void addCard(Card card)
        {
            cards.Add(card);
        }

        public Card pullCard()
        {
            if (cards.Count != 0)
            {
                int index = r.Next(cards.Count);
                Card chosenCard = cards[index];
                cards.RemoveAt(index);
                return chosenCard;
            }
            else
            {
                throw new Exception("Attempting to pull from empty deck");
            }
        }

        public Card pullCard(Card card)
        {
            foreach(Card c in cards)
            {
                if (c == card)
                {
                    Card targetCard = c;
                    cards.Remove(targetCard);
                    return targetCard;
                }
            }
            throw new Exception("Attempting to pull from empty deck");
        }

        public Card find(Card card)
        {
            //specifically pulls the card called
            foreach (Card c in cards)
            {
                if (c == card)
                {
                    Card targetCard = c;
                    return targetCard;
                }
            }
            throw new Exception("Couldn't find card");
        }
        public Player Player
        {
            get { return player; }
        }

        public override string ToString()
        {
            string s = this.Player.Name + "'s hand:";
            foreach (Card c in cards)
            {
                s += "\n" + c.ToString();
            }
            return s;
        }
    }
}
