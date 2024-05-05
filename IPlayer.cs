using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoFish_
{
    internal interface IPlayer
    {
        public void flipTurn();
        public void makeDecision();

        protected virtual void OnCardRevealed(Card card) { } //event trigger

        public void revealCard(Card card);
        public bool search(int targetRank);
    }
}
