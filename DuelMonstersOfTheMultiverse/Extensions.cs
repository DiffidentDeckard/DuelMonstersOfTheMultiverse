using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace DuelMonstersOfTheMultiverse
{
    /// <summary>
    /// Extensions to make repeated code simpler
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// This contains the boiler-plate StartCoroutine/ExhaustCoroutine code that should always be used.
        /// </summary>
        /// <param name="gc"></param>
        /// <param name="coroutine"></param>
        /// <returns></returns>
        public static IEnumerator StartCoroutineEx(this GameController gc, IEnumerator coroutine)
        {
            if (gc.UseUnityCoroutines)
            {
                yield return gc.StartCoroutine(coroutine);
            }
            else
            {
                gc.ExhaustCoroutine(coroutine);
            }
        }

        /// <summary>
        /// Checks to see if this card contains the given keyword.
        /// Unlike the base calls, this is *NOT CASE SENSITIVE*
        /// </summary>
        /// <param name="card"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public static bool KeywordsContainEx(this Card card, string keyword)
        {
            return card.GetKeywords().Any(kw => kw.Equals(keyword, StringComparison.CurrentCultureIgnoreCase));
        }

        /// <summary>
        /// Checks to see if this card contains any of the given keywords.
        /// Unlike the base calls, this is *NOT CASE SENSITIVE*
        /// </summary>
        /// <param name="card"></param>
        /// <param name="keywords"></param>
        /// <returns></returns>
        public static bool KeywordsContainAnyOfEx(this Card card, IEnumerable<string> keywords)
        {
            return card.GetKeywords().Intersect(keywords, StringComparer.CurrentCultureIgnoreCase).Any();
        }

        /// <summary>
        /// Returns all the individual selected cards from a list of SelectCardsDecisions
        /// </summary>
        /// <param name="storedResults"></param>
        /// <returns></returns>
        public static IEnumerable<Card> GetSelectedCardsEx(this List<SelectCardsDecision> storedResults)
        {
            return storedResults.SelectMany(scd => scd.SelectCardDecisions).Select(scd => scd.SelectedCard);
        }
    }
}
